using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class MFJsonSerialzator {
    /// <summary>
    /// 将对象转化为Json格式的字节数组
    /// </summary>
    public static string Serialize(object o) {
        return JsonUtility.ToJson(o);
    }

    /// <summary>
    /// 将Json格式的字节数组转化为对象
    /// </summary>
    public static T DeSerialize<T>(string json) {
        return JsonUtility.FromJson<T>(json);
    }
}