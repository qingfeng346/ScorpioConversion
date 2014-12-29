using System;
using System.Collections.Generic;
using System.Text;


public class MessageBuilder
{
    public void Transform(string path, string package, string[] outPaths)
    {
        Util.InitializeProgram();
        var infos = Util.GetProgramInfos();
        Dictionary<string, List<PackageField>> fields = Util.ParseStructure(path);
        foreach (var pair in fields) {
            foreach (var info in infos.Values) {
                string outPath = outPaths[(int)info.Code];
                if (string.IsNullOrEmpty(outPath)) continue;
                FileUtil.CreateFile(pair.Key, info.GenerateMessage.Generate(pair.Key, package, pair.Value, false), info.Bom, outPath.Split(';'));
            }
        }
    }
}

