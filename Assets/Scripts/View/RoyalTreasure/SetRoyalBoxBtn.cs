//======================================================
//作者:鲁家旗
//日期:2017/1/22
//用途:设置宝箱红点和倒计时
//======================================================
using UnityEngine;
using System.Collections;

public class SetRoyalBoxBtn : MonoBehaviour {

    public UITimer timer;
    public UISprite spRed;
    protected bool isShowRed = false;
    void OnEnable()
    {
        Refresh();
        GameCenter.royalTreasureMng.OnRoyalBoxUpdate += Refresh;
    }
    void OnDisable()
    {
        GameCenter.royalTreasureMng.OnRoyalBoxUpdate -= Refresh;
    }
    void Refresh()
    {
        if (timer != null) timer.gameObject.SetActive(false);
        if (spRed != null) spRed.gameObject.SetActive(false);
        isShowRed = false;
        foreach (RoyalTreaureData data in GameCenter.royalTreasureMng.royalTreasureDic.Values)
        {
            if (!data.curState && data.restTime != 0)//宝箱在倒计时
            {
                if (timer != null)
                {
                    timer.gameObject.SetActive(true);
                    timer.StartIntervalTimer(data.restTime);
                    timer.onTimeOut = delegate
                    {
                        timer.gameObject.SetActive(false);
                        if (spRed != null) spRed.gameObject.SetActive(true);
                    };
                }
            }
            if (!data.curState && data.restTime == 0)//宝箱未领取
            {
                isShowRed = true;
            }
        }
        if (GameCenter.royalTreasureMng.royalTreasureDic.Count != 0)//新需求 当前有宝箱并且都未开启的情况下也显示红点
        {
            if (!GameCenter.royalTreasureMng.isOpeningBox)
            {
                //Debug.Log("有宝箱但是都没有开启");
                if (spRed != null) spRed.gameObject.SetActive(true);
            }
        }
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ROYALBOX, isShowRed);
    }
}
