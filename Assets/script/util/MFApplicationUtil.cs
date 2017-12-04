using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MFRuntimePlatform {
    unkonw,
    editor,
    win,
    mac,
    ios,
    android,
}


public static class MFApplicationUtil {
    public static MFRuntimePlatform runtimePlatform;

    private static bool _debug = false; //是否开启debug模式


    static MFApplicationUtil() {
        SetRuntimePaltform();
    }

    /// <summary>
    /// 设置运行时的平台类型
    /// </summary>
    private static void SetRuntimePaltform() {
#if UNITY_EDITOR
        runtimePlatform = MFRuntimePlatform.editor;
#elif UNITY_STANDALONE_OSX
        runtimePlatform = MFRuntimePlatform.mac;
#elif UNITY_STANDALONE_WIN
        runtimePlatform = MFRuntimePlatform.win;
#elif UNITY_IOS
        runtimePlatform = MFRuntimePlatform.ios;
#elif UNITY_ANDROID
        runtimePlatform = MFRuntimePlatform.android;
#else
        runtimePlatform = MFRuntimePlatform.unkonw;
#endif
    }

    
    /// <summary>
    /// 设置Debug模式
    /// </summary>
    public static void SetDebugMode(bool debug) {
        if (_debug != debug)
            _debug = debug;
    }

    /// <summary>
    /// 切换Debug模式
    /// </summary>
    public static void SwitchDebugMode() {
        _debug = !_debug;
    }

    /// <summary>
    /// 是否开启debug模式
    /// </summary>
    public static bool IsOpenDebug() {
        return _debug;
    }
}
