using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace QTool.Tween
{
    public class QTween
    {
        public static Coroutine Start(IEnumerator enumerator)
        {
            return QTweenManager.Instance.StartCoroutine(enumerator);
        }
      
        public static IEnumerator Tween<T>(Func< T> Get, Action<T> Set,Func<T,T,float,T> Lerp,T end, float duration)
        {
            var  () =>
            {
                float time = 0f;
                var start = Get();
                while (time < duration)
                {
                    yield return null;
                    time += Time.deltaTime;
                    Set(Lerp(start, end, time / duration));
                }
                Set(end);
            };
           
        }
    }
    public static class TransformExtends
    {

        public static float Lerp(float a, float b, float t)
        {
            var dir = b - a;
            return a + dir * t;
        }
        public static Vector3 Lerp(Vector3 star, Vector3 end, float t)
        {
            return new Vector3(Lerp(star.x, end.x, t), Lerp(star.y, end.y, t), Lerp(star.z, end.z, t));
        }
        public static IEnumerator TweenMove(this Transform transform, Vector3 postion,float duration)
        {
            
            return QTween.Tween(() => transform.position,
            (pos) => { transform.position = pos; },
            Lerp, postion, duration);
        }
    }
}

