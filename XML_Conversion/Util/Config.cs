using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
/// <summary>
/// 单个模块数据
/// </summary>
public class SectionValue
{
    /// <summary>
    /// 所有数据
    /// </summary>
    public Dictionary<string, ConfigValue> datas = new Dictionary<string, ConfigValue>();
    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="comment"></param>
    public void Set(string key, string value, string comment)
    {
        if (!datas.ContainsKey(key))
            datas.Add(key, new ConfigValue());
        datas[key].Set(value, comment);
    }
    /// <summary>
    /// 获得值
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public ConfigValue Get(string key)
    {
        if (datas.ContainsKey(key))
            return datas[key];
        return null;
    }
    /// <summary>
    /// 删除值
    /// </summary>
    /// <param name="key"></param>
    public void Remove(string key)
    {
        if (datas.ContainsKey(key))
        {
            datas.Remove(key);
        }
    }

}
/// <summary>
/// 具体数据
/// </summary>
public class ConfigValue
{
    /// <summary>
    /// 值
    /// </summary>
    public string value;
    /// <summary>
    /// 注释
    /// </summary>
    public string comment;
    /// <summary>
    /// 设置值
    /// </summary>
    /// <param name="value"></param>
    /// <param name="comment"></param>
    public void Set(string value,string comment)
    {
        if (value != null)
            this.value = value;
        if (comment != null)
            this.comment = comment;
    }
}
/// <summary>
/// 读取ini文件
/// </summary>
public class Config
{
    /// <summary>
    /// 文件名称
    /// </summary>
    private string m_FileName;
    /// <summary>
    /// 所有数据
    /// </summary>
    public Dictionary<string, SectionValue> m_ConfigData = new Dictionary<string, SectionValue>();
    /// <summary>
    /// 构造函数
    /// </summary>
    public Config() { }
    /// <summary>
    /// 构造函数
    /// </summary>
    public Config(byte[] bytes)
    {
        InitializeFromBuffer(bytes);
    }
    /// <summary>
    /// 构造函数
    /// </summary>
    public Config(string data,bool file)
    {
        if (file)
            InitializeFromFile(data);
        else
            InitializeFromString(data);
    }
    /// <summary>
    /// 根据文件初始化数据
    /// </summary>
    /// <param name="fileName"></param>
    public void InitializeFromFile(string fileName)
    {
        m_FileName = fileName;
        if (File.Exists(m_FileName) == false)
        {
            if (File.Create(m_FileName) == null)
                throw new Exception("create file is fail : " + m_FileName);
        }
        InitializeFromString(FileUtil.GetFileString(m_FileName));
    }
    /// <summary>
    /// 根据BYTE[]初始化数据
    /// </summary>
    public void InitializeFromBuffer(byte[] buffer)
    {
        InitializeFromString(CharsetUtil.defaultEncoding.GetString(buffer));
    }
    /// <summary>
    /// 根据string初始化数据
    /// </summary>
    public void InitializeFromString(string buffer)
    {
        try
        {
            m_ConfigData.Clear();
            string[] datas = buffer.Split('\n');
            string section = "";
            bool startComment = false;
            StringBuilder comment = new StringBuilder();
            int count = datas.Length;
            for (int i = 0;i < count; ++i)
            {
                string data = datas[i].Trim();
                if (!string.IsNullOrEmpty(data))
                {
                    // /* */为区域注释
                    if (data.StartsWith("/*"))
                    {
                        comment.Append(data.Replace(@"/*", "").Replace(@"*/", ""));
                        if (data.EndsWith("*/"))
                        {
                            startComment = false;
                            continue;
                        }
                        startComment = true;
                        continue;
                    }
                    // */ 为区域注释结尾
                    else if (data.EndsWith("*/"))
                    {
                        comment.Append(data.Replace(@"/*", "").Replace(@"*/", ""));
                        startComment = false;
                        continue;
                    }
                    // //为行注释
                    else if (data.StartsWith("//"))
                    {
                        comment.Append(data.Replace(@"//", ""));
                        continue;
                    }
                    if (startComment == true)
                    {
                        continue;
                    }
                    if (data.StartsWith("["))
                    {
                        int indexLeft = data.IndexOf("[");
                        int indexRight = data.IndexOf("]");
                        if (indexLeft >= 0 && indexRight >= 0)
                        {
                            section = data.Substring(indexLeft + 1, indexRight - indexLeft - 1);
                        }
                    }
                    else
                    {
                        int index = data.IndexOf("=");
                        if (index >= 0)
                        {
                            string key = data.Substring(0, index).Trim();
                            string value = data.Substring(index + 1).Trim();
                            Set(section, key, value, comment.ToString());
                            comment = new StringBuilder();
                        }
                        else
                        {
                            Logger.error((i + 1) + " 行填写错误");
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Logger.error("Config initialize is error : {0}", ex.ToString());
        }
    }
    /// <summary>
    /// 返回所有数据
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, SectionValue> GetData()
    {
        return m_ConfigData;
    }
    /// <summary>
    /// 返回单个模块的数据
    /// </summary>
    /// <returns></returns>
    public SectionValue GetSection()
    {
        return GetSection("");
    }
    /// <summary>
    /// 返回单个模块的数据
    /// </summary>
    /// <returns></returns>
    public SectionValue GetSection(string section)
    {
        if (!m_ConfigData.ContainsKey(section))
            return null;
        return m_ConfigData[section];
    }
    /// <summary>
    /// 获得Value
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string Get(string key)
    {
        return Get("", key);
    }
    /// <summary>
    /// 设置Value
    /// </summary>
    /// <param name="section"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public string Get(string section,string key)
    {
        if (m_ConfigData.Count <= 0)
            return "";
        if (section == null) section = "";
        if (!m_ConfigData.ContainsKey(section))
            return "";
        var configValue = m_ConfigData[section].Get(key);
        if (configValue != null)
            return configValue.value;
        return "";
    }
    /// <summary>
    /// 设置Value
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Set(string key, string value)
    {
        Set("", key, value);
    }
    /// <summary>
    /// 设置Value
    /// </summary>
    /// <param name="section"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Set(string section, string key, string value)
    {
        Set(section, key, value, null);
    }
    /// <summary>
    /// 设置Value
    /// </summary>
    /// <param name="section"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="comment"></param>
    public void Set(string section, string key, string value, string comment)
    {
        if (!m_ConfigData.ContainsKey(section))
            m_ConfigData.Add(section, new SectionValue());
        m_ConfigData[section].Set(key, value, comment);
    }
    /// <summary>
    /// 删除Key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public void Remove(string key)
    {
        Remove("", key);
    }
    /// <summary>
    /// 删除Key
    /// </summary>
    /// <param name="section"></param>
    /// <param name="key"></param>
    public void Remove(string section, string key)
    {
        if (m_ConfigData.ContainsKey(section))
            m_ConfigData[section].Remove(key);
    }
    /// <summary>
    /// 清空一个模块
    /// </summary>
    public void ClearSection()
    {
        ClearSection("");
    }
    /// <summary>
    /// 清空一个模块
    /// </summary>
    /// <param name="section"></param>
    public void ClearSection(string section)
    {
        if (m_ConfigData.ContainsKey(section))
            m_ConfigData.Remove(section);
    }
    /// <summary>
    /// 清空所有数据
    /// </summary>
    public void ClearData()
    {
        m_ConfigData.Clear();
    }
    /// <summary>
    /// 返回数据字符串
    /// </summary>
    /// <returns></returns>
    string GetString()
    {
        SortedDictionary<string, SectionValue> data = new SortedDictionary<string, SectionValue>(m_ConfigData);
        StringBuilder builder = new StringBuilder();
        if (data.ContainsKey(""))
        {
            foreach (var val in data[""].datas)
            {
                var configValue = val.Value;
                if (!string.IsNullOrEmpty(configValue.comment))
                    builder.AppendLine(string.Format("/*{0}*/", configValue.comment));
                builder.AppendLine(string.Format("{0}={1}", val.Key, configValue.value));
            }
        }
        foreach (var pair in data)
        {
            if (string.IsNullOrEmpty(pair.Key))
                continue;
            builder.AppendLine(string.Format("[{0}]", pair.Key));
            foreach (var val in pair.Value.datas)
            {
                var configValue = val.Value;
                if (!string.IsNullOrEmpty(configValue.comment))
                    builder.AppendLine(string.Format("/*{0}*/", configValue.comment));
                builder.AppendLine(string.Format("{0}={1}", val.Key, configValue.value));
            }
        }
        return builder.ToString();
    }
    /// <summary>
    /// 保存到读取时候的文件
    /// </summary>
    /// <param name="bom"></param>
    public void Save(bool bom)
    {
        if (!string.IsNullOrEmpty(m_FileName))
            Save(m_FileName, bom);
    }
    /// <summary>
    /// 保存到文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="bom"></param>
    public void Save(string fileName, bool bom)
    {
        FileUtil.CreateFile(fileName, GetString(), bom);
    }
}
