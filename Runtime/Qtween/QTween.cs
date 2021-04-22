using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QTool;
namespace QTool.Tween
{
   
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

        public LinkedList<QTween> tweenList = new LinkedList<QTween>();

        LinkedListNode<QTween> CurNode;
        public QTweenList AddLast(QTween tween)
        {
            
            tweenList.AddLast(tween);
            Duration += tween.Duration;
            return this;
        }
        public override QTween Play(bool playForwads = true)
        {

            if (CurNode == null)
            {
                CurNode = playForwads ? tweenList.First : tweenList.Last;
            }
            CurNode?.Value.OnComplete(Next).Play(playForwads);
            return base.Play(playForwads);
        }
        public void Next()
        {
            if (CurNode != null)
            {
                CurNode.Value.OnCompleteEvent -= Next;
                CurNode = PlayForwads ? CurNode.Next:CurNode.Previous;
                CurNode?.Value.OnComplete(Next).Play(PlayForwads);
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
            if (!IsPlaying)
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
    }
  
}

