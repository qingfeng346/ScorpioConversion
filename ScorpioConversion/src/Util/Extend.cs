using System;
using System.Data;
using NPOI.SS.UserModel;
using Scorpio.Commons;

public static class Extend {
    private readonly static DateTime BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
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
    public static string GetMemory(this long by) => ScorpioUtil.GetMemory(by);
    public static string GetLineName(this int line) => ScorpioUtil.GetExcelColumn(line);
    public static bool IsEmptyString(this string str) => string.IsNullOrWhiteSpace(str);
    public static bool IsEmptyValue(this ValueList value) => value == null || value.values.Count == 0;
    public static bool IsInvalid(this string str) => string.IsNullOrWhiteSpace(str) || str.Trim().StartsWith("!");
    public static bool IsExcel(this string file) => !file.Contains("~$") && (file.EndsWith(".xls") || file.EndsWith(".xlsx"));
    public static bool IsL10N(this string str) => !str.IsEmptyString() && str.Trim().StartsWith('$');
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
                        throw new Exception("不能解析的Name : " + str);
                    }
            }
            s = s.Substring(1);
        }
        throw new Exception("不能解析的Name : " + str);
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
                throw new Exception("字符串不能转换为bool " + value);
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
    public static byte[] ToBytes(this string value) {
        if (value.IsEmptyString()) {
            return INVALID_BYTES;
        } else if (value.StartsWith(BYTES_PROTO_BASE64)) {
            return Convert.FromBase64String(value.Substring(BYTES_PROTO_BASE64.Length));
        } else if (value.StartsWith(BYTES_PROTO_FILE)) {
            var bytes = FileUtil.GetFileBuffer(value.Substring(BYTES_PROTO_FILE.Length));
            if (bytes == null) {
                throw new Exception($"二进制数据文件不存在 : {value}");
            }
            return bytes;
        }
        throw new Exception($"未知的二进制数据 : {value}");
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
}
