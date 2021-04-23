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
        public bool AutoDestory { set; get; } = false;
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
                    try
                    {
                        OnUpdateEvent?.Invoke(time);
                        UpdateValue();
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("��QTween��������ֵ����" + e);
                    }
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
        
}