//本文件为自动生成，请不要手动修改

class TableSpawn {
    constructor() {
        this.m_count = 0
        this.m_dataArray = {}
    }
    Initialize(fileName, reader) {
        var row = reader.ReadInt32();
        if ("484cdae7d179982f1c7868078204d81d" != reader.ReadString()) {
            throw "File schemas do not match [TableSpawn] : ${fileName}";
        }
        ConversionUtil.ReadHead(reader);
        for (var i = 0, row - 1) {
            var pData = new DataSpawn(fileName, reader);
            if (this.m_dataArray.containsKey(pData.ID)) {
                this.m_dataArray[pData.ID].Set(pData);
            } else {
                this.m_dataArray[pData.ID] = pData;
            }
        }
        this.m_count = this.m_dataArray.count();
        return this;
    }
    GetValue(ID) {
        return this.m_dataArray[ID]
    }
    "()"(ID) {
        return this.m_dataArray[ID]
    }
    Contains(ID) {
        return this.m_dataArray.containsKey(ID)
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