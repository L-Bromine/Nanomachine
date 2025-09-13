namespace Nanomachine.Log;

using Chickensoft.Log;

using System;
using System.Collections.Generic;

public static class Logger {
    private static readonly Dictionary<string, ILog> _logInstances = [];
    private static ILogWriter[]? _logWriter;

    public static string Level { get; set; } = "Unknow";

    // 初始化方法，需要在应用程序启动时调用
    public static void Initialize(params ILogWriter[] logWriter) => _logWriter = logWriter;

    // 获取指定类的日志实例
    private static ILog GetLogInstance(string className, string level) {
        Level = level;

        if (_logWriter == null) {
            throw new InvalidOperationException("Logger尚未初始化，请先调用Logger.Initialize方法");
        }

        if (!_logInstances.TryGetValue(className, out var logInstance)) {
            logInstance = new Log(className, _logWriter) {
                Formatter = new LogFormatter() {
                    MessagePrefix = $"[{Level}]"
                }
            };
            _logInstances[className] = logInstance;
        }

        return logInstance;
    }

    // 日志级别类
    public class LogLevel {
        private readonly ILog _log;

        internal LogLevel(ILog log) {
            _log = log;
        }

        public void Log(string content) {
            var formattedMessage = $"[{DateTime.Now:HH:mm:ss}] {content}";
            _log.Print(formattedMessage);
        }
    }

    // 静态方法，通过反射获取调用类名
    public static LogLevel Debug => new(GetLogInstance(GetCallingClassName(), "DEBUG"));
    public static LogLevel Info => new(GetLogInstance(GetCallingClassName(), "INFO"));
    public static LogLevel Warning => new(GetLogInstance(GetCallingClassName(), "WARN"));
    public static LogLevel Error => new(GetLogInstance(GetCallingClassName(), "ERROR"));
    public static LogLevel Test => new(GetLogInstance(GetCallingClassName(), "TEST"));

    // 简写属性
#pragma warning disable IDE1006 // 命名样式
    public static LogLevel d => Debug;
    public static LogLevel i => Info;
    public static LogLevel w => Warning;
    public static LogLevel e => Error;
    public static LogLevel t => Test;
#pragma warning restore IDE1006 // 命名样式

    // 获取调用者类名
    private static string GetCallingClassName() {
        // 使用堆栈跟踪获取调用者的类名
        var stackTrace = new System.Diagnostics.StackTrace();
        if (stackTrace.FrameCount > 3) // 0:当前方法, 1:LogLevel属性, 2:Log, 3:调用者
        {
            var frame = stackTrace.GetFrame(3);
            var method = frame?.GetMethod();
            return method?.DeclaringType?.Name ?? "Unknown";
        }
        return "Unknown";
    }
}
