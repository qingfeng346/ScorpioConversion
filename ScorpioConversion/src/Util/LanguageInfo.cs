using Scorpio.Commons;
using System;
using System.IO;
using System.Collections.Generic;
public class LanguageInfo {
    public string language;         //语言名字
    public string dataOutput;       //数据导出目录
    public string fileOutput;       //代码导出目录
    public string dataSuffix;       //数据文件后缀名
    public string fileSuffix;       //代码文件后缀名
    public string package;          //命名空间
}
public class LanguageConfig {
    public string dataSuffix = "data";      //默认数据文件后缀名
    public string fileSuffix = "file";      //默认代码文件后缀名
    public string package = "";             //默认命名空间
    public List<LanguageInfo> languages = new List<LanguageInfo>();
    public void Generate(TableBuilder tableBuilder, byte[] buffer) {
        var dataFileName = tableBuilder.FileName;
        if (tableBuilder.IsSpawn) {
            dataFileName = $"{tableBuilder.Spawn}_{dataFileName}";
        }
        foreach (var language in languages) {
            if (string.IsNullOrWhiteSpace(language.dataSuffix)) {
                language.dataSuffix = dataSuffix;
            }
            if (string.IsNullOrWhiteSpace(language.fileSuffix)) {
                language.fileSuffix = fileSuffix;
            }
            if (string.IsNullOrWhiteSpace(language.package)) {
                language.package = package;
            }
            FileUtil.CreateFile(Path.Combine(ScorpioUtil.CurrentDirectory, language.dataOutput, $"{dataFileName}.{language.dataSuffix}"), buffer);
            //GeneratorManager.Instance.Get(language.language).Generate();
        }
    }
}