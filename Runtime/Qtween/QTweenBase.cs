using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QTool;

namespace QTool.Tween
{
    public class QTweenList:PoolObject<QTweenList>
    {
        public QTweenBase first;
        public QTweenBase end;
        public void Play(bool forwards=true)
        {
            (forwards ? first : end)?.Play(forwards);
        }
        public override void OnPoolRecover()
        {
            first = null;
            end = null;
        }

        public override void OnPoolReset()
        {
        }
    }
    public abstract class QTweenBase:IPoolObject
    {
      
        //public static QTweenQueue Queue()
        //{
        //    return QTweenQueue.GetQueue().Init().Play() as QTweenQueue;
        //}
        public Func<float, float> TCurve = TweenAnimationCurve.Linear;
        public QTweenBase SetCurve(EaseCurve ease)
        {
            TCurve = Curve.GetEaseFunc(ease);
            return this;
        }
        public QTweenBase SetCurve(Func<float, float> curveFunction)
        {
            TCurve = curveFunction;
            return this;
        }
        public float time = 0f;
        float lastTime = 0f;
        public float Duration { protected set; get; }
        public bool _isPlaying=false;
        public bool autoDestory=true;
        public bool playForwads = false;
        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
        }
        public bool pause = false;
        public bool ignoreTimeScale = false;
        public QTweenBase Next(QTweenBase next)
        {
            if (next == null) return this;
            if (TweenList == null)
            {
                TweenList = QTweenList.Get();
                TweenList.first = this;
            }
            this.next = next;
            next.TweenList = TweenList;
            TweenList.end = next;
            next.Pause();
            next.last = this;
            return next;
        }
        public QTweenBase next;
        public QTweenBase last;
        public QTweenList TweenList { private set; get; }
        public abstract QTweenBase Init();
        public QTweenBase Play(bool playForwads=true)
        {
            this.playForwads = playForwads;
            if (!IsPlaying)
            {
                _isPlaying = true;
                QTween.Instance.TweenUpdate += Update;
            }
            pause = false;
            return this;

        }
        void UpdateTime()
        {
            if (!IsPlaying) return;
            time += (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) * (playForwads ?1:-1);
        }
        public abstract void UpdateValue();
        public abstract void Destory();
        public void Update()
        {
            if (IsPlaying && !pause)
            {
                UpdateTime();
                CheckOver();
                UpdateValue();
            }
        }
        public virtual void CheckOver()
        {
            if (lastTime != time)
            {
          
                if (playForwads)
                {
                    if (lastTime <= Duration && time >= Duration)
                    {
                        Complete();
                    }
                }
                else
                {
                    if (lastTime >= 0 && time <= 0)
                    {
                        Complete();
                    }
                }
            }
            lastTime = time;
        }
    
       
        public void Complete()
        {
            time = playForwads ?Duration:0;
            (playForwads ? next: last  )?.Play(playForwads);
            Stop();
            onComplete?.Invoke();
            if (autoDestory)
            {
                Destory();
            }
           
        }
        public event Action onComplete;

        public QTweenBase OnComplete(Action action)
        {
            onComplete += action;
            return this;
        }
        public virtual QTweenBase Pause()
        {
            pause = true;
            return this;
        }
        public virtual QTweenBase Stop()
        {
            QTween.Instance.TweenUpdate-=Update;
            _isPlaying = false;
            return this;
        }
      
        public QTweenBase IgnoreTimeScale(bool value=true)
        {
            ignoreTimeScale = value;
            return this;
        }
        public virtual QTweenBase AutoDestory(bool destory=true)
        {
            autoDestory = destory;
            return this;
        }
      

        public void OnPoolReset()
        {
            time = 0;
        }

        public void OnPoolRecover()
        {
            onComplete = null;
            _isPlaying = false;
            next = null;
            last = null;
            TweenList?.Recover();
        }
    }

    //public class QTweenQueue : QTween
    //{
    //    static ObjectPool<QTweenQueue> _pool;
    //    public static ObjectPool<QTweenQueue> Pool
    //    {
    //        get
    //        {
    //            return _pool ?? (_pool = PoolManager.GetPool(typeof(QTweenQueue).Name, () =>
    //            {
    //                return new QTweenQueue();
    //            }));
    //        }
    //    }
    //    public static QTweenQueue GetQueue()
    //    {
    //        return Pool.Get();
    //    }
    //    private QTweenQueue()
    //    {

    //    }
        
    //    public List<QTween> tweenList = new List<QTween>();
    // //   public List<QTween> insertList = new List<QTween>();
    //    public override QTween Play(bool back = false)
    //    {
    //        foreach (var tween in tweenList)
    //        {
    //            tween.playBack = back;
    //        }
    //        return base.Play(back);
    //    }
    //    public override QTween Init()
    //    {
    //        return this;
    //    }
    //    public override QTween AutoDestory(bool kill = true)
    //    {
    //        foreach (var tween in tweenList)
    //        {
    //            tween.AutoDestory(kill);
    //        }
    //        return base.AutoDestory(kill);
    //    }
    //    public QTweenQueue PushEnd(QTween tween)
    //    {
    //        tweenList.Add(tween.Stop());
    //        Duration += tween.Duration;
    //        return this;
    //    }
      
    //    public override void UpdateValue()
    //    {
    //    }

    //    public override void Destory()
    //    {
    //        tweenList.Clear();
    //        Pool.Push(this);
    //    }
    //    //public QueueTween Insert(float insertTime,QTween tween)
    //    //{
    //    //    tweenList.Add(tween.Stop());
    //    //    tween.SetStartTime(insertTime);
    //    //    return this;
    //    //}

    //}
    internal class QTweenDelay : QTweenBase
    {
        static ObjectPool<QTweenDelay> _pool;
        public static ObjectPool<QTweenDelay> Pool
        {
            get
            {
                return _pool ?? (_pool = PoolManager.GetPool(typeof(QTweenDelay).Name, () =>
                {
                    return new QTweenDelay();
                }));
            }
        }
        public static QTweenDelay Get(float t)
        {
            var tween= Pool.Get();
            tween.Duration = t;
            return tween;
        }

        public override void Destory()
        {
            Pool.Push(this);
        }

        public override QTweenBase Init()
        {
            time = 0;
            return this;
        }

        public override void UpdateValue()
        {
        }
    }

    internal class QTween<T>:QTweenBase
    {
        static ObjectPool<QTween<T>> _pool;
        public static ObjectPool<QTween<T>> Pool
        {
            get
            {
                return _pool ?? (_pool = PoolManager.GetPool(typeof(QTween<T>).Name, () =>
                {
                    return new QTween<T>();
                }));
            }
        }
        public static QTweenBase GetTween(Func<T> Get, Action<T> Set, Func<T, T, float, T> tweenCurve, T end, float duration)
        {
            var tween = Pool.Get();
            tween.Set = Set;
            tween.Get = Get;
            tween.End = end;
            tween.ValueCurve = tweenCurve;
            tween.Duration = duration;
            return tween;
        }
        private QTween()
        {

        }
        public T Start { private set; get; }
        public T End { private set; get; }

        public Func<T, T, float, T> ValueCurve { private set; get; }
        public Func<T> Get { private set; get; }
        public Action<T> Set { private set; get; }
        public override QTweenBase Init()
        {
         
            time = 0;
            Start = Get();
            return this;
        }
        public override void UpdateValue()
        {
            if (time >=0 && time <= Duration)
            {
                Set(ValueCurve(Start, End, TCurve.Invoke((time - 0) / Duration)));
            }
        }
        public override void Destory()
        {
            Pool.Push(this);
        }
    }
    public static class TransformExtends
    {

        static Vector3 GetPosition(this Transform transform)
        {
            return transform.position;
        }
 
        static void SetPosition(this Transform transform,Vector3 postion)
        {
            transform.position=postion;
        }
        public static QTweenBase QRotate(this Transform transform, Vector3 value, float duration)
        {
            return QTween.Tween(()=>transform.rotation.eulerAngles,
            (setValue)=> { transform.rotation =Quaternion.Euler( setValue); },
            QTween.Lerp, value, duration);
        }
        public static QTweenBase QLocalRotate(this Transform transform, Vector3 value, float duration)
        {
            return QTween.Tween(() => transform.localRotation.eulerAngles,
            (setValue) => { transform.localRotation = Quaternion.Euler(setValue); },
            QTween.Lerp, value, duration);
        }
        public static QTweenBase QMove(this Transform transform, Vector3 postion,float duration)
        {
            return  QTween.Tween(transform.GetPosition,
            transform.SetPosition,
            QTween.Lerp, postion, duration);
        }
        public static QTweenBase QMoveX(this Transform transform, float value, float duration)
        {
            return QTween.Tween(()=>transform.position.x,
            (setValue)=> { transform.position = new Vector3(setValue, transform.position.y, transform.position.z); },
            QTween.Lerp, value, duration);
        }

        public static QTweenBase QMoveY(this Transform transform, float value, float duration)
        {
            return QTween.Tween(() => transform.position.y,
            (setValue) => { transform.position = new Vector3( transform.position.x, setValue, transform.position.z); },
            QTween.Lerp, value, duration);
        }
        public static QTweenBase QMoveZ(this Transform transform, float value, float duration)
        {
            return QTween.Tween(() => transform.position.z,
            (setValue) => { transform.position = new Vector3(transform.position.x, transform.position.y, setValue); },
            QTween.Lerp, value, duration);
        }
        public static QTweenBase QLocalMoveX(this Transform transform, float value, float duration)
        {
            return QTween.Tween(() => transform.localPosition.x,
            (setValue) => { transform.localPosition = new Vector3(setValue, transform.localPosition.y, transform.localPosition.z); },
            QTween.Lerp, value, duration);
        }

        public static QTweenBase QLocalMoveY(this Transform transform, float value, float duration)
        {
            return QTween.Tween(() => transform.localPosition.y,
            (setValue) => { transform.localPosition = new Vector3(transform.localPosition.x, setValue, transform.localPosition.z); },
            QTween.Lerp, value, duration);
        }
        public static QTweenBase QLocalMoveZ(this Transform transform, float value, float duration)
        {
            return QTween.Tween(() => transform.localPosition.z,
            (setValue) => { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, setValue); },
            QTween.Lerp, value, duration);
        }

        public static QTweenBase QLocalMove(this Transform transform, Vector3 postion, float duration)
        {
            return QTween.Tween(() => transform.localPosition,
            (pos) => { transform.localPosition = pos; },
            QTween.Lerp, postion, duration);
        }
        public static QTweenBase QScale(this Transform transform, Vector3 endScale, float duration)
        {
            return QTween.Tween(() => transform.localScale,
            (scale) => { transform.localScale = scale; },
            QTween.Lerp, endScale, duration);
        }
    }
    public static class RectTransformExtends
    {
        public static QTweenBase QAnchorPosition(this RectTransform transform, Vector2 postion, float duration)
        {
            return QTween.Tween(() => transform.anchoredPosition,
            (pos) => { transform.anchoredPosition = pos; },
            QTween.Lerp, postion, duration);
        }
    }
    public static class MaskableGraphicExtends
    {

        public static QTweenBase QFillAmount(this Image graphic,float value, float duration)
        {
            return QTween.Tween(() => graphic.fillAmount,
            (setValue) => { graphic.fillAmount = setValue; },
            QTween.Lerp, value, duration);
        }
        public static QTweenBase QColor(this MaskableGraphic graphic, Color endColor, float duration)
        {
            return QTween.Tween(() => graphic.color,
            (color) => { graphic.color = color;},
            QTween.Lerp, endColor, duration);
        }

        public static QTweenBase QAlpha(this MaskableGraphic graphic, float endAlpha, float duration)
        {
            return QTween.Tween(() => graphic.color.a,
            (alpha) => { graphic.color =new Color(graphic.color.r,graphic.color.g,graphic.color.b,alpha); },
            QTween.Lerp, endAlpha, duration);
        }
    }
}

