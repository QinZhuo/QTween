using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using QTool.Tween;
//using UnityEngine.AddressableAssets;
using UnityEngine.UI;
public class Test : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject cube;
    public Image graphics;
    public QTween tween;
    public string code { set; get; }
    public Vector3 endPos = Vector3.right *3;
    public const int count = 1;
    GameObject[,] objs = new GameObject[count, count];
    QTween[,] tweens = new QTween[count, count];
    private void Start()
    {
        //Addressables.LoadAssetAsync<GameObject>("Assets/Demo/Cube.prefab").Completed += (loader) =>
        //{
        //    Debug.LogError(loader.Result);
        //};
       // //QTween.DelayInvoke(2, () =>
     //    {
             //for (int i = 0; i < count; i++)
             //{
             //    for (int j = 0; j < count; j++)
             //    {
             //        objs[i, j] = Instantiate(cubePrefab, new Vector3(i * 2, 0, j * 2), Quaternion.identity);
                    
             //    }
             //}
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
        if (tween == null)
        {
            tween =QTweenList.Get().AddLast(  cube.transform.QMove( new Vector3(0, 3, 0) , 1).ResetStart(Vector3.zero))
                    .AddLast(cube.transform.QRotate(new Vector3(70, 167, 34) , 1).ResetStart(Quaternion.identity)).Play(to);
        }
        else
        {
            tween.Play(to);
        }
        //for (int i = 0; i < count; i++)
        //{
        //    for (int j = 0; j < count; j++)
        //    {

        //        if (tweens[i, j] == null)
        //        {
        //            tweens[i, j] = objs[i, j].transform.QMove(to ? new Vector3(i * 2, 3, j * 2) : new Vector3(i * 2, 0, j * 2), 1).AutoDestory(false)
        //            .Next(objs[i, j].transform.QRotate(to ? new Vector3(70, 167, 34) : new Vector3(546, 4, 6), 1)).AutoDestory(false)
        //         ;
        //        }
        //        else
        //        {
        //            tweens[i, j].Play(to);
        //        }
        //    }
        //}
        to = !to;
    }
    [ContextMenu("暂停")]
    public void DoTween()
    {
        //for (int i = 0; i < count; i++)
        //{
        //    for (int j = 0; j < count; j++)
        //    {

        //        tweens[i, j].Pause();
        //    }
        //}
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
