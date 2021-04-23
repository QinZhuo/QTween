using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QTool;
using UnityEngine.Serialization;

namespace QTool.Tween
{
    public abstract class QTweenString : QTweenBehavior<string>
    {
        protected override QTween ShowTween()
        {

            return QTweenManager.Tween(() => CurValue, (value) => CurValue = value, EndValue, animTime).ResetStart(StartValue);
        }
    }
    public abstract class QTweenVector2 : QTweenBehavior<Vector2>
    {
        protected override QTween ShowTween()
        {
            return QTweenManager.Tween(() => CurValue, (value) => CurValue = value, EndValue, animTime).ResetStart(StartValue);
        }
    }
    public abstract class QTweenVector3 : QTweenBehavior<Vector3>
    {
        protected override QTween ShowTween()
        {
            return QTweenManager.Tween(() => CurValue, (value) => CurValue = value, EndValue, animTime).ResetStart(StartValue);
        }
    }
    public abstract class QTweenFloat : QTweenBehavior<float>
    {
        protected override QTween ShowTween()
        {
            return QTweenManager.Tween(() => CurValue, (value) => CurValue = value, EndValue, animTime).ResetStart(StartValue);
        }
    }
    public abstract class QTweenColor : QTweenBehavior<Color>
    {
        protected override QTween ShowTween()
        {
            return QTweenManager.Tween(() => CurValue, (value) => CurValue = value, EndValue, animTime).ResetStart(StartValue);
        }
        public override string ToString()
        {
            return ToViewString(StartValue) + " => " + ToViewString(EndValue);
        }
        public static string ToViewString(Color color)
        {
            return ColorUtility.ToHtmlStringRGBA(color);
        }
    }
    public abstract class QTweenQuaternion : QTweenBehavior<Quaternion>
    {
        protected override QTween ShowTween()
        {
            return QTweenManager.Tween(() => CurValue, (value) => CurValue = value, EndValue, animTime).ResetStart(StartValue);
        }
    }
    public abstract class QTweenBehavior<T> : QTweenBehavior
    {
        public EaseCurve curve = EaseCurve.OutQuad;
        [FormerlySerializedAs("animTime")]
        public float animTime = 0.4f;
        public float hideTime = 0.4f;
        [FormerlySerializedAs("HideValue")]
        public T StartValue;
        [FormerlySerializedAs("ShowValue")]
        public T EndValue;
        protected virtual void Reset()
        {
            EndValue = CurValue;
            StartValue = CurValue;
        }
        public override string ToString()
        {
            return StartValue + " => " + EndValue;
        }
        public override void ReverseStartEnd()
        {
            var temp = EndValue;
            EndValue = StartValue;
            StartValue = temp;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }
        RectTransform _rect;
        public RectTransform RectTransform
        {
            get
            {
                return _rect ?? (gameObject.GetComponent<RectTransform>());
            }
        }
        public abstract T CurValue { get; set; }
        public virtual QTween ChangeFunc(T value, float time)
        {
            CurValue = value;
            return null;
        }
        protected override QTween TweenInit(QTween tween)
        {
            return base.TweenInit(tween).SetCurve(curve);
        }

    }
    public abstract class QTweenBehavior : MonoBehaviour
    {

        public ActionEvent OnShow;
        public ActionEvent OnHide;
        protected abstract QTween ShowTween();
        QTween _anim;
        public QTween Anim
        {
            get
            {
                if (_anim == null)
                {
                    _anim = TweenInit(ShowTween());
                }
                return _anim;
            }
        }
        [ContextMenu("动画起止反向")]
        public void Reverse()
        {
            ReverseStartEnd();
            ClearAnim();
        }
        public virtual void ReverseStartEnd()
        {

        }
        [ContextMenu("清除动画缓存")]
        public virtual void ClearAnim()
        {
            _anim = null;
        }
        protected virtual void OnValidate()
        {
            ClearAnim();
        }
        protected virtual QTween TweenInit(QTween tween)
        {
            return tween.OnComplete(AnimOver).AutoDestory(false);
        }
        void AnimOver()
        {
            if (Anim.PlayForwads)
            {
                OnShow?.Invoke();
            }
            else
            {
                OnHide?.Invoke();
            }
        }
        public void Play(bool show)
        {
            Anim.Play(show);
        }
        [ContextMenu("隐藏")]
        public void Hide()
        {
            Play(false);
        }
        [ContextMenu("显示")]
        public void Show()
        {
            Play(true);
        }
    }
    public class QTweenList :QTween
    {
        static ObjectPool<QTweenList> _pool;
        static ObjectPool<QTweenList> Pool
        {
            get
            {
                return _pool ?? (_pool = PoolManager.GetPool(typeof(QTweenList).Name, () =>
                {
                    return new QTweenList();
                }));
            }
        }
        public static QTweenList Get()
        {
            var tween = Pool.Get();
            return tween;
        }
        public enum TweenListType
        {
            同时播放 = 1,
            顺序播放 = 2,
        }
        public class TweenListNode 
        {
            public TweenListType type = TweenListType.同时播放;
            public QTween tween;
            public TweenListNode(QTween tween,TweenListType type= TweenListType.同时播放)
            {
                this.tween = tween;
                this.type = type;
            }
        }
        public LinkedList<TweenListNode> tweenList = new LinkedList<TweenListNode>();

        LinkedListNode<TweenListNode> _curNode;
        LinkedListNode<TweenListNode> CurNode
        {
            get
            {
                if (_curNode == null)
                {
                    return PlayForwads ? tweenList.First : tweenList.Last;
                }
                return _curNode;
            }
            set
            {
                _curNode = value;
            }
        }
        LinkedListNode<TweenListNode> NextNode
        {
            get
            {
                if (CurNode == null) return null;
                return PlayForwads ? CurNode.Next : CurNode.Previous;
            }
        }
        public void Next()
        {
            CurNode = NextNode;
        }
        public QTweenList AddLast(QTween tween, TweenListType listType= TweenListType.顺序播放)
        {
            tweenList.AddLast(new TweenListNode( tween, listType));
            Duration += tween.Duration;
            return this;
        }
        public override QTween Play(bool playForwads = true)
        {
            var tween= base.Play(playForwads);
            if (tweenList.Count > 0)
            {
                InitTween();
            }
            return tween;
        }
        public void InitTween()
        {
            CurNode.Value.tween?.OnStart(SyncPlay).OnComplete(NextPlay).Play(PlayForwads);
        }
        public void SyncPlay()
        {
            var tween = CurNode.Value.tween;
            tween.OnStartEvent -= SyncPlay;
            while (NextNode != null && NextNode.Value.type == TweenListType.同时播放)
            {
                NextNode.Value.tween?.Play(PlayForwads);
                Next();
            }
        }
        public void NextPlay()
        {
            if (NextNode != null )
            {
                if (NextNode.Value.type != TweenListType.顺序播放)
                {
                    throw new Exception("顺序播放出错"+this);
                }
                var tween = CurNode.Value.tween;
                tween.OnCompleteEvent -= NextPlay;
                Next();
                InitTween();
            }
        }
        public override void OnPoolRecover()
        {
            base.OnPoolRecover();
            tweenList.Clear();
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
            return "动画列表[" + tweenList.Count + "]"+Duration;
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
        public override string ToString()
        {
            return "延迟 "+Duration+" 秒";
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
            if (Application.isPlaying&& !IsPlaying)
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
        public override string ToString()
        {
            return ""+typeof(T).Name+" "+StartValue+" to "+EndValue+"";
        }
    }
  
}

