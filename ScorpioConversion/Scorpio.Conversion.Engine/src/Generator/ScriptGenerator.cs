namespace Scorpio.Conversion.Engine {
    public class ScriptGenerator : IGenerator, ScriptBase {
        public ScriptValue Value { get; set; }
        public ScriptGenerator(ScriptValue value, object[] args) {
            Value = value.call(ScriptValue.Null, args);
        }
        public override string GetDataPath(LanguageInfo languageInfo, string name) {
            if (this.Call("GetDataPath", out var ret, languageInfo, name)) {
                return ret.ToString();
            }
            return base.GetDataPath(languageInfo, name);
        }
        public override string GetCodePath(LanguageInfo languageInfo, string name) {
            if (this.Call("GetCodePath", out var ret, languageInfo, name)) {
                return ret.ToString();
            }
            return base.GetDataPath(languageInfo, name);
        }
        public override string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string fileMD5, PackageClass packageClass) {
            return this.Call("GenerateTableClass", packageName, tableClassName, dataClassName, fileMD5, packageClass).ToString();
        }
        public override string GenerateDataClass(string packageName, string className, PackageClass packageClass, bool createID = false) {
            return this.Call("GenerateDataClass", packageName, className, packageClass, createID).ToString();
        }
        public override string GenerateEnumClass(string packageName, string className, PackageEnum packageEnum) {
            return this.Call("GenerateEnumClass", packageName, className, packageEnum).ToString();
        }
    }
}