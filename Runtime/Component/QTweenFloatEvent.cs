using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace QTool.Tween.Component
{
    public class QTweenFloatEvent: QTweenComponent<float>
    {
        public override float CurValue
        {
            get => curValue; set { curValue = value; FloatEvent.Invoke(value); }
        }
		[QName("当前值")]
		public float curValue =0;
		public FloatEvent FloatEvent=new FloatEvent();
    }
}
