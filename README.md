# ScorpioConversion #
* author : while
* QQ群 : 245199668 [加群](http://shang.qq.com/wpa/qunwpa?idkey=8ef904955c52f7b3764403ab81602b9c08b856f040d284f7e2c1d05ed3428de8)

**此工具包含两个功能**
* 网络协议生成工具
* Excel表转换工具
* 示例放在Sample目录下，可以运行ScorpioConversion.exe查看

各个项目作用
* Conversion 整个转换工具项目 
* Conversion/Library 转换工具核心代码
* Conversion/ScorpioZip 转换工具gzip压缩库 其实就是 SharpZip 库  修改了gzip压缩写入是写入固定时间 以免每次转表 md5 码都会改变
* Conversion/ScorpioConversion 转换工具界面代码
* ScorpioCommons 数据解析库 网络协议以及转表数据解析都会用到 如果要使用此工具 项目代码里面要拷入此库
* ScorpioCommons/CSharp c#解析库 以及c#下使用Scorpio的解析
* ScorpioCommons/Java java解析库 以及java下使用Scorpio的解析


## 生成文件后使用方法

网络协议生成的代码文件 
-----------
* 例如协议名为 Msg_C2G_Test 工具会生成 Msg_C2G_Test.cs Msg_C2G_Test.java Msg_C2G_Test.sco 文件具体要生成什么语言可以配置
	* c#和java 语言 可以直接使用 Msg_C2G_Test.Deserialize 和 Msg_C2G_Test.Deserialize 就可以完成 byte[] 和 协议之间的转换
	* Scorpio 脚本 可以使用 ScorpioSerializer.Deserialize 和 ScorpioSerializer.Serialize 完成转换

Excel表转换工具
-----------
* 请自行实现一个继承 **TableUtil.ITableUtil** 的类 然后调用 **TableUtil.SetTableUtil** 设置
* **new** 一个 **TableManager** 对象,然后就可以调用GetXXX函数获取数据了
* 继承 **ITableUtil** 的类是为了实现读取文件的操作 传入文件名 返回 byte[]

## master版本更新和修改内容 ##
(2015-12-24)
-----------
* 更新MD5库为第三方库,避免有些机器上使用系统MD5算法报错(System.InvalidOperationException: 此实现不是 Windows 平台 FIPS 验证的加密算法的一部分。)