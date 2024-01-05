using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace QTool.Tween.Component
{
    public class QTweenUIRotation : QTweenComponent<Quaternion>
	{
        public override Quaternion CurValue { get => RectTransform.localRotation; set => RectTransform.localRotation =value; }
    }
}
