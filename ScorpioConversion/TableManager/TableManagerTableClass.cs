using System;
using System.Collections.Generic;
using System.Text;


public partial class TableManager
{
    /// <summary> 获得Table类的代码 </summary>
    private string GetTableClass(PROGRAM program)
    {
        var info = Util.GetProgramInfo(program);
        var template = FileUtil.GetFileString(CurrentDirectory + "/Table." + info.Extension);
        template = template.Replace("__TableName", TableClassName);
        template = template.Replace("__DataName", DataClassName);
        template = template.Replace("__Key", BasicUtil.GetType(BasicEnum.INT32).Codes[(int)program]);
        template = template.Replace("__MD5", GetClassMD5Code());
        return template.ToString();
    }
}
