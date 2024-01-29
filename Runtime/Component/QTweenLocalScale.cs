using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
namespace QTool.Tween.Component
{
    public class QTweenLocalScale : QTweenComponent<Vector2>
    {
        public override Vector2 CurValue { get => transform.localScale; set => transform.localScale = value; }
    }
}
