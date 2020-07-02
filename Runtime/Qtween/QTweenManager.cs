using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTool.Tween
{
   
    public class QTweenManager : MonoBehaviour
    {
        static QTweenManager _instance;
        public static QTweenManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = new GameObject("QTweenManager");
                    _instance=obj.AddComponent<QTweenManager>();
                }
                return _instance;
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

    }
}

