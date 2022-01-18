
const TableUtil = require("../ScorpioProto/TableUtil")
const DataTest = require("./DataTest").DataTest
class TableTest {
    constructor() {
        this.m_count = 0
        this.m_dataArray = {}
    }
    Initialize(fileName, reader) {
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
    GetValue(ID) {
        if (this.Contains(ID)) return this.m_dataArray[ID];
        //TableUtil.Warning("DataTest key is not exist " + ID);
        return null;
    }
    Contains(ID) {
        return this.m_dataArray[ID] != null;
    }
    Datas() {
        return this.m_dataArray;
    }
    GetValueObject(ID) {
        return this.GetValue(ID);
    }
    ContainsObject(ID) {
        return this.Contains(ID);
    }
    GetDatas() {
        return this.Datas();
    }
    Count() {
        return this.m_count;
    }
}
exports.TableTest = TableTest;
