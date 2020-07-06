using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTool.Tween
{
   
    public class QTweenManager : MonoBehaviour
    {
        static QTweenManager _instance;

        Queue<QTween> addQueue = new Queue<QTween>();
        LinkedList<QTween> tweenList = new LinkedList<QTween>();
        Queue<QTween> removeQueue = new Queue<QTween>();
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
        public static void Add(QTween tween)
        {

            if (!(Instance.tweenList.Contains(tween)|| Instance.addQueue.Contains(tween)))
            {
                Instance.addQueue.Enqueue(tween);
         
            }
           
        }
        public static void Kill(QTween tween)
        {
            Instance.removeQueue.Enqueue(tween);
        }
        private void Update()
        {
            while (addQueue.Count > 0)
            {
                Instance.tweenList.AddLast(addQueue.Dequeue());
            }
           
            while (removeQueue.Count > 0)
            {
                tweenList.Remove(removeQueue.Dequeue());
            }
            
            foreach (var tween in tweenList)
            {
                try
                {
                    tween.Update();
                }
                catch (Exception e)
                {

                    tween.Stop();
                    Debug.LogWarning("【QTween】动画终止：" + e);
                }
               
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

    }
}

