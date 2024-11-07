using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
namespace QTool.Tween.Component {
	public class QTweenLocalScale : QTweenComponent<Vector3> {
		public override Vector3 CurValue { get => transform.localScale; set => transform.localScale = value; }
	}
}
