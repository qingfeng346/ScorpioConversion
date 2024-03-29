﻿using System.Collections.Generic;
namespace Scorpio.Conversion.Engine {
    public class ConstField {
        public string Name;     //常量名字
        public BasicEnum Type;  //常量类型 目前只有 int32 int64 string
        public string Value;    //常量值
    }
    public class PackageConst : IPackage {
        public string Name;
        public List<ConstField> Fields = new List<ConstField>();
    }
}
