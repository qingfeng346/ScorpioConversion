# ScorpioConversion #
* author : while
* QQ群 : 245199668 [加群](http://shang.qq.com/wpa/qunwpa?idkey=8ef904955c52f7b3764403ab81602b9c08b856f040d284f7e2c1d05ed3428de8)

**工具说明**
* 转换Excel配置表为二进制数据
* 配置文件全部使用 sco 脚本配置 Scorpio-CSharp 源码地址 https://github.com/qingfeng346/Scorpio-CSharp
* 示例Excel [Excel](https://github.com/qingfeng346/ScorpioConversion/tree/master/ScorpioConversion/Sample/Excel)
* 示例扩展配置 [Config](https://github.com/qingfeng346/ScorpioConversion/tree/master/ScorpioConversion/Sample/Config)
* 示例info配置 
    * [示例](https://github.com/qingfeng346/ScorpioConversion/blob/master/ScorpioConversion/Sample/info.json)
    * [所有支持的Info配置](https://github.com/qingfeng346/ScorpioConversion/blob/master/ScorpioConversion/Scorpio.Conversion.Engine/src/Util/BuildInfo.cs)
* 执行方式为 命令行执行
* 具体命令行参数可以使用 scov -h 查看

#### 支持的操作系统
- Windows
- MacOS
- Linux

#### 支持的语言
语言    | 生成器            | 需要安装库   
-----   | ----              | ----
c#      | [GeneratorCSharp](https://github.com/qingfeng346/ScorpioConversion/blob/master/ScorpioConversion/Scorpio.Conversion.Engine/src/Generator/GeneratorCSharp.cs)   | [Scorpio.Conversion.Runtime](https://www.nuget.org/packages/Scorpio.Conversion.Runtime)
[sco脚本](https://github.com/qingfeng346/Scorpio-CSharp)      | [GeneratorScorpio](https://github.com/qingfeng346/ScorpioConversion/blob/master/ScorpioConversion/Scorpio.Conversion.Engine/src/Generator/GeneratorScorpio.cs)   | [Scorpio.Conversion.Runtime](https://www.nuget.org/packages/Scorpio.Conversion.Runtime)
Java      | [GeneratorJava](https://github.com/qingfeng346/ScorpioConversion/blob/master/ScorpioConversion/Scorpio.Conversion.Engine/src/Generator/GeneratorJava.cs)   | [Scorpio.Conversion.Runtime.jar](https://github.com/qingfeng346/ScorpioConversion/releases)
Javascript      | [GeneratorJavascript](https://github.com/qingfeng346/ScorpioConversion/blob/master/ScorpioConversion/Scorpio.Conversion.Engine/src/Generator/GeneratorJavascript.cs)   | [Scorpio.Conversion.Runtime](https://www.npmjs.com/package/scorpio.conversion.runtime)

* 更多语言可以自行实现生成器

#### 支持的数据类型
类型    | 类型名1  | 类型名2    | 备注
-----   | ----     | ----       | ----
bool    | bool     | boolean    | 1字节
int8    | int8     | sbyte      | 1字节
uint8   | uint8    | byte       | 1字节
int16   | int16    | short      | 2字节
uint16  | uint16   | ushort     | 2字节
int32   | int32    | int        | 4字节
uint32  | uint32   | uint       | 4字节
int64   | int64    | long       | 8字节
uint64  | uint64   | ulong      | 8字节
float   | float    | float32    | 4字节
double  | double   | float64    | 8字节
string  | string   | string     | urf-8字符串
datetime| datetime | datetime   | 时间戳,8字节
bytes   | bytes    | bytes      | 二进制数据,支持 base64:// file://

* 自定义结构,定义方式参考[示例](https://github.com/qingfeng346/ScorpioConversion/blob/master/Sample/Config/Table.sco)
* 所有数据类型前加上**array**关键字可以定义数组结构,例如 arrayint32

#### Excel表填写示例
![](https://github.com/qingfeng346/qingfeng346.github.io/raw/master/resource/scov.png)
* /FileName		名称,类名 不填默认使用 file或sheet名字
* /Comment		字段注释
* /Name			字段名字
* /Type			字段类型
* /Default		字段默认值
* /Begin		数据开始行(可以有多个Begin)
* /End			数据结束行
* /BeginBranch	分支数据开始行(可以有多个Begin)
* /EndBranch	分支数据结束行