using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QTool.Tween
{
    public class QTweenLoop : QTweenBehavior
    {
        public QTweenBehavior qTween;
        protected override QTween ShowTween()
        {
            return qTween.Anim;
        }
        public override void ClearAnim()
        {
            qTween.ClearAnim();
            base.ClearAnim();
        }
        public void Awake()
        {
            //OnShow.AddListener(() => Anim.Play(false));
            //OnHide.AddListener(() => Anim.Play(true));

        }
        
        public override void Play(bool show)
        {
            if (show)
            {
                OnShow.AddListener(() => Anim.Play(false));
                OnHide.AddListener(() => Anim.Play(true));
                Anim.Play(true);
            }
            else
            {
                OnShow.RemoveAllListeners();
                OnHide.RemoveAllListeners();
                Anim.Play(false);
            }

        }
    }
}
