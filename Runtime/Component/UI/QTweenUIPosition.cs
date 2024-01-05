using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
namespace QTool.Tween.Component
{
   

    public class QTweenUIPosition : QTweenComponent<Vector2>
    {
        public override Vector2 CurValue { get => RectTransform.anchoredPosition; set => RectTransform.anchoredPosition = value; }
    }
}
