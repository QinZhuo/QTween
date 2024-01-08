using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace QTool.Tween
{
    public static class MaskableGraphicExtends
    {
        public static QTween<float> QValue(this Slider text, float value, float duration)
        {
            return QTweenManager.Tween(() => text.value,
            (setValue) => { text.value = setValue; },
            value, duration);
        }
        public static QTween<string> QText(this Text text, string value, float duration)
        {
            return QTweenManager.Tween(() => text.text,
            (setValue) => { text.text = setValue; },
            value, duration);
        }
        public static QTween<float> QFillAmount(this Image graphic, float value, float duration)
        {
            return QTweenManager.Tween(() => graphic.fillAmount,
            (setValue) => { graphic.fillAmount = setValue; },
            value, duration);
        }
        public static QTween<Color> QColor(this MaskableGraphic graphic, Color endColor, float duration)
        {
            return QTweenManager.Tween(() => graphic.color,
            (color) => { graphic.color = color; },
            endColor, duration);
        }

        public static QTween<float> QAlpha(this MaskableGraphic graphic, float endAlpha, float duration)
        {
            return QTweenManager.Tween(() => graphic.color.a,
            (alpha) => { graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha); },
            endAlpha, duration);
        }
    }
}