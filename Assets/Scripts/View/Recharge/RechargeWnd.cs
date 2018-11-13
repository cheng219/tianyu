//==================================
//作者：黄洪兴
//日期：2016/6/18
//用途：充值界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RechargeWnd : GUIBase
{

    public GameObject closeBtn;
    public UILabel VIPLev;
    public RechargeItemUI rechargeItemInstance;
    public UILabel diamoNum;
    public UISlider VIPExpSlider;
    public UILabel VIPExp;
    public GameObject VIPWndBtn;
    public UILabel targetLabel;
   // public UILabel targetVIPNum;
    VIPRef NeeData
    {
        get
        {
            int lev = GameCenter.vipMng.VipData.vLev + 1 > ConfigMng.Instance.GetVIPRefTable().Count ?
                ConfigMng.Instance.GetVIPRefTable().Count : GameCenter.vipMng.VipData.vLev + 1;
            return ConfigMng.Instance.GetVIPRef(lev);
        }
    }


    private Dictionary<int, RechargeRef> rechargeItemRefDic = new Dictionary<int, RechargeRef>();
    //MainPlayerInfo mainPlayerInfo = null;

    void Awake()
    {
        mutualExclusion = true;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        Refresh();
       
    }
    protected override void OnClose()
    {
        base.OnClose();
        
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (closeBtn != null)
                UIEventListener.Get(closeBtn).onClick += CloseThisUI;
            if (VIPWndBtn != null)
                UIEventListener.Get(VIPWndBtn).onClick += delegate { GameCenter.uIMng.SwitchToUI(GUIType.VIP); };
            GameCenter.vipMng.OnVIPDataUpdate += Refresh;
            GameCenter.vipMng.OnRechargeFlageUpdate += RefreshRechargeItem;

        }
        else
        {
            if (closeBtn != null)
                UIEventListener.Get(closeBtn).onClick -= CloseThisUI;
            if (VIPWndBtn != null)
                UIEventListener.Get(VIPWndBtn).onClick -= delegate { GameCenter.uIMng.SwitchToUI(GUIType.VIP); };
            GameCenter.vipMng.OnVIPDataUpdate -= Refresh;
            GameCenter.vipMng.OnRechargeFlageUpdate -= RefreshRechargeItem;
        }
    }



    void Refresh()
    {
        if (diamoNum != null)
        {
            diamoNum.text = GameCenter.mainPlayerMng.MainPlayerInfo.DiamondCountText;
        }


        RefreshRechargeItem();

    }

    void RefreshRechargeItem()
    {
        if (VIPLev != null)
            VIPLev.text = GameCenter.vipMng.VipData.vLev.ToString();
        if (VIPExp != null)
        {
            VIPExp.gameObject.SetActive(true);
                if (targetLabel != null)
                {
                    if (!GameCenter.vipMng.VipData.IsFullLevel)
                    {
                        string[] word = { ((NeeData.exp - GameCenter.vipMng.VipData.vExp) / 10).ToString(), (GameCenter.vipMng.VipData.vLev + 1).ToString() };
                        string s = ConfigMng.Instance.GetUItext(1, word);
                        targetLabel.text = s;
                        VIPExp.text = GameCenter.vipMng.VipData.vExp.ToString() + "/" + NeeData.exp.ToString();
                        if (VIPExpSlider != null)
                            VIPExpSlider.value = (float)GameCenter.vipMng.VipData.vExp / (float)NeeData.exp;
                    }
                    else
                    {
                        targetLabel.text = ConfigMng.Instance.GetUItext(88);
                        VIPExp.gameObject.SetActive(false);
                    }
                }
                //if (targetVIPNum != null)
                //    targetVIPNum.text = "VIP" + (GameCenter.mainPlayerMng.VipData.vLev + 1);
        }

        if (rechargeItemInstance == null||rechargeItemInstance.gameObject==null)
            return;
            Vector3 V3 = Vector3.zero;
            for (int i = 0; i < GameCenter.rechargeMng.RechargeItemList .Count; i++)
            {
                if (!rechargeItemRefDic.ContainsKey(i))
                {
                    GameObject obj = Instantiate(rechargeItemInstance.gameObject) as GameObject;
                    Transform parentTransf = rechargeItemInstance.transform.parent;
                    obj.transform.localPosition = V3;
                    obj.transform.parent = parentTransf;
                    obj.transform.localPosition = V3;
                    obj.transform.localScale = Vector3.one;
                    obj.SetActive(true);
                    V3 = new Vector3(V3.x+291,V3.y,V3.z);
                    if ((i + 1) % 3 == 0)
                    {
                        V3 = new Vector3(0,V3.y-212,V3.z);
                    }
                    RechargeItemUI ItemUI = obj.GetComponent<RechargeItemUI>();
                    if (ItemUI != null)
                    {
                        ItemUI.FillInfo(GameCenter.rechargeMng.RechargeItemList[i]);
                        rechargeItemRefDic[i] = GameCenter.rechargeMng.RechargeItemList[i];
                    }
                }
            }

    }


    void CloseThisUI(GameObject _go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);

    }


}
