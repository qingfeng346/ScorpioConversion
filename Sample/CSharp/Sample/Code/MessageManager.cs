//本文件为自动生成，请不要手动修改
using System;
using System.Collections.Generic;
namespace ScorpioProtoTest {
public class MessageManager {
    public static readonly Dictionary<string, int> MessageToID = new Dictionary<string, int>() {
        {"Msg_C2G_Test", 0},
        {"Msg_C2G_Test2", 1},
    };
    public static readonly Dictionary<int, string> IDToMessage = new Dictionary<int, string>() {
        {0, "Msg_C2G_Test"},
        {1, "Msg_C2G_Test2"},
    };
    public static readonly Dictionary<int, Type> IDToType = new Dictionary<int, Type>() {
        {0, typeof(Msg_C2G_Test)},
        {1, typeof(Msg_C2G_Test2)},
    };
    public static readonly Dictionary<Type, int> TypeToID = new Dictionary<Type, int>() {
        {typeof(Msg_C2G_Test), 0},
        {typeof(Msg_C2G_Test2), 1},
    };
}
}