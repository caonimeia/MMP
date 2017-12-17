using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MFButton : Button {
    public UnityEvent onPointerDown;
    public UnityEvent onPointerUp;

#if UNITY_EDITOR
    protected override void Reset() {
        gameObject.AddComponent<Image>();
        CreateText();
    }

    private void CreateText() {
        GameObject obj = new GameObject("Text");
        Text t = obj.AddComponent<Text>();
        t.color = Color.black;
        t.alignment = TextAnchor.MiddleCenter;
        t.text = "Text";

        obj.transform.SetParent(gameObject.transform, false);
        obj.transform.localPosition = Vector3.zero;

        RectTransform rectTransform = t.rectTransform;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
    }
#endif

    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);

        if (onPointerDown != null)
            onPointerDown.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);

        if (onPointerUp != null)
            onPointerUp.Invoke();
    }
}
