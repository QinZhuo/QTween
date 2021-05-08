using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QTool;
using UnityEngine.Serialization;

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
            if(listType== TweenListType.顺序播放)
            {
                Duration += tween.Duration;
            }
            else if(listType== TweenListType.同时播放)
            {
                Duration = Mathf.Max(tween.Duration, Duration);
            }
          
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
                var  tempTween = NextNode.Value.tween;
                Next();
                tempTween?.Play(PlayForwads);
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
        public override void Complete()
        {
            foreach (var kv in tweenList)
            {
                kv.tween?.Complete();
            }
            base.Complete();
        }
        public override void UpdateValue()
        {
        }
        public override string ToString()
        {
            return "动画列表[" + tweenList.Count + "]"+Duration;
        }
    }
    
   

  
  
}

