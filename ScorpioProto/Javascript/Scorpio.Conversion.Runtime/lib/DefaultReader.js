const Long = require('./Long')
class ScorpioReader {
    constructor(buf) {
        this.offset = 0
        this.buffer = Buffer.from(buf);
    }
    ReadHead() {
        {
            let number = this.ReadInt32();      //字段数量
            for (let i = 0; i < number; ++i) {
                if (this.ReadInt8() == 0) {     //基础类型
                    this.ReadInt8();            //基础类型索引
                } else {                        //自定义类
                    this.ReadString();          //自定义类名称
                }
                this.ReadBool();                //是否是数组
                this.ReadString();              //字段名称
            }
        }
        {
            let customNumber = this.ReadInt32();  //自定义类数量
            for (let i = 0; i < customNumber; ++i) {
                this.ReadString();                //读取自定义类名字
                if (this.ReadInt8() == 1) {
                    let number = this.ReadInt32();
                    for (let j = 0; j < number; ++j) {
                        this.ReadString();
                        this.ReadInt32();
                    }
                } else {
                    let number = this.ReadInt32();      //字段数量
                    for (let j = 0; j < number; ++j) {
                        if (this.ReadInt8() == 0) {     //基础类型
                            this.ReadInt8();            //基础类型索引
                        } else {                        //自定义类
                            this.ReadString();          //自定义类名称
                        }
                        this.ReadBool();                //是否是数组
                        this.ReadString();
                    }
                }
            }
        }
    }
    ReadBool() {
        return this.ReadInt8() == 1;
    }
    ReadInt8() {
        let ret = this.buffer.readInt8(this.offset);
        this.offset += 1
        return ret
    }
    ReadUInt8() {
        let ret = this.buffer.readUInt8(this.offset);
        this.offset += 1
        return ret
    }
    ReadInt16() {
        let ret = this.buffer.readInt16LE(this.offset);
        this.offset += 2
        return ret
    }
    ReadUInt16() {
        let ret = this.buffer.readUInt16LE(this.offset);
        this.offset += 2
        return ret
    }
    ReadInt32() {
        let ret = this.buffer.readInt32LE(this.offset);
        this.offset += 4
        return ret
    }
    ReadUInt32() {
        let ret = this.buffer.readUInt32LE(this.offset);
        this.offset += 4
        return ret
    }
    ReadInt64() {
        let low = this.ReadInt32()
        let high = this.ReadInt32()
        return Long.fromBits(low, high, false)
    }
    ReadUInt64() {
        let low = this.ReadInt32()
        let high = this.ReadInt32()
        return Long.fromBits(low, high, true)
    }
    ReadFloat() {
        let ret = this.buffer.readFloatLE(this.offset);
        this.offset += 4
        return ret
    }
    ReadDouble() {
        let ret = this.buffer.readDoubleLE(this.offset);
        this.offset += 8
        return ret
    }
    ReadString() {
        let length = this.ReadUInt16();
        let start = this.offset
        let end = start + length
        let ret = this.buffer.toString("utf8", start, end)
        this.offset += length
        return ret
    }
    ReadDateTime() {
        let time = this.ReadInt64()
        return new Date(time.toNumber())
    }
    ReadBytes() {
        let length = this.ReadInt32()
        let ret = Buffer.alloc(length);
        this.buffer.copy(ret, 0, this.offset, this.offset + length)
        this.offset += length
        return ret
    }
    Close() { }
}
module.exports = ScorpioReader