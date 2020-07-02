using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace QTool.Tween
{
    public abstract class QTween
    {
        public static QTween Tween<T>(Func<T> Get, Action<T> Set, Func<T, T, float, T> Lerp, T end, float duration)
        {
            var tween = new QTween<T>()
            {
                Set = Set,
                Get = Get,
                end = end,
                Lerp = Lerp,
                duration = duration
            };
            
            return tween.Init();
        }
        public Func<float, float> curve = Curve.Linear;

        public float time = 0f;
        public float duration = 1f;
        public bool AutoKill=true;
        public bool PlayBack = false;
        public QTween Init()
        {
            QTweenManager.Add(this);
            return this;
        }
        public abstract QTween Play();
        public abstract void Update();
    }
    public class QTween<T>:QTween
    {
        public T start;
        public T end;
       
        public Func<T, T, float, T> Lerp;
        public Func<T> Get;
        public Action<T> Set;
        public override QTween Play()
        {
            start = Get();
            return this;
        }
        public override void Update()
        {
           
            if(time>0&& time<=duration)
            {
                Set(Lerp(start, end, curve.Invoke(time / duration)));
            }
            else if(time>=duration)
            {
                if (AutoKill)
                {
                    QTweenManager.Kill(this);
                }
              //  time = -1;
            }
            time += Time.deltaTime;
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
        public static QTween TweenMove(this Transform transform, Vector3 postion,float duration)
        {
            return  QTween.Tween(() => transform.position,
            (pos) => {transform.position = pos; },
            Lerp, postion, duration);
        }
    }
}

