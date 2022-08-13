using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QTool.Tween.Component
{
    [RequireComponent(typeof(QObjectList))]
    public class QTweenLayout: QTweenListBehavior
    {
        public QTweenList.TweenListType listType = QTweenList.TweenListType.顺序播放;
        public QObjectList layout;

        private void Reset()
        {
            layout = GetComponent<QObjectList>();
        }
  
        private void Start()
        {
            layout.OnCreate += (view) =>
            {
				if (view != null)
				{
					var node = new QTweenlistNode { qTween = view.GetComponent<QTweenBehavior>(), type = listType };
                    node.FreshName();
                    tweenList.Add(node);
					ClearAnim();
                }
            };
            layout.OnPush += (view) =>
            {
				var tweenNode= tweenList.Get(view.GetComponent<QTweenBehavior>(), (obj) => obj.qTween);
                if (tweenNode != null)
                {
                    tweenList.Remove(tweenNode);
					ClearAnim();
				}
            };
        }
    }
}
