using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace QTool.Tween.Component
{
    [RequireComponent(typeof(CanvasGroup))]
    public class QTweenAlpha : QTweenBehavior<float>
    {
		CanvasGroup _group = null;

		private CanvasGroup Group => _group ?? GetComponent<CanvasGroup>();

		public bool controlRaycast = true;
		public override float CurValue
        {
            get => Group.alpha;
            set
            {
                Group.alpha = value;
                if (controlRaycast)
                {
                    var boolvalue = Group.alpha >= 0.9f;
                    Group.blocksRaycasts = boolvalue;
                    Group.interactable = boolvalue;
                }
               
            }
        }
    }
}
