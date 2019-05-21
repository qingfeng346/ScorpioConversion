import { IScorpioReader } from '../ScorpioProto/Commons/IScorpioReader'
import { ScorpioUtil } from '../ScorpioProto/Commons/ScorpioUtil'
import { TableUtil } from '../ScorpioProto/Table/TableUtil'
import { IData } from '../ScorpioProto/Table/IData'
import { ITable } from '../ScorpioProto/Table/ITable'
import { DataTest } from './DataTest';
export class TableTest implements ITable {
    private m_count:number = 0;
    private m_dataArray:{[key:number]:DataTest} = {};
    public Initialize(fileName:string, reader:IScorpioReader):TableTest {
        let iRow = TableUtil.ReadHead(reader, fileName, "898169d7e1d0c2be013482d2c80052cc");
        for (let i = 0; i < iRow; ++i) {
            let pData = DataTest.Read(fileName, reader);
            if (this.Contains(pData.ID())) {
                this.m_dataArray[pData.ID()].Set(pData);
            } else {
                this.m_dataArray[pData.ID()] = pData;
            }
        }
        this.m_count = Object.getOwnPropertyNames(this.m_dataArray).length;
        return this;
    }
    public GetValue(ID:number):DataTest|null {
        if (this.Contains(ID)) return this.m_dataArray[ID];
        TableUtil.Warning("DataTest key is not exist " + ID);
        return null;
    }
    public Contains(ID:number):boolean {
        return this.m_dataArray[ID] != null;
    }
    public Datas():{[key:number]:DataTest} {
        return this.m_dataArray;
    }
    public GetValueObject(ID:any):IData|null {
        return this.GetValue(ID as number);
    }
    public ContainsObject(ID:any):boolean {
        return this.Contains(ID as number);
    }
    public GetDatas():any {
        return this.Datas();
    }
    public Count():number {
        return this.m_count;
    }
}
