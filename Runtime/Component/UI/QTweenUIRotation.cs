using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace QTool.Tween.Component
{
    public class QTweenUIRotation : QTweenBehavior<Quaternion>
	{
        public override Quaternion CurValue { get => RectTransform.localRotation; set => RectTransform.localRotation =value; }
    }
}
