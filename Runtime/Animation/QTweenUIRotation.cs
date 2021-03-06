﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace QTool.Tween.Component
{
    public class QTweenUIRotation : QTweenQuaternion
    {
        public override Quaternion CurValue { get => RectTransform.rotation; set => RectTransform.rotation = value; }
    }
}
