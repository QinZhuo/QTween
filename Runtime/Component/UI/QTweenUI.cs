using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;
using QTool.Inspector;
using static UnityEngine.UI.Selectable;

namespace QTool.Tween.Component
{
    public class QTweenPlayer
    {
        public QTween CurTween { get;private set; }
        public void Show(QTweenBehavior newTween)
        {
            Play(newTween, true);
        }
        public void Hide(QTweenBehavior newTween)
        {
            Play(newTween, false);
        }
        public void Play(QTweenBehavior newTween,bool show)
        {
            if (newTween?.Anim == null) return;
            if(CurTween!=null&&CurTween.IsPlaying)
            {
                CurTween.Complete();
            }
            CurTween = newTween.Anim;
            newTween.Play(show);
        }
    }
    public class QTweenUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler
    {
        [HideInInspector]
        public Selectable selectable;
#if UNITY_EDITOR
        protected void Reset()
        {
            Init();
        }
        private void OnValidate()
        {
            Init();
        }
#endif
        public void Init()
        {
            if (selectable == null)
            {
                selectable = GetComponent<Selectable>();
                if (selectable != null)
                {
                    selectable.transition = Transition.None;
                }
            }
        }
        [ViewName("进入动画")]
        public QTweenBehavior enterAnim;
        [ViewName("按下动画")]
        public QTweenBehavior downAnim;
        [ViewName("选中动画", "HasSelectable")]
        public QTweenBehavior selectAnim;
        //[ViewName("禁用动画", "HasSelectable")]
        //public QTweenBehavior InteractableAnim;
        [ViewName("开关动画", "HasToggle")]
        public QTweenBehavior onAnim;
        public QTweenPlayer qTweenPlayer = new QTweenPlayer();
        public bool HasSelectable
        {
            get
            {
                return selectable != null;
            }
        }
        public bool HasToggle
        {
            get
            {
                return selectable is Toggle;
            }
        }

        protected void Awake()
        {
            Init();
            if (HasToggle)
            {
                var toggle = selectable as Toggle;
                if (onAnim != null)
                {
                    toggle.onValueChanged.AddListener(ValueChange);
                }
            }
        }
        void ValueChange(bool value)
        {
            onAnim?.Play(value);
        }
        private void OnDestroy()
        {
            if (onAnim != null && selectable is Toggle)
            {
                var toggle = selectable as Toggle;
                if (onAnim != null)
                {
                    toggle.onValueChanged.RemoveListener(ValueChange);
                }
            }
        }
        private void OnDisable()
        {
            Hide();
        }
        public void Hide()
        {
            qTweenPlayer.Hide(downAnim);
            qTweenPlayer.Hide(enterAnim);
        }
        public bool SelectThis
        {
            get
            {
                return EventSystem.current.currentSelectedGameObject == gameObject;
            }
        }
        public bool Interactable
        {
            get
            {
                if (HasSelectable)
                {
                    return selectable.IsInteractable();
                }
                else
                {
                    return true;
                }
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Interactable) return;
            qTweenPlayer.Show( enterAnim);
        }
        public  void OnPointerExit(PointerEventData eventData)
        {
            qTweenPlayer.Hide(downAnim);
            qTweenPlayer.Hide( enterAnim);
        }


        public  void OnSelect(BaseEventData eventData)
        {
            if (!Interactable) return;
            qTweenPlayer.Show(selectAnim);

        }

        public  void OnDeselect(BaseEventData eventData)
        {
            qTweenPlayer.Hide(selectAnim);
        }

        public  void OnPointerDown(PointerEventData eventData)
        {
            if (!Interactable) return;
            qTweenPlayer.Show(downAnim);
        }

        public  void OnPointerUp(PointerEventData eventData)
        {
            if (!Interactable) return;
            qTweenPlayer.Hide(downAnim);
        }
    }
}
