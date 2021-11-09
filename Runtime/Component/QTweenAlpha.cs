using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace QTool.Tween.Component
{
    [RequireComponent(typeof(CanvasGroup))]
    public class QTweenAlpha : QTweenFloat
    {
        public CanvasGroup group;
        public bool controlRaycast = true;
        protected override void Reset()
        {
            if (group == null)
            {
                group = GetComponent<CanvasGroup>();
            }
            base.Reset();
        }
        public override float CurValue
        {
            get => group.alpha;
            set
            {
                group.alpha = value;
                if (controlRaycast)
                {
                    var boolvalue = group.alpha >= 0.9f;
                    group.blocksRaycasts = boolvalue;
                    group.interactable = boolvalue;
                }
               
            }
        }
    }
}