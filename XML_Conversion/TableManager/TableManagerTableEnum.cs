using System;
using System.Collections.Generic;
using System.Text;

public partial class TableManager
{
    public void CreateEnumCS()
    {
        PROGRAM program = PROGRAM.CS;
        StringBuilder builder = new StringBuilder();
        builder.Append(@"using System;
public class MT_TableEnum {");
        foreach (string key in mTableEnumList)
        {
            string str = @"
    public const string __FieldName = ""__FieldName"";";
            builder.Append(str.Replace("__FieldName", key));
        }
        builder.Append(@"
}");
        FileUtil.CreateFile("MT_TableEnum.cs", builder.ToString(), true, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
    }
    public void CreateEnumJAVA()
    {
        PROGRAM program = PROGRAM.JAVA;
        StringBuilder builder = new StringBuilder();
        builder.Append(@"package table;
public class MT_TableEnum {");
        foreach (string key in mTableEnumList)
        {
            string str = @"
    public static final String __FieldName = ""__FieldName"";";
            builder.Append(str.Replace("__FieldName", key));
        }
        builder.Append(@"
}");
        FileUtil.CreateFile("MT_TableEnum.java", builder.ToString(), false, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
    }
    public void CreateEnumPHP()
    {
        PROGRAM program = PROGRAM.PHP;
        StringBuilder builder = new StringBuilder();
        builder.Append(@"<?PHP
class MT_TableEnum {");
        foreach (string key in mTableEnumList)
        {
            string str = @"
    const __FieldName = ""__FieldName"";";
            builder.Append(str.Replace("__FieldName", key));
        }
        builder.Append(@"
}
?>");
        FileUtil.CreateFile("MT_TableEnum.php", builder.ToString(), false, Util.GetProgramInfo(program).CodeDirectory.Split(';'));
    }
}

