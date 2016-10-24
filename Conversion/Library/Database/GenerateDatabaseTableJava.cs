using System;
using System.Collections.Generic;
using System.Text;
public class GenerateDatabaseTableJava : IGenerate
{
    public GenerateDatabaseTableJava() : base(PROGRAM.Java) { }
    protected override string Generate_impl()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//本文件为自动生成，请不要手动修改
package __Package;
import com.commons.database.advanced.DatabaseFieldDataBase;
public class __ClassName implements DatabaseFieldDataBase<__ClassName> {");
        builder.Append(GenerateDatabaseFields());
        builder.Append(GenerateDatabaseConstructor());
        builder.Append(GenerateDatabaseEquals());
        builder.Append(@"
}");
        builder = builder.Replace("__Package", m_Package);
        builder = builder.Replace("__ClassName", m_ClassName);
        return builder.ToString();
    }
    string GenerateDatabaseFields()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var field in m_Fields) {
            string str = @"
    /** __Comment */
    public __Type __Name = __Default;
    public __Type get__UpCaseName() { return __Name; }";
            string type = GetCodeType(field.Type);
            str = str.Replace("__Type", field.Array ? "java.util.List<" + type + ">" : type);
            str = str.Replace("__Name", field.Name);
            str = str.Replace("__Comment", field.Comment);
            str = str.Replace("__UpCaseName", FileUtil.ToOneUpper(field.Name));
            string def = "";
            if (field.Array) {
                def = "new java.util.ArrayList<" + type + ">()";
            } else if (field.IsBasic) {
                switch (field.Info.BasicIndex) {
                    case BasicEnum.BOOL: def = "false"; break;
                    case BasicEnum.STRING: def = "\"\""; break;
                    case BasicEnum.BYTES: def = "null"; break;
                    case BasicEnum.INT64: def = "0l"; break;
                    case BasicEnum.FLOAT: def = "0f"; break;
                    case BasicEnum.DOUBLE: def = "0d"; break;
                    default: def = "0"; break;
                }
            } else {
                def = "null";
            }
            str = str.Replace("__Default", def);
            builder.Append(str);
        }
        return builder.ToString();
    }
    string GenerateDatabaseConstructor()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    public __ClassName() { }
    public __ClassName(__ClassName value){");
        foreach (var field in m_Fields) {
            string str = @"
        __Assign;";
            str = str.Replace("__Assign", DatabaseUtil.AssignOperate(field.Array, field.Type));
            str = str.Replace("__FieldName", field.Name);
            str = str.Replace("__FieldType", GetCodeType(field.Type));
            builder.Append(str);
        }
        builder.Append(@"
    }");
        return builder.ToString();
    }
    string GenerateDatabaseEquals()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    @Override
    public boolean equals(Object obj) {
        if (this == obj) return true;
		if (obj == null) return false;
		if (getClass() != obj.getClass()) return false;
        __ClassName other = (__ClassName) obj;");
        foreach (var field in m_Fields) {
            string str = @"
        if (!this.__Name.equals(other.__Name)) return false;";
            builder.Append(str.Replace("__Name", field.Name));
        }
        builder.Append(@"
        return true;
    }");
        return builder.ToString();
    }
}

