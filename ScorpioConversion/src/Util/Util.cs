using System;
using System.Collections.Generic;
using System.Text;

namespace ScorpioConversion {
    public static class Util {
        public static void CreateDataClass(Language language, string packageName, string className, List<FieldClass> fields, string path) {
            var generate = Activator.CreateInstance(Type.GetType($"GenerateData{language}")) as IGenerate;
            generate.PackageName = packageName;
            generate.ClassName = className;
            generate.Language = language;
            generate.Package = new PackageClass() { Fields = fields };
            generate.Parameter = true;
            var fileName = $"{className}.{language.GetInfo().extension}";
            if (language == Language.Java) {
                fileName = string.Join("/", packageName.Split(".")) + "/" + fileName;
            }
            Scorpio.Commons.FileUtil.CreateFile(fileName, generate.Generate(), path.Split(","));
        }
        public static void CreateTableClass(Language language, string packageName, string tableClassName, string dataClassName, string md5, List<FieldClass> fields, string path) {
            var generate = Activator.CreateInstance(Type.GetType($"GenerateTable{language}")) as IGenerate;
            generate.PackageName = packageName;
            generate.ClassName = tableClassName;
            generate.Language = language;
            generate.Package = new PackageClass() { Fields = fields };
            var str = generate.Generate();
            str = str.Replace("__KeyType", fields[0].GetLanguageType(language));
            str = str.Replace("__KeyName", fields[0].Name);
            str = str.Replace("__TableName", tableClassName);
            str = str.Replace("__DataName", dataClassName);
            str = str.Replace("__MD5", md5);
            var fileName = $"{tableClassName}.{language.GetInfo().extension}";
            if (language == Language.Java) {
                fileName = string.Join("/", packageName.Split(".")) + "/" + fileName;
            }
            Scorpio.Commons.FileUtil.CreateFile(fileName, str, path.Split(","));
        }
        public static void CreateEnumClass(Language language, string packageName, PackageEnum enums, string path) {
            var generate = Activator.CreateInstance(Type.GetType($"GenerateEnum{language}")) as IGenerate;
            generate.PackageName = packageName;
            generate.Language = language;
            generate.ClassName = enums.Name;
            generate.Package = enums;
            generate.Parameter = true;
            var fileName = $"{enums.Name}.{language.GetInfo().extension}";
            if (language == Language.Java) {
                fileName = string.Join("/", packageName.Split(".")) + "/" + fileName;
            }
            Scorpio.Commons.FileUtil.CreateFile(fileName, generate.Generate(), path.Split(","));
        }
    }
}
