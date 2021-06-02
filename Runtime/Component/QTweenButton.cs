using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static UnityEngine.UI.Selectable;

namespace QTool.Tween.Component
{
    public class QTweenState
    {
        public QTween CurTween { get;private set; }
        public void Show(QTweenBehavior newTween)
        {
            Play(newTween?.Anim, true);
        }
        public void Hide(QTweenBehavior newTween)
        {
            Play(newTween?.Anim, false);
        }
        public void Play(QTween newTween,bool show)
        {
            if (newTween == null) return;
           // if (CurTween != null&&CurTween.IsPlaying&&!CurTween.PlayForwads)
            {
                CurTween?.Stop();
            }
            CurTween = newTween;
            newTween.Play(show);
        }
    }
    //[RequireComponent(typeof(Selectable))]
    public class QTweenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler
    {
        [HideInInspector]
        public Selectable selectable;
#if UNITY_EDITOR
        protected void Reset()
        {
            selectable = GetComponent<Selectable>();
            selectable.transition = Transition.None;
            //selectable.navigation = new UnityEngine.UI.Navigation
            //{
            //    mode = UnityEngine.UI.Navigation.Mode.None
            //};
        }
#endif
        public QTweenBehavior enterAnim;
        public QTweenBehavior selectAnim;
        public QTweenBehavior downAnim;
        public QTweenBehavior InteractableAnim;
        public QTweenBehavior onAnim;
        public QTweenState qTweenPlayer = new QTweenState();
        protected void Awake()
        {
            if (selectable == null)
            {
                selectable = GetComponent<Selectable>();
            }
            if (onAnim != null && selectable is Toggle)
            {
                var toggle = selectable as Toggle;
                toggle.onValueChanged .AddListener( onAnim.Play);
            }
        }
        private void OnDestroy()
        {
            if (onAnim != null && selectable is Toggle)
            {
                var toggle = selectable as Toggle;
                toggle.onValueChanged.RemoveListener(onAnim.Play);
            }
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
                return selectable.interactable;
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
