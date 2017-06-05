//本文件为自动生成，请不要手动修改
using System;
using System.Collections.Generic;
using Scorpio.Message;
namespace ScorpioProtoTest {
public class MessageManager {
    public static IMessage parseByteToMsg(int msgType, byte[] buff) {
        switch (msgType) {
        case 0: return Msg_C2G_Test.Deserialize(buff);
        case 1: return Msg_C2G_Test2.Deserialize(buff);
        case 2: return Msg_C2G_Test3.Deserialize(buff);
        default: throw new Exception("找不到MsgType : " + msgType);
        }
    }
    public static readonly Dictionary<string, int> MessageToID = new Dictionary<string, int>() {
        {"Msg_C2G_Test", 0},
        {"Msg_C2G_Test2", 1},
        {"Msg_C2G_Test3", 2},
    };
    public static readonly Dictionary<int, string> IDToMessage = new Dictionary<int, string>() {
        {0, "Msg_C2G_Test"},
        {1, "Msg_C2G_Test2"},
        {2, "Msg_C2G_Test3"},
    };
    public static readonly Dictionary<int, Type> IDToType = new Dictionary<int, Type>() {
        {0, typeof(Msg_C2G_Test)},
        {1, typeof(Msg_C2G_Test2)},
        {2, typeof(Msg_C2G_Test3)},
    };
    public static readonly Dictionary<Type, int> TypeToID = new Dictionary<Type, int>() {
        {typeof(Msg_C2G_Test), 0},
        {typeof(Msg_C2G_Test2), 1},
        {typeof(Msg_C2G_Test3), 2},
    };
    public static readonly Dictionary<int, IMessage> IDToObject = new Dictionary<int, IMessage>() {
        {0, new Msg_C2G_Test()},
        {1, new Msg_C2G_Test2()},
        {2, new Msg_C2G_Test3()},
    };
}
}