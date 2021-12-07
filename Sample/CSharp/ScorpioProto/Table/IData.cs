using System;
using System.Collections.Generic;
using System.Text;

namespace ScorpioProto.Table {
    public interface IData {
        object GetData(string key);
    }
}
