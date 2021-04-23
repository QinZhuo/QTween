using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QTool.Tween.Component
{
    public class QTweenList : QTweenBehavior
    {
        [System.Serializable]
        public class QTweenlistNode
        {
            [HideInInspector]
            public string name;
            public QTweenBehavior qTween;
            public QTool.Tween.QTweenList.TweenListType type = Tween.QTweenList.TweenListType.Í¬Ê±²¥·Å;
            public QTweenlistNode(QTweenBehavior tween)
            {
                this.qTween = tween;
                FreshName();
            }
            public void FreshName()
            {
                name = qTween.name+"."+qTween;
            }
        }
        public List<QTweenlistNode> tweenList = new List<QTweenlistNode>();
        public override void ReverseStartEnd()
        {
            foreach (var item in tweenList)
            {
                item.qTween.ReverseStartEnd();
            }
        }
        protected override void OnValidate()
        {
            base.OnValidate();
            foreach (var tween in tweenList)
            {
                tween.FreshName();
            }
        }
        protected override QTween ShowTween()
        {
            var list = QTool.Tween.QTweenList.Get();
            foreach (var tween in tweenList)
            {
                list.AddLast(tween.qTween.Anim,tween.type);
            }
            return list;
        }
        private void Reset()
        {
            foreach (var tween in GetComponentsInChildren<QTweenBehavior>())
            {
                if (tween == this) continue;
                tweenList.Add(new QTweenlistNode(tween));
            }
        }
        public override string ToString()
        {
            return Anim.ToString();
        }
    }
}

