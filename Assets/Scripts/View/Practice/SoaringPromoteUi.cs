//==================================
//作者：朱素云
//日期：2016/4/10
//用途：飞升提升ui
//=================================
using UnityEngine;
using System.Collections;

public class SoaringPromoteUi : MonoBehaviour
{
    private int needVal = 0;
    private int lev = 0;
    public UILabel needReiki;
    public UIButton promote; 
    public UILabel needDust; 
    public UIButton breakOut;
    public UILabel needLev;

    protected MainPlayerInfo MainData
    {
        get
        {
            return GameCenter.mainPlayerMng.MainPlayerInfo;
        }
    } 

    public void Show(int _needReiki, bool _isDust , int _needlev = 0)
    {
        needVal = _needReiki;  
        if (_needlev != 0)
        {
            lev = _needlev;
            if (MainData.CurLevel < lev)
            {
                if (needLev != null) needLev.text = "[ff0000]" + ConfigMng.Instance.GetLevelDes(_needlev);
            }
            else
            {
                if (needLev != null) needLev.text = "[6ef574]" + ConfigMng.Instance.GetLevelDes(_needlev);
            }
            
        }
        if (_isDust)//突破，显示仙气
        {
            if (needDust != null)
            {
                if (MainData.HighFlyUpRes >= needVal)
                {
                    needDust.text =needVal + "/" +  "[6ef574]" + MainData.HighFlyUpRes;
                }
                else
                {
                    needDust.text =needVal + "/" +  "[ff0000]" + MainData.HighFlyUpRes;
                }
            }
        }
        else
        {
            if (needReiki != null)
            {
                if (MainData.LowFlyUpRes >= needVal)
                {
                    needReiki.text =needVal + "/" +  "[6ef574]" + MainData.LowFlyUpRes;
                }
                else
                {
                    needReiki.text =needVal + "/" +  "[ff0000]" + MainData.LowFlyUpRes;
                }
            }
        }
        if (promote != null && promote.gameObject.activeSelf) UIEventListener.Get(promote.gameObject).onClick = OnClickPromote;
        if (breakOut != null && breakOut.gameObject.activeSelf) UIEventListener.Get(breakOut.gameObject).onClick = OnClickBreak;
    }
    void OnClickPromote(GameObject go)
    {
        if (MainData.LowFlyUpRes >= needVal)
            GameCenter.practiceMng.C2S_ReqUpLev();
        else
            GameCenter.messageMng.AddClientMsg(169);
    }
    void OnClickBreak(GameObject go)
    {
        if (MainData.HighFlyUpRes < needVal)
        {
            GameCenter.messageMng.AddClientMsg(170);  
        }
        else if (MainData.CurLevel < lev)
        {
            GameCenter.messageMng.AddClientMsg(13);
        }
        else
            GameCenter.practiceMng.C2S_ReqUpLev();
    }
}
