
package Datas;
import java.io.File;
import java.io.FileInputStream;
import Scorpio.Conversion.Runtime.*;
public class TableManager extends TableManagerBase {
    private static TableManager instance = null;
    public static TableManager GetInstance() {
        if (instance == null) {
            instance = new TableManager();
        }
        return instance;
    }
    public IReader GetReader(String name) throws Exception {
        return new DefaultReader(new FileInputStream(new File("../../" + name + ".data")), true);
    }
}