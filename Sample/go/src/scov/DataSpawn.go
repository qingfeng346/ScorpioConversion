package scov
import (
    "scorpioproto"
)

// DataSpawn 类名
type DataSpawn struct {
    
    _ID int32
    _TestInt int32
    _TestString string
    _TestLanguage string
    _TestBool bool
    _TestInt2 *Int2
    _TestEnumName TestEnum
}


// ID 测试ID 此值必须唯一 而且必须为int型  默认值()
func (data *DataSpawn) ID() int32 { return data._ID; }

// GetID 测试ID 此值必须唯一 而且必须为int型  默认值()
func (data *DataSpawn) GetID() int32 { return data._ID; }

// GetTestInt int类型  默认值()
func (data *DataSpawn) GetTestInt() int32 { return data._TestInt; }

// GetTestString string类型  默认值()
func (data *DataSpawn) GetTestString() string { return data._TestString; }

// GetTestLanguage 测试多国语言  默认值()
func (data *DataSpawn) GetTestLanguage() string { return data._TestLanguage; }

// GetTestBool bool类型  默认值()
func (data *DataSpawn) GetTestBool() bool { return data._TestBool; }

// GetTestInt2 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开  默认值()
func (data *DataSpawn) GetTestInt2() *Int2 { return data._TestInt2; }

// GetTestEnumName 自定义枚举  默认值()
func (data *DataSpawn) GetTestEnumName() TestEnum { return data._TestEnumName; }


// GetData 获取数据
func (data *DataSpawn) GetData(key string) interface{} {
    if "ID" == key {
        return data._ID;
    }
    if "TestInt" == key {
        return data._TestInt;
    }
    if "TestString" == key {
        return data._TestString;
    }
    if "TestLanguage" == key {
        return data._TestLanguage;
    }
    if "TestBool" == key {
        return data._TestBool;
    }
    if "TestInt2" == key {
        return data._TestInt2;
    }
    if "TestEnumName" == key {
        return data._TestEnumName;
    }
    return nil;
}


// Set 设置数据
func (data *DataSpawn) Set(value *DataSpawn) {
    data._ID = value._ID;
    data._TestInt = value._TestInt;
    data._TestString = value._TestString;
    data._TestLanguage = value._TestLanguage;
    data._TestBool = value._TestBool;
    data._TestInt2 = value._TestInt2;
    data._TestEnumName = value._TestEnumName;
}


// Read 读取数据
func (data *DataSpawn) Read(fileName string, reader scorpioproto.IScorpioReader) {
    data._ID = reader.ReadInt32();
    data._TestInt = reader.ReadInt32();
    data._TestString = reader.ReadString();
    data._TestLanguage = reader.ReadString();
    data._TestBool = reader.ReadBool();
    data._TestInt2 = Int2Read(fileName, reader);
    data._TestEnumName = TestEnum(reader.ReadInt32());
}

// DataSpawnRead 读取数据
func DataSpawnRead(fileName string, reader scorpioproto.IScorpioReader) *DataSpawn {
    ret := new(DataSpawn)
    ret.Read(fileName, reader)
    return ret
}
