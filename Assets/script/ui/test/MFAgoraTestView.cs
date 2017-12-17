using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFAgoraTestView : MFUIBase {
    private MFAgoraTestBind uiBind;
    private bool defalutspeakerPhone = false;
    private bool speakerPhone = false;
    private bool localMute = false;
    private bool remoteAllMute = false;

    protected override void Awake() {
        base.Awake();

        uiBind = GetComponent<MFAgoraTestBind>();
        uiBind.Close.onClick.AddListener(OnClickClose);
        uiBind.EnableAudio.onClick.AddListener(OnEnableAudio);
        uiBind.DisableAudio.onClick.AddListener(OnDisableAudio);
        uiBind.SetDefaultAudioRouteToSpeakerPhone.onClick.AddListener(OnSetDefaultAudioRouteToSpeakerPhone);
        uiBind.SetEnableSpeakerphone.onClick.AddListener(OnSetEnableSpeakerphone);
        uiBind.MuteLocalAudioStream.onClick.AddListener(OnMuteLocalAudioStream);
        uiBind.MuteAllRemoteAudioStreams.onClick.AddListener(OnMuteAllRemoteAudioStreams);

        uiBind.PressToSpeak.onPointerDown.AddListener(OnPressToSpeakDown);
        uiBind.PressToSpeak.onPointerUp.AddListener(OnPressToSpeakUp);
    }

    private void OnMuteAllRemoteAudioStreams() {
        remoteAllMute = !remoteAllMute;
        MFAgoraMgr.mRtcEngine.MuteAllRemoteAudioStreams(remoteAllMute);
    }

    private void OnMuteLocalAudioStream() {
        localMute = !localMute;
        MFAgoraMgr.mRtcEngine.MuteLocalAudioStream(localMute);
    }

    private void OnSetEnableSpeakerphone() {
        speakerPhone = !speakerPhone;
        MFAgoraMgr.mRtcEngine.SetEnableSpeakerphone(speakerPhone);
    }

    private void OnSetDefaultAudioRouteToSpeakerPhone() {
        defalutspeakerPhone = !defalutspeakerPhone;
        MFAgoraMgr.mRtcEngine.SetDefaultAudioRouteToSpeakerphone(defalutspeakerPhone);
    }

    private void OnDisableAudio() {
        MFAgoraMgr.mRtcEngine.DisableAudio();
    }

    private void OnEnableAudio() {
        MFAgoraMgr.mRtcEngine.EnableAudio();
    }

    private void OnClickClose() {
        MFUIMgr.Close<MFAgoraTestView>();
        MFUIMgr.Open<MFMainView>();
    }

    private void OnPressToSpeakDown() {
        MFAgoraMgr.mRtcEngine.MuteLocalAudioStream(false);
    }

    private void OnPressToSpeakUp() {
        MFAgoraMgr.mRtcEngine.MuteLocalAudioStream(true);
    }

}
