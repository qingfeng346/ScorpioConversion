# ScorpioConversion #
* author : while
* QQ群 : 245199668 [加群](http://shang.qq.com/wpa/qunwpa?idkey=8ef904955c52f7b3764403ab81602b9c08b856f040d284f7e2c1d05ed3428de8)

**工具说明**
* 转换策划Excel表为二进制数据
* 开发工具为 .net core, 读取表数据库为 NPOI
* 配置文件全部使用 sco 脚本配置 Scorpio-CSharp 源码地址 https://github.com/qingfeng346/Scorpio-CSharp
* 示例Excel放在 [Sample/Excel](https://github.com/qingfeng346/ScorpioConversion/tree/master/Sample/Excel) 目录下
* 执行方式为 命令行执行
* 具体命令行参数可以使用 scov -h 查看

#### 支持的操作系统
- Windows
- MacOS
- Linux

#### 支持的语言
- c#
- Java
- Go
- Nodejs
- Typescript
- [sco脚本](https://github.com/qingfeng346/Scorpio-CSharp)

#### 支持的数据类型
* bool
* int8(sbyte)
* int16(short)
* int32(int)
* int64(long)
* float
* double
* string
* datetime
* 自定义结构,定义方式参考[示例](https://github.com/qingfeng346/ScorpioConversion/blob/master/Sample/Config/Table.sco)
* 数据类型前加上**array**关键字可以定义数组结构,例如 arrayint32

#### Excel表填写示例
![](https://raw.githubusercontent.com/qingfeng346/ScorpioConversion/master/Sample/Excel/Sample.png)
* /Package		命令空间 namespace
* /FileName		名称,类名 不填默认使用 file或sheet名字
* /Comment		字段注释
* /Name			字段名字
* /Type			字段类型
* /Default		字段默认值
* /Begin		数据开始行(可以有多个Begin)
* /End			数据结束行