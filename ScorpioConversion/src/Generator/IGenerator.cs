public abstract class IGenerator {
    public string Language { get; private set; }        //语言名字
    public IGenerator(string language) {
        Language = language;
    }
    public abstract string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string suffix, string fileMD5, PackageClass packageClass);
    public abstract string GenerateDataClass(string packageName, string tableClassName, string dataClassName, string suffix, string fileMD5, PackageClass packageClass);
}
