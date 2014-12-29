using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
/// <summary> File工具类 </summary>
public static class FileUtil
{
    /// <summary> 创建一个目录 </summary>
    public static void CreateDirectory(string path)
    {
        try {
            if (!PathExist(path)) Directory.CreateDirectory(path);
        } catch (System.Exception ex) {
            Logger.error("CreateDirectory is error : {0}", ex.ToString());
        }
    }
    /// <summary> 判断文件是否存在 </summary>
    public static bool FileExist(String file)
    {
        return !string.IsNullOrEmpty(file) && File.Exists(file);
    }
    /// <summary> 判断文件夹是否存在 </summary>
    public static bool PathExist(String path)
    {
        return !string.IsNullOrEmpty(path) && Directory.Exists(path);
    }
    /// <summary> 根据字符串创建文件 </summary>
    public static void CreateFile(string fileName, string buffer, bool bom, string[] filePath)
    {
        if (filePath == null || filePath.Length < 0) return;
        for (int i = 0; i < filePath.Length; ++i)
        {
            if (!string.IsNullOrEmpty(filePath[i]))
            {
                CreateFile(filePath[i] + "/" + fileName, buffer, bom);
            }
        }
    }
    /// <summary> 根据字符串创建一个文件 </summary>
	public static void CreateFile(string fileName, string buffer)
	{
		CreateFile(fileName,buffer,false);
	}
    /// <summary> 根据字符串创建一个文件 </summary>
    public static void CreateFile(string fileName, string buffer, bool bom)
    {
        try {
            CreateFile(fileName, Encoding.UTF8.GetBytes(buffer), bom);
        } catch (System.Exception ex) {
            Logger.error("CreateFile is error : {0}", ex.ToString());
        }
    }
    /// <summary> 根据byte[]创建文件 </summary>
    public static void CreateFile(string fileName, byte[] buffer, bool bom, string[] filePath)
    {
        if (filePath == null || filePath.Length < 0) return;
        for (int i = 0; i < filePath.Length; ++i) {
            if (!string.IsNullOrEmpty(filePath[i])) {
                CreateFile(filePath[i] + "/" + fileName, buffer, bom);
            }
        }
    }
    /// <summary> 根据byte[]创建一个文件 </summary>
	public static void CreateFile(string fileName, byte[] buffer)
	{
		CreateFile(fileName,buffer,false);
	}
    /// <summary> 根据byte[]创建一个文件 </summary>
    public static void CreateFile(string fileName, byte[] buffer, bool bom)
    {
        try {
            if (string.IsNullOrEmpty(fileName)) return;
            string path = Path.GetDirectoryName(fileName);
            CreateDirectory(path);
            if (File.Exists(fileName)) File.Delete(fileName);
            FileStream fs = new FileStream(fileName, FileMode.Create);
            if (bom)
            {
                byte[] bomBuffer = new byte[] { 0xef, 0xbb, 0xbf };
                fs.Write(bomBuffer, 0, bomBuffer.Length);
            }
            fs.Write(buffer, 0, buffer.Length);
            fs.Flush();
            fs.Close();
        } catch (System.Exception ex) {
            Logger.error("CreateFile is error : {0}", ex.ToString());
        }
    }
    /// <summary> 删除文件 </summary>
    public static void DeleteFile(string fileName)
    {
        if (FileExist(fileName)) File.Delete(fileName);
    }
    /// <summary> 获得文件字符串 </summary>
    public static String GetFileString(string fileName)
    {
        return Encoding.UTF8.GetString(GetFileBuffer(fileName));
    }
    /// <summary> 获得文件byte[] </summary>
    public static byte[] GetFileBuffer(string fileName)
    {
        if (!FileExist(fileName)) return null;
        FileStream fs = new FileStream(fileName, FileMode.Open);
        long length = fs.Length;
        byte[] buffer = new byte[length];
        fs.Read(buffer, 0, (int)length);
        fs.Close();
        return buffer;
    }
    /// <summary> 获得一个文件的MD5码 </summary>
    public static string GetMD5FromFile(string fileName)
    {
        return GetMD5FromBuffer(GetFileBuffer(fileName));
    }
    /// <summary> 获得一段字符串的MD5 </summary>
    public static string GetMD5FromString(string buffer)
    {
        return GetMD5FromBuffer(Encoding.UTF8.GetBytes(buffer));
    }
    /// <summary> 根据一段内存获得MD5码 </summary>
    public static string GetMD5FromBuffer(byte[] buffer)
    {
        if (buffer == null) return null;
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] retVal = md5.ComputeHash(buffer);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < retVal.Length; i++)
            sb.Append(retVal[i].ToString("x2"));
        return sb.ToString();
    }
}
