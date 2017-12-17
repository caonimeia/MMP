using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;

public static class MFAgoraMgr {
    private static string APP_ID = "4f9373a3ea474adbb9412cafb32e194f";
    public static IRtcEngineForGaming mRtcEngine = null;
    private static uint _uid;

    static MFAgoraMgr() {
        mRtcEngine = IRtcEngineForGaming.GetEngine(APP_ID);
    }

    private static string LocalLogFilePath() {
        return Application.persistentDataPath + "/agorasdk.log";
    }


    public static void Init() {
        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG);
        string rtcLogFile = LocalLogFilePath();
        mRtcEngine.SetLogFile(rtcLogFile);
        mRtcEngine.SetChannelProfile(CHANNEL_PROFILE.GAME_FREE_MODE);
    }

    public static void JoinChannel(string roomName) {
        if (mRtcEngine.JoinChannel(roomName, "", 0) == 0) {

        }
    }

    private static void EngineOnJoinChannelSuccess(string channelName, uint uid, int elapsed) {
        _uid = uid;
    }

    private static void LoadEngineCallbacks() {
        mRtcEngine.OnJoinChannelSuccess += EngineOnJoinChannelSuccess;
    }

    private static void MuteLocal(bool mute) {
        mRtcEngine.MuteLocalAudioStream(mute);
    }

    public static void PressToSpeakBegin() {
        MuteLocal(false);
    }

    public static void PressToSpeakEnd() {
        MuteLocal(true);
    }
}
