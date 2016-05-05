using System;
using System.Collections.Generic;
namespace ScorpioProtoTest {
public class TableManager {
    public void Reset() {
        mTest = null;
        SpawnArray.Clear();
    }
    private TableTest mTest = null;
    public TableTest GetTest() { if (mTest == null) mTest = new TableTest().Initialize("Test"); return mTest; }
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
        return SpawnArray[key] = new TableSpawn().Initialize(key);
    }
}
}