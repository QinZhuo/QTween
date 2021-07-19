using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace QTool.Tween.Component
{
    public class QTweenUIRotation : QTweenVector3
    {
        public override Vector3 CurValue { get => RectTransform.rotation.eulerAngles; set => RectTransform.rotation =Quaternion.Euler( value); }
    }
}
