using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace QTool.Tween.Component
{
    public class UIScaleAnimation : QTweenVector2
    {
        public override Vector2 CurValue { get => transform.localScale; set => transform.localScale = value; }
    }
}
