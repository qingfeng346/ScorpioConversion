import Datas.TableManager;
public class App {
    public static void main(String[] args) throws Exception {
        for (var pair : TableManager.GetInstance().getTest().Datas().entrySet()) {
            System.out.println(pair.getValue().toString());
        }
    }
}
