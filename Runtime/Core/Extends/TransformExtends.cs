using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QTool.Tween
{
    public static class TransformExtends
    {

        static Vector3 GetPosition(this Transform transform)
        {
            return transform.position;
        }

        static void SetPosition(this Transform transform, Vector3 postion)
        {
            transform.position = postion;
        }
        public static QTween<Quaternion> QRotate(this Transform transform, Vector3 value, float duration)
        {
            return QTweenManager.Tween(() => transform.rotation,
            (setValue) => { transform.rotation = setValue; },
            Quaternion.Euler(value), duration);
        }
        public static QTween<Vector3> QLocalRotate(this Transform transform, Vector3 value, float duration)
        {
            return QTweenManager.Tween(() => transform.localRotation.eulerAngles,
            (setValue) => { transform.localRotation = Quaternion.Euler(setValue); },
             value, duration);
        }
        public static QTween<Vector3> QMove(this Transform transform, Vector3 postion, float duration)
        {
            return QTweenManager.Tween(transform.GetPosition,
            transform.SetPosition,
            postion, duration);
        }
		public static QTween QShake(this Transform transform, float duration, float scale)
		{
			return transform.QShake(duration, Vector3.one * scale);
		}
		public static QTween QShake(this Transform transform, float duration, Vector3 scale)
		{
			var startPosition = transform.position;
			return QTweenManager.Delay(duration).OnUpdate(() =>
			{
				var offset = Random.insideUnitSphere;
				transform.position = startPosition + new Vector3(offset.x * scale.x, offset.y * scale.y, offset.z * scale.z);
			}).OnComplete(() =>
			{
				transform.position = startPosition;
			});
		}
		public static QTween<float> QMoveX(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.position.x,
            (setValue) => { transform.position = new Vector3(setValue, transform.position.y, transform.position.z); },
            value, duration);
        }

        public static QTween<float> QMoveY(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.position.y,
            (setValue) => { transform.position = new Vector3(transform.position.x, setValue, transform.position.z); },
            value, duration);
        }
        public static QTween<float> QMoveZ(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.position.z,
            (setValue) => { transform.position = new Vector3(transform.position.x, transform.position.y, setValue); },
            value, duration);
        }
        public static QTween<float> QLocalMoveX(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.localPosition.x,
            (setValue) => { transform.localPosition = new Vector3(setValue, transform.localPosition.y, transform.localPosition.z); },
           value, duration);
        }

        public static QTween<float> QLocalMoveY(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.localPosition.y,
            (setValue) => { transform.localPosition = new Vector3(transform.localPosition.x, setValue, transform.localPosition.z); },
            value, duration);
        }
        public static QTween<float> QLocalMoveZ(this Transform transform, float value, float duration)
        {
            return QTweenManager.Tween(() => transform.localPosition.z,
            (setValue) => { transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, setValue); },
            value, duration);
        }

        public static QTween<Vector3> QLocalMove(this Transform transform, Vector3 postion, float duration)
        {
            return QTweenManager.Tween(() => transform.localPosition,
            (pos) => { transform.localPosition = pos; },
             postion, duration);
        }
        public static QTween<Vector3> QScale(this Transform transform, Vector3 endScale, float duration)
        {
            return QTweenManager.Tween(() => transform.localScale,
            (scale) => { transform.localScale = scale; },
            endScale, duration);
        }
        public static QTween<Vector3> QScale(this Transform transform, float endScale, float duration)
        {
            return QScale(transform, Vector3.one * endScale, duration);
        }
    }
    public static class RectTransformExtends
    {
        public static QTween<Vector2> QAnchorPosition(this RectTransform transform, Vector2 postion, float duration)
        {
            return QTweenManager.Tween(() => transform.anchoredPosition,
            (pos) => { transform.anchoredPosition = pos; },
            postion, duration);
        }
    }
}
