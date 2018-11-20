using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
//using ICSharpCode.SharpZipLib.GZip;
/// <summary> GZIP 工具类 </summary>
public static class GZipUtil
{
    /// <summary> 压缩数据</summary>
    public static byte[] Compress(byte[] source)
    {
        using (MemoryStream stream = new MemoryStream()) {
            //GZipOutputStream zipStream = new GZipOutputStream(stream);
            //zipStream.Write(source, 0, source.Length);
            //zipStream.Finish();
            //byte[] ret = stream.ToArray();
            //zipStream.Close();
            //stream.Close();
            //return ret;
            return null;
        }
    }
    /// <summary> 压缩数据 </summary>
    public static byte[] Compress(Stream source)
    {
        long length = source.Length;
        byte[] buffer = new byte[length];
        source.Read(buffer, 0, (int)length);
        source.Close();
        return Compress(buffer);
    }
    /// <summary> 解压数据 </summary>
    public static byte[] Decompress(byte[] source)
    {
        return Decompress(new MemoryStream(source));
    }
    /// <summary> 解压数据 </summary>
    public static byte[] Decompress(Stream source)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            //GZipInputStream zipStream = new GZipInputStream(source);
            //int count = 0;
            //byte[] data = new byte[4096];
            //while ((count = zipStream.Read(data, 0, data.Length)) != 0)
            //{
            //    stream.Write(data, 0, count);
            //}
            //zipStream.Flush();
            //byte[] ret = stream.ToArray();
            //zipStream.Close();
            //stream.Close();
            //return ret;
            return null;
        }
    }
}
