using System;
using System.Collections.Generic;
using System.Text;

public partial class MessageBuilder
{
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
