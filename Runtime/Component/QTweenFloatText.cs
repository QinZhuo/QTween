using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace QTool.Tween.Component
{
    public class QTweenFloatText : QTweenFloat
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
        public string format="F0";
        public StringEvent OnValueChange; 
        protected override void Reset()
        {
            _text = GetComponent<Text>();
            base.Reset();
        }
        private void Awake()
        {
            OnValueChange.AddListener(ChangeText);
        }
        public void SetFloat(float value)
        {
            Anim.Stop();
            var floatAnim= Anim as QTween<float>;
            floatAnim.StartValue = curValue;
            floatAnim.EndValue = value;
            floatAnim.time = 0;
        }

        public void SetFloatAnim(float value)
        {
            SetFloat(value);
            Show();
        }
    }
}