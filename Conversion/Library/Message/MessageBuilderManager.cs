using System;
using System.Collections.Generic;
using System.Text;

public partial class MessageBuilder
{
    public void CreateManagerCSharp()
    {
        var code = PROGRAM.CSharp;
        var programInfo = Util.GetProgramInfo(code);
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//本文件为自动生成，请不要手动修改
using System;
using System.Collections.Generic;
namespace __Package {
public class MessageManager {
    public static readonly Dictionary<string, int> MessageToID = new Dictionary<string, int>() {");
        for (int i = 0; i < mKeys.Count;++i ) {
            string key = mKeys[i];
            string str = @"
        {""__Filer"", __Index},";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", i.ToString());
            builder.Append(str);
        }
        builder.Append(@"
    };
    public static readonly Dictionary<int, string> IDToMessage = new Dictionary<int, string>() {");
        for (int i = 0; i < mKeys.Count; ++i)
        {
            string key = mKeys[i];
            string str = @"
        {__Index, ""__Filer""},";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", i.ToString());
            builder.Append(str);
        }
        builder.Append(@"
    };
    public static readonly Dictionary<int, Type> IDToType = new Dictionary<int, Type>() {");
        for (int i = 0; i < mKeys.Count; ++i)
        {
            string key = mKeys[i];
            string str = @"
        {__Index, typeof(__Filer)},";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", i.ToString());
            builder.Append(str);
        }
        builder.Append(@"
    };
    public static readonly Dictionary<Type, int> TypeToID = new Dictionary<Type, int>() {");
        for (int i = 0; i < mKeys.Count; ++i)
        {
            string key = mKeys[i];
            string str = @"
        {typeof(__Filer), __Index},";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", i.ToString());
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
    public void CreateManagerJava()
    {
        var code = PROGRAM.Java;
        var programInfo = Util.GetProgramInfo(code);
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//本文件为自动生成，请不要手动修改
package __Package;
import java.util.HashMap;
@SuppressWarnings(""serial"")
public class MessageManager {
    public static final HashMap<String, Integer> MessageToID = new HashMap<String, Integer>() {
        {");
        for (int i = 0; i < mKeys.Count; ++i) {
            string key = mKeys[i];
            string str = @"
            put(""__Filer"", __Index);";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", i.ToString());
            builder.Append(str);
        }
        builder.Append(@"
        }
    };
    public static final HashMap<Integer, String> IDToMessage = new HashMap<Integer, String>() {
        {");
        for (int i = 0; i < mKeys.Count; ++i) {
            string key = mKeys[i];
            string str = @"
            put(__Index, ""__Filer"");";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", i.ToString());
            builder.Append(str);
        }
        builder.Append(@"
        }
    };
    public static final HashMap<Integer, Class<?>> IDToType = new HashMap<Integer, Class<?>>() {
        {");
        for (int i = 0; i < mKeys.Count; ++i) {
            string key = mKeys[i];
            string str = @"
            put(__Index, __Filer.class);";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", i.ToString());
            builder.Append(str);
        }
        builder.Append(@"
        }
    };
    public static final HashMap<Class<?>, Integer> TypeToID = new HashMap<Class<?>, Integer>() {
        {");
        for (int i = 0; i < mKeys.Count; ++i) {
            string key = mKeys[i];
            string str = @"
            put(__Filer.class, __Index);";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", i.ToString());
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
    public void CreateManagerScorpio()
    {
        var code = PROGRAM.Scorpio;
        var programInfo = Util.GetProgramInfo(code);
        StringBuilder builder = new StringBuilder();
        builder.Append(@"//本文件为自动生成，请不要手动修改
//Package = __Package
MessageManager = {");
        for (int i = 0; i < mKeys.Count; ++i)
        {
            string key = mKeys[i];
            string str = @"
    '__Filer' = __Index,
    __Index = '__Filer',";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", i.ToString());
            builder.Append(str);
        }
        builder.Append(@"
}");
        builder = builder.Replace("__Package", mPackage);
        programInfo.CreateFile("MessageManager", builder.ToString());
    }
}

