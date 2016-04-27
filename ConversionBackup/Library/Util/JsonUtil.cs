using System;
using LitJson;
/// <summary> json 工具类 </summary>
public static class JsonUtil
{
    /// <summary> object转成json </summary>
    public static String ObjectToJson(object obj)
	{
        try {
            return JsonMapper.ToJson(obj);
        } catch (Exception e) {
            Logger.error("ObjectToJson is error : {0} ", e.ToString());
        }
        return "";
	}
    /// <summary> json转成object </summary>
    public static T JsonToObject<T>(String json)
    {
        try {
            return JsonMapper.ToObject<T>(json);
        } catch (Exception e) {
            Logger.error("JsonToObject is error json : {0}  error : {1}", json, e.ToString());
        }
        return default(T);
    }
    /// <summary> json转成object </summary>
	public static object JsonToObject(String json,Type clazz)
	{
		try {
            return JsonMapper.ToObject(clazz, json);
		} catch (Exception e) {
            Logger.error("JsonToObject is error json : {0}  error : {1}", json, e.ToString());
		}
		return null;
	}
}
