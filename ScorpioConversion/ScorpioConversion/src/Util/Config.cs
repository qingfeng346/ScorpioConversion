using Scorpio.Conversion;
using System.IO;
using System.Collections.Generic;
using ExcelDataReader;
using Scorpio.Commons;
using Newtonsoft.Json;
using Scorpio.Conversion;
namespace Scorpio.Conversion {
    public class ExcelFile {
        public string file;
        public string fileName;
        public string fileNameWithoutExtension;
        public string extension;
        public ExcelFile(string file) {
            this.file = file;
            this.fileName = Path.GetFileName(file);
            this.fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
            this.extension = Path.GetExtension(file);
        }
        public IExcelDataReader GetExcelReader(FileStream fileStream) {
            return ExcelReaderFactory.CreateReader(fileStream);
        }
        public override string ToString() {
            return fileName;
        }
    }
    public class Config {
        public static HashSet<string> Tags { get; private set; }                    //标签列表
        public static List<ExcelFile> FileList { get; private set; }                //所有要生成的excel文件
        public static BuildInfo BuildInfo { get; private set; }                //Build信息
        public static Dictionary<string, string> SpawnsList { get; private set; }   //派生类MD5列表
        public static List<L10NData> L10NDatas { get; set; }                        //所有的翻译字段
        public static PackageParser Parser { get; private set; }                    //配置文件解析
        public static void Initialize(string[] configs, string[] files, string[] paths, string[] tags, string lang) {
            SpawnsList = new Dictionary<string, string>();                          //派生文件Layout缓存
            Tags = new HashSet<string>(tags);                                       //需要过滤的文件tags 多tag[{Util.Separator}]隔开
            Parser = new PackageParser();
            foreach (var config in configs) {
                Parser.Parse(config);
            }
            FileList = new List<ExcelFile>();
            foreach (var file in files) {
                if (file.IsExcel()) {
                    FileList.Add(new ExcelFile(file));
                }
            }
            foreach (var path in paths) {
                foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories)) {
                    if (file.IsExcel()) {
                        FileList.Add(new ExcelFile(file));
                    }
                }
            }
            BuildInfo = JsonConvert.DeserializeObject<BuildInfo>(FileUtil.GetFileString(lang));
        }
        public static bool ContainsTags(List<string> tags) {
            if (Tags.Count == 0 || tags.Count == 0) { return true; }
            return Tags.Overlaps(tags);
        }
    }
}