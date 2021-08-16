using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QTool.Tween
{
    public class QTweenManager : MonoBehaviour
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
            //Debug.LogError(a + " => " + b + " : " + t+" = " +( a + dir * t));
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
        public static string Lerp(string a, string b, float t)
        {
            var str = "";
            for (int i = 0; i < a.Length||i<b.Length; i++)
            {
                if (i <t*b.Length)
                {
                    str += b[i];
                }
                else if(i<a.Length)
                {
                    str += a[i];
                }
            }
            return str;
        }
        public static QTweenList TweenList()
        {
            return QTweenList.Get();
        }
        public static QTween<string> Tween(Func<string> Get, Action<string> Set, string end, float duration)
        {
            return Tween(Get, Set, end, duration, Lerp);
        }
        public static QTween<float> Tween(Func<float> Get, Action<float> Set, float end, float duration)
        {
           return Tween(Get, Set, end, duration, Lerp);
        }
        public static QTween<Vector2> Tween(Func<Vector2> Get, Action<Vector2> Set, Vector2 end, float duration)
        {
            return Tween(Get, Set, end, duration, Lerp);
        }
        public static QTween<Vector3> Tween(Func<Vector3> Get, Action<Vector3> Set, Vector3 end, float duration)
        {
            return Tween(Get, Set, end, duration, Lerp);
        }
        public static QTween<Quaternion> Tween(Func<Quaternion> Get, Action<Quaternion> Set, Quaternion end, float duration)
        {
            return Tween(Get, Set, end, duration, Lerp);
        }
        public static QTween<Color> Tween(Func<Color> Get, Action<Color> Set, Color end, float duration)
        {
            return Tween(Get, Set, end, duration, Lerp);
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

        public event Action TweenUpdate;
        private void Update()
        {
            if (Time.time == 0) return;
            TweenUpdate?.Invoke();
        }
    }
}

