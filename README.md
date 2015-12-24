ScorpioConversion
==============
## 此工具包含两个功能 
* 网络协议生成工具
* Excel表转换工具
## 示例放在Sample目录下，可以运行ScorpioConversion.exe查看

各个项目作用
* Conversion 整个转换工具项目 
* Conversion/Library 转换工具核心代码
* Conversion/ScorpioZip 转换工具gzip压缩库 其实就是 SharpZip 库  修改了gzip压缩写入是写入固定时间 以免每次转表 md5 码都会改变
* Conversion/ScorpioConversion 转换工具界面代码

* ScorpioCommons 数据解析库 网络协议以及转表数据解析都会用到 如果要使用此工具 项目代码里面要拷入此库
* ScorpioCommons/CSharp c#解析库 以及c#下使用Scorpio的解析
* ScorpioCommons/Java java解析库 以及java下使用Scorpio的解析


## 生成文件后使用方法
* 网络协议生成的代码文件 
** c#或java 例如协议名为 Msg_C2G_Test 工具会生成 Msg_C2G_Test.cs 或 Msg_C2G_Test.java 文件 直接使用 Msg_C2G_Test.Deserialize 和 Msg_C2G_Test.Deserialize 就可以完成 byte[] 和 协议之间的转换
** Scorpio 例如协议名为 Msg_C2G_Test 工具会生成 Msg_C2G_Test.sco 文件 可以使用 ScorpioSerializer.Deserialize 和 ScorpioSerializer.Serialize 完成转换
* Excel表转换工具
** 请自行实现一个继承 TableUtil.ITableUtil 的类 然后调用 TableUtil.SetTableUtil 设置后 直接 new 一个 TableManager 对象 就可以 直接使用了 ITableUtil 是为了实现读取文件的操作 传入文件名 返回 byte[]