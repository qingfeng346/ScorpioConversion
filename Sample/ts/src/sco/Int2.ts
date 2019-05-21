import { IScorpioReader } from '../ScorpioProto/Commons/IScorpioReader'
import { ScorpioUtil } from '../ScorpioProto/Commons/ScorpioUtil'
import { TableUtil } from '../ScorpioProto/Table/TableUtil'
import { IData } from '../ScorpioProto/Table/IData'
import { ITable } from '../ScorpioProto/Table/ITable'

export class Int2 implements IData {
    private m_IsInvalid:boolean = false;
    
    private _value1:any = null;
    /*   默认值() */
    getvalue1():number { return this._value1; }
    ID():number { return this._value1; }
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
    
    Set(value:Int2) {
        this._value1 = value._value1;
        this._value2 = value._value2;
    }
    
    ToString() {
        return `{
            value1 : ${this._value1}, 
            value2 : ${this._value2}
            }`;
    }
    
    public static Read(fileName:string, reader:IScorpioReader):Int2 {
        var ret = new Int2();
        ret._value1 = reader.ReadInt32();
        ret._value2 = reader.ReadInt32();
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }
}