using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Table
{
    public abstract class ITable
    {
        public abstract bool Contains(int ID);
        public abstract int Count();
        public abstract IData GetValue(int key);
    }
}
