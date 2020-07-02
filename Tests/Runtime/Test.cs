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
    private void Start()
    {
       
    }
    public float t=0;

    
    private void Update()
    {
       
    }
    public void Run()
    {
        var a = 20f;
        Debug.LogError((Mathf.PI/2f).ToString());
        graphics.AlphaTo(0, 1);
        transform.LocalMove(endPos, 1).curve = Curve.back.Out();
        transform.ScaleTo(Vector3.one*2, 1).curve = Curve.back.Out();
    }
 
}
