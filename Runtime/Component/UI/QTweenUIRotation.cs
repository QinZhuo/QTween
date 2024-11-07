using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace QTool.Tween.Component {
	public class QTweenUIRotation : QTweenComponent<Vector3> {
		public override Vector3 CurValue { get => RectTransform.localEulerAngles; set => RectTransform.localEulerAngles = value; }
	}
}
