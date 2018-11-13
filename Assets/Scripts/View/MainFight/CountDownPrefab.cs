//=========================================
//作者：易睿
//日期：2015/2/25
//用途：头顶倒计时预制
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CountDownPrefab : MonoBehaviour
{
    public UILabel numAnim;
    public int maxNum = 10;

    void Awake()
    {
        numAnim.text = "";
    }
    public void SetTime(int _time)
    {
        maxNum = _time;
        StartCountDown();
    }
    void StartCountDown()
    {
        StartCoroutine(StartCountDownTime(maxNum));
    }
    IEnumerator StartCountDownTime(int time)
    {
        numAnim.text = time.ToString();
        yield return new WaitForSeconds(1f);
        time--;
        if (time <= 0)
        {
            numAnim.text = "";
            //StartCountDown();
        }
        else
        {
            StopCoroutine("StartCountDownTime");
            StartCoroutine(StartCountDownTime(time));
        }
    }
}
