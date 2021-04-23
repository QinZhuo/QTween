using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace QTool.Tween.Component
{
    [RequireComponent(typeof(Text))]
    public class QTweenFloatText : QTweenFloat
    {
        public Text Text
        {
            get
            {
                return _text;
            }
        }

        public override float CurValue
        {
            get => curValue; set { Text.text = value.ToString(); curValue = value; }
        }

        public Text _text;
        private float curValue = 0;
        public float TargetValue;
        protected override void Reset()
        {
            _text = GetComponent<Text>();
            base.Reset();
        }
        public void SetFloat(float value)
        {
            TargetValue = value;
        }

        public void SetFloatAnim(float value)
        {
            SetFloat(value);
            Show();
        }
    }
}