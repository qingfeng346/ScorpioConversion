//测试信息
enum_TestEnum = {
	Test1 = 1,
	Test2 = 2,
	Test3 = 3,
}
Msg_C2G_Test = {
    Value1 = "1,int",
	Value2 = "2,string",
	Value3 = "3,int,true",
}
Msg_C2G_Test2 = {
	Value1 = "1,Msg_C2G_Test",
}
Msg_C2G_Test3 = {
	Value1 = "1,Msg_C2G_Test,true",
	Value2 = "2,Msg_C2G_Test2",
	Value3 = "3,TestEnum",
	Value4 = "4,TestEnum,true",
}