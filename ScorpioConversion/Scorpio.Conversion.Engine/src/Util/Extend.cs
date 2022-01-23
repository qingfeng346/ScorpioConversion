using System;
using System.Data;
using NPOI.SS.UserModel;
using Scorpio.Commons;
using System.IO;
using System.Collections.Generic;
namespace Scorpio.Conversion {
    public static class Extend {
        private readonly static DateTime BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private const char Separator = ';';
        private const string ArrayString = "array";
        public const bool INVALID_BOOL = false;
        public const sbyte INVALID_INT8 = 0;
        public const byte INVALID_UINT8 = 0;
        public const short INVALID_INT16 = 0;
        public const ushort INVALID_UINT16 = 0;
        public const int INVALID_INT32 = 0;
        public const uint INVALID_UINT32 = 0;
        public const long INVALID_INT64 = 0;
        public const ulong INVALID_UINT64 = 0;
        public const float INVALID_FLOAT = 0;
        public const double INVALID_DOUBLE = 0;
        public const string INVALID_STRING = "";
        public readonly static byte[] INVALID_BYTES = new byte[0];
        public const string BYTES_PROTO_BASE64 = "base64://";
        public const string BYTES_PROTO_FILE = "file://";
        public const string BYTES_PROTO_HTTP = "http://";
        public const string BYTES_PROTO_HTTPS = "https://";

        //枚举
        public class TableEnum {
            //枚举元素
            public class Element {
                public string name;
                public int value;
            }
            public List<Element> Elements = new List<Element>();
            public string Get(int value) {
                return Elements.Find(_ => _.value == value).name;
            }
        }
        //Class
        public class TableClass {
            public enum FieldType {
                BOOL,           //bool类型
                INT8,           //int8类型
                UINT8,          //uint8类型
                INT16,          //int16类型
                UINT16,         //uint16类型
                INT32,          //int32类型
                UINT32,         //uint32类型
                INT64,          //int64类型
                UINT64,         //uint64类型
                FLOAT,          //float类型
                DOUBLE,         //double类型
                STRING,         //string类型
                DATETIME,       //datetime日期时间
                BYTES,          //byte[]类型
                ENUM = 100,     //enum
                CLASS = 200,    //class
            }
            //变量
            public class Field {
                public bool array;
                public string name;
                public FieldType fieldType;
                public string type;
            }
            public List<Field> Fields = new List<Field>();
        }
        public static string GetMemory(this long by) => ScorpioUtil.GetMemory(by);
        public static string GetLineName(this int line) => ScorpioUtil.GetExcelColumn(line);
        public static bool IsEmptyString(this string str) => string.IsNullOrWhiteSpace(str);
        public static bool IsEmptyValue(this ValueList value) => value == null || value.values.Count == 0;
        public static bool IsInvalid(this string str) => string.IsNullOrWhiteSpace(str) || str.Trim().StartsWith("!");
        public static bool IsExcel(this string file) => !file.Contains("~$") && (file.EndsWith(".xls") || file.EndsWith(".xlsx") || file.EndsWith(".xlsb") || file.EndsWith(".csv"));
        public static bool IsL10N(this string str) => !str.IsEmptyString() && str.Trim().StartsWith("$");
        public static string ParseFlag(this string str, out bool invalid, out bool l10n) {
            invalid = false;
            l10n = false;
            if (string.IsNullOrWhiteSpace(str)) {
                return "";
            }
            var s = str;
            while (s.Length > 0) {
                var c = s[0];
                switch (c) {
                    case '!': invalid = true; break;
                    case '$': l10n = true; break;
                    default:
                        if (char.IsLetter(c)) {
                            return s;
                        } else {
                            throw new System.Exception("不能解析的Name : " + str);
                        }
                }
                s = s.Substring(1);
            }
            throw new System.Exception("不能解析的Name : " + str);
        }
        public static string Breviary(this string str, int length) {
            if (length <= 3 || str.Length <= 3 || str.Length <= length) { return str; }
            return str.Substring(0, length - 3) + "...";
        }

        public static bool StringArrayEqual(this string[] array1, string[] array2) {
            if (array1 == null && array2 == null) { return true; }
            if (array1 == null || array2 == null) { return false; }
            if (array1.Length != array2.Length) { return false; }
            for (var i = 0; i < array1.Length; ++i) {
                if (array1[i] != array2[i]) {
                    return false;
                }
            }
            return true;
        }
        public static bool IsArrayType(this string str) {
            return str.StartsWith(ArrayString);
        }
        public static string GetFinalType(this string str) {
            return str.Substring(ArrayString.Length);
        }
        public static bool ToBoolean(this string value) {
            if (value.IsEmptyString()) { return INVALID_BOOL; }
            switch (value.ToLower()) {
                case "1":
                case "true":
                case "yes":
                    return true;
                case "0":
                case "false":
                case "no":
                    return false;
                default:
                    throw new System.Exception("字符串不能转换为bool " + value);
            }
        }
        public static sbyte ToInt8(this string value) {
            return value.IsEmptyString() ? INVALID_INT8 : Convert.ToSByte(value);
        }
        public static byte ToUInt8(this string value) {
            return value.IsEmptyString() ? INVALID_UINT8 : Convert.ToByte(value);
        }
        public static short ToInt16(this string value) {
            return value.IsEmptyString() ? INVALID_INT16 : Convert.ToInt16(value);
        }
        public static ushort ToUInt16(this string value) {
            return value.IsEmptyString() ? INVALID_UINT16 : Convert.ToUInt16(value);
        }
        public static int ToInt32(this string value) {
            return value.IsEmptyString() ? INVALID_INT32 : Convert.ToInt32(value);
        }
        public static uint ToUInt32(this string value) {
            return value.IsEmptyString() ? INVALID_UINT32 : Convert.ToUInt32(value);
        }
        public static long ToInt64(this string value) {
            return value.IsEmptyString() ? INVALID_INT64 : Convert.ToInt64(value);
        }
        public static ulong ToUInt64(this string value) {
            return value.IsEmptyString() ? INVALID_UINT64 : Convert.ToUInt64(value);
        }
        public static float ToFloat(this string value) {
            return value.IsEmptyString() ? INVALID_FLOAT : Convert.ToSingle(value);
        }
        public static double ToDouble(this string value) {
            return value.IsEmptyString() ? INVALID_DOUBLE : Convert.ToDouble(value);
        }
        public static DateTime ToDateTime(this string value) {
            if (double.TryParse(value, out var span)) {
                return DateTime.FromOADate(span);
            }
            if (DateTime.TryParse(value, out var datetime)) {
                return datetime;
            }
            throw new System.Exception($"不能识别时间字符串 : {value}");
        }
        public static byte[] ToBytes(this string value) {
            if (value.IsEmptyString()) {
                return INVALID_BYTES;
            } else if (value.StartsWith(BYTES_PROTO_BASE64)) {
                return Convert.FromBase64String(value.Substring(BYTES_PROTO_BASE64.Length));
            } else if (value.StartsWith(BYTES_PROTO_FILE)) {
                var bytes = FileUtil.GetFileBuffer(value.Substring(BYTES_PROTO_FILE.Length));
                if (bytes == null) {
                    throw new System.Exception($"二进制数据文件不存在 : {value}");
                }
                return bytes;
            }
            throw new System.Exception($"未知的二进制数据类型 : {value}");
        }
        public static string GetCellString(this IRow row, int index) {
            return GetCellString(row, index, "");
        }
        public static string GetCellString(this IRow row, int index, string def) {
            return row.GetCell(index, MissingCellPolicy.CREATE_NULL_AS_BLANK).GetCellString(def);
        }
        public static string GetCellString(this ICell cell, string def) {
            if (cell == null) return def;
            if (cell.CellType == CellType.Numeric) {
                return cell.NumericCellValue.ToString();
            } else {
                cell.SetCellType(CellType.String);
                var value = cell.StringCellValue.Trim();
                return value.IsEmptyString() ? def : value;
            }
        }
        public static void SetCellString(this ICell cell, string value) {
            if (cell == null) return;
            cell.SetCellType(CellType.String);
            cell.SetCellValue(value != null ? value.Trim() : value);
        }
        public static string GetDataString(this object cell, string def = "") {
            if (cell == null || cell == DBNull.Value) { return def; }
            return cell.ToString();
        }
        //public static void Split(this string str, Action<string> action) {
        //    if (str.IsEmptyString()) { return; }
        //    Array.ForEach(str.Split(Separator), action);
        //}
        public static string[] SplitArray(this string str) {
            if (str.IsEmptyString()) { return new string[0]; }
            return str.Split(Separator);
        }
        public static DataTable AsDataSet(this ISheet sheet) {
            int maxColumn = 0;
            for (var i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
                var row = sheet.GetRow(i);
                if (row == null) { continue; }
                maxColumn = Math.Max(maxColumn, (int)row.LastCellNum);
            }
            var dataTable = new DataTable(sheet.SheetName);
            for (var i = 0; i <= sheet.LastRowNum; ++i) {
                dataTable.Rows.Add(dataTable.NewRow());
            }
            for (var i = 0; i <= maxColumn; ++i) {
                dataTable.Columns.Add();
            }
            for (var i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i) {
                var row = sheet.GetRow(i);
                if (row == null) { continue; }
                var dataRow = dataTable.Rows[i];
                for (var j = row.FirstCellNum; j < row.LastCellNum; ++j) {
                    dataRow[j] = row.GetCell(j).GetCellString("");
                }
            }
            return dataTable;
        }
        public static long GetTimeSpan(this DateTime time) {
            return Convert.ToInt64((time - BaseTime).TotalMilliseconds);
        }
        public static void RemoveSheet(this IWorkbook workbook, string name) {
            var index = workbook.GetSheetIndex(name);
            if (index >= 0) {
                workbook.RemoveSheetAt(index);
            }
        }
        public static string GetCodePath(this LanguageInfo languageInfo, string name) {
            return Path.Combine(ScorpioUtil.CurrentDirectory, languageInfo.codeOutput, $"{name}.{languageInfo.codeSuffix}");
        }
        public static string GetDataPath(this LanguageInfo languageInfo, string name) {
            return Path.Combine(ScorpioUtil.CurrentDirectory, languageInfo.dataOutput, $"{name}.{languageInfo.dataSuffix}");
        }
        public static void WriteHead(this IWriter writer, PackageClass packageClass, HashSet<IPackage> customTypes) {
            writer.WriteClass(packageClass.Fields);     //表结构
            writer.WriteInt32(customTypes.Count);       //自定义类数量
            foreach (var customType in customTypes) {
                if (customType is PackageEnum) {
                    writer.WriteString((customType as PackageEnum).Name);
                    writer.WriteInt8(1);
                    writer.WriteEnum((customType as PackageEnum).Fields);
                } else {
                    writer.WriteString((customType as PackageClass).Name);
                    writer.WriteInt8(2);
                    writer.WriteClass((customType as PackageClass).Fields);
                }
            }
        }
        static void WriteClass(this IWriter writer, List<ClassField> fields) {
            writer.WriteInt32(fields.Count);               //字段数量
            foreach (var field in fields) {
                if (field.IsBasic) {
                    writer.WriteInt8(0);
                    writer.WriteInt8((sbyte)field.BasicType.Index);
                } else {
                    writer.WriteInt8(field.IsEnum ? (sbyte)1 : (sbyte)2);
                    writer.WriteString(field.Type);
                }
                writer.WriteBool(field.IsArray);
                writer.WriteString((field.IsL10N ? "$" : "") + field.Name);
            }
        }
        static void WriteEnum(this IWriter writer, List<EnumField> fields) {
            writer.WriteInt32(fields.Count);
            foreach (var field in fields) {
                writer.WriteString(field.Name);
                writer.WriteInt32(field.Index);
            }
        }
        public static void ReadHead(this IReader reader, out TableClass tableClass, out Dictionary<string, TableEnum> customEnums, out Dictionary<string, TableClass> customClasses) {
            tableClass = ReadClass(reader);
            customEnums = new Dictionary<string, TableEnum>();
            customClasses = new Dictionary<string, TableClass>();
            var customNumber = reader.ReadInt32();
            for (var i = 0; i < customNumber; ++i) {
                var typeName = reader.ReadString();
                if (reader.ReadInt8() == 1) {
                    customEnums[typeName] = ReadEnum(reader);
                } else {
                    customClasses[typeName] = ReadClass(reader);
                }
            }
        }
        public static TableClass ReadClass(this IReader reader) {
            var tableClass = new TableClass();
            var number = reader.ReadInt32();
            for (var i = 0; i < number; ++i) {
                var field = new TableClass.Field();
                switch (reader.ReadInt8()) {
                    case 0:
                        field.fieldType = (TableClass.FieldType)reader.ReadInt8();
                        break;
                    case 1:
                        field.fieldType = TableClass.FieldType.ENUM;
                        field.type = reader.ReadString();
                        break;
                    case 2:
                        field.fieldType = TableClass.FieldType.CLASS;
                        field.type = reader.ReadString();
                        break;
                }
                field.array = reader.ReadBool();
                field.name = reader.ReadString();
                tableClass.Fields.Add(field);
            }
            return tableClass;
        }
        public static TableEnum ReadEnum(this IReader reader) {
            var tableEnum = new TableEnum();
            var number = reader.ReadInt32();
            for (var i = 0; i < number; ++i) {
                tableEnum.Elements.Add(new TableEnum.Element() {
                    name = reader.ReadString(),
                    value = reader.ReadInt32()
                });
            }
            return tableEnum;
        }
        public static string ReadField(this IReader reader, TableClass.Field field, Dictionary<string, TableEnum> customEnums, Dictionary<string, TableClass> customClasses) {
            if (field.array) {
                var values = new List<string>();
                var number = reader.ReadInt32();
                for (var i = 0; i < number; ++i) {
                    var value = reader.ReadOneField(field, customEnums, customClasses);
                    if (field.fieldType == TableClass.FieldType.CLASS) {
                        values.Add($"[{value}]");
                    } else {
                        values.Add(value);
                    }
                }
                return string.Join(";", values);
            } else {
                return reader.ReadOneField(field, customEnums, customClasses);
            }
        }
        public static string ReadOneField(this IReader reader, TableClass.Field field, Dictionary<string, TableEnum> customEnums, Dictionary<string, TableClass> customClasses) {
            if (field.fieldType == TableClass.FieldType.ENUM) {
                return customEnums[field.type].Get(reader.ReadInt32());
            } else if (field.fieldType == TableClass.FieldType.CLASS) {
                var values = new List<string>();
                var customClass = customClasses[field.type];
                foreach (var f in customClass.Fields) {
                    var value = reader.ReadField(f, customEnums, customClasses);
                    if (f.fieldType == TableClass.FieldType.CLASS) {
                        values.Add($"[{value}]");
                    } else {
                        values.Add(value);
                    }
                }
                return string.Join(";", values);
            } else {
                return BasicUtil.GetType((BasicEnum)field.fieldType).ReadValue(reader).ToString();
            }
        }
    }
}