using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace QTool.Tween.Component {
	public class QTweenUIScale : QTweenComponent<Vector2> {
		public override Vector2 CurValue { get => transform.localScale; set => transform.localScale = new Vector3(value.x, value.y, 1); }
		public override string ToString() {
			return "大小 " + base.ToString();
		}
	}
}
