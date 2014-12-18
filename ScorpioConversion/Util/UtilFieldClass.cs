using System;
using System.Collections.Generic;
using System.Text;

public static partial class Util
{
    public static string GetFieldClass(PROGRAM program, string strDataName, List<Variable> variables)
    {
        StringBuilder builder = new StringBuilder();
        switch (program)
        {
            case PROGRAM.CS:
                builder.Append(@"
public class __FieldClass {");
                break;
            case PROGRAM.JAVA:
                builder.Append(@"
public class __FieldClass {");
                break;
            case PROGRAM.PHP:
                builder.Append(@"
class __FieldClass {");
                break;
            case PROGRAM.CPP:
                
                break;
        }
        for (int i = 0; i < variables.Count; ++i)
        {
            string strFieldName = variables[i].strFieldName;
            if (program == PROGRAM.CS)
            {
                string str = @"
    public const string __FieldName = ""__FieldName"";";
                builder.Append(str.Replace("__FieldName", strFieldName));
            }
            else if (program == PROGRAM.JAVA)
            {
                string str = @"
    public final String __FieldName = ""__FieldName"";";
                builder.Append(str.Replace("__FieldName", strFieldName));
            }
        }
        if (program == PROGRAM.CPP) {

        } else {
            builder.Append(@"
}");
        }
        return builder.Replace("__FieldClass", strDataName).ToString();
    }
}
