using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Playables;
using System.Threading.Tasks;

namespace QTool.Tween.Component
{
	[RequireComponent(typeof(PlayableDirector))]
    public class QTweenTimeline : QTweenBehavior<float>
    {
		[QName("播放器")]
		public PlayableDirector playableDirector;
		[QName("动画文件")]
		public PlayableAsset playableAsset;
		protected override void Reset()
		{
			playableDirector=GetComponent<PlayableDirector>();
			playableAsset = playableDirector.playableAsset;
			base.Reset();
		}
		public override float CurValue
        {
            get =>(float) playableDirector.time; set
			{playableDirector.SetTime(value);}
        }

		private void OnValidate() 
		{
			StartValue = 0;
			if (playableAsset!=null)
			{
				animTime = (float)playableAsset.duration;
				EndValue = (float)playableAsset.duration;
			}
			else
			{
				animTime = 0;
				EndValue = 0;
			}
		}
		public override Task PlayAsync(bool show)
		{
			playableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
			playableDirector.Play(playableAsset);
			return base.PlayAsync(show);
		}
	}
}
