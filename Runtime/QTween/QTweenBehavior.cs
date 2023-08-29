using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using QTool.Inspector;
using System.Threading.Tasks;

namespace QTool.Tween
{

    public abstract class QTweenBehavior<T> : QTweenBehavior
    {
        [QName("动画曲线")]
		[SerializeField]
		public QEaseCurve curve = QEaseCurve.OutQuad;
        [QName("动画时长")]
		public float animTime = 0.4f;
		[QName("隐藏时长")]
		public float hideTime = 0.4f;
		[QName("开始")]
		[FormerlySerializedAs("HideValue")]
        public T StartValue;
        [QName("结束")]
		[FormerlySerializedAs("ShowValue")]
        public T EndValue;
        protected virtual void Reset()
        {
            EndValue = CurValue;
            StartValue = CurValue;
        }
        [ContextMenu("设为开始值")]
        public void SetStrat()
        {
            StartValue = CurValue;
        }
        [ContextMenu("设为结束值")]
        public void SetEnd()
        {
            EndValue = CurValue;
		}
		public override Task PlayAsync(bool show)
		{
			Anim.SetTimeScale(show || animTime <= 0 ? 1 : hideTime / animTime);
			return base.PlayAsync(show);
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
    public abstract class QTweenBehavior : MonoBehaviour
	{
		[QName("初始播放")]
        public bool playOnAwake = false;
		public ActionEvent OnShow;
        public ActionEvent OnHide;
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
		#region 动画初始化

		public virtual QTween Anim
        {
            get
            {
#if UNITY_EDITOR
				if (!Application.isPlaying && _anim != null && !_anim.IsPlaying)
				{
					_anim = null;
				}
#endif
				if (this == null) return null;
				if (_anim == null)
                {
                    _anim = GetTween().OnComplete(OnAnimOver).SetAutoDestory(this);
				}
                return _anim;
            }
        }

		private QTween _anim;
		/// <summary>
		/// 清空动画缓存 自动生成新动画
		/// </summary>
		public virtual void ClearAnim()
		{
			_anim?.Release();
			_anim = null;
		}

		protected abstract QTween GetTween();
		#endregion
		protected virtual void OnDestroy()
		{
			ClearAnim();
		}
		private void Awake()
		{
			if (playOnAwake)
			{
				Play(true);
			}
		}
		void OnAnimOver()
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
		public void Complete()
		{
			Anim.Complete();
		}
		public void Play(bool show)
        {
			_=PlayAsync(show);
		}
		public virtual async Task PlayAsync(bool show)
		{
			await Anim.PlayAsync(show);
		}
		public async void ShowAndHide()
        {
			await PlayAsync(true);
			if (_anim != null)
			{
				await PlayAsync(false);
			}
		}
		public override string ToString()
		{
			return gameObject + " " + Anim.ToString();
		}

	}
}
