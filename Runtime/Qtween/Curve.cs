using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace QTool.Tween
{
    public enum EaseCurve
    {
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InBack,
        OutBack,
        InOutBack,
        InElastic,
        OutElastic,
        InOutElastic,
        InBounce,
        OutBounce,
        InOutBounce,

    }
    public static class Curve
    {
        public static Func<float,float> GetEaseFunc(EaseCurve ease)
        {
            switch (ease)
            {
                case EaseCurve.Linear:
                    return Linear;
                case EaseCurve.InSine:
                    return Sine;
                case EaseCurve.OutSine:
                    return Sine.Out();
                case EaseCurve.InOutSine:
                    return Sine.InOut();
                case EaseCurve.InQuad:
                    return Quad;
                case EaseCurve.OutQuad:
                    return Quad.Out();
                case EaseCurve.InOutQuad:
                    return Quad.InOut();
                case EaseCurve.InCubic:
                    return Cubic;
                case EaseCurve.OutCubic:
                    return Cubic.Out();
                case EaseCurve.InOutCubic:
                    return Cubic.InOut();
                case EaseCurve.InQuart:
                    return Quart;
                case EaseCurve.OutQuart:
                    return Quart.Out();
                case EaseCurve.InOutQuart:
                    return Quart.InOut();
                case EaseCurve.InQuint:
                    return Quint;
                case EaseCurve.OutQuint:
                    return Quint.Out();
                case EaseCurve.InOutQuint:
                    return Quint.InOut();
                case EaseCurve.InExpo:
                    return Expo;
                case EaseCurve.OutExpo:
                    return Expo.Out();
                case EaseCurve.InOutExpo:
                    return Expo.InOut();
                case EaseCurve.InCirc:
                    return Circ;
                case EaseCurve.OutCirc:
                    return Circ.Out();
                case EaseCurve.InOutCirc:
                    return Circ.InOut();
                case EaseCurve.InBack:
                    return Back;
                case EaseCurve.OutBack:
                    return Back.Out();
                case EaseCurve.InOutBack:
                    return Back.InOut();
                case EaseCurve.InElastic:
                    return Elastic;
                case EaseCurve.OutElastic:
                    return Elastic.Out();
                case EaseCurve.InOutElastic:
                    return Elastic.InOut();
                case EaseCurve.InBounce:
                    return Bounce;
                case EaseCurve.OutBounce:
                    return Bounce.Out();
                case EaseCurve.InOutBounce:
                    return Bounce.InOut();
                default:
                    return Linear;
            }
        }
        public static Func<float,float> Linear
        {
            get
            {
                return TweenAnimationCurve.Linear;
            }
        }
        public static Func<float, float> Sine
        {
            get
            {
                return TweenAnimationCurve.Sine;
            }
        }
       
        public static Func<float, float> Quad
        {
            get
            {
                return TweenAnimationCurve.PowFunc(2);
            }
        }
        public static Func<float, float> Cubic
        {
            get
            {
                return TweenAnimationCurve.PowFunc(3);
            }
        }
        public static Func<float, float> Quart
        {
            get
            {
                return TweenAnimationCurve.PowFunc(4);
            }
        }
        public static Func<float, float> Quint
        {
            get
            {
                return TweenAnimationCurve.PowFunc(5);
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
        public static Func<float, float> Back
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
                return TweenAnimationCurve.Out( TweenAnimationCurve.Bounce);
            }
        }
    }
 
    static class TweenAnimationCurve
    {
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
        static float temp1 = 1.70158f;
        static float d1 = 2.75f;
        static float temp2 = 7.5625f;
        public static float Linear(float t)
        {
            return t;
        }
        public static float Sine(float t)
        {
            return 1-Mathf.Sin((1-t )* Mathf.PI/2);
        }
        public static Func<float,float> PowFunc(float p)
        {
            return (t) => Mathf.Pow(t, p);
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

