using System.IO;
using System.Collections.Generic;
using Scorpio.Commons;
using System.Linq;
using Tomlyn;
namespace Scorpio.Conversion.Engine {
    public class Config {
        public static HashSet<string> Tags { get; private set; }                    //标签列表
        public static HashSet<string> Branches { get; private set; }
        public static List<string> Files { get; private set; }                      //所有配置文件
        public static BuildInfo BuildInfo { get; private set; }                     //Build信息
        public static Dictionary<string, string> SpawnsList { get; private set; }   //派生类MD5列表
        public static List<L10NData> L10NDatas { get; set; }                        //所有的翻译字段
        public static PackageParser Parser { get; private set; }                    //配置文件解析
        public static void Initialize(string[] configs, string[] files, string[] paths, string[] tags, string[] branches, string info) {
            SpawnsList = new Dictionary<string, string>();                          //派生文件Layout缓存
            Tags = new HashSet<string>(tags);                                       //需要过滤的文件tags
            Branches = new HashSet<string>(branches);
            Parser = new PackageParser();
            foreach (var config in configs) {
                Parser.Parse(config);
            }
            Files = new List<string>();
            Files.AddRange(files);
            foreach (var path in paths) {
                Files.AddRange(Directory.GetFiles(path, "*", SearchOption.AllDirectories).Where(_ => _.IsExcel()));
            }
            var tomlOption = new TomlModelOptions();
            tomlOption.ConvertPropertyName = name => name;
            BuildInfo = Toml.ToModel<BuildInfo>(FileUtil.GetFileString(info), null, tomlOption);
        }
        public static bool ContainsTags(string[] tags) {
            if (tags == null || tags.Length == 0 || Tags.Count == 0) { return true; }
            return Tags.Overlaps(tags);
        }
        public static bool ContainsBranches(string[] branches) {
            return Branches.Overlaps(branches);
        }
    }
}