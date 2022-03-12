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
                return _pool ?? (_pool = QPoolManager.GetPool(typeof(QTweenList).Name, () =>
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
            public Action Start;
            public Action Compelete;
            public TweenListNode(QTween tween,TweenListType type= TweenListType.同时播放)
            {
                this.tween = tween;
                this.type = type;
            }
        }
        public List<TweenListNode> tweenList = new List<TweenListNode>();

        public QTweenList AddLast(QTween tween, TweenListType listType= TweenListType.顺序播放)
        {
            tweenList.Add(new TweenListNode( tween, listType));
            initOver = false;
            return this;
        }
        public override QTween Play(bool playForwads = true,float timeScale=1)
        {

            this.PlayForwads = playForwads;
            this.TimeScale = timeScale;
            if (tweenList.Count > 0)
            {
                InitTween();
                if (curNode == null)
                {
                    curNode = playForwads ? tweenList[0] : tweenList[tweenList.Count - 1];
                }
                InitTime();
            }
            curNode?.tween?.Play(playForwads);
            return base.Play(playForwads);
        }
        bool initOver = false;
        public TweenListNode curNode;
        public void InitTime()
        {
            Duration = 0;
            for (int i = 0; i < tweenList.Count; i++)
            {
                var last = (i - 1) >= 0 ? tweenList[i - 1] : null;
                var tweenNode = tweenList[i];
                if(tweenNode.tween is QTweenList)
                {
                    (tweenNode.tween as QTweenList).InitTime();
                }
                if (tweenNode.type == TweenListType.顺序播放)
                {
                    Duration += tweenNode.tween.Duration;
                }
                else if (tweenNode.type == TweenListType.同时播放)
                {
                    if (last != null)
                    {
                        Duration -= last.tween.Duration;
                    }
                    Duration += tweenNode.tween.Duration;
                }
            }
        }
        public void InitTween()
        {
            if (initOver) return;
            initOver = true;
            
            for (int i = 0; i < tweenList.Count; i++)
            {
                var last = (i - 1) >= 0? tweenList[i-1] : null;
                var next= (i + 1) < tweenList.Count ? tweenList[i + 1] : null;
                var tweenNode = tweenList[i];
               
                tweenNode.tween.OnStartEvent -= tweenNode.Start;
                tweenNode.tween.OnCompleteEvent -= tweenNode.Compelete; 
                tweenNode.Start = () =>
                {
                    curNode = tweenNode;
                    TimeScale = curNode.tween.TimeScale;
                    SyncPlay(PlayForwads ? next : last);
                };
                tweenNode.Compelete = () =>
                {
                    NextPlay(PlayForwads ? next : last);
                };
                tweenNode.tween?.OnStart(tweenNode.Start).OnComplete(tweenNode.Compelete);
            }
            InitTime();
        }
        public void SyncPlay(TweenListNode next)
        {
            if (next != null && next.type == TweenListType.同时播放)
            {
                next.tween?.Play(PlayForwads);
            }
        }
        public void NextPlay(TweenListNode next)
        {
            if (next != null && next.type== TweenListType.顺序播放)
            {
                next.tween?.Play(PlayForwads);
            }
        }
        public override void OnPoolRecover()
        {
            base.OnPoolRecover();
            curNode = null;
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

