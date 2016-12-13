using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Scorpio.Commons;
namespace Scorpio.Table
{
    public static class TableUtil {
        public interface ITableUtil {
            byte[] GetBuffer(string resource);
            void Warning(string str);
        }
        private static ITableUtil IUtil = null;
        public static void SetTableUtil(ITableUtil util) {
            IUtil = util;
        }
        public static byte[] GetBuffer(string resource) {
            return IUtil != null ? IUtil.GetBuffer(resource) : null;
        }
        public static void Warning(string str) {
            if (IUtil != null) IUtil.Warning(str);
        }
#if SCORPIO_PROTO_SCO
        public static ScriptTable ReadDatas(Script script, string fileName, string dataName, string keyName, string MD5) {
            ScriptTable ret = script.CreateTable();
            ScorpioReader reader = new ScorpioReader(GetBuffer(fileName));
            int iRow = ReadHead(reader, fileName, MD5);
            ScriptTable data = null;
            double key = 0;
            for (int i = 0; i < iRow;++i ) {
                data = ScorpioSerializer.Read(script, reader, dataName, false);
                key = ScorpioUtil.ToDouble(data.GetValue(keyName).ObjectValue);
                if (ret.HasValue(key))
                    throw new System.Exception("文件[" + fileName + "]有重复项 ID : " + key);
                ret.SetValue(key, data);
				data.SetValue("ID", script.CreateDouble(key));
            }
            return ret;
        }
#endif
        public static int ReadHead(ScorpioReader reader, string fileName, string MD5) {
            int iRow = reader.ReadInt32();          //行数
            if (reader.ReadString() != MD5)         //验证文件MD5(检测结构是否改变)
                throw new System.Exception("文件[" + fileName + "]版本验证失败");
            int i,j,number;
            {
                number = reader.ReadInt32();        //字段数量
                for (i = 0; i < number; ++i) {
                    if (reader.ReadInt8() == 0) {   //基础类型
                        reader.ReadInt8();          //基础类型索引
                        reader.ReadBool();          //是否是数组
                    } else {                        //自定义类
                        reader.ReadString();        //自定义类名称
                        reader.ReadBool();          //是否是数组
                    }
                }
            }
            int customNumber = reader.ReadInt32();  //自定义类数量
            for (i = 0; i < customNumber; ++i) {
                reader.ReadString();                //读取自定义类名字
                number = reader.ReadInt32();        //字段数量
                for (j = 0; j < number; ++j) {
                    if (reader.ReadInt8() == 0) {   //基础类型
                        reader.ReadInt8();          //基础类型索引
                        reader.ReadBool();          //是否是数组
                    } else {                        //自定义类
                        reader.ReadString();        //自定义类名称
                        reader.ReadBool();          //是否是数组
                    }
                }
            }
            return iRow;
        }
        private const SByte INVALID_INT8 = SByte.MaxValue;
        private const Int16 INVALID_INT16 = Int16.MaxValue;
        private const Int32 INVALID_INT32 = Int32.MaxValue;
        private const Int64 INVALID_INT64 = Int64.MaxValue;
        private const float INVALID_FLOAT = -1.0f;
        private const double INVALID_DOUBLE = -1.0;
        public static bool IsInvalidInt8(SByte val) {
            return (val == INVALID_INT8);
        }
        public static bool IsInvalidInt16(Int16 val) {
            return (val == INVALID_INT16);
        }
        public static bool IsInvalidInt32(Int32 val) {
            return (val == INVALID_INT32);
        }
        public static bool IsInvalidInt64(Int64 val) {
            return (val == INVALID_INT64);
        }
        public static bool IsInvalidFloat(float val) {
            return (Math.Abs(INVALID_FLOAT - val) < 0.001f);
        }
        public static bool IsInvalidDouble(double val) {
            return (Math.Abs(INVALID_DOUBLE - val) < 0.001f);
        }
        public static bool IsInvalidString(string val) {
            return string.IsNullOrEmpty(val);
        }
        public static bool IsInvalidList(IList val) {
		    return val.Count == 0;
	    }
        public static bool IsInvalidData(IData val) {
		    return val.IsInvalid();
	    }
        public static bool IsInvalid(object val) {
            if (val is sbyte)
                return IsInvalidInt8((sbyte)val);
            else if (val is Int16)
                return IsInvalidInt16((Int16)val);
            else if (val is Int32)
                return IsInvalidInt32((Int32)val);
            else if (val is Int64)
                return IsInvalidInt64((Int64)val);
            else if (val is float)
                return IsInvalidFloat((float)val);
            else if (val is double)
                return IsInvalidDouble((double)val);
            else if (val is string)
                return IsInvalidString((string)val);
            else if (val is IList)
        	    return IsInvalidList((IList)val);
            else if (val is IData)
        	    return IsInvalidData((IData)val);
            return false;
        }
    }
}
