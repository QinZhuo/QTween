using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTool.Tween.Component
{
	public class QTweenSpriteColor : QTweenComponent<Color>
	{
		public override Color CurValue
		{
			get => spriteRenderer.color;
			set
			{
				spriteRenderer.color = value;
				spriteRenderer.SetDirty();
			}
		}
		private SpriteRenderer spriteRenderer;
		private void OnValidate()
		{
			if(spriteRenderer == null)
			{
				spriteRenderer = GetComponent<SpriteRenderer>();
			}
		}
		public override string ToString()
		{
			return "颜色 " + base.ToString();
		}
	}
}
