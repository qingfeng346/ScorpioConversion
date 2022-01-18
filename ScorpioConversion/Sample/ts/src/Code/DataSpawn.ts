import { IScorpioReader } from '../ScorpioProto/Commons/IScorpioReader'
import { ScorpioUtil } from '../ScorpioProto/Commons/ScorpioUtil'
import { TableUtil } from '../ScorpioProto/Table/TableUtil'
import { IData } from '../ScorpioProto/Table/IData'
import { ITable } from '../ScorpioProto/Table/ITable'
import { Int2 } from './Int2'
import { TestEnum } from './TestEnum'

export class DataSpawn implements IData {
    private m_IsInvalid:boolean = false;
    
    private _ID:any = null;
    /* 测试ID 此值必须唯一 而且必须为int型  默认值() */
    getID():number { return this._ID; }
    ID():number { return this._ID; }
    private _TestInt:any = null;
    /* int类型  默认值() */
    getTestInt():number { return this._TestInt; }
    private _TestString:any = null;
    /* string类型  默认值() */
    getTestString():string { return this._TestString; }
    private _TestLanguage:any = null;
    /* 测试多国语言  默认值() */
    getTestLanguage():string { return this._TestLanguage; }
    private _TestBool:any = null;
    /* bool类型  默认值() */
    getTestBool():boolean { return this._TestBool; }
    private _TestInt2:any = null;
    /* 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开  默认值() */
    getTestInt2():Int2 { return this._TestInt2; }
    private _TestEnumName:any = null;
    /* 自定义枚举  默认值() */
    getTestEnumName():TestEnum { return this._TestEnumName; }
    
    GetData(key:string):any {
        if ("ID" == key) return this._ID;
        if ("TestInt" == key) return this._TestInt;
        if ("TestString" == key) return this._TestString;
        if ("TestLanguage" == key) return this._TestLanguage;
        if ("TestBool" == key) return this._TestBool;
        if ("TestInt2" == key) return this._TestInt2;
        if ("TestEnumName" == key) return this._TestEnumName;
        return null;
    }
    
    IsInvalid():boolean { return this.m_IsInvalid; }
    private CheckInvalid():boolean {
        return false;
    }
    
    Set(value:DataSpawn) {
        this._ID = value._ID;
        this._TestInt = value._TestInt;
        this._TestString = value._TestString;
        this._TestLanguage = value._TestLanguage;
        this._TestBool = value._TestBool;
        this._TestInt2 = value._TestInt2;
        this._TestEnumName = value._TestEnumName;
    }
    
    ToString() {
        return `{
            ID : ${this._ID}, 
            TestInt : ${this._TestInt}, 
            TestString : ${this._TestString}, 
            TestLanguage : ${this._TestLanguage}, 
            TestBool : ${this._TestBool}, 
            TestInt2 : ${this._TestInt2}, 
            TestEnumName : ${this._TestEnumName}
            }`;
    }
    
    public static Read(fileName:string, reader:IScorpioReader):DataSpawn {
        var ret = new DataSpawn();
        ret._ID = reader.ReadInt32();
        ret._TestInt = reader.ReadInt32();
        ret._TestString = reader.ReadString();
        ret._TestLanguage = reader.ReadString();
        ret._TestBool = reader.ReadBool();
        ret._TestInt2 = Int2.Read(fileName, reader);
        ret._TestEnumName = <TestEnum>reader.ReadInt32();
        ret.m_IsInvalid = ret.CheckInvalid();
        return ret;
    }
}