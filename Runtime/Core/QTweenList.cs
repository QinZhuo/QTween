using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QTool;
using UnityEngine.Serialization;
#if UNITY_2021_1_OR_NEWER
using UnityEngine.Pool;
#endif
namespace QTool.Tween
{
   
    public class QTweenList :QTween
    {
		#region 对象池逻辑
		public static QTweenList Get()
		{
			var tween = QObjectPool<QTweenList>.Get();
			tween.Duration = 0;
			return tween;
		}
		#endregion
		#region 内置类型
		public enum TweenListType
		{
			顺序播放 = 0,
			异步播放 = 1,
		}
		public struct TweenListNode
		{
			public QTween tween;
			public TweenListType type;
			public TweenListNode(QTween tween, TweenListType type = TweenListType.顺序播放)
			{
				this.tween = tween;
				this.type = type;
			}
		}
		#endregion
		#region 基础属性

		private List<TweenListNode> List = new List<TweenListNode>();
		public int CurIndex { get; private set; } = -2;
		public TweenListNode CurNode => List.Get(CurIndex);
		#endregion
		public void Clear()
		{
			List.Clear();
		}
		public QTweenList AddLast(QTween tween, TweenListType type= TweenListType.顺序播放)
        {
			switch (type)
			{
				case TweenListType.异步播放:
					if (List.Count > 0)
					{
						if (List.StackPeek().type == TweenListType.异步播放)
						{
							Duration -= Mathf.Min(List.StackPeek().tween.Duration, tween.Duration);
						}
						Duration += tween.Duration;
					}
					else
					{
						Duration += tween.Duration;
					}
					break;
				default:
					Duration += tween.Duration;
					break;
			}
			List.Add(new TweenListNode(tween, type));
			return this;
		}
		public override void OnPoolRelease()
        {
            base.OnPoolRelease();
			CurIndex =-2;
			Clear();
        }
		protected override void OnStart()
		{
			if (CurIndex < -1|| CurIndex > List.Count)
			{
				CurIndex = PlayForwads ? -1 : List.Count;
			}
			base.OnStart();
		}
		public override void Release()
        {
			foreach (var node in List)
			{
				node.tween?.Release();
			}
			Clear();
			QObjectPool<QTweenList>.Release(this);
        }
        protected override void OnComplete()
		{
			while (!CurNode.IsNull())
			{
				if (CurNode.tween != null && !CurNode.tween.IsPlaying)
				{
					CurNode.tween.Play(PlayForwads);
					CurNode.tween.Complete();
				}
				Next();
			}
			foreach (var tween in List)
			{
				if (tween.tween.IsPlaying)
				{
					tween.tween.Complete();
				}
			}
			CurIndex = PlayForwads ? List.Count : -1;
			base.OnComplete();
        }

		public bool IsEnd
		{
			get => PlayForwads ? (CurIndex >= List.Count) : (CurIndex <=-1);
		}
		public TweenListNode NextNode => List.Get(CurIndex +( PlayForwads ? 1 : -1));
		public void Next()
		{
			if (PlayForwads)
			{
				CurIndex++;
			}
			else
			{
				CurIndex--;
			}
			CurIndex = Mathf.Clamp(CurIndex, -1, List.Count );
		}
        protected override void OnUpdate()
        {
			if (CurNode.tween != null && CurNode.tween.PlayForwads != PlayForwads)
			{
				CurNode.tween.Play(PlayForwads);
			}
			while (!IsEnd && (CurNode.type == TweenListType.异步播放 && NextNode.type == TweenListType.异步播放 || CurNode.tween == null || (PlayForwads==CurNode.tween.PlayForwads&&CurNode.tween.IsOver)))
			{
				Next();
				if (CurNode.tween != null)
				{
					CurNode.tween.Play(PlayForwads);
				}
			}
		}
        public override string ToString()
        {
			return nameof(QTweenList)+"[" + List.Count + "]" + nameof(Duration) + "[" + Duration + "]"+base.ToString();
        }
		public override QTween SetAutoDestory(UnityEngine.Object target = null)
		{
			foreach (var node in List)
			{
				node.tween.SetAutoDestory(target);
			}
			return base.SetAutoDestory(target);
		}
		public override QTween SetIgnoreTimeScale(bool value)
		{
			foreach (var node in List)
			{
				node.tween.SetIgnoreTimeScale(value);
			}
			return base.SetIgnoreTimeScale(value);
		}
		public override QTween SetTimeScale(float value)
		{
			foreach (var node in List)
			{
				node.tween.SetTimeScale(value);
			}
			return base.SetTimeScale(value);
		}
	}
    
   

  
  
}

