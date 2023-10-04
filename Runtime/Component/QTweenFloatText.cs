using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace QTool.Tween.Component
{
    public class QTweenFloatText : QTweenBehavior<float>
    {
        void ChangeText(string value)
        {
            if (_text != null)
            {
                _text.text = value;
            }
        }
        public override float CurValue
        {
            get => curValue; set { OnValueChange?.Invoke( value.ToString(format)); curValue = value; }
        }

        public Text _text;
        private float curValue = 0;
		public QTweenBehavior ControlShowHide;
        public string format="F0";
        public StringEvent OnValueChange; 
        protected override void Reset()
        {
            _text = GetComponentInChildren<Text>();
            base.Reset();
        }
        private void Awake()
        {
            OnValueChange.AddListener(ChangeText);
        }
		public void SetFloat(float value)
		{
			if (value.Similar(0))
			{
				ControlShowHide?.Hide();
			}
			else
			{
				ControlShowHide?.Show();
			}
			Anim.Stop();
			var floatAnim = Anim as QTween<float>;
			floatAnim.StartValue = curValue;
			floatAnim.EndValue = value;
		}

        public void SetFloatAnim(float value)
        {
            SetFloat(value);
            Show();
        }
    }
}
