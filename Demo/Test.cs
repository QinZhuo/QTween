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
    public const int count = 1;
    GameObject[,] objs = new GameObject[count, count];
    QTweenBase[,] tweens = new QTweenBase[count, count];
    private void Start()
    {
       // //QTween.DelayInvoke(2, () =>
     //    {
             for (int i = 0; i < count; i++)
             {
                 for (int j = 0; j < count; j++)
                 {
                     objs[i, j] = Instantiate(cubePrefab, new Vector3(i * 2, 0, j * 2), Quaternion.identity);
                    
                 }
             }
       //  });
       
        //.Next(
        //    transform.ScaleTo(Vector3.one * 2, 1).SetCurve(Curve.back.Out()).IgnoreTimeScale()
        //    );
       Time.timeScale = 1;
    }

    
    private void Update()
    {
       // objs[0, 0].transform.rotation = Quaternion.Lerp(objs[0, 0].transform.rotation, Quaternion.Euler(90, 46, 789), Time.deltaTime);
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
                    .Next(objs[i, j].transform.QRotate(to ? new Vector3(70, 167, 34) : new Vector3(546, 4, 6), 1)).AutoDestory(false)
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
    [ContextMenu("暂停")]
    public void DoTween()
    {
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {

                tweens[i, j].Pause();
            }
        }
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
