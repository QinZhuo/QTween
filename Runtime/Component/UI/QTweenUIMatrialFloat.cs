using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace QTool.Tween.Component
{
    public class QTweenUIMatrialFloat : QTweenBehavior<float>
    {
        Material _mat;
        Material Mat
        {
            get
            {
                return _mat??= GetComponent<MaskableGraphic>().GetInstanceMaterial(); 
			}
        }
        public string key = "";
        public override float CurValue { get => Mat == null ? 0 : Mat.GetFloat(key); set { if (Mat != null) { Mat.SetFloat(key, value); }; } }
    }
}
