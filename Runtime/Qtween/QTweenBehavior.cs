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
		[Group(true)]
        [ViewName("动画曲线")]
		[SerializeField]
		public EaseCurve curve = EaseCurve.OutQuad;
        [ViewName("动画时长")]
		public float animTime = 0.4f;
		[ViewName("开始")]
		[FormerlySerializedAs("HideValue")]
        public T StartValue;
        [ViewName("结束")]
		[Group(false)]
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
		protected override QTween GetTween()
		{
			return QTween<T>.PoolGet(() => CurValue, (value) => CurValue = value, StartValue, EndValue, animTime); 
		}
		public override string ToString()
        {
            return StartValue + " => " + EndValue;
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
		[Group(true)]
		[ViewName("初始播放")]
        public bool playOnAwake = false;
		[ViewName("隐藏速度")]
		[Range(0.1f, 5f)]
		[Group(false)]
		public float hideTimeScale = 2f;
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

		public QTween Anim
        {
            get
            {
#if UNITY_EDITOR
				if (!Application.isPlaying&& _anim != null && !_anim.IsPlaying)
				{
					_anim = null;
				}
#endif
				if (_anim == null)
                {
                    _anim = GetTween().OnComplete(OnAnimOver).SetAutoDestory(false);
					_anim.Target = this;
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
			_anim?.Destory();
			_anim = null;
		}

		protected abstract QTween GetTween();
		#endregion

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
		public virtual void Play(bool show)
        {
			_=PlayAsync(show);
		}
		public async Task PlayAsync(bool show)
		{
			await Anim.SetTimeScale(show ? 1 : hideTimeScale).PlayAsync(show);
		}
		public async void ShowAndHide()
        {
            await Anim.PlayAsync(true);
            await Anim.PlayAsync(false);
        }
      
	}
}
