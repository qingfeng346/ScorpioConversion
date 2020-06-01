using System.Collections.Generic;

public class FieldEnum {
    public int Index;       //枚举值
    public string Name;     //枚举名字
}
public class PackageEnum : IPackage {
    public string Name;
    public List<FieldEnum> Fields = new List<FieldEnum>();
}