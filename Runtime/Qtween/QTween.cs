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
        public static QueueTween CreateQueue()
        {
            return QueueTween.GetQueue().Init() as QueueTween;
        }
        public Func<float, float> curve = TweenAnimationCurve.Linear;
        public float startTime { protected set; get; }
        public float endTime { protected set; get; }
        public float time = 0f;
        float lastTime = 0f;
        public virtual float duration
        {
            get
            {
                return endTime - startTime;
            }
            set
            {
                endTime = startTime + value;
            }
        }
        public void SetStartTime(float time)
        {
             var value= time - startTime;
            startTime += value;
            endTime += value;
        }
        bool _autoStop = true;
        public bool autoStop=true;
        public bool playBack = false;
        public bool isPlaying = false;
        public bool ignoreTimeScale = false;
        public abstract QTween Init();
        public virtual QTween Play(bool back=false)
        {
            QTweenManager.Add(this);
            isPlaying = true;
            playBack = back;
            return this;
        }
        public virtual void Update()
        {
            if (!isPlaying) return;
            UpdateTime();
            Update(time);
        }
        public virtual void Update(float time)
        {
            if (!isPlaying) return;
            this.time = time;
            CheckOver();
            UpdateValue();
        }
        public virtual void CheckOver()
        {
      
            if (lastTime != time)
            {
          
                if (!playBack)
                {
                    if (lastTime <= endTime && time >= endTime)
                    {
                        Complete();
                        time = endTime;
                    }
                }
                else
                {
                  
                    if (lastTime >= startTime && time <= startTime)
                    {
                        Complete();
                        time = startTime;
                    }
                }
            }
            lastTime = time;
        }
        void UpdateTime()
        {
            if (!isPlaying) return;
            time += (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) * (playBack ? -1 : 1);
        }
        public abstract void UpdateValue();
        public  void Complete()
        {
            time = playBack ? startTime: endTime;
            isPlaying = false;
        
            if (autoStop)
            {
                QTweenManager.Kill(this);
            }
            OnComplete?.Invoke();
        }
        public  Action OnComplete;

      
        public virtual QTween Pause()
        {
            isPlaying = false;
            return this;
        }
        public virtual QTween Stop()
        {
            isPlaying = false;
            QTweenManager.Kill(this);
            return this;
        }
        public QTween SetCurve(EaseCurve ease)
        {
            this.curve = Curve.GetEaseFunc(ease);
            return this;
        }
        public QTween SetCurve(Func<float, float> curveFunction)
        {
            this.curve = curveFunction;
            return this;
        }
        public QTween IgnoreTimeScale(bool value=true)
        {
            ignoreTimeScale = value;
            return this;
        }
        public virtual QTween AutoStop(bool kill=true)
        {
            autoStop = kill;
            return this;
        }
        public static float Lerp(float a, float b, float t)
        {
            var dir = b - a;
            return a + dir * t;
        }

        public static Vector2 Lerp(Vector2 star, Vector2 end, float t)
        {
            return new Vector2(Lerp(star.x, end.x, t), Lerp(star.y, end.y, t));
        }
        public static Vector3 Lerp(Vector3 star, Vector3 end, float t)
        {
            return new Vector3(Lerp(star.x, end.x, t), Lerp(star.y, end.y, t), Lerp(star.z, end.z, t));
        }
        public static Quaternion Lerp(Quaternion star, Quaternion end, float t)
        {
            var result = Lerp(star.eulerAngles, end.eulerAngles, t);
            return Quaternion.Euler(result);
        }
        public static Color Lerp(Color star, Color end, float t)
        {
            return new Color(Lerp(star.r, end.r, t), Lerp(star.g, end.g, t), Lerp(star.b, end.b, t), Lerp(star.a, end.a, t));
        }
    }

    public class QueueTween : QTween
    {
        public static QueueTween GetQueue()
        {
        
            return new QueueTween();
        }
        private QueueTween()
        {

        }
        
        public List<QTween> tweenList = new List<QTween>();
        public override QTween Play(bool back = false)
        {
            foreach (var tween in tweenList)
            {
                tween.playBack = back;
            }
            return base.Play(back);
        }
        public override QTween Init()
        {
            return this;
        }
        public override QTween AutoStop(bool kill = true)
        {
            foreach (var tween in tweenList)
            {
                tween.AutoStop(kill);
            }
            return base.AutoStop(kill);
        }
        public QueueTween PushEnd(QTween tween)
        {
            tweenList.Add(tween.Stop());
            tween.SetStartTime(duration);
            endTime += tween.duration;
            return this;
        }
        public QueueTween Insert(float insertTime,QTween tween)
        {
            tweenList.Add(tween.Stop());
            tween.SetStartTime(insertTime);
            if (tween.endTime > endTime)
            {
                endTime = tween.endTime;
            }
            return this;
        }
        public override void UpdateValue()
        {
            foreach (var tween in tweenList)
            {
                if (!tween.isPlaying)
                {
                    if (time > tween.startTime && time < tween.endTime)
                    {
                        tween.isPlaying = true;
                    }
                }
                
                tween.Update(time);
            }
        }
    }
    public class QTween<T>:QTween
    {
        public T start;
        public T end;
       
        public new Func<T, T, float, T> Lerp;
        public Func<T> Get;
        public Action<T> Set;
        public override QTween Init()
        {
         
            time = 0;
            start = Get();
            return this;
        }
   

        public override void UpdateValue()
        {
            if (time >=startTime && time <= endTime)
            {
                Set(Lerp(start, end, curve.Invoke((time - startTime) / duration)));
            }
        }
    }
    public static class TransformExtends
    {

        public static Vector3 GetPos(this Transform transform)
        {
            return transform.position;
        }
 
        public static void SetPos(this Transform transform,Vector3 postion)
        {
            transform.position=postion;
        }
        public static QTween RotTo(this Transform transform, Vector3 value, float duration)
        {
            return QTween.Tween(()=>transform.rotation.eulerAngles,
            (setValue)=> { transform.rotation =Quaternion.Euler( setValue); },
            QTween.Lerp, value, duration);
        }
        public static QTween LocalRotTo(this Transform transform, Vector3 value, float duration)
        {
            return QTween.Tween(() => transform.localRotation.eulerAngles,
            (setValue) => { transform.localRotation = Quaternion.Euler(setValue); },
            QTween.Lerp, value, duration);
        }
        public static QTween PosTo(this Transform transform, Vector3 postion,float duration)
        {
            return  QTween.Tween(transform.GetPos,
            transform.SetPos,
            QTween.Lerp, postion, duration);
        }
        public static QTween PosXTo(this Transform transform, float value, float duration)
        {
            return QTween.Tween(()=>transform.position.x,
            (setValue)=> { transform.position = new Vector3(setValue, transform.position.y, transform.position.z); },
            QTween.Lerp, value, duration);
        }

        public static QTween PosYTo(this Transform transform, float value, float duration)
        {
            return QTween.Tween(() => transform.position.y,
            (setValue) => { transform.position = new Vector3( transform.position.x, setValue, transform.position.z); },
            QTween.Lerp, value, duration);
        }
        public static QTween PosZTo(this Transform transform, float value, float duration)
        {
            return QTween.Tween(() => transform.position.z,
            (setValue) => { transform.position = new Vector3(transform.position.x, transform.position.y, setValue); },
            QTween.Lerp, value, duration);
        }
        public static QTween LocalPosXTo(this Transform transform, float value, float duration)
        {
            return QTween.Tween(() => transform.localPosition.x,
            (setValue) => { transform.localPosition = new Vector3(setValue, transform.localPosition.y, transform.localPosition.z); },
            QTween.Lerp, value, duration);
        }

        public static QTween LocalPosYTo(this Transform transform, float value, float duration)
        {
            return QTween.Tween(() => transform.localPosition.y,
            (setValue) => { transform.localPosition = new Vector3(transform.localPosition.x, setValue, transform.localPosition.z); },
            QTween.Lerp, value, duration);
        }
        public static QTween LocalPosZTo(this Transform transform, float value, float duration)
        {
            return QTween.Tween(() => transform.localPosition.z,
            (setValue) => { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, setValue); },
            QTween.Lerp, value, duration);
        }

        public static QTween LocalPosTo(this Transform transform, Vector3 postion, float duration)
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
    public static class RectTransformExtends
    {
        public static QTween AnchorPosTo(this RectTransform transform, Vector2 postion, float duration)
        {
            return QTween.Tween(() => transform.anchoredPosition,
            (pos) => { transform.anchoredPosition = pos; },
            QTween.Lerp, postion, duration);
        }
    }
    public static class MaskableGraphicExtends
    {

        public static QTween FillAmountTo(this Image graphic,float value, float duration)
        {
            return QTween.Tween(() => graphic.fillAmount,
            (setValue) => { graphic.fillAmount = setValue; },
            QTween.Lerp, value, duration);
        }
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

