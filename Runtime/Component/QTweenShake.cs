using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTool.Tween
{
	public class QTweenShake : QTweenComponent
	{
		[QName("时间")]
		public float time = 0.4f;
		[QName("振幅")]
		public Vector3 scale = Vector3.one;
		protected override QTween GetTween()
		{
			return transform.QShake(time, scale);
		}
	}
}
