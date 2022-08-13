using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace QTool.Tween
{
	#region 基础动画逻辑
	public abstract class QTween : IPoolObject
	{
		#region 基础属性
		private bool _isPlaying = false;
		public bool PlayForwads { get; private set; } = true;
		public  float Time { get; private set; } = -1f;
		public  float Duration { get; protected set; }
		public float TimeScale { get;private set; } = 1;
		public bool IgnoreTimeScale { set;private get; } = true;
		public bool AutoDestory { set;private get; } = true;
		public Func<float, float> TweenCurve { get; set; } = Curve.Quad.Out();
		#region 更改数值
		public QTween SetCurve(EaseCurve ease)
		{
			TweenCurve = Curve.GetEaseFunc(ease);
			return this;
		}
		public virtual QTween SetAutoDestory(bool value)
		{
			AutoDestory = value;
			return this;
		}
		public virtual QTween SetIgnoreTimeScale(bool value)
		{
			IgnoreTimeScale = value;
			return this;
		}
		public virtual QTween SetTimeScale(float value)
		{
			TimeScale = value;
			return this;
		}
		public  QTween OnStart(Action action)
		{
			OnStartEvent += action;
			return this;
		}
		public  QTween OnComplete( Action action)
		{
			OnCompleteEvent += action;
			return this;
		}
		#endregion
		public event Action OnStartEvent;
		public event Action OnCompleteEvent;
		public event Action OnUpdateEvent;
		public virtual void OnPoolRecover()
		{
			PlayForwads = true;
			Time = -1;
			TimeScale = 1;
			Duration = 0;
			IsPlaying = false;
			IgnoreTimeScale = true;
			TweenCurve = Curve.Quad.Out();
			AutoDestory = true;
			OnCompleteEvent = null;
			OnUpdateEvent = null;
			OnStartEvent = null;
		}

		public bool IsPlaying
		{
			get
			{
				return _isPlaying;
			}
			private set
			{
				if (value != _isPlaying)
				{
					_isPlaying = value;
					if (Application.isPlaying)
					{
						if (_isPlaying)
						{
							QTweenManager.Instance.TweenUpdate += Update;
						}
						else
						{
							QTweenManager.Instance.TweenUpdate -= Update;
						}
					}
					else
					{
#if UNITY_EDITOR
						if (_isPlaying)
						{
							UnityEditor.EditorApplication.update += Update;
						}
						else
						{
							UnityEditor.EditorApplication.update -= Update;
						}
#endif
					}

				}
			}
		}
		public bool IsOver
		{
			get
			{
				if (PlayForwads)
				{
					return Time >= Duration;
				}
				else
				{
					return Time <= 0;
				}
			}
		}
		#endregion
		#region 正常生命周期
		public QTween Play(bool PlayForwads = true)
		{
			if (!IsPlaying||this. PlayForwads!=PlayForwads)
			{
				this.PlayForwads = PlayForwads;
				IsPlaying = true;
				OnStart();
				OnStartEvent?.Invoke();
			}
			return this;
		}
		protected virtual void OnStart()
		{
			if (Time < 0 || Time > Duration)
			{
				Time = PlayForwads ? 0 : Duration;
			}
		}
		public async Task PlayAsync(bool PlayForwads = true)
		{
			var flag = Application.isPlaying;
			Play(PlayForwads);
			while (!IsOver&& flag==Application.isPlaying)
			{
				await Task.Yield();
			}
		}
		private void Update()
		{
			Time += (Application.isPlaying ? (IgnoreTimeScale ? UnityEngine.Time.unscaledDeltaTime : UnityEngine.Time.deltaTime) : Tool.EditorDeltaTime) * (PlayForwads ? 1 : -1) * TimeScale;
			Update(Time);
		}
		private void Update(float time)
		{
			Time = Mathf.Clamp(time, 0, Duration);
			OnUpdate();
			OnUpdateEvent?.Invoke();
			if (IsOver)
			{
				OnCompleteEvent?.Invoke();
				OnComplete();
			}
		}
		protected abstract void OnUpdate();
		protected virtual void OnComplete()
		{
			IsPlaying = false;
			Time = PlayForwads ? Duration : 0;
			if (AutoDestory)
			{
				Destory();
			}
		}
		public abstract void Destory();
		#endregion
		#region 更改生命周期
		public QTween Stop()
		{
			OnComplete();
			return this;
		}
		public void Complete()
		{
			if (!IsPlaying) return;
			Update(PlayForwads ? Duration : 0);
		}
		#endregion


	}
	#endregion
	#region 延迟
	internal sealed class QTweenDelay : QTween
	{
		static ObjectPool<QTweenDelay> _pool;
		static ObjectPool<QTweenDelay> Pool
		{
			get
			{
				return _pool ?? (_pool = QPoolManager.GetPool(typeof(QTweenDelay).Name, () =>
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

		protected override void OnUpdate()
		{
		}
		public override void Destory()
		{
			Pool.Push(this);
		}
	
		public override string ToString()
		{
			return "延迟 " + Duration + " 秒";
		}
	}
	#endregion
	#region 数值补间动画
	public sealed class QTween<T> : QTween
	{
		#region 对象池逻辑
		static ObjectPool<QTween<T>> _pool;
		private static ObjectPool<QTween<T>> Pool
		{
			get
			{
				return _pool ?? (_pool = QPoolManager.GetPool("[" + typeof(T).Name + "]QTween动画", () =>
				{
					return new QTween<T>();
				}));
			}
		}
		private QTween()
		{
		}
		public static QTween<T> PoolGet(Func<T> Get, Action<T> Set, T start, T end, float duration)
		{
			if (Pool == null)
			{
				Debug.LogError("不存在对象池" + typeof(T));
			}
			var tween = Pool.Get();
			tween.Set = Set;
			tween.Get = Get;
			tween.EndValue = end;
			tween.StartValue = start;
			tween.Duration = duration;
			return tween;
		}

		#endregion
		#region 补间逻辑
		public T StartValue { set; get; }
		public T EndValue { set; get; }

		public static Func<T, T, float, T> ValueLerp;
		public Func<T> Get { private set; get; }
		public Action<T> Set { private set; get; }
		protected override void OnUpdate()
		{
			try
			{
				var t = Duration > 0 ? TweenCurve((Time - 0) / Duration) : (PlayForwads ? 1 : 0);
				Set(ValueLerp(StartValue, EndValue, t));
			}
			catch (Exception e)
			{
				Debug.LogWarning("QTween<" + typeof(T) + ">更新数值出错：" + e);
			}
		}

		#endregion
		public override void Destory()
		{
			Pool.Push(this);
		}
		public override string ToString()
		{
			return "" + typeof(T).Name + " " + StartValue + " to " + EndValue + "";
		}

	}
	#endregion
}
