using System.Collections;
using System.Collections.Generic;

namespace KhojaakhmedovBoburbek_196_1
{
    public class MyBitmap : Bitmap, IEnumerable<string>
    {
        int[] arr;
        List<List<string>> list = new List<List<string>>();
        public MyBitmap(int length)
        {
            arr = new int[length];
        }

        public override void And(Bitmap other)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (other.Get(i) == true)
                    arr[i] = arr[i] & 1;
                else
                    arr[i] = arr[i] & 0;
            }
        }

        public override bool Get(int i)
        {
            return arr[i] == 1;

        }

        public IEnumerator<string> GetEnumerator()
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == 1)
                {
                    string str = "";
                    int amount = list.Count;
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (amount != 1)
                        {
                            str += $"{(list[j])[i]}|";
                            amount--;
                        }
                        else
                        {
                            str += $"{(list[j])[i]}";
                            amount--;
                        }
                    }
                    yield return str;
                }
            }
        }
        public override void Set(int i, bool value)
        {
            if (value)
                arr[i] = 1;
            else
                arr[i] = 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddList(List<string> list)
        {
            this.list.Add(list);
        }

        public int GetOne()
        {
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == 1)
                    count++;
            }
            return count;
        }
    }
}
