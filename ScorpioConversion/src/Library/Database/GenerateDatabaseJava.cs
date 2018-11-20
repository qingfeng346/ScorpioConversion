using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
public class GenerateDatabaseJava : IGenerate
{
    private string m_PrimaryKeyName = "";       //主键名字
    private string m_GeneratedKeyName = "";     //自动递增的字段名字
    private string m_GeneratedKeyType = "";     //自动递增的字段类型
    public GenerateDatabaseJava() : base(PROGRAM.Java) { }
    protected override string Generate_impl()
    {
        m_PrimaryKeyName = string.IsNullOrEmpty(m_Database.key) ? "" : m_Database.key;
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//本文件为自动生成，请不要手动修改
package __Package;
import java.util.Collections;
import java.util.List;
import java.util.Arrays;
import java.util.ArrayList;
import java.sql.Timestamp;
import java.sql.Date;
import java.io.Serializable;
import com.commons.util.Utility;
import com.commons.database.result.DatabaseInsertUpdateResult;
import com.commons.database.advanced.DatabaseSimple;
import com.commons.database.advanced.DatabaseFieldAttribute;
import com.commons.database.advanced.DatabaseTableDataBase;
import com.commons.database.advanced.DatabaseFieldDataBase;
//================__TableComment===========================
@SuppressWarnings(""unused"")
public class Database__ClassName implements DatabaseTableDataBase<Database__ClassName> {
    public static final String TableName = ""__ClassName"";     //数据库表名字
    private DatabaseSimple __simple = null;                     //DatabaseSimple对象 用于获取获得此对象的 数据库对象
    private Database__ClassName __self = null;                  //克隆对象 用于获取差异字段");
        builder.Append(GenerateDatabaseFields());
        builder.Append(GenerateDatabaseBasis());
        builder.Append(GenerateDatabaseDiff());
        builder.Append(GenerateDatabaseSet());
        builder.Append(GenerateDatabaseClear());
        builder.Append(GenerateDatabaseIsEmpty());
        builder.Append(@"
}");
        builder = builder.Replace("__Package", m_Package);
        builder = builder.Replace("__ClassName", m_ClassName);
        if (string.IsNullOrEmpty(m_PrimaryKeyName)) {
            builder = builder.Replace("__PrimaryKeyName", "null");
            builder = builder.Replace("__PrimaryKeyValue", "null");
        } else {
            var keys = m_PrimaryKeyName.Split(',');
            builder = builder.Replace("__PrimaryKeyName", @"new String[] { """ + string.Join("\" , \"", keys) + "\" }");
            builder = builder.Replace("__PrimaryKeyValue", @"new Object[] { " + string.Join(" , ", keys) + " }");
        }
        builder = builder.Replace("__PrimaryKeyName", string.IsNullOrEmpty(m_PrimaryKeyName) ? "" : m_PrimaryKeyName);
        builder = builder.Replace("__PrimaryKeyValue", string.IsNullOrEmpty(m_PrimaryKeyName) ? "null" : m_PrimaryKeyName);
        builder = builder.Replace("__GeneratedKeySet", string.IsNullOrEmpty(m_GeneratedKeyName) ? "" : (m_GeneratedKeyName + " = (" + m_GeneratedKeyType + ")value;"));
        return builder.ToString();
    }
    string GenerateDatabaseFields() {
        StringBuilder builder = new StringBuilder();
        foreach (var field in m_Database.fields) {
            string str = @"
    /** __FieldComment */
    @DatabaseFieldAttribute(fieldName = ""__FieldName"", fieldType = __DatabaseType.class, arrayType = __ArrayType.class)
    public __FinishType __FieldName = null;";
            str = str.Replace("__FieldName", field.name);
            str = str.Replace("__FieldComment", field.comment);
            str = str.Replace("__DatabaseType", DatabaseUtil.GetDatabaseType(field));
            str = str.Replace("__FinishType", DatabaseUtil.GetFinishType(field, true));
            str = str.Replace("__ArrayType", DatabaseUtil.GetFinishType(field, false));
            if (field.auto_increment != -1) {
                m_GeneratedKeyName = field.name;
                m_GeneratedKeyType = DatabaseUtil.GetFinishType(field, false);
            }
            builder.Append(str);
        }
        return builder.ToString();
    }
    string GenerateDatabaseBasis() {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    @Override
    public void setDatabaseSimple(DatabaseSimple value) { __simple = value; }
    @Override
    public DatabaseSimple getDatabaseSimple() { return __simple; }
    @Override
    public String getTableName() { return ""__ClassName""; }
    @Override
    public void sync() { __self = new Database__ClassName().set(this); }
    @Override
    public String[] getPrimaryKeyName() { return __PrimaryKeyName; }
    @Override
    public Object[] getPrimaryKeyValue() { return __PrimaryKeyValue; }
    @Override
    public void setGeneratedKeyValue(Object value) { __GeneratedKeySet }
    @Override
    public DatabaseInsertUpdateResult save() throws Exception { return (__simple != null && getPrimaryKeyValue() != null) ? __simple.Update(this) : new DatabaseInsertUpdateResult(); }");
        return builder.ToString();
    }
    string GenerateDatabaseDiff() {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    @Override
    public Database__ClassName diff() {
        Database__ClassName ret = new Database__ClassName();");
        foreach (var field in m_Database.fields) {
            string str = @"
        if (this.__FieldName != null && (__self == null || !this.__FieldName.equals(__self.__FieldName)))
            __Assign;";
            str = str.Replace("__Assign", DatabaseUtil.AssignOperate(field.array, field.@class, "ret", "this"));
            str = str.Replace("__FieldName", field.name);
            str = str.Replace("__FieldType", DatabaseUtil.GetFinishType(field, true));
            builder.Append(str);
        }
        builder.Append(@"
        return ret;
    }");
        return builder.ToString();
    }
    string GenerateDatabaseSet()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    @Override
    public Database__ClassName set(Database__ClassName value) {");
        foreach (var field in m_Database.fields) {
            string str = @"
        __Assign;";
            str = str.Replace("__Assign", DatabaseUtil.AssignOperate(field.array, field.@class));
            str = str.Replace("__FieldName", field.name);
            str = str.Replace("__FieldType", DatabaseUtil.GetFinishType(field, true));
            builder.Append(str);
        }
        builder.Append(@"
        return this;
    }");
        return builder.ToString();
    }
    string GenerateDatabaseClear()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    @Override
    public void clear() {");
        foreach (var field in m_Database.fields) {
            string str = @"
        this.__FieldName = null;";
            str = str.Replace("__FieldName", field.name);
            builder.Append(str);
        }
        builder.Append(@"
    }");
        return builder.ToString();
    }
    string GenerateDatabaseIsEmpty() {
        StringBuilder builder = new StringBuilder();
        builder.Append(@"
    @Override
    public boolean isEmpty() {");
        foreach (var field in m_Database.fields) {
            string str = @"
        if (this.__FieldName != null) return false;";
            str = str.Replace("__FieldName", field.name);
            builder.Append(str);
        }
        builder.Append(@"
        return true;
    }");
        return builder.ToString();
    }
}

