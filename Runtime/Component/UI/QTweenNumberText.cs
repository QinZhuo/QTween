using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace QTool.Tween.Component
{
    public class QTweenNumberText : QTweenComponent<float>
    {
        public override float CurValue
		{
			get => _curValue; set { OnValueChange?.Invoke(value.ToString(format)); _curValue = value; }
		}
        private float _curValue = 0;
        public string format="F0";
		public UnityEvent<string> OnValueChange = new UnityEvent<string>();
		[UnityEngine.Serialization.FormerlySerializedAs("showTween")]
		public QTweenComponent showTween;
		public QTweenComponent changeTween;
		protected override void Reset()
        {
            var text= GetComponentInChildren<Text>();
			if (text != null)
			{
				OnValueChange.AddPersistentListener(text.GetUnityAction<string>("set_text"));
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
			Complete();
			var floatAnim = Anim as QTween<float>;
			floatAnim.StartValue = EndValue;
			StartValue = EndValue; 
			floatAnim.EndValue = value;
			EndValue = value;
		}
		public override void Play(bool show) {
			if (!EndValue.Similar(0)) {
				showTween?.Show();
				changeTween?.Show();
			}
			base.Play(show);
		}
		protected override void OnComplete() {
			base.OnComplete();
			if (!EndValue.Similar(0)) {
				changeTween?.Hide();
			}
			else {
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
