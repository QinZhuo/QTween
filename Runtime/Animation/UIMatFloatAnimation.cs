using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace QTool.Tween.Component
{
    public class UIMatFloatAnimation :QTweenFloat
    {
        Material _mat;
        Material Mat
        {
            get
            {
                if (_mat == null)
                {
                    var ui = GetComponent<MaskableGraphic>();

                    if (ui.material == null) return null;
                    if (!ui.material.name.Contains("temp_"))
                    {
                        if (Application.isPlaying)
                        {
                            var temp = new Material(ui.material);
                            temp.name = "temp_" + temp.GetHashCode();
                            ui.material = temp;

                        }
                    }
                    _mat = ui.material;
                }
                return _mat;
                // return GetComponent<MaskableGraphic>().material;
            }
        }
        public string key = "";
        public override float CurValue { get => Mat == null ? 0 : Mat.GetFloat(key); set { if (Mat != null) { Mat.SetFloat(key, value); }; } }
    }
}