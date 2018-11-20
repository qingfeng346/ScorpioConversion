using System;
using System.Collections.Generic;
using System.Text;

public partial class MessageBuilder {
    public void CreateManagerCPP()
    {
//        var code = PROGRAM.CPP;
//        var programInfo = Util.GetProgramInfo(code);
//        StringBuilder builder = new StringBuilder();
//        builder.AppendLine(@"#ifndef ____MessageManager_H__
//#define ____MessageManager_H__");
//        builder.Append(TemplateCPP.Head);
//        string[] packages = mPackage.Split('.');
//        foreach (var package in packages) {
//            builder.AppendLine("namespace " + package + "{");
//        }
//        builder.Append(@"//本文件为自动生成，请不要手动修改
//class MessageManager {
//    public: 
//        static std::unordered_map<char *, __int32> MessageToID;
//        static std::unordered_map<__int32, char *> IDToMessage;
//");
//        foreach (var package in packages) {
//            builder.AppendLine("}");
//        }
//        builder.Append("#endif");
//        programInfo.CreateFile("MessageManager", builder.ToString());



//        builder.Append(@"//本文件为自动生成，请不要手动修改
//class MessageManager {
//    public: static std::unordered_map<char *, __int32> MessageToID = new Dictionary<string, int>() {");
//        for (int i = 0; i < mKeys.Count; ++i)
//        {
//            string key = mKeys[i];
//            string str = @"
//        {""__Filer"", __Index},";
//            str = str.Replace("__Filer", key);
//            str = str.Replace("__Index", i.ToString());
//            builder.Append(str);
//        }
//        builder.Append(@"
//    };
//    public static readonly Dictionary<int, string> IDToMessage = new Dictionary<int, string>() {");
//        for (int i = 0; i < mKeys.Count; ++i)
//        {
//            string key = mKeys[i];
//            string str = @"
//        {__Index, ""__Filer""},";
//            str = str.Replace("__Filer", key);
//            str = str.Replace("__Index", i.ToString());
//            builder.Append(str);
//        }
//        builder.Append(@"
//    };
//    public static readonly Dictionary<int, Type> IDToType = new Dictionary<int, Type>() {");
//        for (int i = 0; i < mKeys.Count; ++i)
//        {
//            string key = mKeys[i];
//            string str = @"
//        {__Index, typeof(__Filer)},";
//            str = str.Replace("__Filer", key);
//            str = str.Replace("__Index", i.ToString());
//            builder.Append(str);
//        }
//        builder.Append(@"
//    };
//    public static readonly Dictionary<Type, int> TypeToID = new Dictionary<Type, int>() {");
//        for (int i = 0; i < mKeys.Count; ++i)
//        {
//            string key = mKeys[i];
//            string str = @"
//        {typeof(__Filer), __Index},";
//            str = str.Replace("__Filer", key);
//            str = str.Replace("__Index", i.ToString());
//            builder.Append(str);
//        }
//        builder.Append(@"
//    };");
//        builder.Append(@"
//}
//");
//        builder = builder.Replace("__Package", mPackage);
//        foreach (var package in packages) {
//            builder.AppendLine("}");
//        }
//        builder.Append("#endif");
//        programInfo.CreateFile("MessageManager", builder.ToString());
    }
}
