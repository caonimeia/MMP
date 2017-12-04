using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public static class MFTimer {
    private enum TimerType {
        once,
        forever,
    }

    private struct TimerStruct {
        public float callBackTime;
        public int actionIndex;

        public TimerStruct(float callBackTime, int actionIndex) {
            this.callBackTime = callBackTime;
            this.actionIndex = actionIndex;
        }
    }
 
    private class CallBackStruct {
        public int timerId;
        public float delayTime;
        public TimerType type;
        public Action action;
        public bool active;

        public CallBackStruct(int timeId, float delayTime, TimerType type, Action action) {
            this.timerId = timeId;
            this.delayTime = delayTime;
            this.type = type;
            this.action = action;

            active = true;  // 默认定时器处于激活状态
        }
    }

    private static int _curTimerId;
    private static Dictionary<float, List<CallBackStruct>> eventsDic;
    private static Dictionary<int, TimerStruct> timerStructDic;

    static MFTimer() {
        _curTimerId = 0;
        eventsDic = new Dictionary<float, List<CallBackStruct>>();
        timerStructDic = new Dictionary<int, TimerStruct>();
    }

    private static float CalcCallBackTime(float delayTime) {
        float dlyTime = MFMath.OneDecimal(delayTime);
        float curTime = MFMath.OneDecimal(Time.time);
        return curTime + dlyTime;
    }

    private static int _Register(float delayTime, TimerType timerType, Action callBack) {
        float callBackTime = CalcCallBackTime(delayTime);

        if (!eventsDic.ContainsKey(callBackTime))
            eventsDic[callBackTime] = new List<CallBackStruct>();

        _curTimerId++;
        eventsDic[callBackTime].Add(new CallBackStruct(_curTimerId, delayTime, timerType, callBack));
        timerStructDic.Add(_curTimerId, new TimerStruct(callBackTime, eventsDic[callBackTime].Count - 1));
        return _curTimerId;
    }

    private static void _RegisterAgain(CallBackStruct cbs) {
        float callBackTime = CalcCallBackTime(cbs.delayTime);

        if (!eventsDic.ContainsKey(callBackTime))
            eventsDic[callBackTime] = new List<CallBackStruct>();

        eventsDic[callBackTime].Add(cbs);
        timerStructDic[cbs.timerId] = new TimerStruct(callBackTime, eventsDic[callBackTime].Count - 1);
    }

    /// <summary>
    /// 注册单次定时器 精确到0.1秒
    /// </summary>
    public static int RegisterOnce(float delayTime, Action callBack) {
        return _Register(delayTime, TimerType.once, callBack);
    }

    /// <summary>
    /// 注册循环定时器 精确到0.1秒
    /// </summary>
    public static int RegisterSchedule(float delayTime, Action callBack) {
        return _Register(delayTime, TimerType.forever, callBack);
    }

    /// <summary>
    /// 注销定时器
    /// </summary>
    public static void UnRegister(int timerId) {
        if (!timerStructDic.ContainsKey(timerId)) {
            return;
        }

        TimerStruct ts = timerStructDic[timerId];
        List<CallBackStruct> cbsList;
        if (!eventsDic.TryGetValue(ts.callBackTime, out cbsList)) {
            return;
        }

        // 标记为未激活状态 等到定时器将要触发的时刻销毁
        cbsList[ts.actionIndex].active = false;
    }

    public static void Update() {
        float curTime = MFMath.OneDecimal(Time.time);
        List<CallBackStruct> cbsList;
        if (!eventsDic.TryGetValue(curTime, out cbsList)) {
            return;
        }

        for (int i = 0; i < cbsList.Count; i++) {
            CallBackStruct cbs = cbsList[i];
            if (cbs.active)
                cbs.action();

            timerStructDic.Remove(cbs.timerId);
            // 循环定时器被注销之后就不会再次触发
            if (cbs.active && cbs.type == TimerType.forever) {
                _RegisterAgain(cbs);
            }
        }

        cbsList.Clear();
        eventsDic.Remove(curTime);
    }
}