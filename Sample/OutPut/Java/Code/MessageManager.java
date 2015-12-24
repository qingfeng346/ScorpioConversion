//本文件为自动生成，请不要手动修改
package scorpiogame.proto;
import java.util.HashMap;
@SuppressWarnings("serial")
public class MessageManager {
    public static final HashMap<String, Integer> MessageToID = new HashMap<String, Integer>() {
        {
            put("Msg_C2G_Test", 0);
        }
    };
    public static final HashMap<Integer, String> IDToMessage = new HashMap<Integer, String>() {
        {
            put(0, "Msg_C2G_Test");
        }
    };
    public static final HashMap<Integer, Class<?>> IDToType = new HashMap<Integer, Class<?>>() {
        {
            put(0, Msg_C2G_Test.class);
        }
    };
    public static final HashMap<Class<?>, Integer> TypeToID = new HashMap<Class<?>, Integer>() {
        {
            put(Msg_C2G_Test.class, 0);
        }
    };
}