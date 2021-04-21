using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace QTool.Tween.Component
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIAlphaAnimation : QTweenFloat
    {
        public CanvasGroup group;
        private void Reset()
        {
            if (group == null)
            {
                group = GetComponent<CanvasGroup>();
            }
        }
        public override float CurValue
        {
            get => group.alpha;
            set
            {
                group.alpha = value;
                var boolvalue = group.alpha >= 0.9f;
                group.blocksRaycasts = boolvalue;
                group.interactable = boolvalue;
            }
        }
    }
}