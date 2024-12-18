using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_2021_1_OR_NEWER
using UnityEngine.Pool;
#endif
namespace QTool.Tween
{

	#region 基础动画逻辑
	public abstract class QTween : IQPoolObject
	{
		#region 基础属性
		private bool _isPlaying = false;
		public bool PlayForwads { get; private set; } = false;
		public float Time { get; set; } = -1f;
		public float Duration { get; protected set; }
		public float TimeScale { get; private set; } = 1;
		public bool IgnoreTimeScale { private set; get; } = true;
		public bool AutoDestory { private set; get; } = true;
		public Func<float, float> TweenCurve { get; set; } = QCurve.Linear;
		public UnityEngine.Object Target { get; private set; }
		public override string ToString()
		{
			var info = "[" + GetHashCode() + "]" + nameof(IsPlaying) + "[" + IsPlaying + "]";
			if (AutoDestory)
			{
				info += nameof(AutoDestory);
			}
			else
			{
				info += "Target[" + (Target == null ? "null" : Target.name + "(" + Target?.GetType()?.Name + ")") + "]";
			}
			return info;
		}
		#region 更改数值
		public QTween SetCurve(QEaseCurve ease)
		{
			TweenCurve = QCurve.Get(ease);
			return this;
		}
		public virtual QTween SetAutoDestory(UnityEngine.Object target = null)
		{
			AutoDestory = target == null;
			this.Target = target;
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
		public QTween OnStart(Action action)
		{
			OnStartEvent += action;
			return this;
		}
		public QTween OnUpdate(Action<float> action)
		{
			OnUpdateEvent += action;
			return this;
		}
		public QTween OnComplete(Action action) {
			OnCompleteEvent += action;
			return this;
		}
		#endregion
		public event Action OnStartEvent;
		public event Action OnCompleteEvent;
		public event Action<float> OnUpdateEvent;
		public virtual void OnPoolGet()
		{

		}
		public virtual void OnPoolRelease()
		{
			SetAutoDestory();
			PlayForwads = false;
			Time = -1;
			TimeScale = 1;
			Duration = 0;
			IsPlaying = false;
			IgnoreTimeScale = true;
			TweenCurve = QCurve.Linear;
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
						if (!_isPlaying && !QTweenManager.Exists)
							return;
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
		static QDictionary<UnityEngine.Object, QTween> Players = new QDictionary<UnityEngine.Object, QTween>();
		/// <summary>
		/// 设置播放器 同一个物体同时只有一个动画播放 如果有动画在播放中则停止动画
		/// </summary>
		public QTween SetPlayer(UnityEngine.Object player = null)
		{
			if (player != null)
			{
				if (Players.ContainsKey(player) && Players[player].IsPlaying)
				{
					Players[player].Stop();
				}
				else
				{
					Players[player] = this;
				}
			}
			return this;
		}
		public QTween Play(bool PlayForwads = true) {
			this.PlayForwads = PlayForwads;
			if (!IsPlaying || this.PlayForwads != PlayForwads) {
				IsPlaying = true;
				OnStart();
				OnStartEvent?.Invoke();
			}
			return this;
		}
		protected virtual void OnStart()
		{
			if (Time < 0)
			{
				Time = PlayForwads ? 0 : Duration;
			}
		}
	
		public IEnumerator WaitOver()
		{
			var flag = Application.isPlaying;
			var PlayForwads = this.PlayForwads;
			while (PlayForwads == this.PlayForwads && !IsOver && flag == Application.isPlaying)
			{
				yield return null;
			}
		}
		private void Update()
		{
			if (!AutoDestory && Target == null)
			{
				Stop();
				return;
			}
			Time += (Application.isPlaying ? (IgnoreTimeScale ? UnityEngine.Time.unscaledDeltaTime : UnityEngine.Time.deltaTime) : Mathf.Min(QTime.EditorDeltaTime, 0.1f)) * (PlayForwads ? 1 : -1) * TimeScale;
			Update(Time);
		}
		private void Update(float time)
		{
			Time = Mathf.Clamp(time, 0, Duration);
			OnUpdate();
			OnUpdateEvent?.Invoke(time);
			if (IsOver) {
				OnComplete();
			}
		}
		protected abstract void OnUpdate();
		protected virtual void OnComplete() {
			OnCompleteEvent?.Invoke();
			if (IsOver) {
				Stop();
			}
		}
		public abstract void Release();
		#endregion
		#region 更改生命周期
		public QTween Stop()
		{
			Time = PlayForwads ? Duration : 0;
			IsPlaying = false;
			if (AutoDestory || Target == null)
			{
				Release();
			}
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
	internal class QTweenDelay : QTween
	{
		public static QTweenDelay Get(float t)
		{
			var tween = QObjectPool<QTweenDelay>.Get();
			tween.Duration = t;
			return tween;
		}

		protected override void OnUpdate()
		{
		}
		public override void Release()
		{
			QObjectPool<QTweenDelay>.Release(this);
		}
	
		public override string ToString()
		{
			return "延迟 " + Duration + " 秒";
		}

	}
	#endregion
	#region 数值补间动画
	public class QTween<T> : QTween
	{
		#region 对象池逻辑
		public QTween()
		{
		}
		public static QTween<T> PoolGet(Func<T> Get, Action<T> Set, T start, T end, float duration)
		{
			var tween = QObjectPool<QTween<T>>.Get();
			tween.Set = Set;
			tween.Get = Get;
			tween.EndValue = end;
			tween.StartValue = start;
			tween.Duration = duration;
			return tween;
		}
		protected override void OnStart()
		{
			if (!AutoDestory && Target == null)
			{
				return;
			}
			if (StartValue.Equals(Get()))
			{
				Time = 0;
			}
			else if (EndValue.Equals(Get()))
			{
				Time = Duration;
			}
			else
			{
				base.OnStart();
			}
		}
		#endregion
		#region 补间逻辑
		public T StartValue { set; get; }
		public T EndValue { set; get; }

		public static Func<T, T, float, T> ValueLerp;
		public Func<T> Get { private set; get; }
		public Action<T> Set { private set; get; }
		public override void OnPoolRelease()
		{
			Get = null;
			Set = null;
			base.OnPoolRelease();
		}
		protected override void OnUpdate()
		{
			try
			{
				var t = Duration > 0 ? TweenCurve((Time - 0) / Duration) : (PlayForwads ? 1 : 0);
				Set(ValueLerp(StartValue, EndValue, t));
			}
			catch (Exception e)
			{
				if (Target == null)
				{
					Debug.LogWarning("更新动画 QTween<" + typeof(T) + ">出错：" + e);
				}
				else
				{
					Debug.LogWarning((Target as MonoBehaviour)?.transform.GetPath() + " " + Target + " 更新动画 QTween<" + typeof(T) + ">出错：" + e);
				}
			}
		}

		#endregion
		public override void Release()
		{
			QObjectPool<QTween<T>>.Release(this);
		}
		public override string ToString()
		{
			return nameof(QTween)+"<"+typeof(T).Name + "> [" + StartValue + " => " + EndValue + "]"+base.ToString();
		}
	}
	#endregion
}
