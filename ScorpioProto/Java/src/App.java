import java.io.File;
import java.io.FileInputStream;
import Datas.TableTest;
import Scorpio.Conversion.DefaultReader;

public class App {
    public static void main(String[] args) throws Exception {
        TableTest table = new TableTest();
        table.Initialize("Test", new DefaultReader(new FileInputStream(new File("../../Sample/Test.data"))));
        for (var pair : table.Datas().entrySet()) {
            System.out.println(pair.getValue().toString());
        }
    }
}
