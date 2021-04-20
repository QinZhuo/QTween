using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using QTool.Tween;
using UnityEngine.UI;
public class Test : MonoBehaviour
{
    public GameObject cubePrefab;
    public Image graphics;
    public string code { set; get; }
    public Vector3 endPos = Vector3.right *3;
    QTweenBase tween;
    public const int count = 30;
    GameObject[,] objs = new GameObject[count, count];
    QTweenBase[,] tweens = new QTweenBase[count, count];
    private void Start()
    {
        QTween.DelayInvoke(2, () =>
         {
             for (int i = 0; i < count; i++)
             {
                 for (int j = 0; j < count; j++)
                 {
                     objs[i, j] = Instantiate(cubePrefab, new Vector3(i * 2, 0, j * 2), Quaternion.identity);
                 }
             }
         });
       
        //.Next(
        //    transform.ScaleTo(Vector3.one * 2, 1).SetCurve(Curve.back.Out()).IgnoreTimeScale()
        //    );
       Time.timeScale = 1;
    }

    
    private void Update()
    {
       
    }
    public bool to = true;
    [ContextMenu("QTween测试")]
    public void RunTween()
    {
        //graphics.FillAmountTo(0, 1);
        //if (tween == null)
        //{
        //    tween =transform.LocalRotTo(Vector3.one*780, 1).SetCurve( Curve.Back.Out()).IgnoreTimeScale().AutoStop();
           
        //}
        //else
        //{
        //    tween.Play(!tween.playBack);
        //}
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {

                if (tweens[i, j] == null)
                {
                    tweens[i, j] = objs[i, j].transform.QMove(to ? new Vector3(i * 2, 3, j * 2) : new Vector3(i * 2, 0, j * 2), 1).AutoDestory(false)
                    .Next(objs[i, j].transform.QRotate(to ? new Vector3(180, 180, 180) : new Vector3(90, 90, 90), 1)).AutoDestory(false)
                 ;
                }
                else
                {
                    tweens[i, j].Play(to);
                }
            }
        }
        to = !to;
    }
    [ContextMenu("DoTween测试")]
    public void DoTween()
    {
        ////graphics.FillAmountTo(0, 1);
        ////if (tween == null)
        ////{
        ////    tween =transform.LocalRotTo(Vector3.one*780, 1).SetCurve( Curve.Back.Out()).IgnoreTimeScale().AutoStop();

        ////}
        ////else
        ////{
        ////    tween.Play(!tween.playBack);
        ////}.PlayBackwards()
        //for (int i = 0; i < count; i++)
        //{
        //    for (int j = 0; j < count; j++)
        //    {
        //        objs[i, j].transform.DOMove(to ? new Vector3(i * 2, 3, j * 2) : new Vector3(i * 2, 0, j * 2), 1).SetEase(Ease.OutBack).SetAutoKill();
        //    }
        //}
        //to = !to;
    }

}
