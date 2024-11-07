using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace QTool.Tween.Component
{
    public class QTweenMatrialFloat : QTweenComponent<float>
    {
        Material _mat;
        Material Mat
        {
            get
            {
				if (_mat == null)
				{
					_mat= GetComponent<Renderer>().material;
				}
				return _mat;
			}
        }
        public string key = "";
        public override float CurValue { get => Mat == null ? 0 : Mat.GetFloat(key); set { if (Mat != null) { Mat.SetFloat(key, value); }; } }
    }
}
