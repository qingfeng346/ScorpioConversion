using Scorpio.Commons;
using System.Collections.Generic;
namespace Scorpio.Conversion {
    public class LanguageInfo {
        public string language;         //语言名字
        public string codeOutput;       //代码导出目录
        public string codeSuffix;       //代码文件后缀名
        public string dataOutput;       //数据导出目录
        public string dataSuffix;       //数据文件后缀名
        public string package;          //命名空间
        public string writer;           //写入流
    }
    public class BuildInfo {
        public string codeSuffix = "code";      //默认代码文件后缀名
        public string codeOutput = "";          //默认代码导出目录
        public string dataSuffix = "data";      //默认数据文件后缀名
        public string dataOutput = "";          //默认数据文件导出目录
        public string package = "scov";         //默认命名空间
        public string writer = "default";       //默认写入流
        public List<LanguageInfo> languages = new List<LanguageInfo>();
        private LanguageInfo GetLanguageInfo(LanguageInfo languageInfo) {
            if (string.IsNullOrWhiteSpace(languageInfo.codeOutput)) {
                languageInfo.codeOutput = codeOutput;
            }
            if (string.IsNullOrWhiteSpace(languageInfo.codeSuffix)) {
                languageInfo.codeSuffix = codeSuffix;
            }
            if (string.IsNullOrWhiteSpace(languageInfo.dataOutput)) {
                languageInfo.dataOutput = dataOutput;
            }
            if (string.IsNullOrWhiteSpace(languageInfo.dataSuffix)) {
                languageInfo.dataSuffix = dataSuffix;
            }
            if (string.IsNullOrWhiteSpace(languageInfo.package)) {
                languageInfo.package = package;
            }
            if (string.IsNullOrWhiteSpace(languageInfo.writer)) {
                languageInfo.writer = writer;
            }
            return languageInfo;
        }
        public void Generate(TableBuilder tableBuilder) {
            var tableName = $"Table{tableBuilder.Name}";
            var dataName = $"Data{tableBuilder.Name}";
            var packageClass = tableBuilder.PackageClass;
            foreach (var language in languages) {
                var languageInfo = GetLanguageInfo(language);
                var generator = GeneratorManager.Instance.Get(languageInfo.language);
                FileUtil.CreateFile(generator.GetDataPath(languageInfo, tableBuilder.FileName), tableBuilder.CreateBytes(languageInfo.writer));
                FileUtil.CreateFile(generator.GetCodePath(languageInfo, tableName), generator.GenerateTableClass(languageInfo.package, tableName, dataName, tableBuilder.LayoutMD5, packageClass));
                FileUtil.CreateFile(generator.GetCodePath(languageInfo, dataName), generator.GenerateDataClass(languageInfo.package, dataName, packageClass, true));
            }
        }
        public void GenerateCustom(PackageParser parser) {
            foreach (var language in languages) {
                var languageInfo = GetLanguageInfo(language);
                var generator = GeneratorManager.Instance.Get(language.language);
                foreach (var pair in parser.Tables) {
                    FileUtil.CreateFile(generator.GetCodePath(languageInfo, pair.Value.Name), generator.GenerateDataClass(languageInfo.package, pair.Value.Name, pair.Value));
                }
                foreach (var pair in parser.Enums) {
                    FileUtil.CreateFile(generator.GetCodePath(languageInfo, pair.Value.Name), generator.GenerateEnumClass(languageInfo.package, pair.Value.Name, pair.Value));
                }
            }
        }
    }
}