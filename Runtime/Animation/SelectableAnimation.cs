﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
            if (CurTween != null&&CurTween.IsPlaying&&!CurTween.PlayForwads)
            {
                CurTween.Stop();
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
        public QTweenState qTweenPlayer = new QTweenState();
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
