using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MFMath {

    /// <summary>
    /// 截取至一位小数
    /// </summary>
    public static float OneDecimal(float value) {        
        return (float)(int)(value * 10) / 10;
    }
}
