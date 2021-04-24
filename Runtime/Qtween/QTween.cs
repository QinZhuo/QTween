using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTool.Tween
{
    public abstract class QTween:IPoolObject
    {
        public float Duration { protected set; get; }
        public bool PlayForwads
        {
            get;
            protected set;
        }
        public event Action OnCompleteEvent;
        public event Action OnStopEvent;
        public event Action OnStartEvent;
        public event Action<float> OnUpdateEvent;
        public QTween Stop()
        {
            IsPlaying = false;
            OnStopEvent?.Invoke();
            return this;
        }
        public Func<float, float> TCurve = Curve.Quad.Out();
        public QTween SetCurve(EaseCurve ease)
        {
            TCurve = Curve.GetEaseFunc(ease);
            return this;
        }
        public QTween SetCurve(Func<float, float> curveFunction)
        {
            TCurve = curveFunction;
            return this;
        }
        public float time = -1f;
        public bool AutoDestory { set; get; } = true;
        private bool _isPlaying = false;
        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
            protected set
            {
                if (value != _isPlaying)
                {
                    _isPlaying = value;
                    if (_isPlaying)
                    {
                        QTweenManager.Manager.TweenUpdate += Update;
                    }
                    else
                    {
                        QTweenManager.Manager.TweenUpdate -= Update;
                    }

                }
            }
        }
        public bool IsStop
        {
            get
            {
                return !IsPlaying && time < 0;
            }
        }
        public bool IgnoreTimeScale {  set; get; } = true;



        public virtual QTween Play(bool playForwads=true)
        {
            this.PlayForwads = playForwads;
            Start();
            if (time < 0)
            {
                time = playForwads ? 0 : Duration;
            }
            if (!IsPlaying)
            {
                if (Application.isPlaying)
                {
                    IsPlaying = true;
                }
                else
                {
                    Complete();
                }

            }
            return this;

        }
        protected virtual void Start()
        {
            OnStartEvent?.Invoke();
        }
        bool UpdateTime()
        {
            if (!IsPlaying) return false;
            time += (IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) * (PlayForwads ? 1 : -1);
            time = Mathf.Clamp(time, 0, Duration);
            CheckOver();
            return IsPlaying;
        }
        public abstract void UpdateValue();
        public abstract void Destory();
        public void Update()
        {
            if (IsPlaying)
            {
                if (UpdateTime())
                {
                    OnUpdateEvent?.Invoke(time);
                    UpdateValue();
                }
            }
        }
        public virtual void CheckOver()
        {
            if (PlayForwads)
            {
                if (time >= Duration)
                {
                    Complete();
                }
            }
            else
            {
                if (time <= 0)
                {
                    Complete();
                }
            }
        }


        public virtual void Complete()
        {
            time = PlayForwads ? Duration : 0;
            UpdateValue();
            OnCompleteEvent?.Invoke();
            Stop();
            if (AutoDestory)
            {
                Destory();
            }
        }

       

        public virtual void OnPoolReset()
        {
            time = -1;
        }

        public virtual void OnPoolRecover()
        {
            OnCompleteEvent = null;
            OnUpdateEvent = null;
            OnStopEvent = null;
            OnStartEvent = null;
            IsPlaying = false;
        }
    }

    internal class QTweenDelay : QTween
    {
        static ObjectPool<QTweenDelay> _pool;
        static ObjectPool<QTweenDelay> Pool
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
            var tween = Pool.Get();
            tween.Duration = t;
            return tween;
        }

        public override void Destory()
        {
            Pool.Push(this);
        }
        public override void UpdateValue()
        {
        }
        public override string ToString()
        {
            return "延迟 " + Duration + " 秒";
        }
    }
    public class QTween<T> : QTween
    {
        static ObjectPool<QTween<T>> _pool;
        public static ObjectPool<QTween<T>> Pool
        {
            get
            {
                return _pool ?? (_pool = PoolManager.GetPool("[" + typeof(T).Name + "]QTween动画", () =>
                {
                    return new QTween<T>();
                }));
            }
        }
        public static QTween<T> GetTween(Func<T> Get, Action<T> Set, T end, float duration)
        {
            if (Pool == null)
            {
                Debug.LogError("不存在对象池" + typeof(T));
            }
            var tween = Pool.Get();
            tween.Set = Set;
            tween.Get = Get;
            tween.EndValue = end;
            tween.runtimeEnd = end;
            tween.Duration = duration;
            tween.ResetStart(tween.Get());
            return tween;
        }
        protected QTween()
        {

        }
        protected override void Start()
        {
            base.Start();
            if (Application.isPlaying && !IsPlaying)
            {
                var curVallue = Get();
                if (PlayForwads)
                {
                    runtimeStart = curVallue;
                    runtimeEnd = EndValue;
                }
                else
                {
                    runtimeEnd = curVallue;
                    runtimeStart = StartValue;
                }
            }

        }
        public T StartValue { private set; get; }
        public QTween<T> ResetStart(T start)
        {
            StartValue = start;
            runtimeStart = start;
            return this;
        }
        T runtimeStart;
        T runtimeEnd;
        public T EndValue { private set; get; }
        public static Func<T, T, float, T> ValueLerp;
        public Func<T> Get { private set; get; }
        public Action<T> Set { private set; get; }
        public override void UpdateValue()
        {
            if (time >= 0 && time <= Duration)
            {
                try
                {
                    Set(ValueLerp(runtimeStart, runtimeEnd, TCurve.Invoke((time - 0) / Duration)));
                }
                catch (Exception e)
                {
                    Debug.LogWarning("【QTween】更新数值出错：" + e);
                }

            }
        }

        public override void Destory()
        {
            Pool.Push(this);
        }
        public override string ToString()
        {
            return "" + typeof(T).Name + " " + StartValue + " to " + EndValue + "";
        }
    }
}