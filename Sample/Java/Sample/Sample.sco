print("*****************下面是中文***********************")
TableManager.setLanguage("CN");
//输出Test表 ID 为 10000 的数据
var data = TableManager.GetTest().GetElement(10001)
print(json.encode(data))
//(注意 脚本里面是直接使用的变量字段名 c#里面是使用的函数)
print("data.TestInt = " + data.TestInt)
print("*****************下面是英文***********************")
TableManager.Reset()
TableManager.setLanguage("EN");
//输出Test表 ID 为 10000 的数据
print(json.encode(TableManager.GetTest().GetElement(10001)))


print("================================下面代码是序列化反序列化消息数据====================================")
//先初始化数据Table 要跟序列化的数据字段名字对上 例如要使用 Msg_C2G_Test
var ser = {Value2 = "123123", Value3 = [1,2,3,4,5,6]}
//序列化数据 第一个参数传入 Script句柄  _SCRIPT为Script句柄, 第二个参数为 数据内容, 第三个参数为 数据的模板    返回一个byte数组
var bytes = ScorpioSerializer.Serialize(_SCRIPT, ser, "Msg_C2G_Test")
//反序列化数据 第一个参数为Script句柄, 第二个参数为数据, 第三个参数为数据模板
var deser = ScorpioSerializer.Deserialize(_SCRIPT, bytes, "Msg_C2G_Test")
//输出数据 sco 暂时不支持 HasSign 的判断
print(deser.Value2)
print(array.count(deser.Value3) + "    " + deser.Value3[2])