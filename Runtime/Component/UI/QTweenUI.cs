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
    public class QTweenUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerDownHandler, IPointerUpHandler
    {
		private Selectable m_Selectable = null;
		public Selectable Selectable => m_Selectable ??= GetComponent<Selectable>();
	
		[QName("进入动画"),QPopup]
        public QTweenComponent enterAnim;
        [QName("按下动画"),QPopup]
        public QTweenComponent downAnim;
        [QName("选中动画", nameof(HasSelectable)), QPopup]
        public QTweenComponent selectAnim;
        [QName("开关动画", nameof(HasToggle)), QPopup]
        public QTweenComponent onAnim;
        public bool HasSelectable
        {
            get
            {
                return Selectable != null;
            }
        }
        public bool HasToggle
        {
            get
            {
                return Selectable is Toggle;
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
				if (HasSelectable)
				{
					return Selectable.IsInteractable();
				}
				else
				{
					return true;
				}
			}
		}
		private void Reset()
		{
			Selectable.transition = Transition.None;
			if (Selectable is Toggle toggle)
			{
				if (onAnim != null)
				{
					toggle.onValueChanged.AddPersistentListener(ValueChange);
				}
			}
		}
		private void OnDisable()
		{
			downAnim?.Hide();
			enterAnim?.Hide();
		}
		public void ValueChange(bool value)
        {
            onAnim?.Play(value);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Interactable) return;
			enterAnim?.Show( );
        }
        public void OnPointerExit(PointerEventData eventData)
        {
			downAnim?.Hide();
			enterAnim?.Hide( );
        }


        public void OnSelect(BaseEventData eventData)
        {
            if (!Interactable) return;
			selectAnim?.Show();

        }
        public void OnDeselect(BaseEventData eventData)
        {
			selectAnim?.Hide();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Interactable) return;
			if (downAnim == null)
			{
				enterAnim?.Hide();
			}
			else
			{
				downAnim.Show();
			}
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!Interactable) return;
			if (downAnim == null)
			{
				enterAnim?.Show();
			}
			else
			{
				downAnim.Hide();
			}
        }
    }

}
