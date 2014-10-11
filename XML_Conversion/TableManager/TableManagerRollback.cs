using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

public partial class TableManager
{
    /// <summary>
    /// 反转表文件
    /// </summary>
    public void Rollback(string[] fileName)
    {
        if (fileName == null || fileName.Length <= 0)
        {
            MessageBox.Show("请选择要转换的文件");
            return;
        }
        Progress.Count = fileName.Length;
        int Count = 0;
        for (int i = 0; i < fileName.Length; ++i)
        {
            Progress.Current = (i + 1);
            try
            {
                byte[] buffer = FileUtil.GetFileBuffer(fileName[i]);
                MemoryStream stream = new MemoryStream(GZipUtil.Decompress(buffer));
                BinaryReader reader = new BinaryReader(stream);
                Rollback_impl(reader, fileName[i]);
                stream.Close();
                reader.Close();
                Count++;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("{0} 文件出错\r\n{1}", fileName[i], ex.ToString()));
                continue;
            }
        }
        if (Count > 0)
            MessageBox.Show("转换结束");
    }
    private void Rollback_impl(BinaryReader reader, string fileName)
    {
        string fileTitle = fileName.Substring(0, fileName.LastIndexOf("."));
        FileStream fs = new FileStream(fileTitle + ".xlt", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        int iRows = reader.ReadInt32();         //行数量
        int iColums = reader.ReadInt32();       //列数量
        int iCodeNum = reader.ReadInt32();      //自定义类数量
        for (int i = 0; i < iCodeNum; ++i)      //读取所有自定义类MD5码
        {
            Util.ReadString(reader);
        }
        string strMD5Code = Util.ReadString(reader);      //读取Table类MD5码
        int[] fieldIndex = new int[iColums];
        int[] fieldArray = new int[iColums];
        string[] nameArray = new string[iColums];
        Dictionary<string, List<int>> typeList = new Dictionary<string, List<int>>();
        for (int i = 0; i < iColums; ++i)
        {
            fieldIndex[i] = reader.ReadInt32();     //取出字段类型
            fieldArray[i] = reader.ReadInt32();     //取出字段是否是数组

            if (fieldIndex[i] == (int)ElementType.CLASS)
            {
                nameArray[i] = Util.ReadString(reader);       //取出
                if (!typeList.ContainsKey(nameArray[i]))
                    typeList[nameArray[i]] = new List<int>();
                typeList[nameArray[i]].Clear();
                int nCount = reader.ReadInt32();
                for (int k = 0; k < nCount; ++k)
                {
                    typeList[nameArray[i]].Add(reader.ReadInt32());
                }
            }
        }
        FileUtil.CreateFile(string.Format("{0}.conversion", fileTitle), Util.GetRollbackClassData(typeList));
        string strStream = "";
        for (int i = 0; i < iColums; ++i)
        {
            Element element = Util.GetElement((ElementType)fieldIndex[i]);
            if (element.Type != ElementType.CLASS)
                strStream += string.Format("{0}{1}", fieldArray[i] != 0 ? "array" : "", element.GetVariable(PROGRAM.CS));
            else
                strStream += string.Format("{0}{1}", fieldArray[i] != 0 ? "array" : "", nameArray[i]);
            strStream += "\t";
        }
        strStream += Util.ReturnString;
        sw.Write(strStream);
        for (int i = 0; i < iRows; ++i)
        {
            Progress.Value = ((float)(i + 1) / (float)(iRows) * 100f);
            string str = "";
            for (int j = 0; j < iColums; ++j)
            {
                Element element = Util.GetElement((ElementType)fieldIndex[j]);
                if (element.Type != ElementType.CLASS)
                {
                    str += element.ReadValueByType(reader, fieldArray[j] != 0);
                }
                else
                {
                    str += element.ReadValueByType(reader, typeList[nameArray[j]], fieldArray[j] != 0);
                }
                str += "\t";
            }
            str += "\r\n";
            sw.Write(str);
        }
        sw.Flush();
        sw.Close();
        fs.Close();
    }
}

