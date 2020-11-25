using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhojaakhmedovBoburbek_196_2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                string input = args[0];
                string output = args[1];

                string path_to_input = @"input\" + input;
                string path_to_output = @"output\" + output;
                Console.WriteLine();

                input = ReadText(path_to_input);
                output = ReadText(path_to_output);

                if (input.Length >= 0 && input.Length <= 1000)
                {
                    Palindrom(input, output);
                }
                else
                {
                    Console.WriteLine("Содержимое файла некорректно!");
                }
            }
            else
            {
                Console.WriteLine("Введены неверные данные!");
            }
        }
        public static string ReadText(string path)
        {
            string line;
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    return line;
                }
            }
            return line;
        }
        public static void Palindrom(string input, string output)
        {
            char[] arr = input.ToCharArray();
            int odd = 0;
            int even = 0;
            int count = arr.Length;
            List<char> tmp;
            List<string> list_of_words = new List<string>();
            string word = "";
            string reversed_word = "";

            for (int j = 0; j < count; j++)
            {
                for (int i = 1; i < arr.Length + 1; i++)
                {
                    word = GetWord(i, arr);
                    if (list_of_words.Contains(word))
                    {
                        if (word.Length % 2 == 0)
                        {
                            even++;
                            continue;
                        }
                        else
                        {
                            odd++;
                            continue;
                        }
                    }
                    else
                        reversed_word = Reverse(word);
                    if (word == reversed_word)
                    {
                        if (word.Length % 2 == 0)
                        {
                            even++;
                            list_of_words.Add(word);
                        }
                        else
                        {
                            odd++;
                            list_of_words.Add(word);
                        }
                    }
                }
                tmp = arr.ToList();// Преобразование в список
                tmp.RemoveAt(0); // Удаление элемента
                arr = tmp.ToArray(); // Преобразование в массив
            }
            int total = even + odd;
            Console.WriteLine("Мой результат:" + total + " " + even + " " + odd);
            Console.WriteLine("Результат из папки output:" + output);
        }
        public static string GetWord(int n, char[] arr)
        {
            string result = "";
            for (int i = 0; i < n; i++)
            {
                result += arr[i];
            }
            return result;
        }
        public static string Reverse(string text)
        {
            char[] cArray = text.ToCharArray();
            string reverse = String.Empty;
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }
    }
}
