using System;
using System.Collections.Generic;
using System.Text;

public partial class MessageBuilder
{
    public void CreateManagerJava()
    {
        var code = PROGRAM.Java;
        var programInfo = Util.GetProgramInfo(code);
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//本文件为自动生成，请不要手动修改
package __Package;
import java.util.HashMap;
import Scorpio.Message.IMessage;
@SuppressWarnings(""serial"")
public class MessageManager {
    public static final IMessage parseByteToMsg(int msgType, byte[] buff) {
        switch (msgType) {");
        foreach (var pair in mKeys) {
            var key = pair.Key;
            var id = pair.Value;
            string str = @"
        case __Index: return __Filer.Deserialize(buff);";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", id.ToString());
            builder.Append(str);
        }
        builder.Append(@"
        default: throw new RuntimeException(""找不到MsgType : "" + msgType);
        }
    };
    public static final HashMap<String, Integer> MessageToID = new HashMap<String, Integer>() {
        {");
        foreach (var pair in mKeys) {
            var key = pair.Key;
            var id = pair.Value;
            string str = @"
            put(""__Filer"", __Index);";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", id.ToString());
            builder.Append(str);
        }
        builder.Append(@"
        }
    };
    public static final HashMap<Integer, String> IDToMessage = new HashMap<Integer, String>() {
        {");
        foreach (var pair in mKeys) {
            var key = pair.Key;
            var id = pair.Value;
            string str = @"
            put(__Index, ""__Filer"");";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", id.ToString());
            builder.Append(str);
        }
        builder.Append(@"
        }
    };
    public static final HashMap<Integer, Class<?>> IDToType = new HashMap<Integer, Class<?>>() {
        {");
        foreach (var pair in mKeys) {
            var key = pair.Key;
            var id = pair.Value;
            string str = @"
            put(__Index, __Filer.class);";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", id.ToString());
            builder.Append(str);
        }
        builder.Append(@"
        }
    };
    public static final HashMap<Class<?>, Integer> TypeToID = new HashMap<Class<?>, Integer>() {
        {");
        foreach (var pair in mKeys) {
            var key = pair.Key;
            var id = pair.Value;
            string str = @"
            put(__Filer.class, __Index);";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", id.ToString());
            builder.Append(str);
        }
        builder.Append(@"
        }
    };
    public static final HashMap<Integer, IMessage> IDToObject = new HashMap<Integer, IMessage>() {
        {");
        foreach (var pair in mKeys) {
            var key = pair.Key;
            var id = pair.Value;
            string str = @"
            put(__Index, new __Filer());";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", id.ToString());
            builder.Append(str);
        }
        builder.Append(@"
        }
    };");
        builder.Append(@"
}");
        builder = builder.Replace("__Package", mPackage);
        programInfo.CreateFile("MessageManager", builder.ToString());
    }
}
