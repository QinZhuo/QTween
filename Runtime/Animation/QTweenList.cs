using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QTool.Tween.Component
{
    public class QTweenList : QTweenBehavior
    {
        public List<QTweenBehavior> tweenList = new List<QTweenBehavior>();

        public override void ReverseStartEnd()
        {
            foreach (var item in tweenList)
            {
                item.ReverseStartEnd();
            }
        }

        protected override QTween ShowTween()
        {

            var list = QTool.Tween.QTweenList.Get();
            foreach (var tween in tweenList)
            {
                list.AddLast(tween.Anim);
            }
            return list;
        }
        private void Reset()
        {
            tweenList.AddRange(GetComponentsInChildren<QTweenBehavior>());
            tweenList.Remove(this);
        }
    }
}

