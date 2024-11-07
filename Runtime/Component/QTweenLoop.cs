using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace QTool.Tween
{
	public class QTweenLoop : QTweenComponent {
		public QTweenComponent qTween;
		public bool repeat = false;
		public override QTween Anim {
			get {
				if (_anim != qTween._anim) {
					ClearAnim();
				}
				return base.Anim;
			}
		}
		protected override QTween GetTween()
		{
			return qTween.Anim;
		}
		private void Reset()
        {
			 qTween = GetComponent<QTweenComponent>();
        }
		public override void Play(bool show) {
			if (show) {
				base.Play(show);
			}
			else {
				Anim.OnCompleteEvent -= OnComplete;
				Anim.Play(false);
				Anim.Complete();
				Anim.OnCompleteEvent += OnComplete;
			}
		}
		public override void ClearAnim() {
			qTween?.ClearAnim();
			_anim = null;
		}
		protected override void OnComplete() {
			base.OnComplete();
			if (repeat) {
				if (Anim.PlayForwads) {
					Anim.Time = 0;
					Anim.Play(true);
				}
			}
			else {
				Anim.Play(!Anim.PlayForwads);
			}
		}
	}
}
