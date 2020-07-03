using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using QTool.Tween;
using UnityEngine.UI;
public class Test : MonoBehaviour
{
    public GameObject cube;
    public MaskableGraphic graphics;
    public string code { set; get; }
    public Vector3 endPos = Vector3.right *3;
    QTween tween;
    private void Start()
    {
      
        //.Next(
        //    transform.ScaleTo(Vector3.one * 2, 1).SetCurve(Curve.back.Out()).IgnoreTimeScale()
        //    );
       Time.timeScale = 1;
    }

    
    private void Update()
    {
       
    }
    public void Run()
    {
        if (tween == null)
        {
            tween = QTween.CreateQueue().PushEnd(transform.LocalMove(endPos, 1).SetCurve(Curve.back.Out()))
                .PushEnd(transform.ScaleTo(Vector3.one * 2, 1).SetCurve(Curve.back.Out())).Insert(0.5f, graphics.ColorTo(Color.clear, 1f)).AutoStop(false).IgnoreTimeScale().Play();
        }
        else
        {
            tween.Play(!tween.playBack);
        }
        var a = 20f;
        Debug.LogError((Mathf.PI/2f).ToString());
    }
 
}
