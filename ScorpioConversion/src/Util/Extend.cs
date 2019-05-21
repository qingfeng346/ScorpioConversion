using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using Scorpio;
using Scorpio.Commons;

public static class Extend {
    private readonly static DateTime BaseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    private const string EmptyString = "####";
    private const string ArrayString = "array";
    public const bool INVALID_BOOL = false;
    public const sbyte INVALID_INT8 = SByte.MaxValue;
    public const byte INVALID_UINT8 = Byte.MaxValue;
    public const short INVALID_INT16 = Int16.MaxValue;
    public const ushort INVALID_UINT16 = UInt16.MaxValue;
    public const int INVALID_INT32 = Int32.MaxValue;
    public const uint INVALID_UINT32 = UInt32.MaxValue;
    public const long INVALID_INT64 = Int64.MaxValue;
    public const ulong INVALID_UINT64 = UInt64.MaxValue;
    public const float INVALID_FLOAT = -1.0f;
    public const double INVALID_DOUBLE = -1.0;
    public const string INVALID_STRING = "";
    public static string GetMemory(this long by) {
        return Scorpio.Commons.Util.GetMemory(by);
    }
    public static string GetLineName(this int line) {
        return Scorpio.Commons.Util.GetExcelColumn(line);
    }
    public static bool IsEmptyString(this string str) {
        return string.IsNullOrWhiteSpace(str) || str == EmptyString;
    }
    public static bool IsEmptyValue(this ValueList value) {
        return value == null || value.values.Count == 0;
    }
    public static bool IsInvalid(this string str) {
        return str.Trim().StartsWith("!");
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
    public static string GetCellString(this IRow row, int index) {
        return GetCellString(row, index, "");
    }
    public static string GetCellString(this IRow row, int index, string def) {
        return row.GetCell(index, MissingCellPolicy.CREATE_NULL_AS_BLANK).GetCellString(def);
    }
    public static string GetCellString(this ICell cell) {
        return GetCellString(cell, "");
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
    public static long GetTimeSpan(this DateTime time) {
        return Convert.ToInt64((time - BaseTime).TotalMilliseconds);
    }
    public static LanguageInfo GetInfo(this Language language) {
        return Attribute.GetCustomAttribute(language.GetType().GetMember(language.ToString())[0], typeof(LanguageInfo)) as LanguageInfo;
    }
}
