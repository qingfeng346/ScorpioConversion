package scorpiogame.proto;
public class TableManager {
    public void Reset() {
        mTest = null;
        SpawnArray.clear();
    }
    private TableTest mTest = null;
    public TableTest GetTest() { if (mTest == null) mTest = new TableTest().Initialize("Test"); return mTest; }
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
        TableSpawn spawns = new TableSpawn().Initialize(key);
        SpawnArray.put(key, spawns);
        return spawns;
    }
}