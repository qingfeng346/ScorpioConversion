using System;
using System.Collections.Generic;
using System.Text;
public class Progress
{
    private static int m_Current = 0;
    public static int Count;
    public static int Current { 
        get { return m_Current; } 
        set { 
            m_Current = value;
            if (m_Current > Count) m_Current = Count;
        } 
    }
}
public interface ILogger
{
    void info(string value);
    void warn(string value);
    void error(string value);
}
public class Logger
{
    private static ILogger logger;
    public static void SetLogger(ILogger logger)
    {
        Logger.logger = logger;
    }
    public static void info(string format, params object[] args)
    {
        if (logger != null) logger.info(string.Format(format, args));
    }
    public static void warn(string format, params object[] args)
    {
        if (logger != null) logger.warn(string.Format(format, args));
    }
    public static void error(string format, params object[] args)
    {
        if (logger != null) logger.error(string.Format(format, args));
    }
}
