using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
public partial class MessageBuilder
{
    private string mPackage;
    private List<string> mKeys;
    private Dictionary<string, List<PackageField>> mCustoms = new Dictionary<string, List<PackageField>>();
    private Dictionary<string, List<PackageEnum>> mEnums = new Dictionary<string, List<PackageEnum>>();
    public void Transform(string path)
    {
        try {
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
                    info.CreateFile(pair.Key, info.GenerateMessage.Generate(pair.Key, mPackage, pair.Value, false));
                }
            }
            foreach (var pair in mEnums) {
                ++Progress.Count;
                foreach (var info in infos.Values) {
                    if (!info.Create) continue;
                    info.CreateFile(pair.Key, info.GenerateEnum.Generate(pair.Key, mPackage, pair.Value));
                }
            }
            foreach (var info in infos.Values) {
                if (!info.Create) continue;
                info.CreateMessageManager.Invoke(this, null);
            }
        } catch (Exception ex) {
            string error = "转换消息出错 " + ex.ToString();
            Logger.error(error);
            MessageBox.Show(error);
        }
    }
}