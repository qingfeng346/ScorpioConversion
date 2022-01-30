using Scorpio.Commons;
using System.IO;
using System;

namespace Scorpio.Conversion.Engine {
    public abstract class IGenerator : IDisposable {
        public virtual string GetDataPath(LanguageInfo languageInfo, string name) {
            return languageInfo.GetDataPath(name);
        }
        public virtual string GetCodePath(LanguageInfo languageInfo, string name) {
            return languageInfo.GetCodePath(name);
        }
        public abstract string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string fileMD5, PackageClass packageClass);
        public abstract string GenerateDataClass(string packageName, string className, PackageClass packageClass, bool createID = false);
        public abstract string GenerateEnumClass(string packageName, string className, PackageEnum packageEnum);
        public void Dispose() { }
    }
}