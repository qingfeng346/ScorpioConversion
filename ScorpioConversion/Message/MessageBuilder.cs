using System;
using System.Collections.Generic;
using System.Text;


public partial class MessageBuilder
{
    private string mPackage;
    private List<string> mKeys;
    private Dictionary<string, List<PackageField>> mCustoms = new Dictionary<string, List<PackageField>>();
    private Dictionary<string, List<PackageEnum>> mEnums = new Dictionary<string, List<PackageEnum>>();
    public void Transform(string path)
    {
        Util.InitializeProgram();
        Util.ParseStructure(path, ref mCustoms, ref mEnums);
        mPackage = Util.GetConfig(ConfigKey.PackageName, ConfigFile.InitConfig);
        mKeys = new List<string>(mCustoms.Keys);
        mKeys.Sort();
        var infos = Util.GetProgramInfos();
        Progress.Count = mCustoms.Count + mEnums.Count;
        foreach (var pair in mCustoms) {
            ++Progress.Count;
            foreach (var info in infos.Values) {
                if (!info.Create) continue;
                info.CreateFile(pair.Key, info.GenerateMessage.Generate(pair.Key, mPackage, pair.Value, new List<string>(mEnums.Keys), false));
            }
        }
        foreach (var pair in mEnums)
        {
            ++Progress.Count;
            foreach (var info in infos.Values) {
                if (!info.Create) continue;
                info.CreateFile(pair.Key, info.GenerateEnum.Generate(pair.Key, mPackage, pair.Value));
            }
        }
        foreach (var info in infos.Values)
        {
            if (!info.Create) continue;
            info.CreateMessageManager.Invoke(this, null);
        }
    }
}

