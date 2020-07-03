using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace QTool.Tween
{

    public enum Curve
    {
        Linear,
        InSin,
        OutSin,
        InOutSin,
        InBack,
        OutBack,
        InOutBack,
    }
 
    public static class CurveTool
    {
        public static Func<float,float> GetFunction(Curve curve)
        {
            switch (curve)
            {
                case Curve.Linear:
                    return TweenAnimationCurve.Linear;
                case Curve.InSin:
                    return TweenAnimationCurve.Sin;
                case Curve.OutSin:
                    return Out(TweenAnimationCurve.Sin);
                case Curve.InOutSin:
                    return InOut( TweenAnimationCurve.Sin);
                case Curve.InBack:
                    return TweenAnimationCurve.back;
                case Curve.OutBack:
                    return Out(TweenAnimationCurve.back);
                case Curve.InOutBack:
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

