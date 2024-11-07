using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
namespace QTool.Tween.Component
{
    public class QTweenText : QTweenComponent<string>
    {
        protected override void Reset()
        {
            base.Reset();
        }
		private string text;
        public override string CurValue {
			get => text; set {
				text = value;
				textChange.Invoke(text);
			}
		}
		public UnityEvent<string> textChange;
    }

}
