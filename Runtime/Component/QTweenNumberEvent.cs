using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

namespace QTool.Tween.Component
{
    public class QTweenNumberEvent: QTweenComponent<float>
    {
        public override float CurValue
        {
            get => curValue; set { curValue = value; FloatEvent.Invoke(value); }
        }
		[QName("当前值")]
		public float curValue =0;
		public UnityEvent<float> FloatEvent =new UnityEvent<float>();
    }
}
