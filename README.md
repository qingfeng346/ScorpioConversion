# ScorpioConversion #
* author : while
* QQ群 : 245199668 [加群](http://shang.qq.com/wpa/qunwpa?idkey=8ef904955c52f7b3764403ab81602b9c08b856f040d284f7e2c1d05ed3428de8)

**工具说明**
* 此工具是为了解决网络协议以及策划Excel表热更新
* 配置文件全部使用Scorpio-CSharp脚本配置 Scorpio-CSharp源码地址 https://github.com/qingfeng346/Scorpio-CSharp
* 示例放在Sample目录下，可以运行ScorpioConversion.exe查看
* ![](https://github.com/qingfeng346/ScorpioConversion/blob/master/Sample/Tools/Readme.png)

**此工具包含两个功能**
* 网络协议生成工具
* Excel表转换工具

**各个项目作用**
* Conversion 整个转换工具项目 
* Conversion/Library 转换工具核心代码
* Conversion/ScorpioZip 转换工具gzip压缩库 其实就是 SharpZip 库  修改了gzip压缩写入是写入固定时间 以免每次转表 md5 码都会改变
* Conversion/ScorpioConversion 转换工具界面代码

**支持的语言**
- [x] c#
- [x] java
- [x] sco脚本
- [x] c++

**需要导入的库**
* sco脚本暂时只支持c#版和java版
* 要使用c#语言 请导入**ScorpioProto-CSharp**库 源码地址 https://github.com/qingfeng346/ScorpioProto-CSharp
* 要使用Java语言 请导入**ScorpioProto-Java**库 源码地址 https://github.com/qingfeng346/ScorpioProto-Java
* 要使用c++语言 请导入**ScorpioProto-CPP**库 源码地址 https://github.com/qingfeng346/ScorpioProto-CPP

## 生成文件后使用方法
-----------
**网络协议源文件说明**
```javascript
//测试信息
Msg_C2G_Test = {
    Value1 = "1,int",
	Value2 = "2,string",
	Value3 = "3,int,true",
}
//嵌套类型使用
Msg_C2G_Test1 = {
	Value1 = "1,Msg_C2G_Test",
}
/*
定义一个名为**Msg_C2G_Test**的协议
Key值为字段名
Value值格式为  字段索引(不能重复),字段类型,字段是否为数组(不填默认为不是数组)
例如 Value1字段 Value1为字段名,索引为1,类型为int型,不是数组
*/
```
**网络协议生成的代码文件** 
* 例如协议名为 **Msg_C2G_Test** 工具会生成对应语言的名为**Msg_C2G_Test.xx 以及 MessageManager.xx(自动生成的协议名和ID的对应文件)**文件，具体要生成什么语言可以配置
	* **Sco 脚本** 可以直接使用 **ScorpioSerializer.Deserialize** 和 **ScorpioSerializer.Serialize** 进行序列化和反序列化
	* **其他语言** 可以直接使用 **Msg_C2G_Test**类的 **Deserialize函数** 和 **Serialize函数** 进行序列化和反序列化

**网络协议源文件支持数据类型**
* bool
* int8
* int16
* int32
* int
* int64
* float
* double
* string
* bytes

**Excel表转换工具**
* 请自行实现一个继承 **TableUtil.ITableUtil** 的类 然后调用 **TableUtil.SetTableUtil** 设置
* **new** 一个 **TableManager** 对象,然后就可以调用 **GetXXX** 函数获取数据了
* 继承 **ITableUtil** 的类是为了实现读取文件的操作 传入文件名 返回 byte[]

**Excel表填写示例**
* ![](https://github.com/qingfeng346/ScorpioConversion/blob/master/Sample/Tools/Excel.png)
* 第一行为字段注释 没有实际作用 转换的时候会自动添加上 字段注释
* 第二行为字段名称 不能重复
* 第三行为字段属性(暂时无用)
* 第四行为字段默认值 此列的数据如果不填 则默认为此值 (例如:7行C列没有填值,转换的时候则会使用4行C列的值,也就是[aaa])
* 第五行为字段类型 目前支持的基础数据类型有:
	* bool
	* int8
	* int16
	* int32
	* int
	* int64
	* float
	* double
	* string
* 第一列的数据类型固定为int型,并且必须唯一
* 如果字段类型前面 加上 array关键字,例如 arrayint arraybool arraystring  则此字段为数组类型
* 字段类型还支持 自定义数据结构 和 枚举类型，定义的格式请参照 https://github.com/qingfeng346/ScorpioConversion/blob/master/Sample/Tools/ExcelConfig/Table.sco
* 转表工具还支持批量表转换,请先在转表工具界面关键字列表输入框设置批量表关键字,多个关键字请以**;**隔开
* 然后以关键字开头的表会转成一个map数组的数据,关键字表的字段数据格式必须一致,具体用法可以参考 Sample/Excel 下的 Spawn_Test1.xls Spawn_Test2.xls
* Sample下是使用示例,Tools文件夹下是原始文件,使用前请阅读相应语言下的**Readme.txt**文件

## master版本更新和修改内容 ##
(2016-5-11)
-----------
* 修复一处c++内存泄漏的问题

(2016-5-10)
-----------
* 增加对c++语言的支持 (ps:Excel表转换的类没有添加释放函数)

(2016-5-6)
-----------
* Excel表头增加一行,第三行为字段属性
* 整理代码结构,重新调整工具界面,整理Winform和GTK公用代码
* GTK工具补上TinyPNG

(2016-3-4)
-----------
* 增加GTK#界面工程,可以跨平台运行转换工具,暂时缺少功能TinyPNG

(2015-12-29)
-----------
* 整合TinyPNG图片压缩工具 TinyPNG的具体功能请查看 https://tinypng.com/

(2015-12-28)
-----------
* 修改生成Data文件压缩选项不起作用的BUG
* 修复ScorpioZip GZIP压缩 写入时间的问题

(2015-12-24)
-----------
* 更新MD5库为第三方库,避免有些机器上使用系统MD5算法报错(System.InvalidOperationException: 此实现不是 Windows 平台 FIPS 验证的加密算法的一部分。)