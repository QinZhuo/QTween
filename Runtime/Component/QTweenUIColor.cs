using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace QTool.Tween.Component
{
    public class QTweenUIColor : QTweenColor
    {
        protected override void Reset()
        {
            if (ui == null)
            {
                ui = GetComponent<MaskableGraphic>();
            }
            base.Reset();
        }
        public override Color CurValue
        {
            get => ui.color;
            set {
                ui.color = onlyAlpha ? new Color(ui.color.r, ui.color.g, ui.color.b, value.a) : value;
                LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
            }
        }
        public bool onlyAlpha = false;
        public MaskableGraphic ui;
        private void Awake()
        {
            if (ui == null)
            {
                ui = GetComponent<MaskableGraphic>();
            }
        }
        public override string ToString()
        {
            return "颜色 "+ base.ToString();
        }
    }

}