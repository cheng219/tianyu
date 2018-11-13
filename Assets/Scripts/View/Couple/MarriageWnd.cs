//==================================
//作者：朱素云
//日期：2016/5/4
//用途：选择婚礼界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarriageWnd : GUIBase
{
    public UIButton closeBtn;
    public UIButton sureBtn; 
    public UIToggle[] weddingTog;

    protected int tokenId = 0;

    void Awake()
    {
        mutualExclusion = false;
        layer = GUIZLayer.TIP;
        for (int i = 0, len = weddingTog.Length; i < len; i++)
        {
            if (weddingTog[i] != null)
            { 
                EventDelegate.Add(weddingTog[i].onChange, UITogOnChange);
            }
        }
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = OnCloseWnd;
        if (sureBtn != null) UIEventListener.Get(sureBtn.gameObject).onClick = delegate {
            GameCenter.coupleMng.C2S_ReqMarriage(tokenId);
            GameCenter.uIMng.ReleaseGUI(GUIType.MARRIAGE);
        };
    }
	void OnDestroy()
    {
        for (int i = 0, len = weddingTog.Length; i < len; i++)
        {
            if (weddingTog[i] != null)
            {
                EventDelegate.Remove(weddingTog[i].onChange, UITogOnChange);
            }
        }
    }
    protected override void OnOpen()
    { 
        base.OnOpen();
        Show();
        GameCenter.coupleMng.OnMerriageUpdata += Show;
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.coupleMng.OnMerriageUpdata -= Show;
    }
    void OnCloseWnd(GameObject go)
    {
        GameCenter.uIMng.ReleaseGUI(GUIType.MARRIAGE); 
    }

    void UITogOnChange()
    {
        for (int i = 0, len = weddingTog.Length; i < len; i++)
        {
            if (weddingTog[i] != null && weddingTog[i].value)
            {
                MarriageToken marriage = weddingTog[i].GetComponent<MarriageToken>();
                if (marriage != null) tokenId = marriage.WeddingData.token_id;
                break;
            }
        }
    }

    void Show()
    {
        FDictionary info= ConfigMng.Instance.GetWeddingRefTable();  
        int i = 0; 
        foreach(WeddingRef wedding in info.Values)
        { 
            if (weddingTog.Length > i)
            {
                MarriageToken token = weddingTog[i].GetComponent<MarriageToken>();
                if (token != null) token.WeddingData = wedding;
            }
            ++i;
        }  
    }
}
