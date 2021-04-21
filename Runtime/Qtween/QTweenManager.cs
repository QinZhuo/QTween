using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTool.Tween
{
    public static class QTween
    {
        static QTweenManager _instance;
        public static QTweenManager Manager
        {
            get
            {
                if (_instance == null && Application.isPlaying)
                {
                    var obj = new GameObject("QTweenManager");
                    _instance = obj.AddComponent<QTweenManager>();
                    GameObject.DontDestroyOnLoad(obj);
                }
                return _instance;
            }
        }
        public static float Lerp(float a, float b, float t)
        {
            var dir = b - a;
            return a + dir * t;
        }

        public static Vector2 Lerp(Vector2 star, Vector2 end, float t)
        {
            return new Vector2(Lerp(star.x, end.x, t), Lerp(star.y, end.y, t));
        }
        public static Vector3 Lerp(Vector3 star, Vector3 end, float t)
        {
            return new Vector3(Lerp(star.x, end.x, t), Lerp(star.y, end.y, t), Lerp(star.z, end.z, t));
        }
        public static Quaternion Lerp(Quaternion star, Quaternion end, float t)
        {
            return Quaternion.Lerp(star, end, t);
        }
        public static Color Lerp(Color star, Color end, float t)
        {
            return Color.Lerp(star, end, t);
        }
        public static QTweenBase Tween<T>(Func<T> Get, Action<T> Set, Func<T, T, float, T> tweenCurve, T end, float duration)
        {
            return QTween<T>.GetTween(Get, Set, tweenCurve, end, duration).Init().Play();
        }
        public static QTweenBase Tween<T>(Func<T> Get, Action<T> Set, Func<T, T, float, T> tweenCurve, T start, T end, float duration)
        {
            return QTween<T>.GetTween(Get, Set, tweenCurve, start, end, duration).Init().Play();
        }
        public static QTweenBase Delay(float duration)
        {
            return QTweenDelay.Get(duration).Init().Play();
        }
        public static void DelayInvoke(float time, Action action)
        {
            Delay(time).OnComplete(action);
        }
    }
    public class QTweenManager : MonoBehaviour
    {
        public event Action TweenUpdate;
        private void Update()
        {
            TweenUpdate?.Invoke();
        }
    }
}

