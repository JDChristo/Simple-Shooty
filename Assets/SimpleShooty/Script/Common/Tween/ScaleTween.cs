using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    public Vector3 targetScale;
    public float duration;
    public LeanTweenType easeType;
    public bool isInfinite;
    public Action onComplete;

    public LTDescr tween;
    private void OnEnable()
    {
        tween = transform.LeanScale(targetScale, duration).setEase(easeType);
        if (isInfinite)
        {
            tween.setLoopPingPong();
        }
        tween.setOnComplete(onComplete);
    }
    public void OnDisable()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one;
    }
}
