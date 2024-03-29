﻿using Scorpio.Commons;
using System.Collections.Generic;
namespace Scorpio.Conversion.Engine {
    public class LanguageInfo {
        public string codeOutput { get; set; }                      //代码导出目录
        public string codeSuffix { get; set; }                      //代码文件后缀名
        public string dataOutput { get; set; }                      //数据导出目录
        public string dataSuffix { get; set; }                      //数据文件后缀名
        public string package { get; set; }                         //命名空间
        public string writer { get; set; }                          //写入流
        public string generator { get; set; }                       //生成器
        public List<string> handler { get; set; } = new List<string>();      //后续处理
    }
    public class BuildInfo {
        public string codeSuffix { get; set; } = "code";            //默认代码文件后缀名
        public string codeOutput { get; set; } = "";                //默认代码导出目录
        public string dataSuffix { get; set; } = "data";            //默认数据文件后缀名
        public string dataOutput { get; set; } = "";                //默认数据文件导出目录
        public string package { get; set; } = "scov";               //默认命名空间
        public string writer { get; set; } = "default";             //默认写入流
        public List<string> handler { get; set; } = new List<string>();                   //全局后续处理,只执行一次
        public List<LanguageInfo> languages { get; set; } = new List<LanguageInfo>();     //所有语言
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
                var generator = GeneratorManager.Instance.Get(languageInfo.generator);
                FileUtil.CreateFile(generator.GetDataPath(languageInfo, tableBuilder.FileName), tableBuilder.CreateBytes(languageInfo.writer));
                FileUtil.CreateFile(generator.GetCodePath(languageInfo, tableName), generator.GenerateTableClass(languageInfo.package, tableName, dataName, tableBuilder.LayoutMD5, packageClass));
                FileUtil.CreateFile(generator.GetCodePath(languageInfo, dataName), generator.GenerateDataClass(languageInfo.package, dataName, packageClass, true));
            }
        }
        public void GenerateCustom(PackageParser parser) {
            foreach (var language in languages) {
                var languageInfo = GetLanguageInfo(language);
                var generator = GeneratorManager.Instance.Get(language.generator);
                foreach (var pair in parser.Classes) {
                    FileUtil.CreateFile(generator.GetCodePath(languageInfo, pair.Value.Name), generator.GenerateDataClass(languageInfo.package, pair.Value.Name, pair.Value));
                }
                foreach (var pair in parser.Enums) {
                    FileUtil.CreateFile(generator.GetCodePath(languageInfo, pair.Value.Name), generator.GenerateEnumClass(languageInfo.package, pair.Value.Name, pair.Value));
                }
            }
        }
        public void Handle(List<TableBuilder> successTables, SortedDictionary<string, List<TableBuilder>> successSpawns, CommandLine command) {
            handler.ForEach(h => {
                HandlerManager.Instance.Get(h).Handle(null, successTables, successSpawns, Config.L10NDatas, command);
            });
            foreach (var language in languages) {
                var languageInfo = GetLanguageInfo(language);
                languageInfo.handler.ForEach(h => {
                    HandlerManager.Instance.Get(h).Handle(languageInfo, successTables, successSpawns, Config.L10NDatas, command);
                });
            }
        }
    }
}