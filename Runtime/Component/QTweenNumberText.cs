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
		[QReadonly]
        public Text text;
        private float curValue = 0;
        public string format="F0";
        public StringEvent OnValueChange;
		[UnityEngine.Serialization.FormerlySerializedAs("ControlShowHide")]
		public QTweenComponent showTween;
		public QTweenComponent changeTween;
		protected override void Reset()
        {
            text = GetComponentInChildren<Text>();
            base.Reset();
        }
        private void Awake()
        {
            OnValueChange.AddListener(ChangeText);
        }
		private void ChangeText(string value)
		{
			if (text != null)
			{
				text.text = value;
			}
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
			if (show && !EndValue.Similar(0))
			{
				showTween?.Show();
			}
			if (!EndValue.Equals(StartValue))
			{
				changeTween?.Show();
			}
			await base.PlayAsync(show);
			if (!EndValue.Equals(StartValue))
			{
				changeTween?.Hide();
			}
		}
		protected override void OnAnimOver()
		{
			base.OnAnimOver();
			if (curValue.Similar(0))
			{
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
