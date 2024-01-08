using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QTool.Inspector;
namespace QTool.Tween.Component
{
    public class QTweenDelayComponent : QTweenComponent
    {
        [QName("延迟时间")]
        public float time = 0.4f;
        protected override QTween GetTween()
        {
            return QTweenManager.Delay(time);
        }
       
    }
}
