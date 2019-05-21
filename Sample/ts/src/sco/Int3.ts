import { IScorpioReader } from '../ScorpioProto/Commons/IScorpioReader'
import { ScorpioUtil } from '../ScorpioProto/Commons/ScorpioUtil'
import { TableUtil } from '../ScorpioProto/Table/TableUtil'
import { IData } from '../ScorpioProto/Table/IData'
import { ITable } from '../ScorpioProto/Table/ITable'
import { Int2 } from './Int2'

export class Int3 implements IData {
    private m_IsInvalid:boolean = false;
    
    private _value1:any = null;
    /*   默认值() */
    getvalue1():Int2 { return this._value1; }
    ID():Int2 { return this._value1; }
    private _value2:any = null;
    /*   默认值() */
    getvalue2():number { return this._value2; }
    
    GetData(key:string):any {
        if ("value1" == key) return this._value1;
        if ("value2" == key) return this._value2;
        return null;
    }
    
    IsInvalid():boolean { return this.m_IsInvalid; }
    private CheckInvalid():boolean {
        return false;
    }
    
    Set(value:Int3) {
        this._value1 = value._value1;
        this._value2 = value._value2;
    }
    
    ToString() {
        return `{
            value1 : ${this._value1}, 
            value2 : ${this._value2}
            }`;
    }
    
    public static Read(fileName:string, reader:IScorpioReader):Int3 {
        var ret = new Int3();
        ret._value1 = Int2.Read(fileName, reader);
        ret._value2 = reader.ReadInt32();
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }
}