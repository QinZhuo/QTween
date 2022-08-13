using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QTool.Tween.Component
{
    public class QTweenListBehavior : QTweenBehavior
    {
        [System.Serializable]
        public class QTweenlistNode
        {
            [HideInInspector]
            public string name;
            public QTweenBehavior qTween;
            public QTweenList.TweenListType type = QTweenList.TweenListType.异步播放;
            public void FreshName()
            {
				name = qTween?.name;
			}
        }
		[ViewName("动画队列")]
		[SerializeField]
        protected List<QTweenlistNode> tweenList = new List<QTweenlistNode>();
        protected override QTween GetTween()
        {
			var list= QTweenList.Get();
            foreach (var tween in tweenList)
            {
                if (tween.qTween == null) continue;
				list.AddLast(tween.qTween.Anim, tween.type);
            }
            return list;
        }
        public override string ToString()
        {
            return Anim.ToString();
        }
		public override void ClearAnim()
		{
			foreach (var node in tweenList)
			{
				node.qTween?.ClearAnim();
			}
			base.ClearAnim();
		}
	}
}

