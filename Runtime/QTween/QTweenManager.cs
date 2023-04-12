using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTool.Tween
{
    public class QTweenManager : QToolManagerBase<QTweenManager>
    {
#if UNITY_EDITOR
		[UnityEditor.InitializeOnLoadMethod]
#else
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#endif
		static void InitValueLerp()
		{
			QTween<string>.ValueLerp = QLerp.Lerp;
			QTween<float>.ValueLerp = QLerp.Lerp; 
			QTween<double>.ValueLerp = QLerp.Lerp;
			QTween<Vector2>.ValueLerp = QLerp.Lerp;
			QTween<Vector3>.ValueLerp = QLerp.Lerp;
			QTween<Quaternion>.ValueLerp = QLerp.Lerp;
			QTween<Color>.ValueLerp = QLerp.Lerp;
		}
	

		public static QTweenList TweenList()
        {
            return QTweenList.Get();
        }
        public static QTween<string> Tween(Func<string> Get, Action<string> Set, string end, float duration)
        {
            return GetTween(Get, Set, end, duration,QLerp.Lerp);
        }
        public static QTween<float> Tween(Func<float> Get, Action<float> Set, float end, float duration)
        {
           return GetTween(Get, Set, end, duration, QLerp.Lerp);
        }
        public static QTween<Vector2> Tween(Func<Vector2> Get, Action<Vector2> Set, Vector2 end, float duration)
        {
            return GetTween(Get, Set, end, duration, QLerp.Lerp);
        }
        public static QTween<Vector3> Tween(Func<Vector3> Get, Action<Vector3> Set, Vector3 end, float duration)
        {
            return GetTween(Get, Set, end, duration, QLerp.Lerp);
        }
        public static QTween<Quaternion> Tween(Func<Quaternion> Get, Action<Quaternion> Set, Quaternion end, float duration)
        {
            return GetTween(Get, Set, end, duration, QLerp.Lerp);
        }
        public static QTween<Color> Tween(Func<Color> Get, Action<Color> Set, Color end, float duration)
        {
            return GetTween(Get, Set, end, duration, QLerp.Lerp);
        }
        private static QTween<T> GetTween<T>(Func<T> Get, Action<T> Set, T end, float duration,Func<T, T, float, T> ValueLerp)
        {
            if (QTween<T>.ValueLerp == null)
            {
                QTween<T>.ValueLerp = ValueLerp;
            } 
            return QTween<T>.PoolGet(Get, Set,Get(), end, duration);
        }
        public static QTween Delay(float duration)
        {
            return QTweenDelay.Get(duration);
        }
		public event Action TweenUpdate;
     
        private void Update()
        {
            if (Time.time == 0) return;
            TweenUpdate?.Invoke();
        }
    }
}

