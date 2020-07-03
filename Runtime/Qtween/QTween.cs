using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
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
            
            return tween.Init().Play();
        }
        public Func<float, float> curve = Curve.Linear;

        public float time = 0f;
        public float duration = 1f;
        public bool AutoKill=true;
        public bool PlayBack = false;
        public bool isPlaying = false;
        public bool ignoreTimeScale = false;
        public List<QTween> tweenList;
        public QTween Init()
        {
            QTweenManager.Add(this);
            return this;
        }
        public abstract QTween Play();
        public abstract void Update();
        public abstract void Complete();
        public  Action OnComplete;

        public virtual void Pause()
        {
            isPlaying = false;
        }
        public QTween SetCurve(Func<float,float> curveFunction)
        {
            this.curve = curveFunction;
            return this;
        }
        public QTween IgnoreTimeScale(bool value=true)
        {
            ignoreTimeScale = value;
            return this;
        }
        public QTween Add(QTween tween)
        {
            if (tweenList == null)
            {
                tweenList = new List<QTween>();
            }
            tween.Pause();
            tweenList.Add(tween);
            return this;
        }
        public static float Lerp(float a, float b, float t)
        {
            var dir = b - a;
            return a + dir * t;
        }
       
        public static Vector3 Lerp(Vector3 star, Vector3 end, float t)
        {
            return new Vector3(Lerp(star.x, end.x, t), Lerp(star.y, end.y, t), Lerp(star.z, end.z, t));
        }
        public static Color Lerp(Color star, Color end, float t)
        {
            return new Color(Lerp(star.r, end.r, t), Lerp(star.g, end.g, t), Lerp(star.b, end.b, t), Lerp(star.a, end.a, t));
        }
    }
    public class QTween<T>:QTween
    {
        public T start;
        public T end;
       
        public new Func<T, T, float, T> Lerp;
        public Func<T> Get;
        public Action<T> Set;
        
        public override QTween Play()
        {
            start = Get();
            isPlaying = true;
            return this;
        }

        public override void Update()
        {
            if (!isPlaying) return;
            if(time>0&& time<=duration)
            {
                Set(Lerp(start, end, curve.Invoke(time / duration)));
            }
            else if(time>=duration)
            {
                Complete();
            }
            time += ignoreTimeScale?Time.unscaledDeltaTime: Time.deltaTime;
        }
        public override void Complete()
        {
            time = duration;
            Set(Lerp(start, end, curve.Invoke(time / duration)));
            isPlaying = false;
            if (tweenList != null)
            {
                foreach (var tween in tweenList)
                {
                    tween.Play();
                }
            }
            if (AutoKill)
            {
                QTweenManager.Kill(this);
            }
            OnComplete?.Invoke();
        }

    }
    public static class TransformExtends
    {

        public static QTween Move(this Transform transform, Vector3 postion,float duration)
        {
            return  QTween.Tween(() => transform.position,
            (pos) => {transform.position = pos; },
            QTween.Lerp, postion, duration);
        }
        public static QTween LocalMove(this Transform transform, Vector3 postion, float duration)
        {
            return QTween.Tween(() => transform.localPosition,
            (pos) => { transform.localPosition = pos; },
            QTween.Lerp, postion, duration);
        }
        public static QTween ScaleTo(this Transform transform, Vector3 endScale, float duration)
        {
            return QTween.Tween(() => transform.localScale,
            (scale) => { transform.localScale = scale; },
            QTween.Lerp, endScale, duration);
        }
    }
    public static class MaskableGraphicExtends
    {

        public static QTween ColorTo(this MaskableGraphic graphic, Color endColor, float duration)
        {
            return QTween.Tween(() => graphic.color,
            (color) => { graphic.color = color;},
            QTween.Lerp, endColor, duration);
        }

        public static QTween AlphaTo(this MaskableGraphic graphic, float endAlpha, float duration)
        {
            return QTween.Tween(() => graphic.color.a,
            (alpha) => { graphic.color =new Color(graphic.color.r,graphic.color.g,graphic.color.b,alpha); },
            QTween.Lerp, endAlpha, duration);
        }
    }
}

