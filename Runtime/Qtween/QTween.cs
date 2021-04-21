using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QTool;

namespace QTool.Tween
{
  
    public abstract class QTween:IPoolObject
    {
        public class QTweenList : PoolObject<QTweenList>
        {
            public QTween first;
            public QTween end;
            public QTween current;
            public override void OnPoolRecover()
            {
                first = null;
                end = null;
            }

            public override void OnPoolReset()
            {
            }
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
        public float time = 0f;
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
        public QTween Start<T>(T startValue)
        {
            if(this is QTween<T>)
            {
                var tween = (this as QTween<T>);
                tween.Start = startValue;
                return this;
            }
            else
            {
                throw new Exception(this + "错误的起始数值[" + startValue + "]");
            }
        }
        public bool pause = false;
        public bool ignoreTimeScale = true;
        public QTween Next(QTween next)
        {
            if (next == null) return this;
            if (TweenList == null)
            {
                TweenList = QTweenList.Get();
                TweenList.first = this;
                TweenList.current = this;
            }
            this.next = next;
            next.TweenList = TweenList;
            TweenList.end = next;
            next._Stop();
            next.last = this;
            return next;
        }
        public QTween next;
        public QTween last;
        public QTweenList TweenList { private set; get; }
        public abstract QTween Init();
        public QTween StartTween
        {
            get
            {
                return TweenList == null ? this : (playForwads ? TweenList.first : TweenList.end);
            }
        }
        public QTween EndTween
        {
            get
            {
                return TweenList == null ? this : (!playForwads ? TweenList.first : TweenList.end);
            }
        }
        public QTween Play(bool playForwads=true)
        {
            if (CurTween.IsPlaying)
            {
                CurTween._Stop();
               
            }
            this.playForwads = playForwads;
            StartTween._Play(playForwads);
            return this;
        }
        private QTween _Play(bool playForwads )
        {
            this.playForwads = playForwads;
            if (TweenList != null)
            {
                TweenList.current = this;
            }
            if (!_isPlaying)
            {

                _isPlaying = true;
                if (Application.isPlaying)
                {
                    QTweenManager.Manager.TweenUpdate += Update;
                }
                else
                {
                    Complete();
                }
              
            }
            pause = false;
            return this;

        }
        bool UpdateTime()
        {
            if (!_isPlaying) return false;
            time += (ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) * (playForwads ?1:-1);
            time = Mathf.Clamp(time, 0, Duration);
            CheckOver();
            return _isPlaying;
        }
        public abstract void UpdateValue();
        public abstract void Destory();
        public void Update()
        {
            if (IsPlaying && !pause)
            {
                if(UpdateTime())
                {
                    try
                    {
                        onUpdate?.Invoke(time);
                        UpdateValue();
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning("【QTween】更新数值出错：" + e);
                    }
                }
            }
        }
        public virtual void CheckOver()
        {
            if (playForwads)
            {
                if ( time >= Duration)
                {
                    Complete();
                }
            }
            else
            {
                if ( time <= 0)
                {
                    Complete();
                }
            }
        }
    
       
        public void Complete()
        {
            time = playForwads ? Duration : 0;
            UpdateValue();
            onComplete?.Invoke();
            _Stop();
            (playForwads ? next: last  )?._Play(playForwads);
            if (autoDestory)
            {
                Destory();
            }
           
        }
        public event Action onComplete;
        public event Action<float> onUpdate;
        public QTween OnComplete(Action action)
        {
            onComplete += action;
            return this;
        }
        public QTween OnComplete(Action<float> action)
        {
            onUpdate += action;
            return this;
        }
        public QTween CurTween
        {
            get
            {

                return TweenList == null ? this : TweenList.current;
            }
        }
        public QTween Pause()
        {
            return CurTween._Pause();
        }
        private QTween _Pause()
        {
            pause = true;
            return this;
        }
        public QTween Stop()
        {
            return CurTween._Stop();
        }
        private QTween _Stop()
        {
            if (Application.isPlaying)
            {
                QTweenManager.Manager.TweenUpdate -= Update;
            }
            _isPlaying = false;
            return this;
        }
      
        public QTween IgnoreTimeScale(bool value=true)
        {
            ignoreTimeScale = value;
            return this;
        }
        public virtual QTween AutoDestory(bool destory=true)
        {
            autoDestory = destory;
            return this;
        }
      

        public virtual void OnPoolReset()
        {
            time = 0;
        }

        public virtual void OnPoolRecover()
        {
            onComplete = null;
            _isPlaying = false;
            next = null;
            last = null;
            TweenList?.Recover();
        }
    }

  
    internal class QTweenDelay : QTween
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

        public override QTween Init()
        {
            time = 0;
            return this;
        }

        public override void UpdateValue()
        {
        }
    }

    internal class QTween<T> : QTween
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
        public static QTween<T> GetTween(Func<T> Get, Action<T> Set, Func<T, T, float, T> tweenCurve, T end, float duration)
        {
            if (Pool == null)
            {
                Debug.LogError("不存在对象池" + typeof(T));
            }
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
        public T Start {  set; get; }
        public T End { private set; get; }
        public Func<T, T, float, T> ValueCurve { private set; get; }
        public Func<T> Get { private set; get; }
        public Action<T> Set { private set; get; }
        public override QTween Init()
        {
            Start = Get();
            return this;
        }
        public override void UpdateValue()
        {
            if (time >= 0 && time <= Duration)
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
        public static QTween QRotate(this Transform transform, Vector3 value, float duration)
        {
            return QTweenManager.Tween(()=>transform.rotation,
            (setValue)=> { transform.rotation =setValue; },
            QTweenManager.Lerp,Quaternion.Euler( value), duration);
        }
        public static QTween QLocalRotate(this Transform transform, Vector3 value, float duration)
        {
            return QTweenManager.Tween(() => transform.localRotation.eulerAngles,
            (setValue) => { transform.localRotation = Quaternion.Euler(setValue); },
            QTweenManager.Lerp, value, duration);
        }
        public static QTween QMove(this Transform transform, Vector3 postion,float duration)
        {
            return  QTweenManager.Tween(transform.GetPosition,
            transform.SetPosition,
            QTweenManager.Lerp, postion, duration);
        }
        public static QTween QMoveX(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(()=>transform.position.x,
            (setValue)=> { transform.position = new Vector3(setValue, transform.position.y, transform.position.z); },
            QTweenManager.Lerp, value, duration);
        }

        public static QTween QMoveY(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.position.y,
            (setValue) => { transform.position = new Vector3( transform.position.x, setValue, transform.position.z); },
            QTweenManager.Lerp, value, duration);
        }
        public static QTween QMoveZ(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.position.z,
            (setValue) => { transform.position = new Vector3(transform.position.x, transform.position.y, setValue); },
            QTweenManager.Lerp, value, duration);
        }
        public static QTween QLocalMoveX(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.localPosition.x,
            (setValue) => { transform.localPosition = new Vector3(setValue, transform.localPosition.y, transform.localPosition.z); },
            QTweenManager.Lerp, value, duration);
        }

        public static QTween QLocalMoveY(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.localPosition.y,
            (setValue) => { transform.localPosition = new Vector3(transform.localPosition.x, setValue, transform.localPosition.z); },
            QTweenManager.Lerp, value, duration);
        }
        public static QTween QLocalMoveZ(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.localPosition.z,
            (setValue) => { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, setValue); },
            QTweenManager.Lerp, value, duration);
        }

        public static QTween QLocalMove(this Transform transform, Vector3 postion, float duration)
        {
            return QTweenManager.Tween(() => transform.localPosition,
            (pos) => { transform.localPosition = pos; },
            QTweenManager.Lerp, postion, duration);
        }
        public static QTween QScale(this Transform transform, Vector3 endScale, float duration)
        {
            return QTweenManager.Tween(() => transform.localScale,
            (scale) => { transform.localScale = scale; },
            QTweenManager.Lerp, endScale, duration);
        }
        public static QTween QScale(this Transform transform, float endScale, float duration)
        {
            return QScale(transform, Vector3.one * endScale, duration);
        }
    }
    public static class RectTransformExtends
    {
        public static QTween QAnchorPosition(this RectTransform transform, Vector2 postion, float duration)
        {
            return QTweenManager.Tween(() => transform.anchoredPosition,
            (pos) => { transform.anchoredPosition = pos; },
            QTweenManager.Lerp, postion, duration);
        }
    }
    public static class MaskableGraphicExtends
    {

        public static QTween QFillAmount(this Image graphic,float value, float duration)
        {
            return QTweenManager.Tween(() => graphic.fillAmount,
            (setValue) => { graphic.fillAmount = setValue; },
            QTweenManager.Lerp, value, duration);
        }
        public static QTween QColor(this MaskableGraphic graphic, Color endColor, float duration)
        {
            return QTweenManager.Tween(() => graphic.color,
            (color) => { graphic.color = color;},
            QTweenManager.Lerp, endColor, duration);
        }

        public static QTween QAlpha(this MaskableGraphic graphic, float endAlpha, float duration)
        {
            return QTweenManager.Tween(() => graphic.color.a,
            (alpha) => { graphic.color =new Color(graphic.color.r,graphic.color.g,graphic.color.b,alpha); },
            QTweenManager.Lerp, endAlpha, duration);
        }
    }
}

