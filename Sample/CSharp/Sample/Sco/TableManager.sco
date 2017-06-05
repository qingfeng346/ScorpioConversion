//Package = ScorpioProtoTest
TableManager = {
    function Reset() {
        this.mTest = null
        TableLanguage_CN = null
        TableLanguage_EN = null
        TableSpawn_Test1 = null
        TableSpawn_Test2 = null
    }
    mTest = null,
    function GetTest() { if (this.mTest == null){ this.mTest = TableTest.Initialize(this, "Test"); } return this.mTest; }
    function GetLanguage_CN() { if (TableLanguage_CN == null){ TableLanguage_CN = clone(TableLanguage).Initialize(this, "Language_CN"); } return TableLanguage_CN; }
    function GetLanguage_EN() { if (TableLanguage_EN == null){ TableLanguage_EN = clone(TableLanguage).Initialize(this, "Language_EN"); } return TableLanguage_EN; }
    function GetSpawn_Test1() { if (TableSpawn_Test1 == null){ TableSpawn_Test1 = clone(TableSpawn).Initialize(this, "Spawn_Test1"); } return TableSpawn_Test1; }
    function GetSpawn_Test2() { if (TableSpawn_Test2 == null){ TableSpawn_Test2 = clone(TableSpawn).Initialize(this, "Spawn_Test2"); } return TableSpawn_Test2; }
    function getValue(attribute, fileName, name, id) {
        if (attribute.Language) {
            return this.getLanguageText(fileName + "_" + name + "_" + id)
        }
        return "";
    }
    m_Language = null,
    m_Languages = {},
    function setLanguage(language) {
    	this.m_Language = language
        this.m_Languages = {}
        var lans = this["GetLanguage_" + language]().Datas()
	    foreach (var data in vpairs(lans)) {
            this.m_Languages[data.Key] = data.Text
	    }
    }
    function getLanguage() {
    	return this.m_Language;
    }
    function getLanguageText(key) {
        if (table.containskey(this.m_Languages, key)) {
            return this.m_Languages[key]
        }
    	return "";
    }
}