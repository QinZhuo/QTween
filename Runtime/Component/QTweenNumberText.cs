using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

namespace QTool.Tween.Component
{
    public class QTweenNumberText : QTweenComponent<float>, IQEvent<float>
    {
        public override float CurValue
        {
            get => _curValue; set { OnValueChange?.Invoke( value.ToString(format)); _curValue = value; }
        }
        private float _curValue = 0;
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
		private void Awake()
		{
			if (showTween != null)
			{
				showTween.Hide();
				showTween.Complete();
			}
		}
		public void SetWithoutAnim(float value)
		{
			Anim.Stop();
			var floatAnim = Anim as QTween<float>;
			floatAnim.StartValue = _curValue;
			floatAnim.EndValue = value;
		}
		public override async Task PlayAsync(bool show)
		{
			if (!CurValue.Similar(0))
			{
				showTween?.Show();
				changeTween?.Show();
			}
			await base.PlayAsync(show);
			if (!CurValue.Similar(0))
			{
				changeTween?.Hide();
			}
			else
			{
				changeTween.Complete();
				showTween?.Hide();
			}
		}
		public void Set(float value)
		{
			SetWithoutAnim(value);
			if (!StartValue.Similar(EndValue))
			{
				Show();
			}
		}
	}
}
