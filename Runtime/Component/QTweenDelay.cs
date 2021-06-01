using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QTool.Inspector;
namespace QTool.Tween.Component
{
    [RequireComponent(typeof(CanvasGroup))]
    public class QTweenDelay : QTweenBehavior
    {
        [ViewName("延迟时间")]
        public float time = 0.4f;
        protected override QTween ShowTween()
        {
            return QTweenManager.Delay(time);
        }
        public override string ToString()
        {
            return Anim.ToString();
        }
    }
}