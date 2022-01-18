import { IScorpioReader } from '../ScorpioProto/Commons/IScorpioReader'
import { ScorpioUtil } from '../ScorpioProto/Commons/ScorpioUtil'
import { TableUtil } from '../ScorpioProto/Table/TableUtil'
import { IData } from '../ScorpioProto/Table/IData'
import { ITable } from '../ScorpioProto/Table/ITable'
import { TestEnum } from './TestEnum'
import { Int3 } from './Int3'

export class DataTest implements IData {
    private m_IsInvalid:boolean = false;
    
    private _TestID:any = null;
    /*   默认值() */
    getTestID():number { return this._TestID; }
    ID():number { return this._TestID; }
    private _testEnum:any = null;
    /*   默认值() */
    gettestEnum():TestEnum { return this._testEnum; }
    private _TestDate:any = null;
    /*   默认值() */
    getTestDate():Array<Int3> { return this._TestDate; }
    
    GetData(key:string):any {
        if ("TestID" == key) return this._TestID;
        if ("testEnum" == key) return this._testEnum;
        if ("TestDate" == key) return this._TestDate;
        return null;
    }
    
    IsInvalid():boolean { return this.m_IsInvalid; }
    private CheckInvalid():boolean {
        return false;
    }
    
    Set(value:DataTest) {
        this._TestID = value._TestID;
        this._testEnum = value._testEnum;
        this._TestDate = value._TestDate;
    }
    
    ToString() {
        return `{
            TestID : ${this._TestID}, 
            testEnum : ${this._testEnum}, 
            TestDate : ${ScorpioUtil.ToString(this._TestDate)}
            }`;
    }
    
    public static Read(fileName:string, reader:IScorpioReader):DataTest {
        var ret = new DataTest();
        ret._TestID = reader.ReadInt32();
        ret._testEnum = <TestEnum>reader.ReadInt32();
        {
            let list = new Array<Int3>();
            let number = reader.ReadInt32();
            for (let i = 0; i < number; ++i) { list.push(Int3.Read(fileName, reader)); }
            ret._TestDate = list;
        }
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }
}