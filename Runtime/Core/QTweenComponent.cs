using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using QTool.Inspector;
using System.Threading.Tasks;
using UnityEngine.Events;
using System;
using UnityEngine.UIElements;

namespace QTool.Tween
{

    public abstract class QTweenComponent<T> : QTweenComponent
    {
        [QName("动画曲线")] 
		[SerializeField]
		public QEaseCurve curve = QEaseCurve.Linear;
        [QName("动画时长")]
		public float animTime = 0.4f;
		[QName("隐藏时长")]
		public float hideTime = 0.2f;
		[QName("开始")]
        public T StartValue;
        [QName("结束")]
        public T EndValue;
        protected virtual void Reset()
        {
			StartValue = default;
			EndValue = CurValue;
		}
        [QName("设为开始值")]
        public void SetStart()
        {
            StartValue = CurValue;
			OnValidate();

		}
        [QName("设为结束值")]
        public void SetEnd()
        {
            EndValue = CurValue;
		}
		public override void Play(bool show) {
			Anim.SetTimeScale(show || animTime <= 0 ? 1 : animTime / hideTime);
			base.Play(show);
		}
		protected override QTween GetTween()
		{
			return QTween<T>.PoolGet(() => CurValue, (value) => CurValue = value, StartValue, EndValue, animTime).SetCurve(curve); ; 
		}
		public override string ToString()
        {
            return gameObject+" "+ StartValue + " => " + EndValue;
        }
        public RectTransform RectTransform
        {
            get
            {
                return transform as RectTransform;
            }
        }
        public abstract T CurValue { get; set; }
        public virtual QTween ChangeFunc(T value, float time)
        {
            CurValue = value;
            return null;
        }
       

    }
    public abstract class QTweenComponent : MonoBehaviour {
		[QName("初始播放")]
        public bool playOnAwake = false;
		public UnityEvent OnShow;
        public UnityEvent OnHide;
		
		[QName("显示")]
		public void Show()
		{
			Play(true);
		}
		[QName("隐藏")]
		public void Hide()
		{
			Play(false);
		}
		[QName("立即隐藏")]
		public void HideAndComplete()
		{
			Hide();
			Complete();
		}
		protected virtual void OnValidate()
		{
#if UNITY_EDITOR
			if (!Application.isPlaying && _anim != null) {
				_anim.OnStartEvent -= OnStart;
				_anim.OnCompleteEvent -= OnComplete;
				_anim = null;
			}
#endif
		}
		#region 动画初始化

		public virtual QTween Anim
        {
            get
            {
				if (this == null) return null;
				if (_anim == null)
                {
					_anim = GetTween().OnStart(OnStart).OnComplete(OnComplete).SetAutoDestory(this);
				}
                return _anim;
            }
        }

		internal QTween _anim;
		/// <summary>
		/// 清空动画缓存 自动生成新动画
		/// </summary>
		public virtual void ClearAnim()
		{
			if (_anim != null) {
				_anim.Complete();
				_anim.Release();
				_anim = null;
			}
		}

		protected abstract QTween GetTween();
		#endregion
		protected virtual void OnDestroy()
		{
			ClearAnim();
		}
		private void OnEnable() {
			if (playOnAwake) {
				Play(true);
			}
		}
		protected virtual void OnStart()
		{
		}
		protected virtual void OnComplete() {
			if (Anim.PlayForwads)
            {
				OnShow?.Invoke();
			}
			else
			{
				OnHide?.Invoke();
			}
			gameObject.SetDirty();
		}
		public void Complete()
		{
			Anim.Complete();
		}
		public virtual void Play(bool show) {
			Anim.SetPlayer(this).Play(show);
		}
		//public virtual async Task PlayAsync(bool show)
		//{
		//	Anim.SetPlayer(this).Play(show);
		//	await Anim.WaitOverAsync();
		//}

		public void ShowAndHide(float delaytime = 0)
		{
			Action action = null;
			action = () => {
				Anim.OnCompleteEvent -= action;
				Play(false);
			};
			Anim.OnComplete(action);
			Play(true);
		}
		public override string ToString()
		{
			return gameObject + " " + Anim.ToString();
		}

	}
}
