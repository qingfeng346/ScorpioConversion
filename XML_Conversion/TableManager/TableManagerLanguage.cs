using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI;
using NPOI.HSSF.Util;
public partial class TableManager
{
    public class Entry
    {
        //关键字
        public string Key;
        //中文字符串
        public string Value;
        //其他语言字符串
        public Dictionary<string, string> KeyValue = null;
        public Entry(string key,string value, Dictionary<string, string> keyValue)
        {
            this.Key = key;
            this.Value = value;
            this.KeyValue = keyValue;
        }
    }
    public class Item
    {
        public string Table;
        public Dictionary<string, string> Text = new Dictionary<string, string>();
    }
    //源文件的多国语言
    private Dictionary<string, Entry> m_CacheLanguages = new Dictionary<string, Entry>();
    private string m_TranslationDirectory = null;
    private string m_LanguageDirectory = "";
    private string m_Languages = "";
    private string m_TranslationFile = "";
    private string m_TranslationBackUp = "";
    private void CreateLanguage()
    {
        Progress.Value = 0;
        m_CacheLanguages.Clear();
        m_Languages = Util.GetConfig(ConfigKey.AllLanguage, ConfigFile.LanguageConfig);
        m_TranslationDirectory = Util.GetConfig(ConfigKey.TranslationDirectory, ConfigFile.LanguageConfig);
        m_LanguageDirectory = Util.GetConfig(ConfigKey.LanguageDirectory, ConfigFile.LanguageConfig);
        m_TranslationFile = m_TranslationDirectory + "Translation.xls";
        m_TranslationBackUp = m_TranslationDirectory + "Translation备份.xls";
        CreateTranslation();
    }
    private void CreateTranslation()
    {
        if (File.Exists(m_TranslationFile)) {
            IWorkbook workbook = new HSSFWorkbook(new FileStream(m_TranslationFile, FileMode.Open, FileAccess.Read));
            int index = 0;
            foreach (var pair in mKeyWords) {
                Progress.Value = ((float)(index + 1) / (float)(mKeyWords.Count) * 100f);
                ++index;
                try {
                    ISheet sheet = workbook.GetSheet(pair.Key);
                    if (sheet == null) continue;
                    IRow firstRow = sheet.GetRow(0);
                    //查出所有的多国语言
                    List<string> languages = new List<string>();
                    for (int i = 2; i < firstRow.LastCellNum; ++i) {
                        languages.Add(Util.ReadCellString(firstRow.GetCell(i)));
                    }
                    //查出旧的翻译
                    for (int i = 1; i <= sheet.LastRowNum; ++i) {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        var key = Util.ReadCellString(row.GetCell(0));
                        if (string.IsNullOrEmpty(key)) continue;
                        var value = Util.ReadCellString(row.GetCell(1));
                        Dictionary<string, string> keyValue = new Dictionary<string, string>();
                        for (int j = 0; j < languages.Count; ++j) {
                            keyValue.Add(languages[j], Util.ReadCellString(row.GetCell(2 + j)));
                        }
                        m_CacheLanguages[key] = new Entry(key, value, keyValue);
                    }
                } catch (System.Exception e) {
                    Logger.error("CreateAlterLanguage is error : " + e.ToString());
                }
            }
        }
        CreateTranslation_impl();
    }
    private void CreateTranslation_impl()
    {
        List<string> Languages = new List<string>(m_Languages.Split(';'));
        Languages.Remove("");
        Languages.Remove(null);
        IWorkbook workbook = new HSSFWorkbook();
        ICellStyle changeStyle = workbook.CreateCellStyle();
        changeStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
        changeStyle.FillPattern = FillPattern.SolidForeground;

        int index = 0;
        foreach (var pair in mKeyWords)
        {
            Progress.Value = ((float)(index + 1) / (float)(mKeyWords.Count) * 100f);
            ++index;
            ISheet sheet = workbook.CreateSheet(pair.Key);
            IRow firstRow = sheet.CreateRow(0);
            firstRow.CreateCell(0).SetCellValue("关键字");
            for (int i = 0; i < Languages.Count; ++i) {
                firstRow.CreateCell(i + 1).SetCellValue(Languages[i]);
            }
            int count = 1;
            bool changed = false;
            foreach (var lanPair in pair.Value.Languages){
                IRow row = sheet.CreateRow(count);
                var language = lanPair.Value;
                row.CreateCell(0).SetCellValue(lanPair.Key);
                row.CreateCell(1).SetCellValue(language.Value);
                bool rowChanged = false;
                if (!m_CacheLanguages.ContainsKey(language.Key)) {
                    changed = true;
                    rowChanged = true;
                    row.RowStyle = changeStyle;
                } else {
                    if (m_CacheLanguages[language.Key].Value != language.Value) {
                        changed = true;
                        rowChanged = true;
                        row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        IClientAnchor anchor = workbook.GetCreationHelper().CreateClientAnchor();
                        anchor.Col1 = 1;
                        anchor.Col2 = 2;
                        anchor.Row1 = row.RowNum;
                        anchor.Row2 = row.RowNum + 3;
                        IComment comment = sheet.CreateDrawingPatriarch().CreateCellComment(anchor);
                        comment.String = new HSSFRichTextString(m_CacheLanguages[language.Key].Value);
                        row.GetCell(1).CellComment = comment;
                    }
                    for (int i = 1; i < Languages.Count; ++i)
                    {
                        if (m_CacheLanguages[language.Key].KeyValue.ContainsKey(Languages[i]))
                            row.CreateCell(i + 1).SetCellValue(m_CacheLanguages[language.Key].KeyValue[Languages[i]]);
                    }
                }
                if (rowChanged) row.RowStyle = changeStyle;
                language.Row = count;
                ++count;
            }
            if (changed) sheet.TabColorIndex = NPOI.HSSF.Util.HSSFColor.Red.Index;
        }
        if (File.Exists(m_TranslationFile)) {
            File.Copy(m_TranslationFile, m_TranslationBackUp, true);
        }
        FileStream stream = new FileStream(m_TranslationFile, FileMode.Create);
        workbook.Write(stream);
        stream.Close();
        RefreshLanguage();
    }
    private IWorkbook m_Translate = null;
    private Dictionary<string, Item> m_Text = new Dictionary<string, Item>();
    public void RefreshLanguage()
    {
        try
        {
            m_Text.Clear();
            m_Languages = Util.GetConfig(ConfigKey.AllLanguage, ConfigFile.LanguageConfig);
            m_TranslationDirectory = Util.GetConfig(ConfigKey.TranslationDirectory, ConfigFile.LanguageConfig);
            m_LanguageDirectory = Util.GetConfig(ConfigKey.LanguageDirectory, ConfigFile.LanguageConfig);
            m_TranslationFile = m_TranslationDirectory + "Translation.xls";
            m_TranslationBackUp = m_TranslationDirectory + "Translation备份.xls";
            List<string> Languages = new List<string>(m_Languages.Split(';'));
            m_Translate = new HSSFWorkbook(new FileStream(m_TranslationFile, FileMode.Open, FileAccess.Read));
            for (int i = 0; i < m_Translate.NumberOfSheets; ++i) {
                ISheet sheet = m_Translate.GetSheetAt(i);
                for (int j = 1; j <= sheet.LastRowNum; ++j)
                {
                    Item item = new Item();
                    item.Table = sheet.SheetName;
                    IRow row = sheet.GetRow(j);
                    if (row != null) {
                        ICell cel = row.GetCell(0);
                        if (cel != null) {
                            string key = row.GetCell(0).StringCellValue;
                            for (int z = 0; z < Languages.Count; ++z)
                                item.Text[Languages[z]] = row.GetCell(z + 1, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue;
                            if (!string.IsNullOrEmpty(key))
                                m_Text[key] = item;
                        }
                    }
                }
            }
            for (int i = 0; i < Languages.Count; ++i) {
                CreateLanguageXLS(Languages[i]);
            }
            TransformLanguage(Languages);
        } catch (System.Exception ex) {
            Logger.error("RefreshLanguage is error : " + ex.ToString());
        }
    }
    private void CreateLanguageXLS(string language)
    {
        IWorkbook workbook = new HSSFWorkbook();
        ISheet sheet = workbook.CreateSheet();
        {
            IRow row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("索引");
            row.CreateCell(1).SetCellValue("关键字");
            row.CreateCell(2).SetCellValue("文字");
        }
        {
            IRow row = sheet.CreateRow(1);
            row.CreateCell(0).SetCellValue("Index");
            row.CreateCell(1).SetCellValue("Key");
            row.CreateCell(2).SetCellValue("Text");
        }
        {
            IRow row = sheet.CreateRow(2);
            row.CreateCell(0).SetCellValue("int");
            row.CreateCell(1).SetCellValue("arraystring");
            row.CreateCell(2).SetCellValue("fstring");
        }
        int index = 3;
        List<string> Values = new List<string>();
        foreach (var pair in m_Text)
        {
            string val = pair.Value.Text[language];
            if (Values.Contains(val))
                continue;
            Values.Add(val);
            string key = "";
            bool first = true;
            foreach (var cell in m_Text)
            {
                if (cell.Value.Text[language] == val)
                {
                    if (first == false) key += ",";
                    first = false;
                    key += cell.Key;
                }
            }
            IRow row = sheet.CreateRow(index);
            row.CreateCell(0).SetCellValue(index.ToString());
            row.CreateCell(1).SetCellValue(key);
            row.CreateCell(2).SetCellValue(val);
            ++index;
        }
        FileStream stream = new FileStream(m_LanguageDirectory + "Language_" + language + ".xls", FileMode.Create);
        workbook.Write(stream);
        stream.Close();
    }
    private void TransformLanguage(List<string> languages)
    {
        List<string> files = new List<string>();
        for (int i=0;i<languages.Count;++i) {
            files.Add(m_LanguageDirectory + "Language_" + languages[i] + ".xls");
        }
        Transform(files, false, false, false, false);
    }
}

