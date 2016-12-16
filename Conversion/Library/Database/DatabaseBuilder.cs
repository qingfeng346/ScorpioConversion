using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class DatabaseBuilder
{
    private string mPackage;
    private Dictionary<string, List<PackageField>> mCustoms = new Dictionary<string, List<PackageField>>();
    private Dictionary<string, List<PackageEnum>> mEnums = new Dictionary<string, List<PackageEnum>>();
    private Dictionary<string, PackageDatabase> mDatabases = new Dictionary<string, PackageDatabase>();
    public void Transform(string configPath, string package, Dictionary<PROGRAM, ProgramConfig> programConfigs)
    {
        try {
            Util.InitializeProgram(programConfigs);
            Util.ParseStructure(configPath, null, mEnums, null, mDatabases, mCustoms, null);
            mPackage = package;
            var info = Util.GetProgramInfo(PROGRAM.Java);
            Progress.Count = mCustoms.Count + mEnums.Count + mDatabases.Count;
            Progress.Current = 0;
            foreach (var pair in mDatabases) {
                ++Progress.Current;
                foreach (var table in pair.Value.tables) {
                    Logger.info("正在转换数据库 {0}/{1} [{2}.{3}]", Progress.Current, Progress.Count, pair.Key, table.Key);
                    info.CreateFile("Database" + table.Key, new GenerateDatabaseJava().Generate(table.Key, mPackage, table.Value));
                }
            }
            foreach (var pair in mCustoms) {
                ++Progress.Current;
                Logger.info("正在转换类 {0}/{1} [{2}]", Progress.Current, Progress.Count, pair.Key);
                info.CreateFile(pair.Key, new GenerateDatabaseTableJava().Generate(pair.Key, mPackage, pair.Value));
            }
            foreach (var pair in mEnums) {
                ++Progress.Current;
                Logger.info("正在转换枚举 {0}/{1} [{2}]", Progress.Current, Progress.Count, pair.Key);
                info.CreateFile(pair.Key, info.GenerateEnum.Generate(pair.Key, mPackage, pair.Value));
            }
        } catch (Exception ex) {
            Logger.error("转换消息出错 " + ex.ToString());
        }
    }
}

