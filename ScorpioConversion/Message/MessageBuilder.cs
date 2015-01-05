using System;
using System.Collections.Generic;
using System.Text;


public partial class MessageBuilder
{
    private string mPackage;
    private List<string> mKeys;
    private Dictionary<string, List<PackageField>> mCustoms;
    public void Transform(string path)
    {
        Util.InitializeProgram();
        mCustoms = Util.ParseStructure(path);
        mPackage = Util.GetConfig(ConfigKey.PackageName, ConfigFile.InitConfig);
        mKeys = new List<string>(mCustoms.Keys);
        mKeys.Sort();
        var infos = Util.GetProgramInfos();
        Progress.Count = mCustoms.Count;
        foreach (var pair in mCustoms) {
            ++Progress.Count;
            foreach (var info in infos.Values) {
                if (!info.Create) continue;
                FileUtil.CreateFile(info.GetFile(pair.Key), info.GenerateMessage.Generate(pair.Key, mPackage, pair.Value, false), info.Bom, info.CodeDirectory.Split(';'));
            }
        }
        foreach (var info in infos.Values)
        {
            if (!info.Create) continue;
            info.CreateMessageManager.Invoke(this, null);
        }
    }
}

