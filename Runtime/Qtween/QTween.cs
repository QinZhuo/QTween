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
        public float time = -1f;
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
           // next._Stop();
            next.last = this;
            return next;
        }
        public QTween next;
        public QTween last;
        public QTweenList TweenList { private set; get; }
      
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
                CurTween._Pause();
            }
            this.playForwads = playForwads;
            StartTween._Play(playForwads);
            return this;
        }
        private QTween _Play(bool playForwads )
        {
            this.playForwads = playForwads;
            if (time < 0)
            {
                time = playForwads ? 0 : Duration;
            }
            
            if (TweenList != null)
            {
                TweenList.current = this;
            }
            Start();
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
            return this;

        }
        protected virtual void Start()
        {

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
            if (IsPlaying )
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
    
       
        public virtual void Complete()
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
            if (Application.isPlaying)
            {
                QTweenManager.Manager.TweenUpdate -= Update;
            }
            _isPlaying = false;
            return this;
        }
        public QTween Stop()
        {
            return CurTween._Stop();
        }
        protected virtual QTween _Stop()
        {

            _Pause();
            if (EndTween == this)
            {
                time = -1;
            }
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
            time = -1;
        }

        public virtual void OnPoolRecover()
        {
            onComplete = null;
            _isPlaying = false;
            next = null;
            last = null;
            TweenList?.Recover();
            TweenList = null;
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
        public override void UpdateValue()
        {
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
            var curVallue = Get();
            base.Start();
            if (playForwads)
            {
                runtimeStart = curVallue;
            }
            else
            {
                runtimeEnd = curVallue;
            }
        }
        protected override QTween _Stop()
        {
            if (playForwads)
            {
                runtimeStart = StartValue;
            }
            else
            {
                runtimeEnd = EndValue;
            }
            return base._Stop();
        }
        public override void Complete()
        {
            //if (playForwads)
            //{
            //    runtimeStart = StartValue;
            //}
            //else
            //{
            //    runtimeEnd = EndValue;
            //}
            base.Complete();
        }
        public T StartValue { private set; get; }
        public QTween<T> ResetStart(T start)
        {
            StartValue=start;
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
                Set(ValueLerp(runtimeStart, runtimeEnd, TCurve.Invoke((time - 0) / Duration)));
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
        public static QTween<Quaternion> QRotate(this Transform transform, Vector3 value, float duration)
        {
            return QTweenManager.Tween(()=>transform.rotation,
            (setValue)=> { transform.rotation =setValue; },
            Quaternion.Euler( value), duration);
        }
        public static QTween<Vector3> QLocalRotate(this Transform transform, Vector3 value, float duration)
        {
            return QTweenManager.Tween(() => transform.localRotation.eulerAngles,
            (setValue) => { transform.localRotation = Quaternion.Euler(setValue); },
             value, duration);
        }
        public static QTween<Vector3> QMove(this Transform transform, Vector3 postion,float duration)
        {
            return  QTweenManager.Tween(transform.GetPosition,
            transform.SetPosition,
            postion, duration);
        }
        public static QTween<float> QMoveX(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(()=>transform.position.x,
            (setValue)=> { transform.position = new Vector3(setValue, transform.position.y, transform.position.z); },
            value, duration);
        }

        public static QTween<float> QMoveY(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.position.y,
            (setValue) => { transform.position = new Vector3( transform.position.x, setValue, transform.position.z); },
            value, duration);
        }
        public static QTween<float> QMoveZ(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.position.z,
            (setValue) => { transform.position = new Vector3(transform.position.x, transform.position.y, setValue); },
            value, duration);
        }
        public static QTween<float> QLocalMoveX(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.localPosition.x,
            (setValue) => { transform.localPosition = new Vector3(setValue, transform.localPosition.y, transform.localPosition.z); },
           value, duration);
        }

        public static QTween<float> QLocalMoveY(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.localPosition.y,
            (setValue) => { transform.localPosition = new Vector3(transform.localPosition.x, setValue, transform.localPosition.z); },
            value, duration);
        }
        public static QTween<float> QLocalMoveZ(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.localPosition.z,
            (setValue) => { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, setValue); },
            value, duration);
        }

        public static QTween<Vector3> QLocalMove(this Transform transform, Vector3 postion, float duration)
        {
            return QTweenManager.Tween(() => transform.localPosition,
            (pos) => { transform.localPosition = pos; },
             postion, duration);
        }
        public static QTween<Vector3> QScale(this Transform transform, Vector3 endScale, float duration)
        {
            return QTweenManager.Tween(() => transform.localScale,
            (scale) => { transform.localScale = scale; },
            endScale, duration);
        }
        public static QTween<Vector3> QScale(this Transform transform, float endScale, float duration)
        {
            return QScale(transform, Vector3.one * endScale, duration);
        }
    }
    public static class RectTransformExtends
    {
        public static QTween<Vector2> QAnchorPosition(this RectTransform transform, Vector2 postion, float duration)
        {
            return QTweenManager.Tween(() => transform.anchoredPosition,
            (pos) => { transform.anchoredPosition = pos; },
            postion, duration);
        }
    }
    public static class MaskableGraphicExtends
    {

        public static QTween<float> QFillAmount(this Image graphic,float value, float duration)
        {
            return QTweenManager.Tween(() => graphic.fillAmount,
            (setValue) => { graphic.fillAmount = setValue; },
            value, duration);
        }
        public static QTween<Color> QColor(this MaskableGraphic graphic, Color endColor, float duration)
        {
            return QTweenManager.Tween(() => graphic.color,
            (color) => { graphic.color = color;},
            endColor, duration);
        }

        public static QTween<float> QAlpha(this MaskableGraphic graphic, float endAlpha, float duration)
        {
            return QTweenManager.Tween(() => graphic.color.a,
            (alpha) => { graphic.color =new Color(graphic.color.r,graphic.color.g,graphic.color.b,alpha); },
            endAlpha, duration);
        }
    }
}

