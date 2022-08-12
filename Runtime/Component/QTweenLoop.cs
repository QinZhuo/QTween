using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace QTool.Tween
{
    public class QTweenLoop : QTweenBehavior
    {
        public QTweenBehavior qTween;
        protected override QTween GetTween()
        {
            return qTween?.Anim;
        }
        private void Reset()
        {
            qTween = GetComponent<QTweenBehavior>();
        }
		string loopKey = QId.GetNewId();
        public override async void Play(bool show)
        {
			if (show)
			{
				var flag = loopKey;
				while (loopKey ==flag && this != null)
				{
					await Anim.PlayAsync(true);
					await Anim.PlayAsync(false);
				}
			}
			else
			{
				loopKey = QId.GetNewId();
				Anim.Play(false);
			}
		
        }
    }
}
