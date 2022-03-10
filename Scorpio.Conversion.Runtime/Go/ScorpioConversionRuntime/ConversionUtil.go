package ScorpioConversionRuntime

func ReadHead(reader IReader) {
	{
		var number = int(reader.ReadInt32()) //表结构字段数量
		for i := 0; i < number; i++ {
			if reader.ReadInt8() == 0 { //基础类型
				reader.ReadInt8() //基础类型索引
			} else { //自定义类
				reader.ReadString() //自定义类名称
			}
			reader.ReadBool()   //是否是数组
			reader.ReadString() //字段名称
		}
	}
	{
		var customNumber = int(reader.ReadInt32()) //自定义类数量
		for i := 0; i < customNumber; i++ {
			reader.ReadString() //读取自定义类名字
			if reader.ReadInt8() == 1 {
				var number = int(reader.ReadInt32())
				for j := 0; j < number; j++ {
					reader.ReadString()
					reader.ReadInt32()
				}
			} else {
				var number = int(reader.ReadInt32()) //字段数量
				for j := 0; j < number; j++ {
					if reader.ReadInt8() == 0 { //基础类型
						reader.ReadInt8() //基础类型索引
					} else { //自定义类
						reader.ReadString() //自定义类名称
					}
					reader.ReadBool() //是否是数组
					reader.ReadString()
				}
			}
		}
	}
}
