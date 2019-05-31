using System;
using System.Collections.Generic;
using System.Text;

public class TemplateScorpio2 {
    public const string Table = @"
ScorpioSerializer = import_type(""ScorpioProto.Commons.ScorpioSerializer"")
class __TableName {
    constructor() {
        this.m_count = 0
        this.m_dataArray = {}
    }
    Initialize(fileName, reader) {
        this.m_dataArray = ScorpioSerializer.ReadDatas(_SCRIPT, fileName, reader, this.m_dataArray, ""__DataName"", ""__KeyName"", ""__MD5"")
        this.m_count = this.m_dataArray.length()
        return this
    }
    GetValue(ID) {
        return this.m_dataArray[ID]
    }
    Contains(ID) {
        return this.m_dataArray.containsKey(ID)
    }
    Count() {
        return this.m_count 
    }
    Datas() {
        return this.m_dataArray
    }
}
__TableName[""()""] = function(ID) {
    return this.m_dataArray[ID]
}";
}

public class GenerateDataScorpio2 : GenerateDataScorpio {
}
public class GenerateTableScorpio2 : IGenerate {
    protected override string Generate_impl() {
        return TemplateScorpio2.Table;
    }
}
public class GenerateEnumScorpio2 : GenerateEnumScorpio {
}