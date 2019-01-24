using System.Collections.Generic;

public class FieldConst {
    public string Name;     //常量名字
    public BasicEnum Type;  //常量类型 目前只有 int32 int64 string
    public string Value;    //常量值
}
public class PackageConst {
    public List<FieldConst> Fields = new List<FieldConst>();
}