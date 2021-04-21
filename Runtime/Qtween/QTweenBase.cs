using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QTool;

namespace QTool.Tween
{
  
    public abstract class QTweenBase:IPoolObject
    {
        public class QTweenList : PoolObject<QTweenList>
        {
            public QTweenBase first;
            public QTweenBase end;
            public QTweenBase current;
            public override void OnPoolRecover()
            {
                first = null;
                end = null;
            }

            public override void OnPoolReset()
            {
            }
        }
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
        public bool ignoreTimeScale = true;
        public QTweenBase Next(QTweenBase next)
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
            next._Pause();
            next.last = this;
            return next;
        }
        public QTweenBase next;
        public QTweenBase last;
        public QTweenList TweenList { private set; get; }
        public abstract QTweenBase Init();
        public QTweenBase Play(bool playForwads=true)
        {
            var cur = this;
            if (TweenList != null)
            {
                cur = playForwads ? TweenList.first : TweenList.end;
            }
            cur._Play(playForwads);
            return this;
        }
        private QTweenBase _Play(bool playForwads )
        {
            this.playForwads = playForwads;
            if (TweenList != null)
            {
                TweenList.current = this;
            }
            if (!IsPlaying)
            {
                _isPlaying = true;
                QTween.Manager.TweenUpdate += Update;
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
                onUpdate?.Invoke(time);
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
            (playForwads ? next: last  )?._Play(playForwads);
            _Stop();
            onComplete?.Invoke();
            if (autoDestory)
            {
                Destory();
            }
           
        }
        public event Action onComplete;
        public event Action<float> onUpdate;
        public QTweenBase OnComplete(Action action)
        {
            onComplete += action;
            return this;
        }
        public QTweenBase OnComplete(Action<float> action)
        {
            onUpdate += action;
            return this;
        }
        public QTweenBase CurTween
        {
            get
            {

                return TweenList == null ? this : TweenList.current;
            }
        }
        public QTweenBase Pause()
        {
            return CurTween._Pause();
        }
        private QTweenBase _Pause()
        {
            pause = true;
            return this;
        }
        public QTweenBase Stop()
        {
            return CurTween._Stop();
        }
        private QTweenBase _Stop()
        {
            QTween.Manager.TweenUpdate-=Update;
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
                return _pool ?? (_pool = PoolManager.GetPool("["+typeof(T).Name+"]QTween动画", () =>
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
        public static QTweenBase GetTween(Func<T> Get, Action<T> Set, Func<T, T, float, T> tweenCurve,T start, T end, float duration)
        {
            var tween=GetTween(Get, Set, tweenCurve, end, duration);
            tween.Start = start;
            tween.initStart = false;
            return tween;
        }
        private QTween()
        {

        }
        public T Start { private set; get; }
        public T End { private set; get; }
        bool initStart = true;
        public Func<T, T, float, T> ValueCurve { private set; get; }
        public Func<T> Get { private set; get; }
        public Action<T> Set { private set; get; }
        public override QTweenBase Init()
        {
            if (initStart)
            {
                Start = Get();
            }
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
        public override void OnPoolReset()
        {
            base.OnPoolReset();
            initStart = true;
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
            return QTween.Tween(()=>transform.rotation,
            (setValue)=> { transform.rotation =setValue; },
            QTween.Lerp,Quaternion.Euler( value), duration);
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

