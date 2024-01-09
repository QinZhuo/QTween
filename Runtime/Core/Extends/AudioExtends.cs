using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QTool.Tween
{
    public static class AudioExtends 
    {
		public static void Play(this AudioSource audio, AudioClip clip, float delay)
		{
			audio.volume = 0;
			audio.clip = clip;
			audio.Play();
			QTweenManager.Tween(() => audio.volume, (value) => audio.volume = value, 1, delay).SetPlayer(audio).Play(true);
		}
		public static void Stop(this AudioSource audio, float delay)
		{
			audio.volume = 1;
			QTweenManager.Tween(() => audio.volume, (value) => audio.volume = value, 0, delay).OnComplete(() =>
			{
				audio.Stop();
			}).SetPlayer(audio).Play(true);
		}
    }

}
