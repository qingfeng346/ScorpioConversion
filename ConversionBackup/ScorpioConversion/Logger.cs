using System;
using System.Collections.Generic;
using System.Text;

public class LibLogger : ILogger {
    public void info(string value) {
        ConversionLogger.info(value);
    }
    public void warn(string value) {
        ConversionLogger.warn(value);
    }
    public void error(string value) {
        ConversionLogger.error(value);
    }
}

public enum LogType {
    Info,
    Warn,
    Error,
}
public class LogValue {
    public LogType type;
    public string message;
    public LogValue(LogType type, string message) {
        this.type = type;
        this.message = message;
    }
}
public class ConversionLogger {
    public static Queue<LogValue> OutMessage = new Queue<LogValue>();
    public static void info(string value) {
        lock (OutMessage) { OutMessage.Enqueue(new LogValue(LogType.Info, value)); }
    }
    public static void warn(string value){
        lock (OutMessage) { OutMessage.Enqueue(new LogValue(LogType.Warn, value)); }
    }
    public static void error(string value) {
        lock (OutMessage) { OutMessage.Enqueue(new LogValue(LogType.Error, value)); }
    }
}
