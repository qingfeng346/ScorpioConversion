using System;
using System.Collections.Generic;
using System.Text;

public partial class MessageBuilder {
    public void CreateManagerCSharp()
    {
        var code = PROGRAM.CSharp;
        var programInfo = Util.GetProgramInfo(code);
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//本文件为自动生成，请不要手动修改
using System;
using System.Collections.Generic;
using Scorpio.Message;
namespace __Package {
public class MessageManager {
    public static IMessage parseByteToMsg(int msgType, byte[] buff) {
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
        default: throw new Exception(""找不到MsgType : "" + msgType);
        }
    }
    public static readonly Dictionary<string, int> MessageToID = new Dictionary<string, int>() {");
        foreach (var pair in mKeys) {
            var key = pair.Key;
            var id = pair.Value;
            string str = @"
        {""__Filer"", __Index},";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", id.ToString());
            builder.Append(str);
        }
        builder.Append(@"
    };
    public static readonly Dictionary<int, string> IDToMessage = new Dictionary<int, string>() {");
        foreach (var pair in mKeys) {
            var key = pair.Key;
            var id = pair.Value;
            string str = @"
        {__Index, ""__Filer""},";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", id.ToString());
            builder.Append(str);
        }
        builder.Append(@"
    };
    public static readonly Dictionary<int, Type> IDToType = new Dictionary<int, Type>() {");
        foreach (var pair in mKeys) {
            var key = pair.Key;
            var id = pair.Value;
            string str = @"
        {__Index, typeof(__Filer)},";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", id.ToString());
            builder.Append(str);
        }
        builder.Append(@"
    };
    public static readonly Dictionary<Type, int> TypeToID = new Dictionary<Type, int>() {");
        foreach (var pair in mKeys) {
            var key = pair.Key;
            var id = pair.Value;
            string str = @"
        {typeof(__Filer), __Index},";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", id.ToString());
            builder.Append(str);
        }
        builder.Append(@"
    };
    public static readonly Dictionary<int, IMessage> IDToObject = new Dictionary<int, IMessage>() {");
        foreach (var pair in mKeys) {
            var key = pair.Key;
            var id = pair.Value;
            string str = @"
        {__Index, new __Filer()},";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", id.ToString());
            builder.Append(str);
        }
        builder.Append(@"
    };");
        builder.Append(@"
}
}");
        builder = builder.Replace("__Package", mPackage);
        programInfo.CreateFile("MessageManager", builder.ToString());
    }
}
