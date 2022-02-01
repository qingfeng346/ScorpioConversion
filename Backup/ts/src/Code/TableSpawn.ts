import { IScorpioReader } from '../ScorpioProto/Commons/IScorpioReader'
import { ScorpioUtil } from '../ScorpioProto/Commons/ScorpioUtil'
import { TableUtil } from '../ScorpioProto/Table/TableUtil'
import { IData } from '../ScorpioProto/Table/IData'
import { ITable } from '../ScorpioProto/Table/ITable'
import { DataSpawn } from './DataSpawn';
export class TableSpawn implements ITable {
    private m_count:number = 0;
    private m_dataArray:{[key:number]:DataSpawn} = {};
    public Initialize(fileName:string, reader:IScorpioReader):TableSpawn {
        let iRow = TableUtil.ReadHead(reader, fileName, "484cdae7d179982f1c7868078204d81d");
        for (let i = 0; i < iRow; ++i) {
            let pData = DataSpawn.Read(fileName, reader);
            if (this.Contains(pData.ID())) {
                this.m_dataArray[pData.ID()].Set(pData);
            } else {
                this.m_dataArray[pData.ID()] = pData;
            }
        }
        this.m_count = Object.getOwnPropertyNames(this.m_dataArray).length;
        return this;
    }
    public GetValue(ID:number):DataSpawn|null {
        if (this.Contains(ID)) return this.m_dataArray[ID];
        TableUtil.Warning("DataSpawn key is not exist " + ID);
        return null;
    }
    public Contains(ID:number):boolean {
        return this.m_dataArray[ID] != null;
    }
    public Datas():{[key:number]:DataSpawn} {
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
