using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
namespace QTool.Tween.Component
{
    public abstract class QTweenString : QTweenBehavior<string>
    {
        protected override QTween ShowTween()
        {

            return QTweenManager.Tween(() => CurValue, (value) => CurValue = value, ShowValue, showTime).ResetStart(HideValue);
        }
    }
    public abstract class QTweenVector2 : QTweenBehavior<Vector2>
    {
        protected override QTween ShowTween()
        {

            return QTweenManager.Tween(() => CurValue, (value) => CurValue = value, ShowValue, showTime).ResetStart(HideValue);
        }
    }
    public abstract class QTweenVector3 : QTweenBehavior<Vector3>
    {
        protected override QTween ShowTween()
        {
            return QTweenManager.Tween(() => CurValue, (value) => CurValue = value, ShowValue, showTime).ResetStart(HideValue);
        }
    }
    public abstract class QTweenFloat : QTweenBehavior<float>
    {
        protected override QTween ShowTween()
        {
            return QTweenManager.Tween(() => CurValue, (value) => CurValue = value, ShowValue, showTime).ResetStart(HideValue);
        }
    }
    public abstract class QTweenColor : QTweenBehavior<Color>
    {
        protected override QTween ShowTween()
        {
            return QTweenManager.Tween(() => CurValue, (value) => CurValue = value, ShowValue, showTime).ResetStart(HideValue);
        }
    }
    public abstract class QTweenQuaternion : QTweenBehavior<Quaternion>
    {
        protected override QTween ShowTween()
        {
            return QTweenManager.Tween(() => CurValue, (value) => CurValue = value, ShowValue, showTime).ResetStart(HideValue);
        }
    }
    public abstract class QTweenBehavior<T> : QTweenBehavior
    {
        RectTransform _rect;
        public RectTransform RectTransform
        {
            get
            {
                return _rect ?? (_rect.GetComponent<RectTransform>());
            }
        }
        public T ShowValue;
        public T HideValue;
        public abstract T CurValue { get; set; }
        public virtual QTween ChangeFunc(T value, float time)
        {
            CurValue = value;
            
            Debug.Log("Value:" + CurValue);
            return null;
        }

    }
    public abstract class QTweenBehavior : MonoBehaviour
    {
        public EaseCurve curve = EaseCurve.OutQuad;
        [FormerlySerializedAs("animTime")]
        public float showTime = 0.4f;
        public float hideTime = 0.4f;
        protected abstract QTween ShowTween();
        [FormerlySerializedAs("nextUIShow")]
        public QTweenBehavior nextTween;
        [FormerlySerializedAs("uiShowList")]
        public List<QTweenBehavior> startTweenList = new List<QTweenBehavior>();

        public ActionEvent OnShow;
        public ActionEvent OnHide;
        QTween _anim;
        public QTween Anim
        {
            get
            {
                if (_anim == null)
                {
                    _anim = ShowTween().SetCurve(curve).AutoDestory(false);
                    if (nextTween != null)
                    {
                        _anim.Next(nextTween.Anim);
                    }
                }
                return _anim;  
            }
        }
        public void Play(bool show)
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
        public void Complete()
        {
            Anim.Stop();
            //Anim.Complete(false);
        }
        //private void OnDrawGizmos()
        //{
        //    Gizmos.DrawIcon(transform.position, "UIAnimationIcon");
        //}
    }

    public class UIPosAnimation : QTweenVector2
    {
        public override Vector2 CurValue { get => RectTransform.anchoredPosition; set => RectTransform.anchoredPosition = value; }
    }
}