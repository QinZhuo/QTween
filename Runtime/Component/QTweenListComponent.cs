using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QTool.Tween.Component
{
    public class QTweenListComponent : QTweenComponent
    {
        [System.Serializable]
        public class QTweenlistNode
        {
            [HideInInspector]
            public string name;
			[QPopup]
            public QTweenComponent qTween;
            public QTweenList.TweenListType type = QTweenList.TweenListType.异步播放;
            public void FreshName()
            {
				name = qTween?.name;
			}
        }
		[QName("动画队列")]
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
		
	}
}

