using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QTool.Tween
{
    public static class AudioExtends 
    {
        public static void Play(this AudioSource audio, AudioClip clip, float delay)
        {
            if (audio.isPlaying)
            {
                audio.Stop(delay);
            }
            audio.volume = 0;
            audio.clip = clip;
            audio.Play();
            var tween = QTweenManager.Tween(() => audio.volume, (value) => audio.volume = value, 1, delay);
            tween.Play();
        }
        public static void Stop(this AudioSource audio, float delay)
        {
            audio.volume = 1;
            QTweenManager.Tween(() => audio.volume, (value) => audio.volume = value, 0, delay).OnComplete(() => {
                audio.Stop();
            }).Play();
        }
    }

}