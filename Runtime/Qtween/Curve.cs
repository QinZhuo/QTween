using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace QTool.Tween
{

    public enum TweenCurve
    {
        Linear,
        InSin,
        OutSin,
        InOutSin,
        InBack,
        OutBack,
        InOutBack,
    }
 
    public static class Curve
    {
        public static Func<float,float> GetFunction(TweenCurve curve)
        {
            switch (curve)
            {
                case TweenCurve.Linear:
                    return TweenAnimationCurve.Linear;
                case TweenCurve.InSin:
                    return TweenAnimationCurve.Sin;
                case TweenCurve.OutSin:
                    return Out(TweenAnimationCurve.Sin);
                case TweenCurve.InOutSin:
                    return InOut( TweenAnimationCurve.Sin);
                case TweenCurve.InBack:
                    return TweenAnimationCurve.back;
                case TweenCurve.OutBack:
                    return Out(TweenAnimationCurve.back);
                case TweenCurve.InOutBack:
                    return InOut(TweenAnimationCurve.back);
                default:
                    return null;
            }
            
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
        static float temp1 = 1.7f;
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
    }
  
}

