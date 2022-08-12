using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace QTool.Tween.Component
{
    [RequireComponent(typeof(CanvasGroup))]
    public class QTweenAlpha : QTweenBehavior<float>
    {
        private CanvasGroup group;
        public bool controlRaycast = true;
		private void Awake()
		{
			group = GetComponent<CanvasGroup>();
			if (group == null)
			{
				Debug.LogError(name + "获取 " + nameof(CanvasGroup) + " 出错");
			}
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
