//本文件为自动生成，请不要手动修改

const DataTest = require("./DataTest")
class TableTest {
    constructor() {
        this.m_count = 0;
        this.m_dataArray = new Map();
    }
    Initialize(fileName, reader) {
        let row = reader.ReadInt32();
        if ("5c86f5006b60d711c1ca95a5ea69b8db" != reader.ReadString()) {
            throw new Error("File schemas do not match [TableTest] : ${fileName}");
        }
        reader.ReadHead();
        for (let i = 0; i < row; i ++) {
            let pData = new DataTest(fileName, reader);
            if (this.m_dataArray.has(pData.ID)) {
                this.m_dataArray.get(pData.ID).Set(pData);
            } else {
                this.m_dataArray.set(pData.ID, pData);
            }
        }
        this.m_count = this.m_dataArray.size;
        return this;
    }
    GetValue(ID) {
        return this.m_dataArray.get(ID)
    }
    Contains(ID) {
        return this.m_dataArray.has(ID)
    }
    Datas() {
        return this.m_dataArray
    }
    GetValueObject(ID) {
        return this.GetValue(ID)
    }
    ContainsObject(ID) {
        return this.Contains(ID)
    }
    Count() {
        return this.m_count;
    }
}
module.exports = TableTest;