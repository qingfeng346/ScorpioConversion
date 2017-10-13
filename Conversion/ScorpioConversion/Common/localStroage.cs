using System;
using System.Collections.Generic;
using System.Text;
public class localStroage {
    private static ScorpioIni getConfig() {
        return new ScorpioIni(ConversionUtil.CurrentDirectory + "config.ini", Encoding.UTF8);
    }
    private static void save(ScorpioIni config) {
        FileUtil.CreateFile(ConversionUtil.CurrentDirectory + "config.ini", config.GetString());
    }
    public static void set(string key, string value) {
        var config = getConfig();
        config.Set(key, value);
        save(config);
    }
    public static string get(string key) {
        return getConfig().Get(key);
    }
    public static void remove(string key) {
        var config = getConfig();
        config.Remove(key);
        save(config);
    }
    public static void clear() {
        var config = getConfig();
        config.ClearData();
        save(config);
    }
}
