using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace QTool.Tween
{
    public static class CurveExtend
    {
        public static Func<float, float> Out(this Func<float, float> InFunc)
        {
            return (t) =>
            {
                return 1 - InFunc(1-t);
            };
        }
        public static Func<float, float> InOut(this Func<float, float> InFunc)
        {
            return (t) =>
            {

                if (t < 0.5f)
                {
                    return InFunc(t * 2)/2;
                }
                else
                {
                    return InFunc.Out()(t* 2-1)/2+0.5f ;
                }
            };
        }
    }
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
        public static Func<float, float> Square
        {
            get
            {
                return TweenAnimationCurve.Square;
            }
        }
        public static Func<float, float> Cubic
        {
            get
            {
                return TweenAnimationCurve.Cubic;
            }
        }
        public static Func<float, float> Quartic
        {
            get
            {
                return TweenAnimationCurve.Quartic;
            }
        }
        public static Func<float, float> Quintic
        {
            get
            {
                return TweenAnimationCurve.Quintic;
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

