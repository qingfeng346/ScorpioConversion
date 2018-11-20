using System.IO;
using System.Collections.Generic;
//using NPOI.SS.UserModel;
//using NPOI.HSSF.UserModel;
public partial class LanguageBuilder {
    private class LanguageItem {
        public string Table;        //所在table表
        public Dictionary<string, string> Text = new Dictionary<string, string>();      //躲过语言数据
    }
    private Dictionary<string, LanguageItem> m_Items = new Dictionary<string, LanguageItem>();
    public bool RefreshLanguage() {
        if (!Check()) {
            return false;
        }
        //Logger.info("正在生成Language表");
        //m_Items.Clear();
        //IWorkbook workbook = new HSSFWorkbook(new FileStream(m_TranslationFile, FileMode.Open, FileAccess.Read));
        //for (int i = 0; i < workbook.NumberOfSheets; ++i) {
        //    ISheet sheet = workbook.GetSheetAt(i);
        //    int j = 1, z = 0;
        //    try {
        //        for (j = 1; j <= sheet.LastRowNum; ++j) {
        //            LanguageItem item = new LanguageItem();
        //            item.Table = sheet.SheetName;
        //            IRow row = sheet.GetRow(j);
        //            if (row == null) continue;
        //            ICell cell = row.GetCell(0);
        //            if (cell == null) continue;
        //            string key = cell.StringCellValue;
        //            for (z = 0; z < m_Languages.Length; ++z) {
        //                item.Text[m_Languages[z]] = Util.GetCellString(row.GetCell(z + 1, MissingCellPolicy.CREATE_NULL_AS_BLANK));
        //            }
        //            if (!string.IsNullOrEmpty(key))
        //                m_Items[key] = item;
        //        }
        //    } catch (System.Exception ex) {
        //        throw new System.Exception(string.Format("获取数据出错 Sheet:{0} 行:{1} 列:{2} Error:{3}", sheet.SheetName, j + 1, Util.GetLineName(z + 1), ex.ToString()));
        //    }
        //}
        //foreach (var language in m_Languages) {
        //    CreateLanguageXLS(language);
        //}
        return true;
    }
    public void CreateLanguageXLS(string language) {
        //IWorkbook workbook = new HSSFWorkbook();
        //ISheet sheet = workbook.CreateSheet();
        //{
        //    IRow row = sheet.CreateRow(0);
        //    row.CreateCell(0).SetCellValue("索引");
        //    row.CreateCell(1).SetCellValue("关键字");
        //    row.CreateCell(2).SetCellValue("文字");
        //}
        //{
        //    IRow row = sheet.CreateRow(1);
        //    row.CreateCell(0).SetCellValue("Index");
        //    row.CreateCell(1).SetCellValue("Key");
        //    row.CreateCell(2).SetCellValue("Text");
        //}
        //{
        //    IRow row = sheet.CreateRow(4);
        //    row.CreateCell(0).SetCellValue("int");
        //    row.CreateCell(1).SetCellValue("string");
        //    row.CreateCell(2).SetCellValue("string");
        //}
        //int index = 5;
        //List<string> Values = new List<string>();
        //foreach (var pair in m_Items) {
        //    string val = pair.Value.Text[language];
        //    if (string.IsNullOrEmpty(val)) continue;
        //    IRow row = sheet.CreateRow(index);
        //    row.CreateCell(0).SetCellValue(index.ToString());
        //    row.CreateCell(1).SetCellValue(pair.Key);
        //    row.CreateCell(2).SetCellValue(val);
        //    ++index;
        //}
        //FileStream stream = new FileStream(m_LanguageDirectory + "/Language_" + language + ".xls", FileMode.Create);
        //workbook.Write(stream);
        //stream.Close();
    }
}

