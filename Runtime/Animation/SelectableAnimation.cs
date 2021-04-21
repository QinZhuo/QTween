using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace QTool.Tween.Component
{
    //public class TweenRef
    //{
    //    AnimRef anim = new AnimRef();
    //    QTweenBehavior _tween;
    //    public QTweenBehavior Tween
    //    {
    //        get
    //        {
    //            return _tween;
    //        }
    //        set
    //        {
    //            if (value != _tween)
    //            {
    //                _tween?.Complete();
    //                _tween = value;
    //            }
    //            anim.Anim = _tween.CurAnim;
    //        }
    //    }
    //}
    public class SelectableAnimation : Selectable, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler
    {
        Selectable ui
        {
            get
            {
                return this;
            }
        }
        public UnityEvent onSelect = new UnityEvent();
        [FormerlySerializedAs("selectAnim")]
        public QTweenBehavior enterAnim;
        public QTweenBehavior selectAnim;
        public QTweenBehavior downAnim;
        //TweenRef CurAnim = new TweenRef();

        public bool SelectThis
        {
            get
            {
                return EventSystem.current.currentSelectedGameObject == gameObject;
            }
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!interactable) return;
            if (enterAnim != null)
            {
                enterAnim.Show();
            //    CurAnim.Tween = enterAnim;
            }
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (downAnim != null
                // &&  CurAnim != null && CurAnim.Tween == downAnim
                )
            {
                downAnim.Hide();
                downAnim.Complete();
            }
            if (enterAnim != null)
            {
                enterAnim?.Hide();
           //     CurAnim.Tween = enterAnim;
            }


        }


        public override void OnSelect(BaseEventData eventData)
        {
            if (!interactable) return;
            onSelect?.Invoke();
            if (selectAnim != null)
            {
                selectAnim.Show();
        //        CurAnim.Tween = selectAnim;
            }

        }

        public override void OnDeselect(BaseEventData eventData)
        {
            if (selectAnim != null)
            {
                selectAnim?.Hide();
      //          CurAnim.Tween = selectAnim;
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!IsInteractable()) return;
            if (downAnim != null)
            {
                downAnim.Show();
       //         CurAnim.Tween = downAnim;
            }

        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!IsInteractable()) return;
            if (downAnim != null)
            {
                downAnim.Hide();
       //         CurAnim.Tween = downAnim;
            }
        }
    }
}
