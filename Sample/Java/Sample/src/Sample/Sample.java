package Sample;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
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
	static void log(Object obj) {
		System.out.println(obj);
	}
	public static void main(String[] args) {
		try {
			Properties properties = System.getProperties();
			String curPath = properties.getProperty("user.dir");

            TableUtil.SetTableUtil(new TableDis());
            TableManager tableManger = new TableManager();
            
            log("======================下面代码是Java读取Data数据=============================");
            log("**********下面是中文**********");
            tableManger.setLanguage("CN");
            //获得 Test 表 ID 为 10001 的数据
            log(tableManger.GetTest().GetElement(10000).toString());
            //获得 关键字表 Spawn_Test1 ID 为 10000 的数据
            log(tableManger.GetSpawns(TableManager.Spawn.Spawn_Test1).GetElement(10000).toString());
            //获得 关键字表 Spawn_Test2 ID 为 10000 的数据
            log(tableManger.GetSpawns_Spawn("Test2").GetElement(10000).toString());
            log("**********下面是英文**********");
            tableManger.Reset();
            tableManger.setLanguage("EN");
            //获得 Test 表 ID 为 10001 的数据
            log(tableManger.GetTest().GetElement(10000).toString());
            //获得 关键字表 Spawn_Test1 ID 为 10000 的数据
            log(tableManger.GetSpawns(TableManager.Spawn.Spawn_Test1).GetElement(10000).toString());
            //获得 关键字表 Spawn_Test2 ID 为 10000 的数据
            log(tableManger.GetSpawns_Spawn("Test2").GetElement(10000).toString());

            log("======================下面代码是java消息序列化代码=============================");
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
            log("序列化后数据长度 : " + bytes.length + " 数据内容 : " + ser.toString());
            //反序列化协议
            Msg_C2G_Test deser = Msg_C2G_Test.Deserialize(bytes);
            log(deser.getValue2());
            log(deser.getValue3().size() + "   " + deser.getValue3().get(2));
            //HasSign函数可以返回此协议是否给某字段赋值了 传入的值为 协议原型定义的索引
            log(deser.HasSign(3));
            log("解析后的数据内容 : " + deser.toString());
            
            log("======================下面代码是Sco脚本读取Data数据=============================");
            //剩下的是使用java版sco的代码 要使用sco  如果不适用sco请删除 Scorpio\Commons\ScorpioSerializer.java文件
            Scorpio.Script script = new Scorpio.Script();
            script.LoadLibrary();
            //请先引入 TableUtil
            script.SetObject("ScorpioSerializer", ScorpioSerializer.class);
            File path = new File(curPath + "/Sco/");
            File[] files = path.listFiles();
            for (File file : files) {
            	script.LoadFile(file.getAbsolutePath());
            }
            script.LoadFile(curPath + "/Sample.sco");
        } catch (Exception ex) {
        	log("Error : " + ex.toString());
        }
	}
}
