using System;
using System.Collections.Generic;
namespace ScorpioProtoTest {
public class TableManager {
    public void Reset() {
        mTest = null;
        LanguageArray.Clear();
        SpawnArray.Clear();
    }
    private TableTest mTest = null;
    public TableTest GetTest() { if (mTest == null) mTest = new TableTest().Initialize(this, "Test"); return mTest; }
    public enum Language {
        Language_CN,
        Language_EN,
    }
    private Dictionary<string, TableLanguage> LanguageArray = new Dictionary<string, TableLanguage>();
    public TableLanguage GetSpawns(Language key) { return GetSpawns_Language_impl(key.ToString()); }
    public TableLanguage GetSpawns_Language(string key) { return GetSpawns_Language_impl("Language_" + key); }
    private TableLanguage GetSpawns_Language_impl(string key) { 
        if (LanguageArray.ContainsKey(key))
            return LanguageArray[key];
        return LanguageArray[key] = new TableLanguage().Initialize(this, key);
    }
    public enum Spawn {
        Spawn_Test1,
        Spawn_Test2,
    }
    private Dictionary<string, TableSpawn> SpawnArray = new Dictionary<string, TableSpawn>();
    public TableSpawn GetSpawns(Spawn key) { return GetSpawns_Spawn_impl(key.ToString()); }
    public TableSpawn GetSpawns_Spawn(string key) { return GetSpawns_Spawn_impl("Spawn_" + key); }
    private TableSpawn GetSpawns_Spawn_impl(string key) { 
        if (SpawnArray.ContainsKey(key))
            return SpawnArray[key];
        return SpawnArray[key] = new TableSpawn().Initialize(this, key);
    }
    private String m_Language;
    private Dictionary<String, String> m_Languages = new Dictionary<String, String>();
    public void setLanguage(String language) {
    	m_Language = language;
        m_Languages.Clear();
        Dictionary<int, DataLanguage> lans = GetSpawns_Language(language).Datas();
        foreach (DataLanguage data in lans.Values) {
        	m_Languages[data.getKey()] = data.getText();
        }
    }
    public String getLanguage() {
    	return m_Language;
    }
    public String getLanguageText(String key) {
    	if (m_Languages.ContainsKey(key)) {
    		return m_Languages[key];
    	}
    	return "";
    }
}
}