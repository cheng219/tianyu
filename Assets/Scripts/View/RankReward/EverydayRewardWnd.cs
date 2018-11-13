//==================================
//作者：朱素云/鲁家旗
//日期：2016/5/7
//用途：每日奖励界面和礼包领取
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EverydayRewardWnd : GUIBase
{
    public UIToggle[] uiTog;
    private int page = 0;
    public UIButton closeBtn;
   
    #region 每日奖励
    public EverydayRewardUi everydayRewardUi;
    public UIGrid itemParent;
    public GameObject daliyChackMark;
    public GameObject everyday;
    protected List<EverydayRewardUi> everydayRewardUiList = new List<EverydayRewardUi>();
    public FDictionary everydayRewardTable = new FDictionary();
    protected Dictionary<int, int> rewardDic
    {
        get
        {
            return GameCenter.rankRewardMng.rewardDic;
        }
    }
    public GameObject vipRemind;
    public UIButton getVip;
    public UIButton stillTake;
    public UIButton closeRemind;
    public UILabel remindLab;
    #endregion

    #region 礼包领取
    public GameObject giftsReceived;
    public UIGrid giftGird;
    public GameObject giftGo;
    protected List<UIToggle> giftTogList = new List<UIToggle>();
    protected List<CDKeyRef> refList = new List<CDKeyRef>();
    public UILabel giftName;
    public UIGrid rewardGird;
    public GameObject rewardGo;
    public UIInput inputCdKey;
    public UIButton getRewardBtn;
    public UIButton haveGetReward;
    protected FDictionary itemGoDic = new FDictionary();
    protected List<GameObject> itemGolist = new List<GameObject>();
    public FDictionary giftDic = null; 
    #endregion

    void Awake()
    { 
        mutualExclusion = true;
        layer = GUIZLayer.NORMALWINDOW;
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = OnCloseWnd;
        everydayRewardTable = ConfigMng.Instance.GetEverydayRewardTable();

        #region 礼包领取
        giftDic = ConfigMng.Instance.GetCdKeyRefTable();
        giftGo.SetActive(false);
        rewardGo.SetActive(false);
        if (getRewardBtn != null) UIEventListener.Get(getRewardBtn.gameObject).onClick = delegate
        {
            if (inputCdKey.value == string.Empty)
            {
                GameCenter.messageMng.AddClientMsg(175);
            }
            else
            {
                GameCenter.rankRewardMng.C2S_ReqGetGiftReward(inputCdKey.value);
            }
        };
        #endregion
    } 
    protected override void OnOpen()
    { 
        base.OnOpen(); 
        Show(); 
        GameCenter.rankRewardMng.OnRewardUpdate += Show;
        for (int i = 0; i < uiTog.Length; i++)
        {
            if (uiTog[i] != null)
            {
                EventDelegate.Add(uiTog[i].onChange, OnChange);
            }
        }
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.rankRewardMng.OnRewardUpdate -= Show;
        for (int i = 0; i < uiTog.Length; i++)
        {
            if (uiTog[i] != null)
            {
                EventDelegate.Remove(uiTog[i].onChange, OnChange);
            }
        } 
    }
    void OnCloseWnd(GameObject go)
    {
        if (GameCenter.openServerRewardMng.isOpenFirstRecharge && GameCenter.openServerRewardMng.reminTime > 0)
        {
            GameCenter.openServerRewardMng.isOpenFirstRecharge = false;
            GameCenter.uIMng.SwitchToUI(GUIType.DAILYFIRSTRECHARGE);
        }
        else
        {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        }
    }
    void OnClickTakeReward(GameObject go)
    {
        int vipLev = GameCenter.vipMng.VipData.vLev;
        EverydayRewardRef reward = (EverydayRewardRef)UIEventListener.Get(go.gameObject).parameter;
        if (reward != null)
        {
            if (reward.id == GameCenter.rankRewardMng.rewardDay)
            {
                if (!rewardDic.ContainsKey(GameCenter.rankRewardMng.rewardDay))//如果当天的还没有领取
                {
                    if (vipRemind != null && vipLev < reward.vip)//如果当前vip等级小于翻倍领取要达到的等级就弹出提示框
                    {
                        vipRemind.SetActive(true);
                        if (getVip != null) UIEventListener.Get(getVip.gameObject).onClick = delegate
                          { 
                              GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                          };
                        if (stillTake != null) UIEventListener.Get(stillTake.gameObject).onClick = delegate
                          {
                              GameCenter.rankRewardMng.C2S_ReqTakeEverydayReward(reward.id);
                          };
                        if (closeRemind != null) UIEventListener.Get(closeRemind.gameObject).onClick = delegate
                          {
                              vipRemind.SetActive(false);
                          };
                        if (remindLab != null) remindLab.text = ConfigMng.Instance.GetUItext(65, new string[2] { vipLev.ToString(), reward.vip.ToString() });
                    }
                    else
                        GameCenter.rankRewardMng.C2S_ReqTakeEverydayReward(reward.id);
                }
            }
        }
    } 

    void OnChange()
    {
        for (int i = 0; i < uiTog.Length; i++)
        {
            if (uiTog[i] != null && uiTog[i].value)
            {
                page = i; 
                break;
            }
        }
        Show();
    }

    void Show()
    {
        int curId = GameCenter.rankRewardMng.rewardDay;
        vipRemind.SetActive(false);
        everyday.SetActive(false);
        giftsReceived.SetActive(false);
        if (0 == page)//每日签到
        { 
            everyday.SetActive(true);
            if (rewardDic.ContainsKey(curId))
            {
                if (daliyChackMark != null) daliyChackMark.SetActive(false); 
            }
            else
            {
                if (daliyChackMark != null) daliyChackMark.SetActive(true); 
            }
            for (int j = 0; j < everydayRewardUiList.Count; j++)
            {
                everydayRewardUiList[j].gameObject.SetActive(false);
            }
            int i = 0;
            foreach (EverydayRewardRef data in everydayRewardTable.Values)
            {
                if (data.id > 0)
                {
                    if (data.id == 1)
                    {
                        everydayRewardUi.EverydayRewardRef = data;
                        everydayRewardUiList.Add(everydayRewardUi);
                    }
                    else
                    {
                        if (everydayRewardUiList.Count < i + 1)
                        {
                            everydayRewardUiList.Add(everydayRewardUi.CreateNew(itemParent.transform, i));
                        }
                        if (everydayRewardUiList.Count > i) everydayRewardUiList[i].EverydayRewardRef = data;
                    }
                    everydayRewardUiList[i].gameObject.SetActive(true); 

                    if (!rewardDic.ContainsKey(data.id))//判断是否领取
                    {
                        if (data.id == curId)
                        {
                            if (everydayRewardUiList[i].item != null && !everydayRewardUiList[i].item.ShowTooltip)//如果今天的没有领取点击热感领取奖励
                            {
                                UIEventListener.Get(everydayRewardUiList[i].item.gameObject).onClick -= OnClickTakeReward;
                                UIEventListener.Get(everydayRewardUiList[i].item.gameObject).onClick += OnClickTakeReward;
                                UIEventListener.Get(everydayRewardUiList[i].item.gameObject).parameter = everydayRewardUiList[i].EverydayRewardRef;
                            }
                        }

                    }
                    if (everydayRewardUiList[i].EverydayRewardRef != null)
                    {
                        UIEventListener.Get(everydayRewardUiList[i].gameObject).onClick -= OnClickTakeReward;
                        UIEventListener.Get(everydayRewardUiList[i].gameObject).onClick += OnClickTakeReward;
                        UIEventListener.Get(everydayRewardUiList[i].gameObject).parameter = everydayRewardUiList[i].EverydayRewardRef;
                    }
                    i++;
                }
                if (itemParent != null) itemParent.repositionNow = true;
            } 
        }
        else//领取礼包
        {
            if (giftDic == null) return;
            giftsReceived.SetActive(true);
            HideAllGift();
            giftGird.maxPerLine = giftDic.Count;
            int index = 0;
            giftTogList.Clear();
            refList.Clear();
            foreach (CDKeyRef data in giftDic.Values)//动态创建礼包
            {
                GameObject go = null;
                if (!itemGoDic.ContainsKey(data.id))
                {
                    go = Instantiate(giftGo) as GameObject;
                    itemGoDic[data.id] = go;
                }
                go = itemGoDic[data.id] as GameObject;
                go.transform.parent = giftGo.transform.parent;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                GiftItemUI giftItemUI = go.GetComponent<GiftItemUI>();
                if (giftItemUI != null)
                    giftItemUI.RefreshGift(data);
                if (index == 0) go.GetComponent<UIToggle>().startsActive = true;
                giftTogList.Add(go.GetComponent<UIToggle>());
                EventDelegate.Remove(go.GetComponent<UIToggle>().onChange, OnChangeToggle);
                EventDelegate.Add(go.GetComponent<UIToggle>().onChange, OnChangeToggle);
                refList.Add(data);
                go.SetActive(true);
                index ++;
            }
            giftGird.repositionNow = true;
        }
    }
    #region 礼包领取
    void HideAllGift()
    {
        foreach (GameObject go in itemGoDic.Values)
        {
            if(go != null)go.SetActive(false);
        }
    }
    void HideAllItem()
    {
        for (int i = 0; i < itemGolist.Count; i++)
        {
            if (itemGolist[i] != null) itemGolist[i].SetActive(false);
        }
    }
    void OnChangeToggle()
    {
        for (int i = 0; i < refList.Count; i++)
        {
            if (giftTogList[i].value)
            {
                rewardGird.maxPerLine = refList[i].item.Count;
                HideAllItem();
                for (int j = 0; j < refList[i].item.Count; j++)//动态创建礼包里的奖品
                {
                    GameObject item = null;
                    if (refList[i].item.Count > itemGolist.Count)
                    {
                        item = Instantiate(rewardGo) as GameObject;
                        itemGolist.Add(item);
                    }
                    item = itemGolist[j];
                    item.transform.parent = rewardGo.transform.parent;
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localScale = Vector3.one;
                    ItemUI itemUI = item.GetComponent<ItemUI>();
                    if(itemUI != null)
                        itemUI.FillInfo(new EquipmentInfo(refList[i].item[j].eid, refList[i].item[j].count, EquipmentBelongTo.PREVIEW));
                    item.SetActive(true);
                }
            }
            rewardGird.repositionNow = true;
        }
    }
    #endregion
}
