using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using QTool.Tween;
public class Test : MonoBehaviour
{
    public GameObject cube;
    public string code { set; get; }
    public Vector3 endPos = Vector3.right *3;
    private void Start()
    {
        transform.TweenMove(endPos,1);
    }
    public float t=0;

    
    private void Update()
    {
       
    }
    public void Run()
    {
        var a = 20f;
        Debug.LogError((Mathf.PI/2f).ToString());
    }
 
}
