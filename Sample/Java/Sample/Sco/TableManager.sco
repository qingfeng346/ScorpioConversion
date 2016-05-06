//Package = ScorpioProtoTest
TableManager = {
    function Reset() {
        this.mTest = null
        TableSpawn_Test1 = null
        TableSpawn_Test2 = null
    }
    mTest = null,
    function GetTest() { if (this.mTest == null){ this.mTest = TableTest.Initialize("Test"); } return this.mTest; }
    function GetSpawn_Test1() { if (TableSpawn_Test1 == null){ TableSpawn_Test1 = clone(TableSpawn).Initialize("Spawn_Test1"); } return TableSpawn_Test1; }
    function GetSpawn_Test2() { if (TableSpawn_Test2 == null){ TableSpawn_Test2 = clone(TableSpawn).Initialize("Spawn_Test2"); } return TableSpawn_Test2; }
}