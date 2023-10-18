using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
				var tween = view.GetComponent<QTweenBehavior>();
				var tweenNode = tweenList.FirstOrDefault(obj => Equals(obj.qTween, tween));
				if (tweenNode != null)
				{
					tweenList.Remove(tweenNode);
					ClearAnim();
				}
			};
        }
    }
}
