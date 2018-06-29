using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 计时器,挂Timer上
/// </summary>
public class CountDown : MonoBehaviour
{
    public int totalTime; //总时长
    TextMesh tm; 
    void Start()
    {
        tm = GetComponent<TextMesh>();
        InvokeRepeating("Diminishing", 1, 1); //每秒时间减少1
    }
    void Update()
    {
        TimeSpan ts = new TimeSpan(0, 0, totalTime); //实时更新时间

        //一小时以上显示时分秒,否则只显示分秒
        if (totalTime >= 3600)
            tm.text = (int)ts.TotalHours + ":" + string.Format("{0:D2}", ts.Minutes) + ":" + string.Format("{0:D2}", ts.Seconds);
        else 
            tm.text = string.Format("{0:D2}", ts.Minutes) + ":" + string.Format("{0:D2}", ts.Seconds);

        //60秒以上是绿色,否则变红
        if (totalTime >=60) 
            tm.color = new Color(0, 1, 0);
        else
            tm.color = new Color(1, 0, 0);
    }
    void Diminishing() //时间减少1
    {
        if (totalTime > 0)
            totalTime--;
        else
        {
            FindObjectOfType<Character>().Death(); //调用死亡方法
            CancelInvoke();//停止全部延迟调用
        }
    }
}
