using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace QTool.Tween
{

    public static class Curve
    {
        public static Func<float,float> Linear
        {
            get
            {
                return TweenAnimationCurve.Linear;
            }
        }
        public static Func<float, float> Sin
        {
            get
            {
                return TweenAnimationCurve.Sin;
            }
        }
        public static Func<float, float> Cubic
        {
            get
            {
                return TweenAnimationCurve.Cubic;
            }
        }
        public static Func<float, float> Expo
        {
            get
            {
                return TweenAnimationCurve.Expo;
            }
        }
        public static Func<float, float> Circ
        {
            get
            {
                return TweenAnimationCurve.Circ;
            }
        }
        public static Func<float, float> back
        {
            get
            {
                return TweenAnimationCurve.back;
            }
        }
        public static Func<float, float> Elastic
        {
            get
            {
                return TweenAnimationCurve.Elastic;
            }
        }
        public static Func<float, float> Bounce
        {
            get
            {
                return CurveTool.Out( TweenAnimationCurve.Bounce);
            }
        }
    }
 
    public static class CurveTool
    {

        public static Func<float, float> In(this Func<float, float> InFunc)
        {
            return InFunc;
        }
        public static Func<float, float> Out(this Func<float, float> InFunc)
        {
            return (t) =>
            {
                return 1 - InFunc(1 - t);
            };
        }
        public static Func<float, float> InOut(this Func<float, float> InFunc)
        {
            return (t) =>
            {

                if (t < 0.5f)
                {
                    return InFunc(t * 2) / 2;
                }
                else
                {
                    return InFunc.Out()(t * 2 - 1) / 2 + 0.5f;
                }
            };
        }
    }
    static class TweenAnimationCurve
    {
        static float temp1 = 1.70158f;
        static float d1 = 2.75f;
        static float temp2 = 7.5625f;
        public static float Linear(float t)
        {
            return t;
        }
        public static float Sin(float t)
        {
            return 1-Mathf.Sin((1-t )* Mathf.PI/2);
        }
        public static float Square(float t)
        {
            return t * t;
        }
        public static float Cubic(float t)
        {
            return t * t * t;
        }
        public static float Quartic(float t)
        {
            return t * t * t * t;
        }
        public static float Quintic(float t)
        {
            return t * t * t * t * t;
        }
        public static float Expo(float t)
        {
            return t == 0 ? t : Mathf.Pow(2, 10 * (t - 1));
        }
        public static float Circ(float t)
        {
            return 1 - Mathf.Sqrt(1 - t * t);
        }
        public static float back(float t)
        {
            return  t * t * t * (temp1 + 1) - t * t * temp1;
        }
        public static float Elastic(float t)
        {
            if (t == 0 || t == 1)
            {
                return t;
            }
            else
            {
                return -Mathf.Pow(2, 10 * (t - 1)) * Mathf.Sin((t * 10*(1 - 0.75f)) * Mathf.PI*2/3);
            }
        }
        static float BonceTool(float t,float p)
        {
            return temp2 * Mathf.Pow(t - (p / d1), 2);
        }
        public static float Bounce(float t)
        {
            if (t < 1/ d1)
            {
                return temp2 * t * t;
            }
            else if(t<2/d1)
            {
                return BonceTool(t, 1.5f) + 0.75f;
            }
            else if (t < 2.5 / d1)
            {
                return BonceTool(t, 2.25f) + 0.93375f;
            }
            else
            {
                return BonceTool(t, 2.625f) + 0.984375f;
            }
        }
    }
  
}

