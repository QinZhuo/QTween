using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTool.Tween
{
    public class QTweenManager : InstanceManager<QTweenManager>
    {
		protected override void Awake()
		{
			base.Awake();
			GameObject.DontDestroyOnLoad(gameObject);
		}

		public static QTweenList TweenList()
        {
            return QTweenList.Get();
        }
        public static QTween<string> Tween(Func<string> Get, Action<string> Set, string end, float duration)
        {
            return Tween(Get, Set, end, duration,QLerp.LerpTo);
        }
        public static QTween<float> Tween(Func<float> Get, Action<float> Set, float end, float duration)
        {
           return Tween(Get, Set, end, duration, QLerp.LerpTo);
        }
        public static QTween<Vector2> Tween(Func<Vector2> Get, Action<Vector2> Set, Vector2 end, float duration)
        {
            return Tween(Get, Set, end, duration, QLerp.LerpTo);
        }
        public static QTween<Vector3> Tween(Func<Vector3> Get, Action<Vector3> Set, Vector3 end, float duration)
        {
            return Tween(Get, Set, end, duration, QLerp.LerpTo);
        }
        public static QTween<Quaternion> Tween(Func<Quaternion> Get, Action<Quaternion> Set, Quaternion end, float duration)
        {
            return Tween(Get, Set, end, duration, QLerp.LerpTo);
        }
        public static QTween<Color> Tween(Func<Color> Get, Action<Color> Set, Color end, float duration)
        {
            return Tween(Get, Set, end, duration, QLerp.LerpTo);
        }
        public static QTween<T> Tween<T>(Func<T> Get, Action<T> Set, T end, float duration,Func<T, T, float, T> ValueLerp)
        {
            if (QTween<T>.ValueLerp == null)
            {
                QTween<T>.ValueLerp = ValueLerp;
            } 
            return QTween<T>.GetTween(Get, Set, end, duration);
        }
        public static QTween Delay(float duration)
        {
            return QTweenDelay.Get(duration);
        }
        public static void DelayInvoke(float time, Action action)
        {
            Delay(time).OnComplete(action).Play();
        }

		public static event Action TweenUpdate;
     
        private void Update()
        {
            if (Time.time == 0) return;
            TweenUpdate?.Invoke();
        }
    }
}

