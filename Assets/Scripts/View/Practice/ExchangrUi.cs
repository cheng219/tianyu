//==================================
//作者：朱素云
//日期：2016/4/12
//用途：仙气兑换UI
//=================================
using UnityEngine;
using System.Collections;
using System;

public class ExchangrUi : MonoBehaviour {

    private int dust;
    private int reiki;
    public UILabel dustLab;
    public UILabel reikiLab;
    public UIInput inputDust;
    public UILabel reikiExchange;
    public UIButton exchange;
    public UIButton closeUi;
    private int val = 1;
    protected MainPlayerInfo MainPlayerInfo
    {
        get
        {
            return GameCenter.mainPlayerMng.MainPlayerInfo;
        }
    }
    void OnEnable()
    {
        Show(ActorBaseTag.LowFlyUpRes, 1, false);   
        val = 1;
        reikiExchange.text = "5"; 
        EventDelegate.Add(inputDust.onChange, updateinPuts);
        if (closeUi != null) UIEventListener.Get(closeUi.gameObject).onClick = delegate { gameObject.SetActive(false); };
        if (exchange != null) UIEventListener.Get(exchange.gameObject).onClick += OnClisckexchange;
        if (MainPlayerInfo != null) MainPlayerInfo.OnBaseUpdate += Show;
    }
    void OnDisable()
    {
        EventDelegate.Remove(inputDust.onChange, updateinPuts);
        if (exchange != null) UIEventListener.Get(exchange.gameObject).onClick -= OnClisckexchange;
        if (MainPlayerInfo != null) MainPlayerInfo.OnBaseUpdate -= Show;
    }
    void updateinPuts()
    {
        if (!string.IsNullOrEmpty(inputDust.value))
        {
            string vals;
            if (inputDust.value.Length > 6)
            {
                vals = inputDust.value.Substring(0, 6);
                inputDust.value = vals;
            }
            else
                vals = inputDust.value;
            
            int.TryParse(vals, out val);
            if (val > 0) reikiExchange.text = (val * 5).ToString();
            else
            {
                reikiExchange.text = "5";
            }
        }
    }

	void Show(ActorBaseTag tag, ulong y, bool da)
    {
        if (tag == ActorBaseTag.HighFlyUpRes || tag == ActorBaseTag.LowFlyUpRes)
        {
            dustLab.text = MainPlayerInfo.HighFlyUpRes.ToString();
            reikiLab.text = MainPlayerInfo.LowFlyUpRes.ToString();
        }
    }
    void OnClisckexchange(GameObject go) 
    { 
        if (val > 0)
        {
            if (MainPlayerInfo != null && MainPlayerInfo.LowFlyUpRes >= (val * 5))
                GameCenter.practiceMng.C2S_ReqConvent((uint)val);
            else
                GameCenter.messageMng.AddClientMsg(169);
        }
    }
}
