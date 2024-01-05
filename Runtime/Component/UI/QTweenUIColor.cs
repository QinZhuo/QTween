using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace QTool.Tween.Component
{
    public class QTweenUIColor : QTweenComponent<Color>
    {
      
        public override Color CurValue
        {
            get => UI.color;
            set {
                UI.color = onlyAlpha ? new Color(UI.color.r, UI.color.g, UI.color.b, value.a) : value;
                UI.SetDirty();
            }
        }
        public bool onlyAlpha = false;
		private MaskableGraphic _ui;
		private MaskableGraphic UI => _ui ?? GetComponent<MaskableGraphic>();
        
        public override string ToString()
        {
            return "颜色 "+ base.ToString();
        }
    }

}
