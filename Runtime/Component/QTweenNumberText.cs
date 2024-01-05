using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

namespace QTool.Tween.Component
{
    public class QTweenNumberText : QTweenComponent<float>
    {
        public override float CurValue
        {
            get => curValue; set { OnValueChange?.Invoke( value.ToString(format)); curValue = value; }
        }
        private float curValue = 0;
        public string format="F0";
		public StringEvent OnValueChange = new StringEvent();
		[UnityEngine.Serialization.FormerlySerializedAs("showTween")]
		public QTweenComponent showTween;
		public QTweenComponent changeTween;
		protected override void Reset()
        {
            var text= GetComponentInChildren<Text>();
			if (text != null)
			{
				OnValueChange.AddPersistentListener(text.GetAction<string>("set_text"));
			}
            base.Reset();
        }
		public void SetFloat(float value)
		{
			Anim.Stop();
			var floatAnim = Anim as QTween<float>;
			floatAnim.StartValue = curValue;
			floatAnim.EndValue = value;
		}
		public override async Task PlayAsync(bool show)
		{
			//if (changeTween != null && !EndValue.Equals(StartValue))
			//{
			//	changeTween.ShowAndHide();
			//}
			if (!CurValue.Similar(0))
			{
				showTween?.Show();
			}
			changeTween?.Show();
			await base.PlayAsync(show);
			changeTween?.Hide();
			if (CurValue.Similar(0))
			{
				changeTween.Complete();
				showTween?.Hide();
			}
		}
		public void SetFloatAnim(float value)
        {
            SetFloat(value);
            Show();
        }
    }
}
