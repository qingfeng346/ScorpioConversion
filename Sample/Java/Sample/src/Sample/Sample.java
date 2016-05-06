package Sample;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

import Scorpio.Table.TableUtil;
import Scorpio.Commons.ScorpioSerializer;
import ScorpioProtoTest.Msg_C2G_Test;
import ScorpioProtoTest.TableManager;

public class Sample {
	public static class TableDis implements TableUtil.ITableUtil {
		@Override
		public byte[] GetBuffer(String resource) {
			try {
				Properties properties = System.getProperties();
		    	ByteArrayOutputStream output = new ByteArrayOutputStream();
				FileInputStream stream = new FileInputStream(new File(properties.getProperty("user.dir") + "/Data/" + resource + ".data"));
		        int n = 0;
		        byte[] buffer = new byte[4096];
		        while (-1 != (n = stream.read(buffer))) {
		            output.write(buffer, 0, n);
		        }
		        stream.close();
		        return output.toByteArray();
			} catch (Exception e) {
				return null;
			}
		}
		@Override
		public void Warning(String str) {
			System.out.println(str);
		}
	}
	public static void main(String[] args) {
		try {
			Properties properties = System.getProperties();
			String curPath = properties.getProperty("user.dir");

            TableUtil.SetTableUtil(new TableDis());
            
            System.out.println("======================下面代码是Java读取Data数据=============================");
            TableManager tableManger = new TableManager();
            //输出 Test 表 ID 为 10001 的数据 的 TestString 字段
            System.out.println(tableManger.GetTest().GetElement(10001).getTestString());
            //输出 Test 表的数据数量
            System.out.println(tableManger.GetTest().Count());
            //数据关键字表 Spawn_Test1 ID 为 10000 的 TestString 字段
            System.out.println(tableManger.GetSpawns(TableManager.Spawn.Spawn_Test1).GetElement(10000).getTestString());
            //也可以使用字符串获取关键字表
            System.out.println(tableManger.GetSpawns_Spawn("Test1").GetElement(10000).getTestString());

            System.out.println("======================下面代码是java消息序列化代码=============================");
            Msg_C2G_Test ser = new Msg_C2G_Test();
            ser.setValue2("123123");
            List<Integer> list = new ArrayList<Integer>();
            list.add(1);
            list.add(2);
            list.add(3);
            list.add(4);
            list.add(5);
            list.add(6);
            ser.setValue3(list);
            //序列化协议
            byte[] bytes = ser.Serialize();
            System.out.println("数据长度 " + bytes.length);
            //反序列化协议
            Msg_C2G_Test deser = Msg_C2G_Test.Deserialize(bytes);
            System.out.println(deser.getValue2());
            System.out.println(deser.getValue3().size() + "   " + deser.getValue3().get(2));
            //HasSign函数可以返回此协议是否给某字段赋值了 传入的值为 协议原型定义的索引
            System.out.println(deser.HasSign(3));
            System.out.println("======================下面代码是Sco脚本读取Data数据=============================");
            
            //剩下的是使用java版sco的代码 要使用sco  如果不适用sco请删除 Scorpio\Commons\ScorpioSerializer.java文件和Scorpio\Table\TableUtil.java文件的ReadData函数
            Scorpio.Script script = new Scorpio.Script();
            script.LoadLibrary();
            //请先引入 TableUtil
            script.SetObject("TableUtil", TableUtil.class);
            File path = new File(curPath + "/Sco/");
            File[] files = path.listFiles();
            for (File file : files) {
            	script.LoadFile(file.getAbsolutePath());
            }
            script.LoadString("" + 
//输出 Test 表 ID 为 10001 的数据 的 TestString 字段 (注意 脚本里面是直接使用的变量字段名 c#里面是使用的函数)
"print(TableManager.GetTest().GetElement(10001).TestString)" +
//输出 Test 表的数据数量
"print(TableManager.GetTest().Count());" + 
//数据关键字表 Spawn_Test1 ID 为 10000 的 TestString 字段 如果想动态获取请使用TableManager[""GetSpawn_"" + ""Test1""]
"print(TableManager.GetSpawn_Test1().GetElement(10000).TestString)" +
//数据关键字表 Spawn_Test2 ID 为 10000 的 TestString 字段
"print(TableManager.GetSpawn_Test2().GetElement(10000).TestString)"
);
            System.out.println("======================下面代码是Sco消息序列化代码=============================");
            //请先引入 ScorpioSerializer
            script.SetObject("ScorpioSerializer", ScorpioSerializer.class);
            //引入Array 获取 数组长度
            script.SetObject("Array", Array.class);
            script.LoadString("" + 
    		//先初始化数据Table 要跟序列化的数据字段名字对上 例如要使用 Msg_C2G_Test
    		"var ser = {Value2 = \"123123\", Value3 = [1,2,3,4,5,6]}" +
    		//序列化数据 第一个参数传入 Script句柄  _SCRIPT为Script句柄, 第二个参数为 数据内容, 第三个参数为 数据的模板
    		"var bytes = ScorpioSerializer.Serialize(_SCRIPT, ser, \"Msg_C2G_Test\")" + 
    		//输出数据长度
    		"print(\"数据长度\" + Array.getLength(bytes))" +
    		//反序列化数据 第一个参数为Script句柄, 第二个参数为数据, 第三个参数为数据模板, 第四个参数固定为true
    		"var deser = ScorpioSerializer.Deserialize(_SCRIPT, bytes, \"Msg_C2G_Test\", true)" +
    		//输出数据 sco 暂时不支持 HasSign 的判断
    		"print(deser.Value2)" + 
    		"print(array.count(deser.Value3) + \"   \" + deser.Value3[2])"
);
        } catch (Exception ex) {
        	System.out.println("Error : " + ex.toString());
        }
	}
}
