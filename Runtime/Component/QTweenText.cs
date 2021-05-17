using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
namespace QTool.Tween.Component
{
    public class QTweenText : QTweenString
    {
        private void Reset()
        {
            text = GetComponent<Text>();
        }
        public override string CurValue { get => text.text; set => text.text = value; }
        public Text text;
    }

}
