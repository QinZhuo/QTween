using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QTool.Tween.Component
{
    public class QTweenUISize : QTweenComponent<Vector2>
    {
        public override Vector2 CurValue { get => RectTransform.sizeDelta; set => RectTransform.sizeDelta = value; }
    }
}

