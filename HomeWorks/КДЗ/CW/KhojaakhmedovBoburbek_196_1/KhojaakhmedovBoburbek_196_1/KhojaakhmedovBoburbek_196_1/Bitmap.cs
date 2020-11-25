using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhojaakhmedovBoburbek_196_1
{
    public abstract class Bitmap
    {
        public abstract void And(Bitmap other);

        public abstract void Set(int i, bool value);

        public abstract bool Get(int i);
    }
}
