#本文件为自动生成，请不要手动修改

from DataTestCsv import *
class TableTestCsv:
    def __init__(this):
        this.m_count = 0
        this.m_dataArray = {}

    def Initialize(this, fileName, reader):
        row = reader.ReadInt32()
        if "f07d3fff17de6b37025a951b272f6c4c" != reader.ReadString():
            raise Exception("File schemas do not match [TableTestCsv] : " + fileName)
        reader.ReadHead()
        for i in range(0, row):
            pData = DataTestCsv(fileName, reader)
            if this.m_dataArray.__contains__(pData.ID):
                this.m_dataArray[pData.ID].Set(pData)
            else:
                this.m_dataArray[pData.ID] = pData
        this.m_count = len(this.m_dataArray)
        return this

    def GetValue(this, ID):
        return this.m_dataArray[ID]

    def Contains(this, ID):
        return this.m_dataArray.__contains__(ID)

    def Datas(this):
        return this.m_dataArray

    def GetValueObject(this, ID):
        return this.GetValue(ID)

    def ContainsObject(this, ID):
        return this.Contains(ID)

    def Count(this):
        return this.m_count
