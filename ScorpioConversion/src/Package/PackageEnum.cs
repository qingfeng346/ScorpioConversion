using System.Collections.Generic;

public class FieldEnum {
    public int Index;       //枚举值
    public string Name;     //枚举类型
}
public class PackageEnum {
    public List<FieldEnum> Fields = new List<FieldEnum>();
}