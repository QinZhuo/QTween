﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace QTool.Tween.Component
{
    public class QTweenUIScale : QTweenVector2
    {
        public override Vector2 CurValue { get => transform.localScale; set => transform.localScale = value; }
        public override string ToString()
        {
            return "大小 "+ base.ToString();
        }
    }
}
