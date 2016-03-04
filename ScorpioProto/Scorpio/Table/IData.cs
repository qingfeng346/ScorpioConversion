using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Table
{
    public interface IData {
        object GetData(string key);
        bool IsInvalid();
    }
}
