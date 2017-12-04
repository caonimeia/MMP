using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

public static class MFLog {
    private static void Log(LogType logType, params object[] messages) {
        string message = "";
        foreach (object tmp in messages) {
            message = message + tmp + '\t';
        }

        message = DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss] ") + message;
        Debug.unityLogger.Log(logType, message);
    }

    /// <summary>
    /// 打印日志
    /// </summary>
    public static void LogInfo(params object[] messages) {
        Log(LogType.Log, messages);
    }

    /// <summary>
    /// 打印错误日志
    /// </summary>    
    public static void LogError(params object[] messages) {
        Log(LogType.Error, messages);
    }

    /// <summary>
    /// 打印警告日志
    /// </summary>
    public static void LogWarning(params object[] messages) {
        Log(LogType.Warning, messages);
    }

    /// <summary>
    /// 打印对象属性 目前只支持public属性
    /// </summary>
    public static void LogObject(object o) {
        Type t = o.GetType();
        foreach(FieldInfo pi in t.GetFields()) {
            LogInfo(pi.Name, pi.GetValue(o));
        }
    }

    public static void AssertNotNull(object obj) {
        Assert.IsNotNull(obj);
    }
}