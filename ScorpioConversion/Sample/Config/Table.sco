//测试枚举类型 关键字开头为 enum_
enum_TestEnum =
{
	Test1 = 1,
	Test2 = 2,
	Test3 = 3,
    value1 = 4,
    value2 = 5,
}
const_TestConst = {
    ValueNumber = 11111,
    ValueString = "22222"
}
//Excel表自定义类型 关键字开头为 table_
table_Int2 = [
    {
        name: "Value1",
        type: "int32",
    },
    {
        name: "Value2",
        type: "int32"
    }
]
table_Int3 = [
    {
        name: "Value1",
        type: "Int2",
        array: true,
    },
    {
        name: "Value2",
        type: "int32"
    }
]