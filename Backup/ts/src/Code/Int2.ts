import { IScorpioReader } from '../ScorpioProto/Commons/IScorpioReader'
import { ScorpioUtil } from '../ScorpioProto/Commons/ScorpioUtil'
import { TableUtil } from '../ScorpioProto/Table/TableUtil'
import { IData } from '../ScorpioProto/Table/IData'
import { ITable } from '../ScorpioProto/Table/ITable'

export class Int2 implements IData {
    private m_IsInvalid:boolean = false;
    
    private _Value1:any = null;
    /*   默认值() */
    getValue1():number { return this._Value1; }
    ID():number { return this._Value1; }
    private _Value2:any = null;
    /*   默认值() */
    getValue2():number { return this._Value2; }
    
    GetData(key:string):any {
        if ("Value1" == key) return this._Value1;
        if ("Value2" == key) return this._Value2;
        return null;
    }
    
    IsInvalid():boolean { return this.m_IsInvalid; }
    private CheckInvalid():boolean {
        return false;
    }
    
    Set(value:Int2) {
        this._Value1 = value._Value1;
        this._Value2 = value._Value2;
    }
    
    ToString() {
        return `{
            Value1 : ${this._Value1}, 
            Value2 : ${this._Value2}
            }`;
    }
    
    public static Read(fileName:string, reader:IScorpioReader):Int2 {
        var ret = new Int2();
        ret._Value1 = reader.ReadInt32();
        ret._Value2 = reader.ReadInt32();
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }
}