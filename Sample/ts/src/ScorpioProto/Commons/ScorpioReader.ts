import { IScorpioReader } from './IScorpioReader';
import { Buffer } from 'buffer'; 
import Long from 'long'
export class ScorpioReader implements IScorpioReader {
    offset:number = 0;
    buffer:Buffer;
    constructor(buf:any) {
        this.offset = 0
        this.buffer = Buffer.from(buf);
    }
    ReadBool():boolean {
        return this.ReadInt8() == 1;
    }
    ReadInt8():number {
        let ret = this.buffer.readInt8(this.offset);
        this.offset += 1
        return ret
    }
    ReadInt16():number {
        let ret = this.buffer.readInt16LE(this.offset);
        this.offset += 2
        return ret
    }
    ReadInt32():number {
        let ret = this.buffer.readInt32LE(this.offset);
        this.offset += 4
        return ret
    }
    ReadInt64():any {
        let low = this.ReadInt32()
        let high = this.ReadInt32()
        return Long.fromBits(low, high, false)
    }
    ReadFloat():number {
        let ret = this.buffer.readFloatLE(this.offset);
        this.offset += 4
        return ret
    }
    ReadDouble():number {
        let ret = this.buffer.readDoubleLE(this.offset);
        this.offset += 8
        return ret
    }
    ReadString():string {
        let length = this.ReadInt32();
        let start = this.offset
        let end = start + length
        let ret = this.buffer.toString("utf8", start, end)
        this.offset += length
        return ret
    }
    ReadDateTime():Date {
        let time = this.ReadInt64()
        return new Date(time.toNumber())
    }
}