package ScorpioProtoTest;
public class TableManager {
    public void Reset() {
        mTest = null;
        LanguageArray.clear();
        SpawnArray.clear();
    }
    private TableTest mTest = null;
    public TableTest GetTest() { if (mTest == null) mTest = new TableTest().Initialize(this, "Test"); return mTest; }
    public enum Language {
        Language_CN,
        Language_EN,
    }
    private java.util.HashMap<String, TableLanguage> LanguageArray = new java.util.HashMap<String, TableLanguage>();
    public TableLanguage GetSpawns(Language key) { return GetSpawns_Language_impl(key.toString()); }
    public TableLanguage GetSpawns_Language(String key) { return GetSpawns_Language_impl("Language_" + key); }
    private TableLanguage GetSpawns_Language_impl(String key) {
        if (LanguageArray.containsKey(key))
            return LanguageArray.get(key);
        TableLanguage spawns = new TableLanguage().Initialize(this, key);
        LanguageArray.put(key, spawns);
        return spawns;
    }
    public enum Spawn {
        Spawn_Test1,
        Spawn_Test2,
    }
    private java.util.HashMap<String, TableSpawn> SpawnArray = new java.util.HashMap<String, TableSpawn>();
    public TableSpawn GetSpawns(Spawn key) { return GetSpawns_Spawn_impl(key.toString()); }
    public TableSpawn GetSpawns_Spawn(String key) { return GetSpawns_Spawn_impl("Spawn_" + key); }
    private TableSpawn GetSpawns_Spawn_impl(String key) {
        if (SpawnArray.containsKey(key))
            return SpawnArray.get(key);
        TableSpawn spawns = new TableSpawn().Initialize(this, key);
        SpawnArray.put(key, spawns);
        return spawns;
    }
    private String m_Language;
    private java.util.HashMap<String, String> m_Languages = new java.util.HashMap<String, String>();
    public void setLanguage(String language) {
    	m_Language = language;
        m_Languages.clear();
        java.util.HashMap<Integer, DataLanguage> lans = GetSpawns_Language(language).Datas();
        for (DataLanguage data : lans.values()) {
        	m_Languages.put(data.getKey(), data.getText());
        }
    }
    public String getLanguage() {
    	return m_Language;
    }
    public String getLanguageText(String key) {
    	if (m_Languages.containsKey(key)) {
    		return m_Languages.get(key);
    	}
    	return "";
    }
}