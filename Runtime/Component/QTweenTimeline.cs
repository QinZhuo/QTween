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
    public class QTweenTimeline : QTweenComponent<float>
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
		protected override void OnValidate() {
			base.OnValidate();
			StartValue = 0;
			EndValue = (float)playableDirector.duration;
			playableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
		}
		public override float CurValue
        {
            get =>(float) playableDirector.time; set
			{playableDirector.SetTime(value);}
        }
		public override void Play(bool show) {
			playableDirector.Play(playableAsset);
			base.Play(show);
		}
	}
}
