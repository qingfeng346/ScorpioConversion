using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI;
using NPOI.HSSF.Util;
using System.Reflection;
public partial class TableBuilder
{
    /// <summary> 反转表文件 </summary>
    public void Rollback(string files)
    {
        List<string> fileNames = new List<string>(files.Split(';'));
        while (fileNames.Remove("")) { }
        if (fileNames.Count == 0) {
            Logger.info("请选择要转换的文件");
            return;
        }
        Progress.Count = fileNames.Count;
        int Count = 0;
        for (int i = 0; i < fileNames.Count; ++i) {
            Progress.Current = (i + 1);
            try {
                byte[] buffer = FileUtil.GetFileBuffer(fileNames[i]);
                try { buffer = GZipUtil.Decompress(buffer); } catch (System.Exception) { }
                TableReader reader = new TableReader(buffer);
                Rollback_impl(reader, fileNames[i]);
                reader.Close();
                Count++;
            } catch (System.Exception ex) {
                throw new Exception(string.Format("{0} 文件出错\r\n{1}", fileNames[i], ex.ToString()));
            }
        }
        if (Count > 0)
            Logger.warn("转换结束");
    }
    private List<PackageField> GetFields(TableReader reader)
    {
        List<PackageField> fields = new List<PackageField>();
        int number = reader.ReadInt32();        //字段数量
        for (int i = 0; i < number; ++i) {
            if (reader.ReadInt8() == 0) {   //基础类型
                int type = reader.ReadInt8();       //基础类型索引
                bool array = reader.ReadBool();     //是否是数组
                fields.Add(new PackageField() {
                    Index = i,
                    Name = "Value" + i,
                    Type = BasicUtil.GetType((BasicEnum)type).ScorpioName,
                    Array = array,
                });
            } else {                        //自定义类
                string type = reader.ReadString();   //自定义类名称
                bool array = reader.ReadBool();      //是否是数组
                fields.Add(new PackageField() {
                    Index = i,
                    Name = "Value" + i,
                    Type = type,
                    Array = array,
                });
            }
        }
        return fields;
    }
    private string ReadValue(TableReader reader, PackageField field, int oldLevel)
    {
        int level = oldLevel;
        if (field.Array) --level;
        if (field.Info != null)
        {
            if (field.Array) {
                int number = reader.ReadInt32();
                StringBuilder builder = new StringBuilder();
                if (level <= 0) builder.Append("[");
                for (int i = 0; i < number; ++i)
                {
                    if (i != 0) builder.Append(",");
                    builder.Append(field.Info.ReadValue(reader).ToString());
                }
                if (level <= 0) builder.Append("]");
                return builder.ToString();
            } else {
                return field.Info.ReadValue(reader).ToString();
            }
        }
        else
        {
            if (field.Array) {
                int number = reader.ReadInt32();
                StringBuilder builder = new StringBuilder();
                if (level <= 0) builder.Append("[");
                for (int i = 0; i < number; ++i)
                {
                    if (i != 0) builder.Append(",");
                    builder.Append(ReadFieldsInterior(reader, mCustoms[field.Type], level - 1));
                }
                if (level <= 0) builder.Append("]");
                return builder.ToString();
            } else {
                return ReadFieldsInterior(reader, mCustoms[field.Type], level - 1);
            }
        }
    }
    private string ReadFieldsInterior(TableReader reader, List<PackageField> fields, int level)
    {
        List<string> ret = ReadFields(reader, fields, level);
        StringBuilder builder = new StringBuilder();
        if (level <= 0) builder.Append("[");
        for (int i = 0; i < ret.Count; ++i)
        {
            if (i != 0) builder.Append(",");
            builder.Append(ret[i]);
        }
        if (level <= 0) builder.Append("]");
        return builder.ToString();
    }
    private List<string> ReadFields(TableReader reader, List<PackageField> fields, int level)
    {
        List<string> ret = new List<string>();
        foreach (var field in fields)
        {
            ret.Add(ReadValue(reader, field, level));
        }
        return ret;
    }
    private void Rollback_impl(TableReader reader, string fileName)
    {
        string fileTitle = fileName.Substring(0, fileName.LastIndexOf("."));
        string filePath = Path.GetDirectoryName(fileName);
        IWorkbook workbook = new HSSFWorkbook();
        ISheet sheet = workbook.CreateSheet("Sheet1");
        int iRows = reader.ReadInt32();         //行数量
        reader.ReadString();                    //读取MD5
        mCustoms.Clear();
        List<PackageField> Fields = GetFields(reader);  //读取表结构
        int customNumber = reader.ReadInt32();  //自定义类数量
        for (int i = 0; i < customNumber; ++i) {
            List<PackageField> fields = new List<PackageField>();
            string str = reader.ReadString();   //读取自定义类名字
            mCustoms[str] = GetFields(reader);  //读取自定义类结构
        }
        {
            IRow row0 = sheet.CreateRow(0);
            IRow row1 = sheet.CreateRow(1);
            IRow row2 = sheet.CreateRow(2);
            IRow row3 = sheet.CreateRow(3);
            for (int i = 0; i < Fields.Count; ++i) {
                PackageField field = Fields[i];
                string str = field.Array ? "array" + field.Type : field.Type;
                row0.CreateCell(i).SetCellValue("注释(请自行粘贴)");
                row1.CreateCell(i).SetCellValue("字段名(请自行粘贴)");
                row2.CreateCell(i).SetCellValue("字段默认值(请自行粘贴)");
                row3.CreateCell(i).SetCellValue(str);
            }
        }
        foreach (var pair in mCustoms)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"__Class = {");
            foreach (var field in pair.Value)
            {
                string str = @"
    __Field = ""__Index,__Type,__Array"",";
                str = str.Replace("__Field", field.Name);
                str = str.Replace("__Index", field.Index.ToString());
                str = str.Replace("__Type", field.Type);
                str = str.Replace("__Array", field.Array ? "true" : "false");
                builder.Append(str);
            }
            builder.Append(@"
}");
            builder = builder.Replace("__Class", pair.Key);
            FileUtil.CreateFile(string.Format("{0}/{1}.js", filePath, pair.Key), builder.ToString(), false);
        }
        for (int i = 0; i < iRows; ++i) {
            IRow row = sheet.CreateRow(i + START_ROW);
            List<string> strs = ReadFields(reader, Fields, 2);
            for (int j = 0; j < strs.Count;++j )
                row.CreateCell(j).SetCellValue(strs[j]);
        }
        string rollbackFileName = fileTitle + ".xls";
        FileUtil.DeleteFile(rollbackFileName);
        FileStream stream = new FileStream(rollbackFileName, FileMode.Create);
        workbook.Write(stream);
        stream.Close();
    }
}

