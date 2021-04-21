using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace QTool.Tween.Component
{
    public class QTweenPlayer
    {
        public QTweenBehavior CurTween { get;private set; }
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
            if (newTween == null) return;
            if (CurTween != null)
            {
                CurTween.Complete();
            }
            CurTween = newTween;
            newTween.Play(show);
        }
    }
    [RequireComponent(typeof(Selectable))]
    public class SelectableAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler
    {
        [HideInInspector]
        public Selectable selectable;
        private void Reset()
        {
            selectable = GetComponent<Selectable>();
        }
        public UnityEvent onSelect = new UnityEvent();
        public QTweenBehavior enterAnim;
        public QTweenBehavior selectAnim;
        public QTweenBehavior downAnim;
        public QTweenPlayer qTweenPlayer = new QTweenPlayer();
        private void Awake()
        {
            if (selectable == null)
            {
                selectable = GetComponent<Selectable>();
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
        public  void OnPointerEnter(PointerEventData eventData)
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
            onSelect?.Invoke();
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
