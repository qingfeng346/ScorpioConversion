using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
/// <summary> File工具类 </summary>
public static class FileUtil
{
    /// <summary> 字符串头一个字母大写 </summary>
    public static string ToOneUpper(string str)
    {
        if (string.IsNullOrEmpty(str)) 
            return str;
        if (str.Length <= 1)
            return str.ToUpper();
        return char.ToUpper(str[0]) + str.Substring(1);
    }
    /// <summary> 格式化路径\换成/ </summary>
    public static string FormatPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return "";
        string ret = path.Replace('\\', '/');
        return ret;
    }
    /// <summary> 创建一个目录 </summary>
    public static void CreateDirectory(string path)
    {
        try {
            if (!PathExist(path))
                Directory.CreateDirectory(path);
        } catch (System.Exception ex) {
            Logger.error("CreateDirectory is error : {0}", ex.ToString());
        }
    }
    /// <summary> 创建一个目录 </summary>
    public static void CreateDirectoryByFile(string file)
    {
        string path = Path.GetDirectoryName(file);
        CreateDirectory(path);
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
    /// <summary>
	/// 根据字符串创建一个文件
	/// </summary>
	public static void CreateFile(string fileName, string buffer)
	{
		CreateFile(fileName,buffer,false);
	}
    /// <summary>
    /// 根据字符串创建一个文件
    /// </summary>
    public static void CreateFile(string fileName, string buffer, bool bom)
    {
        try {
            CreateFile(fileName, Encoding.UTF8.GetBytes(buffer), bom);
        } catch (System.Exception ex) {
            Logger.error("CreateFile is error : {0}", ex.ToString());
        }
    }
    /// <summary>
    /// 根据byte[]创建文件
    /// </summary>
    public static void CreateFile(string fileName, byte[] buffer, bool bom, string[] filePath)
    {
        if (filePath == null || filePath.Length < 0) return;
        for (int i = 0; i < filePath.Length; ++i) {
            if (!string.IsNullOrEmpty(filePath[i])) {
                CreateFile(filePath[i] + "/" + fileName, buffer, bom);
            }
        }
    }
    /// <summary>
    /// 根据byte[]创建一个文件
    /// </summary>
	public static void CreateFile(string fileName, byte[] buffer)
	{
		CreateFile(fileName,buffer,false);
	}
    /// <summary>
    /// 根据byte[]创建一个文件
    /// </summary>
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
    /// <summary>
    /// 获得文件字符串
    /// </summary>
    public static String GetFileString(string fileName)
    {
        return Encoding.UTF8.GetString(GetFileBuffer(fileName));
    }
    /// <summary>
    /// 获得文件byte[]
    /// </summary>
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
    /// <summary>
    /// 获得路径下所有文件和文件夹
    /// </summary>
    /// <param name="strDirectory">指定路径</param>
    /// <param name="strFilePattern">要与strDir 中的文件名匹配的搜索字符串</param>
    /// <param name="directorys">查询得到的所有目录ArrayList</param>
    /// <param name="files">查询得到的所有文件名称ArrayList</param>
    /// <param name="recursive">是否递归查询</param>
    public static void GetFileList(string strDirectory, string strFilePattern, List<string> directorys, List<string> files, bool recursive)
    {
        try {
            if (!PathExist(strDirectory)) return;
            // 取得指定路径下所有符合条件的文件
            string[] strFiles = Directory.GetFiles(strDirectory, strFilePattern);
            // 取得指定路径下所有目录
            string[] strDirectorys = Directory.GetDirectories(strDirectory);
            // 将所有文件名称加入结果ArrayList中
            foreach (string name in strFiles)
                files.Add(name);

            // 将所有目录加入结果ArrayList中
            foreach (string name in strDirectorys)
                directorys.Add(name);

            // 递归
            if (recursive) {
                if (strDirectorys.Length > 0) {
                    // 递归遍历所有文件夹
                    foreach (string directory in strDirectorys)
                        GetFileList(directory, strFilePattern, directorys, files, recursive);
                }
            }
        } catch (System.Exception ex) {
            Logger.error("GetFileList is error : {0}", ex.ToString());
        }
    }
    /// <summary> 删除文件夹 </summary>
    public static void DeleteFiles(string sourceFolder, string strFilePattern, bool recursive)
    {
        if (!PathExist(sourceFolder)) return;
        string[] files = Directory.GetFiles(sourceFolder, strFilePattern);
        foreach (string file in files)
        {
            File.Delete(file);
        }
        if (recursive)
        {
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
                DeleteFiles(folder, strFilePattern, recursive);
        }
        Directory.Delete(sourceFolder);
    }
    /// <summary> 删除文件夹 </summary>
    public static void DeleteFiles(string sourceFolder, string strFilePattern, bool recursive, List<string> detach)
    {
        DeleteFiles_impl(sourceFolder, sourceFolder, strFilePattern, recursive, detach);
    }
    static bool DeleteFiles_impl(string parent, string sourceFolder, string strFilePattern, bool recursive, List<string> detach)
    {
        if (!PathExist(sourceFolder)) 
            return false;
        bool detachFile = false;
        string[] files = Directory.GetFiles(sourceFolder, strFilePattern);
        foreach (string file in files) {
            string formatFile = FormatPath(file).Replace(parent, "");
            if (detach != null && detach.Contains(formatFile)) {
                detachFile = true;
            } else {
                File.Delete(file);
            }  
        }
        if (recursive)            {
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders) {
                if (DeleteFiles_impl(parent, folder, strFilePattern, recursive, detach)) {
                    detachFile = true;
                }
            }
        }
        if (!detachFile) {
            Directory.Delete(sourceFolder);
        }
        return detachFile;
    }
    /// <summary> 删除文件 </summary>
    public static void DeleteFile(string fileName)
    {
        if (FileExist(fileName))
            File.Delete(fileName);
    }
    /// <summary> 删除文件 </summary>
    public static void DeleteFile(string fileName, string[] filePath)
    {
        if (filePath == null || filePath.Length < 0) return;
        for (int i = 0; i < filePath.Length; ++i)
        {
            if (!string.IsNullOrEmpty(filePath[i]))
            {
                DeleteFile(filePath[i] + "/" + fileName);
            }
        }
    }
    /// <summary> 复制文件 </summary>
    public static void CopyFile(string sourceFile, string destFile, bool overwrite)
    {
        if (FileExist(sourceFile))
        {
            CreateDirectoryByFile(destFile);
            File.Copy(sourceFile, destFile, overwrite);
        }
    }
    /// <summary> 移动文件 </summary>
    public static void MoveFile(string sourceFile, string destFile, bool overwrite)
    {
        if (FileExist(sourceFile))
        {
            CreateDirectoryByFile(destFile);
            if (overwrite && FileExist(destFile))
                DeleteFile(destFile);
            File.Move(sourceFile, destFile);
        }
    }
    /// <summary>
    /// 拷贝文件夹
    /// </summary>
    /// <param name="sourceFolder">源路径</param>
    /// <param name="destFolder">目标路径</param>
    /// <param name="strFilePattern">文件名匹配的搜索字符串</param>
    public static void CopyFolder(string sourceFolder, string destFolder, string strFilePattern)
    {
        if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);
        string[] files = Directory.GetFiles(sourceFolder, strFilePattern);
        foreach (string file in files)
        {
            string name = Path.GetFileName(file);
            string dest = Path.Combine(destFolder, name);
            File.Copy(file, dest, true);
        }
        string[] folders = Directory.GetDirectories(sourceFolder);
        foreach (string folder in folders)
        {
            string name = Path.GetFileName(folder);
            string dest = Path.Combine(destFolder, name);
            CopyFolder(folder, dest, strFilePattern);
        }
    }
    /// <summary>
    /// 获得一个文件的MD5码
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetMD5FromFile(string fileName)
    {
        return GetMD5FromBuffer(GetFileBuffer(fileName));
    }
    /// <summary>
    /// 获得一段字符串的MD5
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static string GetMD5FromString(string buffer)
    {
        return GetMD5FromBuffer(Encoding.UTF8.GetBytes(buffer));
    }
    /// <summary>
    /// 根据一段内存获得MD5码
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
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
