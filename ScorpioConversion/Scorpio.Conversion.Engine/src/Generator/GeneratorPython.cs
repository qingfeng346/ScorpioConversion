using System.Text;
namespace Scorpio.Conversion.Engine {
    [AutoGenerator("python")]
    public class GeneratorPython : IGenerator {
        public const string Head = @"#本文件为自动生成，请不要手动修改
";
        public override string GenerateTableClass(string packageName, string tableClassName, string dataClassName, string fileMD5, PackageClass packageClass) {
            return $@"{Head}
from {dataClassName} import *
class {tableClassName}:
    def __init__(this):
        this.m_count = 0
        this.m_dataArray = {{}}

    def Initialize(this, fileName, reader):
        row = reader.ReadInt32()
        if ""{fileMD5}"" != reader.ReadString():
            raise Exception(""File schemas do not match [{tableClassName}] : "" + fileName)
        reader.ReadHead()
        for i in range(0, row):
            pData = {dataClassName}(fileName, reader)
            if this.m_dataArray.__contains__(pData.ID):
                this.m_dataArray[pData.ID].Set(pData)
            else:
                this.m_dataArray[pData.ID] = pData
        this.m_count = len(this.m_dataArray)
        return this

    def GetValue(this, ID):
        return this.m_dataArray[ID]

    def Contains(this, ID):
        return this.m_dataArray.__contains__(ID)

    def Datas(this):
        return this.m_dataArray

    def GetValueObject(this, ID):
        return this.GetValue(ID)

    def ContainsObject(this, ID):
        return this.Contains(ID)

    def Count(this):
        return this.m_count
";
        }
        public override string GenerateDataClass(string packageName, string className, PackageClass packageClass, bool createID) {
            return $@"{Head}
{ImportList(packageClass, className, createID)}
class {className}:
    {FunctionConstructor(packageClass, className, createID)}
    {FunctionGetData(packageClass)}
    {FunctionSet(packageClass, className)}
    {FunctionToString(packageClass)}
";
        }
        string ImportList(PackageClass packageClass, string dataClassName, bool createID) {
            var builder = new StringBuilder();
            foreach (var field in packageClass.Fields) {
                if (field.IsClass) {
                    builder.Append($@"
from {field.Type} import *;
");
                }
            }
            return builder.ToString();
        }
        string FunctionConstructor(PackageClass packageClass, string dataClassName, bool createID) {
            var first = true;
            var builder = new StringBuilder();
            builder.Append($@"
    def __init__(this, fileName, reader):");
            foreach (var field in packageClass.Fields) {
                string fieldRead;
                if (field.Attribute != null && field.Attribute.GetValue("Language").IsTrue) {
                    fieldRead = $@"reader.ReadL10n(fileName + "".{field.Name}."" + this.ID())";
                } else if (field.IsBasic) {
                    fieldRead = $"reader.Read{field.BasicType.Name}()";
                } else if (field.IsEnum) {
                    fieldRead = $"reader.ReadInt32()";
                } else {
                    fieldRead = $"{field.Type}(fileName, reader)";
                }
                if (field.IsArray) {
                    builder.Append($@"
        list = []
        number = reader.ReadInt32()
        for i in range(0, number):
            list.append({fieldRead})

        this.{field.Name} = list");
                } else {
                    builder.Append($@"
        this.{field.Name} = {fieldRead}");
                }
                if (first) {
                    first = false;
                    if (createID && field.Name != "ID") {
                        builder.Append($@"
        this.ID = this.{field.Name}");
                    }
                }
            }
            return builder.ToString();
        }
        string FunctionGetData(PackageClass packageClass) {
            var builder = new StringBuilder();
            builder.Append(@"
    def GetData(this, key):");
            foreach (var field in packageClass.Fields) {
                builder.Append($@"
        if ""{field.Name}"" == key:
            return this.{field.Name}");
            }
            builder.Append(@"
        return None
");
            return builder.ToString();
        }
        string FunctionSet(PackageClass packageClass, string dataClassName) {
            var builder = new StringBuilder();
            builder.Append($@"
    def Set(this, value):");
            foreach (var field in packageClass.Fields) {
                builder.Append($@"
        this.{field.Name} = value.{field.Name}");
            }
            return builder.ToString();
        }
        string FunctionToString(PackageClass packageClass) {
            var builder = new StringBuilder();
            builder.Append(@"
    def __str__(this):
        return ");
            var first = true;
            foreach (var field in packageClass.Fields) {
                if (first == false) {
                    builder.Append(" + \",\" + ");
                }
                first = false;
                builder.Append($"\"{field.Name}:\" + str(this.{field.Name})");
            }
            builder.Append(@"
");
            return builder.ToString();
        }
        public override string GenerateEnumClass(string packageName, string className, PackageEnum packageEnum) {
            var builder = new StringBuilder();
            builder.Append($@"{Head}
class {className}:");
            foreach (var info in packageEnum.Fields) {
                builder.Append($@"
    {info.Name} = {info.Index}");
            }
            builder.Append(@"
");
            return builder.ToString();
        }
    }
}