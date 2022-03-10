package datas

//本文件为自动生成，请不要手动修改
import (
	"fmt"

	"github.com/qingfeng346/ScorpioConversion/Scorpio.Conversion.Runtime/Go/ScorpioConversionRuntime"
)

type DataSpawn struct {
	ID           int32
	TestInt      int32
	TestString   string
	TestLanguage string
	TestBool     bool
	TestInt2     *Int2
	TestEnumName int32
}

// GetID 测试ID 此值必须唯一 而且必须为int型  默认值()
func (data *DataSpawn) GetID() int32 {
	return data.ID
}

// GetTestInt int类型  默认值()
func (data *DataSpawn) GetTestInt() int32 {
	return data.TestInt
}

// GetTestString string类型  默认值()
func (data *DataSpawn) GetTestString() string {
	return data.TestString
}

// GetTestLanguage 测试多国语言  默认值()
func (data *DataSpawn) GetTestLanguage() string {
	return data.TestLanguage
}

// GetTestBool bool类型  默认值()
func (data *DataSpawn) GetTestBool() bool {
	return data.TestBool
}

// GetTestInt2 自定义类型 根据ExcelConfig下 table.sco文件定义的Int2解析 类型为table_后面的名字 格式为 , 隔开  默认值()
func (data *DataSpawn) GetTestInt2() *Int2 {
	return data.TestInt2
}

// GetTestEnumName 自定义枚举  默认值()
func (data *DataSpawn) GetTestEnumName() int32 {
	return data.TestEnumName
}

func NewDataSpawn(fileName string, reader ScorpioConversionRuntime.IReader) *DataSpawn {
	data := &DataSpawn{}
	data.ID = reader.ReadInt32()
	data.TestInt = reader.ReadInt32()
	data.TestString = reader.ReadString()
	data.TestLanguage = reader.ReadString()
	data.TestBool = reader.ReadBool()
	data.TestInt2 = NewInt2(fileName, reader)
	data.TestEnumName = reader.ReadInt32()
	return data
}

func (data *DataSpawn) GetData(key string) interface{} {
	if key == "ID" {
		return data.ID
	}
	if key == "TestInt" {
		return data.TestInt
	}
	if key == "TestString" {
		return data.TestString
	}
	if key == "TestLanguage" {
		return data.TestLanguage
	}
	if key == "TestBool" {
		return data.TestBool
	}
	if key == "TestInt2" {
		return data.TestInt2
	}
	if key == "TestEnumName" {
		return data.TestEnumName
	}
	return nil
}

func (data *DataSpawn) Set(value *DataSpawn) {
	data.ID = value.ID
	data.TestInt = value.TestInt
	data.TestString = value.TestString
	data.TestLanguage = value.TestLanguage
	data.TestBool = value.TestBool
	data.TestInt2 = value.TestInt2
	data.TestEnumName = value.TestEnumName
}

func (data *DataSpawn) String() string {
	return fmt.Sprintf("ID : %v , TestInt : %v , TestString : %v , TestLanguage : %v , TestBool : %v , TestInt2 : %v , TestEnumName : %v", data.ID, data.TestInt, data.TestString, data.TestLanguage, data.TestBool, data.TestInt2, data.TestEnumName)
}
