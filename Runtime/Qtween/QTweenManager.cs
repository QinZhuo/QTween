using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTool.Tween
{
   
    public class QTweenManager : MonoBehaviour
    {
        static QTweenManager _instance;
        public List<QTween> tweenList = new List<QTween>();
        public Queue<QTween> removeQueue = new Queue<QTween>();
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
            if (!Instance.tweenList.Contains(tween))
            {
                Instance.tweenList.Add(tween);
            }
           
        }
        public static void Kill(QTween tween)
        {
            Instance.removeQueue.Enqueue(tween);
        }
        private void Update()
        {
            foreach (var tween in tweenList)
            {
                tween.Update();
            }
            while (removeQueue.Count>0)
            {
                tweenList.Remove(removeQueue.Dequeue());
            }
            Debug.LogError("count: " + tweenList.Count);
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

    }
}

