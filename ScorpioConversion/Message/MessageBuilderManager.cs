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
        builder.Append(@"namespace __Package {
public class MessageManager {");
        for (int i = 0; i < mKeys.Count;++i ) {
            string key = mKeys[i];
            string str = @"
    public const int __Filer = __Index;";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", i.ToString());
            builder.Append(str);
        }
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
        builder.Append(@"package __Package;
public class MessageManager {");
        for (int i = 0; i < mKeys.Count; ++i)
        {
            string key = mKeys[i];
            string str = @"
    public static final int __Filer = __Index;";
            str = str.Replace("__Filer", key);
            str = str.Replace("__Index", i.ToString());
            builder.Append(str);
        }
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
        builder.Append(@"//Package = __Package
MessageManager = {");
        for (int i = 0; i < mKeys.Count; ++i)
        {
            string key = mKeys[i];
            string str = @"
    __Filer = __Index,";
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

