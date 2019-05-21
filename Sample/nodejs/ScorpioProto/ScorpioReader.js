const Long = require('long')
class ScorpioReader {
    constructor(buf) {
        this.offset = 0
        this.buffer = Buffer.from(buf);
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
}
module.exports = ScorpioReader