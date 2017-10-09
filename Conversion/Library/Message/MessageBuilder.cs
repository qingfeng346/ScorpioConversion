using System;
using System.Collections.Generic;
using System.Text;
public partial class MessageBuilder
{
    private string mPackage;
    private Dictionary<string, int> mKeys = new Dictionary<string, int>();
    private Dictionary<string, List<PackageField>> mCustoms = new Dictionary<string, List<PackageField>>();
    private Dictionary<string, List<PackageEnum>> mEnums = new Dictionary<string, List<PackageEnum>>();
    private Dictionary<string, List<PackageConst>> mConsts = new Dictionary<string, List<PackageConst>>();
    public void Transform(string configPath, string package, Dictionary<PROGRAM, ProgramConfig> programConfigs)
    {
        try {
            Util.InitializeProgram(programConfigs);
            Util.ParseStructure(configPath, mCustoms, mEnums, null, null, null, mConsts);
            mPackage = package;
            var keys = new List<string>(mCustoms.Keys);
            keys.Sort();
            for (int i = 0; i< keys.Count; ++i) {
                mKeys[keys[i]] = i;
            }
            //mKeys = new List<string>(mCustoms.Keys);
            //mKeys.Sort();
            var infos = Util.GetProgramInfos();
            Progress.Count = mCustoms.Count + mEnums.Count + mConsts.Count;
            Progress.Current = 0;
            foreach (var pair in mCustoms) {
                ++Progress.Current;
                Logger.info("正在生成消息 {0}/{1} [{2}]", Progress.Current, Progress.Count, pair.Key);
                foreach (var info in infos.Values) {
                    if (!info.Create) continue;
                    info.CreateFile(pair.Key, info.GenerateMessage.Generate(pair.Key, mPackage, pair.Value, mKeys));
                }
            }
            foreach (var pair in mEnums) {
                ++Progress.Current;
                Logger.info("正在转换枚举 {0}/{1} [{2}]", Progress.Current, Progress.Count, pair.Key);
                foreach (var info in infos.Values) {
                    if (!info.Create) continue;
                    info.CreateFile(pair.Key, info.GenerateEnum.Generate(pair.Key, mPackage, pair.Value));
                }
            }
            foreach (var pair in mConsts) {
                ++Progress.Current;
                Logger.info("正在转换常量 {0}/{1} [{2}]", Progress.Current, Progress.Count, pair.Key);
                foreach (var info in infos.Values) {
                    if (!info.Create) continue;
                    info.CreateFile(pair.Key, info.GenerateConst.Generate(pair.Key, mPackage, pair.Value));
                }
            }
            foreach (var info in infos.Values) {
                if (!info.Create) continue;
                if (info.CreateMessageManager == null) {
                    Logger.error("找不到生成MessageManager函数 " + info.Code);
                } else {
                    info.CreateMessageManager.Invoke(this, null);
                }
            }
        } catch (Exception ex) {
            Logger.error("转换消息出错 " + ex.ToString());
        }
    }
}