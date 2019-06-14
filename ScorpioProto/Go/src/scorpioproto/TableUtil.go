package scorpioproto

import "fmt"

// TableUtilReadHead 读取表头
func TableUtilReadHead(reader IScorpioReader, fileName string, md5 string) int {
	iRow := reader.ReadInt32()
	if reader.ReadString() != md5 {
		fmt.Println("MD5验证失败")
		return 0
	}
	{
		number := int(reader.ReadInt32())
		for i := 0; i < number; i++ {
			if reader.ReadInt8() == 0 { //基础类型
				reader.ReadInt8() //基础类型索引
			} else { //自定义类
				reader.ReadString() //自定义类名称
			}
			reader.ReadBool() //是否是数组
		}
	}
	{
		customNumber := int(reader.ReadInt32()) //自定义类数量
		for i := 0; i < customNumber; i++ {
			reader.ReadString()               //读取自定义类名字
			number := int(reader.ReadInt32()) //字段数量
			for j := 0; j < number; j++ {
				if reader.ReadInt8() == 0 { //基础类型
					reader.ReadInt8() //基础类型索引
				} else { //自定义类
					reader.ReadString() //自定义类名称
				}
				reader.ReadBool() //是否是数组
			}
		}
	}
	return int(iRow)
}
