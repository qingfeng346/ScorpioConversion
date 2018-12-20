using System;
using System.Collections.Generic;
using System.Text;
using Scorpio;

public class PackageField {
    public int Index;                   //字段索引
    public string Comment;              //字段注释
    public string Name;                 //字段名字
    public ScriptTable Attribute;       //字段属性,字段配置
    public string Default;              //字段默认值
    public string Type;                 //字段类型
    public bool valid = true;           //字段是否有效
    public bool Enum = false;           //是否是枚举
    public bool Const = false;          //是否是常量
    public bool Array = false;          //是否是数组

    //是否是基本数据
    public bool IsBasic {       
        get { return true; }
    }
}