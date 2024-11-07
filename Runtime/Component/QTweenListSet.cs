using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace QTool.Tween.Component
{
    public class QTweenListSet : QTweenComponent
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
		public override void Play(bool show) {
#if UNITY_EDITOR
			if (!Application.isPlaying) {
				ClearAnim();
			}
#endif
			base.Play(show);
		}
		public override void ClearAnim()
		{
			foreach (var node in tweenList)
			{
				node.qTween?.ClearAnim();
			}
			if(Anim is QTweenList list)
			{
				list.Clear();
			}
			base.ClearAnim();
		}
	}
}

