using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using QTool.Inspector;
using System.Threading.Tasks;

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
        [ViewName("动画曲线")]
        public EaseCurve curve = EaseCurve.OutQuad;
        [ViewName("动画时长")]
        [FormerlySerializedAs("animTime")]
        public float animTime = 0.4f;
        [ViewName("隐藏速度")]
        [Range(0.1f,5f)]
        public float hideTimeScale = 2f;
        [ViewName("开始")]
        [FormerlySerializedAs("HideValue")]
        public T StartValue;
        [ViewName("结束")]
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
        public override string ToString()
        {
            return StartValue + " => " + EndValue;
        }
        public override void ReverseStartEnd()
        {
            var temp = EndValue;
            EndValue = StartValue;
            StartValue = temp;
            this.SetDirty();
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
        protected override QTween TweenInit(QTween tween)
        {
            tween.HideTimeScale = hideTimeScale;
            return base.TweenInit(tween).SetCurve(curve);
        }

    }
    public abstract class QTweenBehavior : MonoBehaviour
    {
#if UNITY_EDITOR
        [ViewName("当前时间")]
        public float curTime;
        private void Update()
        {
            curTime = Anim.time;
        }
#endif

        public bool playOnAwake = false;
        public ActionEvent OnShow;
        public ActionEvent OnHide;
        private void Awake()
        {
            if (playOnAwake)
            {
                Play(true);
            }
        }
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
        public virtual async Task PlayAsync(bool show)
        {
            await Anim.PlayAsync(show);
        }
        public virtual void Play(bool show)
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
}