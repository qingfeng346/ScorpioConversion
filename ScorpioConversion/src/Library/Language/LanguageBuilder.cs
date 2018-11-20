using System.IO;
using System.Collections.Generic;
//using NPOI.SS.UserModel;
//using NPOI.HSSF.UserModel;
//多国语言的单个元素
public class Language {
    public string Key = "";             //Key值(等于 [表名]_[字段名]_[行ID])
    public string Value = "";           //多国语言值
    public Language(string key, string value) {
        Key = key;
        Value = value;
    }
}
//一个表的多国语言
public class LanguageTable {
    //Key 值
    public Dictionary<string, Language> Languages = new Dictionary<string, Language>();
}
public partial class LanguageBuilder {
    //Translation表原始数据 旧数据
    private class LanguageEntry {
        public string Key;      //关键字
        public string Value;    //中文字符串
        public Dictionary<string, string> KeyValue = null;      //其他语言字符串
        public LanguageEntry(string key, string value, Dictionary<string, string> keyValue) {
            this.Key = key;
            this.Value = value;
            this.KeyValue = keyValue;
        }
    }
    private Dictionary<string, LanguageEntry> m_CacheLanguages = new Dictionary<string, LanguageEntry>();   //原始数据
    private Dictionary<string, LanguageTable> m_Tables;     //最新的翻译数据
    private string[] m_Languages;           //多国语言列表
    private string m_LanguageDirectory;     //Language_XXX.xls文件生成目录
    private string m_TranslationDirectory;  //翻译表保存目录
    private string m_TranslationFile;       //翻译表
    private string m_TranslationBackUp;     //翻译表备份
    public LanguageBuilder(string languages, string languageDirectory, string translationDirectory, Dictionary<string, LanguageTable> tables) {
        var list = new List<string>();
        list.AddRange(languages.Split(';'));
        while (list.Remove("")) { }
        m_Languages = list.ToArray();
        m_LanguageDirectory = languageDirectory;
        m_TranslationDirectory = translationDirectory;
        m_Tables = tables;
        m_TranslationFile = m_TranslationDirectory + "/Translation.xls";
        m_TranslationBackUp = m_TranslationDirectory + "/Translation备份.xls";
    }
    public string[] GetLanguages() {
        return m_Languages;
    }
    public bool Check() {
        int length = m_Languages.Length;
        if (length == 0) {
            Logger.error("多国语言列表格式错误，列表以【;】隔开，第一个语言为默认语言，单个语言不能为空");
            return false;
        }
        for (int i = 0; i < length; ++i) {
            if (string.IsNullOrEmpty(m_Languages[i])) {
                Logger.error("多国语言列表格式错误，列表以【;】隔开，第一个语言为默认语言，单个语言不能为空");
                return false;
            }
        }
        return true;
    }
    public bool Build() {
        if (!Check()) {
            return false;
        }
        GetCacheLanguages();
        CreateTranslation();
        RefreshLanguage();
        return true;
    }
    //获取就得翻译表数据
    public void GetCacheLanguages() {
        //Logger.info("正在获取旧翻译数据");
        //if (!FileUtil.FileExist(m_TranslationFile)) { return; }
        //IWorkbook workbook = new HSSFWorkbook(new FileStream(m_TranslationFile, FileMode.Open, FileAccess.Read));
        //foreach (var pair in m_Tables) {
        //    try {
        //        ISheet sheet = workbook.GetSheet(pair.Key);
        //        if (sheet == null) continue;
        //        //获得Translation表第一行数据
        //        IRow firstRow = sheet.GetRow(0);
        //        //查出所有的多国语言
        //        //0 列是关键字 1列是中文
        //        List<string> languages = new List<string>();
        //        for (int i = 2; i < firstRow.LastCellNum; ++i) {
        //            languages.Add(Util.GetCellString(firstRow.GetCell(i)));
        //        }
        //        //查出旧的翻译
        //        for (int i = 1; i <= sheet.LastRowNum; ++i) {
        //            IRow row = sheet.GetRow(i);
        //            if (row == null) continue;
        //            var key = Util.GetCellString(row.GetCell(0));       //取出关键字
        //            if (string.IsNullOrEmpty(key)) continue;
        //            var value = Util.GetCellString(row.GetCell(1));     //取出中文
        //            Dictionary<string, string> keyValue = new Dictionary<string, string>();
        //            for (int j = 0; j < languages.Count; ++j) {
        //                keyValue.Add(languages[j], Util.GetCellString(row.GetCell(2 + j)));     //取出其他语言
        //            }
        //            m_CacheLanguages[key] = new LanguageEntry(key, value, keyValue);
        //        }
        //    } catch (System.Exception e) {
        //        Logger.error("GetCacheLanguages is error {0} : {1}", pair.Key, e.ToString());
        //    }
        //}
    }
    //创建最新的翻译表
    public void CreateTranslation() {
        Logger.info("正在生成新的翻译表");
        //IWorkbook workbook = new HSSFWorkbook();
        ////创建标记有修改的行
        //ICellStyle changeStyle = workbook.CreateCellStyle();
        //changeStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
        //changeStyle.FillPattern = FillPattern.SolidForeground;

        //foreach (var pair in m_Tables) {
        //    ISheet sheet = workbook.CreateSheet(pair.Key);
        //    sheet.DefaultColumnWidth = 50;
        //    var creationHelper = workbook.GetCreationHelper();
        //    var patriarch = sheet.CreateDrawingPatriarch();
        //    IRow firstRow = sheet.CreateRow(0);
        //    firstRow.CreateCell(0).SetCellValue("关键字");
        //    for (int i = 0; i < m_Languages.Length; ++i) {
        //        firstRow.CreateCell(i + 1).SetCellValue(m_Languages[i]);
        //    }
        //    int count = 1;          //行数
        //    bool changed = false;   //此Sheet是否有改变
        //    foreach (var lanPair in pair.Value.Languages) {
        //        var language = lanPair.Value;
        //        IRow row = sheet.CreateRow(count);                      //创建一行
        //        row.CreateCell(0).SetCellValue(lanPair.Key);            //第一列写入关键字
        //        row.CreateCell(1).SetCellValue(language.Value);         //第二列写入中文
        //        bool rowChanged = false;                                //本行是否有改变
        //        //源文件没有包含此关键字 证明此句翻译是新加的
        //        if (!m_CacheLanguages.ContainsKey(language.Key)) {
        //            changed = true;
        //            rowChanged = true;
        //        } else {
        //            var data = m_CacheLanguages[language.Key];
        //            //源文件中文和最新中文不一致
        //            if (data.Value != language.Value) {
        //                changed = true;
        //                rowChanged = true;
        //                row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
        //                //如果中文有改变则创建一个注释,注释内容为旧的文本
        //                try {
        //                    row.GetCell(1).RemoveCellComment();
        //                    IClientAnchor anchor = creationHelper.CreateClientAnchor();
        //                    anchor.Col1 = 1;
        //                    anchor.Col2 = 2;
        //                    anchor.Row1 = row.RowNum;
        //                    anchor.Row2 = row.RowNum + 3;
        //                    IComment comment = patriarch.CreateCellComment(anchor);
        //                    comment.String = new HSSFRichTextString(data.Value);
        //                    row.GetCell(1).CellComment = comment;
        //                } catch (System.Exception) {
        //                    //如果创建中文失败 则在最后一行创建一个内容为旧文本的格子
        //                    row.CreateCell(m_Languages.Length + 1).SetCellValue(data.Value);
        //                }
        //            }
        //            for (int i = 1; i < m_Languages.Length; ++i) {
        //                if (data.KeyValue.ContainsKey(m_Languages[i]))
        //                    row.CreateCell(i + 1).SetCellValue(data.KeyValue[m_Languages[i]]);
        //            }
        //        }
        //        if (rowChanged) {
        //            row.RowStyle = changeStyle;
        //            foreach (var cell in row.Cells) {
        //                cell.CellStyle = changeStyle;
        //            }
        //        }
        //        ++count;
        //    }
        //    if (changed) sheet.TabColorIndex = NPOI.HSSF.Util.HSSFColor.Red.Index;
        //}
        //if (FileUtil.FileExist(m_TranslationFile)) { File.Copy(m_TranslationFile, m_TranslationBackUp, true); }
        //FileStream stream = new FileStream(m_TranslationFile, FileMode.Create);
        //workbook.Write(stream);
        //stream.Close();
    }
}