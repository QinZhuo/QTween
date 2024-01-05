using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace QTool.Tween.Component
{
    [RequireComponent(typeof(CanvasGroup))]
    public class QTweenUIAlpha : QTweenComponent<float>
    {
		CanvasGroup _group = null;

		private CanvasGroup Group {
			get
			{
				if (_group == null)
				{
					_group = GetComponent<CanvasGroup>();
				}
				return _group;
			}
		}

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
