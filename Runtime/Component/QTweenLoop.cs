using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace QTool.Tween
{
    public class QTweenLoop : QTweenComponent
    {
        public QTweenComponent qTween;
		public override QTween Anim => qTween.Anim;
		protected override QTween GetTween()
		{
			return null;
		}
		private void Reset()
        {
			 qTween = GetComponent<QTweenComponent>();
        }
		string loopKey = QTool.GetGuid();
		public override async Task PlayAsync(bool show)
		{
			if (show)
			{
				var flag = loopKey;
				while (loopKey == flag && this != null&&Anim!=null)
				{
					Anim.Play(true,this);
					await Anim.WaitOverAsync();
					Anim.Play(false,this);
					await Anim.WaitOverAsync();
				}
			}
			else
			{
				loopKey = QTool.GetGuid();
				Anim.Play(false,this);
			}
		}
		protected override void OnDestroy()
		{
			loopKey = QTool.GetGuid();
			base.OnDestroy();
		}

	}
}
