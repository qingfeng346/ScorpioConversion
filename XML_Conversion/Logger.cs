using System;
using System.Collections.Generic;
using System.Text;

public enum LogType
{
    INFO,
    WARNING,
    ERROR,
}
public class LogValue
{
    public LogType type;
    public string message;
    public LogValue(LogType type, string message)
    {
        this.type = type;
        this.message = message;
    }
}
public class Progress
{
    public static int Count;
    public static int Current;
    public static float Value;
}
public class Logger
{
    public static Queue<LogValue> OutMessage = new Queue<LogValue>();
    public static void info(string format, params object[] args)
    {
        lock (OutMessage) {
            OutMessage.Enqueue(new LogValue(LogType.INFO, string.Format(format, args)));
        }
    }
    public static void warn(string format, params object[] args)
    {
        lock (OutMessage) {
            OutMessage.Enqueue(new LogValue(LogType.WARNING, string.Format(format, args)));
        }
    }
    public static void error(string format, params object[] args)
    {
        lock (OutMessage) {
            OutMessage.Enqueue(new LogValue(LogType.ERROR, string.Format(format, args)));
        }
    }
}
