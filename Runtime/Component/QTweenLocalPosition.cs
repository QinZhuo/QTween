using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
namespace QTool.Tween.Component
{
   

    public class QTweenLocalPosition : QTweenComponent<Vector2>
    {
        public override Vector2 CurValue { get => transform.localPosition; set => transform.localPosition = value; }
    }
}
