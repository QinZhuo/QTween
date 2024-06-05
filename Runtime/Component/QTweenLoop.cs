using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace QTool.Tween {
	public class QTweenLoop : QTweenComponent {
		public QTweenComponent qTween;
		public override QTween Anim => qTween.Anim;
		public bool repeat = false;
		protected override QTween GetTween() {
			return null;
		}
		private void Reset() {
			qTween = GetComponent<QTweenComponent>();
		}
		string loopKey = QTool.GetGuid();
		public override async Task PlayAsync(bool show) {
			if (show) {
				var flag = loopKey;
				while (loopKey == flag && this != null && Anim != null) {
					Anim.SetPlayer(this).Play(true);
					await Anim.WaitOverAsync();
					if (repeat) {
						Anim.Play(false);
						Anim.Complete();
					}
					else {
						Anim.SetPlayer(this).Play(false);
						await Anim.WaitOverAsync();
					}
					
				}
			}
			else {
				loopKey = QTool.GetGuid();
				Anim.SetPlayer(this).Play(false);
			}
		}
		protected override void OnDestroy() {
			loopKey = QTool.GetGuid();
			base.OnDestroy();
		}

	}
}
