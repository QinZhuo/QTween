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
    public Image graphics;
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
        graphics.FillAmountTo(0, 1);
        if (tween == null)
        {
            tween =transform.LocalRotTo(Vector3.one*780, 1).SetCurve( Curve.Back.Out()).IgnoreTimeScale().AutoStop();
           
        }
        else
        {
            tween.Play(!tween.playBack);
        }
    }
 
}
