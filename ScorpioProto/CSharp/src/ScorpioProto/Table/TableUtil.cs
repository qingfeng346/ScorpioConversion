using ScorpioProto.Commons;
namespace ScorpioProto.Table {
    public static class TableUtil {
        public interface ITableUtil {
            void Warning(string str);
        }
        private static ITableUtil IUtil = null;
        public static void SetTableUtil(ITableUtil util) {
            IUtil = util;
        }
        public static void Warning(string str) {
            if (IUtil != null) IUtil.Warning(str);
        }
        /// <summary> 读取Excel文件头结构 </summary>
        public static int ReadHead(IScorpioReader reader, string fileName, string MD5) {
            int iRow = reader.ReadInt32();          //行数
            if (reader.ReadString() != MD5)         //验证文件MD5(检测结构是否改变)
                throw new System.Exception("文件[" + fileName + "]版本验证失败");
            {
                var number = reader.ReadInt32();        //字段数量
                for (var i = 0; i < number; ++i) {
                    if (reader.ReadInt8() == 0) {   //基础类型
                        reader.ReadInt8();          //基础类型索引
                    } else {                        //自定义类
                        reader.ReadString();        //自定义类名称
                    }
                    reader.ReadBool();          //是否是数组
                }
            }
            {
                var customNumber = reader.ReadInt32();  //自定义类数量
                for (var i = 0; i < customNumber; ++i) {
                    reader.ReadString();                //读取自定义类名字
                    var number = reader.ReadInt32();        //字段数量
                    for (var j = 0; j < number; ++j) {
                        if (reader.ReadInt8() == 0) {   //基础类型
                            reader.ReadInt8();          //基础类型索引
                        } else {                        //自定义类
                            reader.ReadString();        //自定义类名称
                        }
                        reader.ReadBool();          //是否是数组
                    }
                }
            }
            
            return iRow;
        }
    }
}
