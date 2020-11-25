using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KhojaakhmedovBoburbek_196
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<Point> myList = null;
                string direction = args[0];
                string form = args[1];
                string path_to_output = args[3];
                if (!File.Exists(args[3]))
                {
                    path_to_output = "";
                    path_to_output = @"output\" + args[3];
                }
                myList = GetData(args, direction, form, myList, path_to_output);
                List<Point> reserved = myList;
                FirstStep(myList); //Шаг номер 1, находим левый нижний элемеент и ставим ее на 1 место в списке
                SecondStep(myList); //Сортировка по левизне
                var result = ThirdStep(myList); //Удалеум все точки, где выполняется правый поворот
                var output = GeneratingList(path_to_output);
                ShowData(result, direction, form, output, reserved);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static List<Point> GeneratingList(string path)
        {
            int counter = 1;
            int n;
            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine();
                if (counter == 1 && int.TryParse(line, out n) && (n >= 3 && n <= 1000))
                {
                    List<Point> ms = new List<Point>();
                    for (int i = 0; i < n; i++)
                    {
                        line = sr.ReadLine();
                        var nums = line.Split(' ');
                        ms.Add(new Point(int.Parse(nums[0]), int.Parse(nums[1])));
                    }
                    return ms;
                }
                else
                {
                    throw new Exception("Неверные входные параметры!");
                }
            }
        }
        static void ChangeIndex(List<Point> list, int indexA, int indexB)
        {
            var tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
        public static void DeletePoints(List<Point> points)
        {
            for (int i = 2; i < points.Count; i++)
            {
                Point A = points[i - 2];
                Point B = points[i - 1];
                Point C = points[i];
                if ((C.X - A.X) * (B.Y - A.Y) == (C.Y - A.Y) * (B.X - A.X))
                {
                    points.RemoveAt(i - 1);
                    i--;
                }
            }
        }
        public static double Rotate(Point A, Point B, Point C)
        {
            return (B.X - A.X) * (C.Y - B.Y) - (B.Y - A.Y) * (C.X - B.X);
        }
        public static List<Point> GetData(string[] args, string direction, string form, List<Point> myStack, string path_to_output)
        {
            if (args.Length == 4)
            {
                if (direction == "cw" || direction == "cc")
                {
                    if (form == "plain" || form == "wkt")
                    {
                        string path_to_input = args[2];
                        if (File.Exists(path_to_input))
                        {
                            myStack = GeneratingList(path_to_input);
                            return myStack;
                        }
                        else
                        {
                            path_to_input = "";
                            path_to_input += @"input\" + args[2];
                            myStack = GeneratingList(path_to_input);
                            return myStack;
                        }
                    }
                    else
                    {
                        throw new Exception("Доступные форматы вывода: Plain или Well-Known Text!");
                    }
                }
                else
                {
                    throw new Exception("Неправильно указаны направления!");
                }

            }
            else
            {
                throw new Exception("Неверные входные данные!");
            }
        }
        public static void FirstStep(List<Point> list)
        {
            Point result = list.OrderBy(x => x.Y).ThenBy(x => x.X).FirstOrDefault();
            ChangeIndex(list, list.IndexOf(result), 0);
        }
        public static void SecondStep(List<Point> list)
        {
            for (int i = 2; i < list.Count; i++)
            {
                int j = i;
                while (j > 1 && Rotate(list[0], list[j - 1], list[j]) < 0)
                {
                    ChangeIndex(list, j - 1, j);
                    j--;
                }
            }
        }
        public static List<Point> ThirdStep(List<Point> list)
        {
            MyStack<Point> myStack = new MyStack<Point>();
            myStack.Push(list[0]);
            myStack.Push(list[1]);
            for (int i = 2; i < list.Count; i++)
            {
                while (Rotate(myStack.NextToTop(), myStack.Peek(), list[i]) < 0)
                {
                    myStack.Pop();
                }
                myStack.Push(list[i]);
            }
            List<Point> points = new List<Point>();
            foreach (var item in myStack)
            {
                points.Add(item);
            }
            DeletePoints(points);
            return points;
        }
        public static void ShowData(List<Point> list, string direction, string form, List<Point> list_output, List<Point> reserved)
        {
            if (direction == "cc" && form == "plain")
            {
                Console.WriteLine("Мой результат: cc and plain");
                Console.WriteLine(list.Count);
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine("Результат из папки output:");
                Console.WriteLine(list_output.Count);
                foreach (var item in list_output)
                {
                    Console.WriteLine(item);
                }
            }
            if (direction == "cw" && form == "plain")
            {
                Console.WriteLine("Мой результат: cw and plain");
                Console.WriteLine(list.Count);
                Console.WriteLine(list[0]);
                list.Remove(list[0]);
                list.Reverse();
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine("Результат из папки output:");
                Console.WriteLine(list_output.Count);
                foreach (var item in list_output)
                {
                    Console.WriteLine(item);
                }
            }
            if (direction == "cc" && form == "wkt")
            {
                string rez = "MULTIPOINT(";
                for (int i = 0; i < reserved.Count; i++)
                {
                    if (i == reserved.Count - 1)
                    {
                        rez += "(" + reserved[i].ToString() + "))";
                        continue;
                    }
                    rez += "(" + reserved[i].ToString() + "), ";
                }
                Console.WriteLine(rez);

                string rez2 = "";
                foreach (var item in list)
                {
                    rez2 += item.ToString() + ", ";
                }
                var sum = rez2.Substring(0, rez2.Length - 1);
                sum = "POLYGON ((" + sum + list[0].ToString() + "))";
                Console.WriteLine(sum);
            }
            if (direction == "cw" && form == "wkt")
            {
                string rez = "MULTIPOINT(";
                for (int i = 0; i < reserved.Count; i++)
                {
                    if (i == reserved.Count - 1)
                    {
                        rez += "(" + reserved[i].ToString() + "))";
                        continue;
                    }
                    rez += "(" + reserved[i].ToString() + "), ";
                }
                Console.WriteLine(rez);

                string rez2 = "";
                var point = list[0];
                list.Remove(point);
                list.Reverse();
                rez2 += point.ToString() + ",";
                foreach (var item in list)
                {
                    rez2 += item.ToString() + ", ";
                }
                var sum = rez2.Substring(0, rez2.Length - 1);
                sum = "POLYGON ((" + sum + point.ToString() + "))";
                Console.WriteLine(sum);
            }
        }
    }
    public class Point
    {
        private int x;
        private int y;
        public int X
        {
            private set
            {
                if (value <= 10000)
                    x = value;
                else
                    throw new Exception("Неверное значение X!");
            }
            get
            {
                return x;
            }
        }
        public int Y
        {
            private set
            {
                if (value <= 10000)
                    y = value;
                else
                    throw new Exception("Неверное значение X!");
            }
            get
            {
                return y;
            }
        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return X.ToString() + " " + Y.ToString();
        }
    }
    public class MyStack<T> : IEnumerable<T>
    {
        private List<T> data;
        public const int capacity = 1000; //В условии сказано, количество деревьев от 3 до 1000
        public int Maximum { get; set; }
        public int Size { get { return data.Count; } }
        public MyStack(int size)
        {
            if (size < 3)
                throw new Exception("Количество деревьев должно быть больше 3!");
            else
            {
                if (size >= 3 && size <= capacity)
                {
                    data = new List<T>();
                    this.Maximum = size;
                }
                else
                    throw new Exception("Максимальный размер стека 1000!");
            }
        }
        public MyStack()
        {
            data = new List<T>();
            this.Maximum = 1000;
        }
        public bool isEmpty
        {
            get
            {
                return data.Count == 0;
            }
        }
        public void Push(T el)
        {
            if (el == null)
                throw new ArgumentNullException(el.ToString());
            else
            {
                if (Maximum != data.Count)
                    data.Add(el);
                else
                    throw new StackOverflowException("Достигнуто максимально возможное количество элементов!");
            }
        }
        public T Pop()
        {
            if (isEmpty)
                throw new NullReferenceException("Стек пуст!");
            else
            {
                var element = data[data.Count - 1];
                data.RemoveAt(data.Count - 1);
                return element;
            }
        }
        public T Peek()
        {
            if (isEmpty)
                throw new NullReferenceException("Стек пуст!");
            else
                return data.LastOrDefault();
        }
        public T NextToTop()
        {
            if (isEmpty)
                throw new NullReferenceException("Стек пуст!");
            else
            {
                return data[data.Count - 2];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
