import { IScorpioReader } from '../Commons/IScorpioReader'

export class TableUtil {
    static ReadHead(reader:IScorpioReader, fileName:string, md5:string):number {
        let iRow = reader.ReadInt32();          //行数
        if (reader.ReadString() != md5)         //验证文件MD5(检测结构是否改变)
            throw "文件[" + fileName + "]版本验证失败";
        {
            let number = reader.ReadInt32();        //字段数量
            for (let i = 0; i < number; ++i) {
                if (reader.ReadInt8() == 0) {   //基础类型
                    reader.ReadInt8();          //基础类型索引
                } else {                        //自定义类
                    reader.ReadString();        //自定义类名称
                }
                reader.ReadBool();          //是否是数组
            }
        }
        {
            let customNumber = reader.ReadInt32();  //自定义类数量
            for (let i = 0; i < customNumber; ++i) {
                reader.ReadString();                //读取自定义类名字
                let number = reader.ReadInt32();        //字段数量
                for (let j = 0; j < number; ++j) {
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
    static Warning(str:string) {

    }
}