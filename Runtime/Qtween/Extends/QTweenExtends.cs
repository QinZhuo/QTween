using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QTool.Tween
{
    public static class QTweenExtends
    {
        public static QTween OnStart(this QTween tween, Action action)
        {
            tween.OnStartEvent += action;
            return tween;
        }
        public static QTween OnStop(this QTween tween, Action action)
        {
            tween.OnStopEvent += action;
            return tween;
        }
        public static QTween OnComplete(this QTween tween, Action action)
        {
            tween.OnCompleteEvent += action;
            return tween;
        }
        public static QTween AutoDestory(this QTween tween, bool destory = true)
        {
            tween.AutoDestory = destory;
            return tween;
        }
    }
}