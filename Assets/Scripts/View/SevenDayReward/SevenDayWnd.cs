//====================
//作者：鲁家旗
//日期：2016/4/29
//用途：七天奖励界面
//====================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SevenDayWnd : GUIBase
{
    #region 控件
    public UIButton closeBtn;
    public UIButton topUpBtn;
    public UIButton vipBtn;

    protected List<SevenDaysRef> list = null;
    public GameObject sevendayItem;
    public UIGrid uiGird;
    protected List<SevendayItem> sevendayItemlist = new List<SevendayItem>();
    //protected bool isCreate = false;
    public ItemUI[] showItem = new ItemUI[3];
    #endregion

    void Awake()
    {
        mutualExclusion = true;
        Layer = GUIZLayer.NORMALWINDOW;
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate
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
        };
        if (vipBtn != null) UIEventListener.Get(vipBtn.gameObject).onClick = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.VIP);
        };
        if (topUpBtn != null) UIEventListener.Get(topUpBtn.gameObject).onClick = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
        };
        list = ConfigMng.Instance.GetSevendayRefTable(GameCenter.mainPlayerMng.MainPlayerInfo.Prof);
        sevendayItem.SetActive(false);
    }
    //void Update()
    //{
    //    if (isCreate)
    //    {
    //        CreateItem();
    //        isCreate = false;
    //    }
    //}
    protected override void OnOpen()
    {
        base.OnOpen();
        //isCreate = true;
        CreateItem();
        GameCenter.sevenDayMng.OnRewardChange += Refresh;
    }
    protected override void OnClose()
    {
        base.OnClose();
        //isCreate = false;
        GameCenter.sevenDayMng.OnRewardChange -= Refresh;
    }
    void CreateItem()
    {
        //标题栏添加重要展示物品
        if (list.Count > 0)
        {
            List<int> itemIdlist = list[0].itemIdList;
            for (int i = 0; i < showItem.Length; i++)
            {
                if (i < itemIdlist.Count)
                    showItem[i].FillInfo(new EquipmentInfo(itemIdlist[i],EquipmentBelongTo.PREVIEW));
            }
        }
        if (uiGird != null) uiGird.maxPerLine = list.Count;
        for (int i = 0; i < list.Count; i++)
        {
            GameObject item = Instantiate(sevendayItem) as GameObject;
            item.transform.parent = sevendayItem.transform.parent;
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one;
            SevendayItem sevendItem = item.GetComponent<SevendayItem>();
            if (sevendItem != null)
            {
                sevendayItemlist.Add(sevendItem);
                sevendItem.SetItem(list[i]);
            }
            item.SetActive(true);
        }
        if (uiGird != null) uiGird.repositionNow = true;
    }
    void Refresh()
    {
        for (int i = 0; i < sevendayItemlist.Count; i++)
        {
            sevendayItemlist[i].SetItem(list[i]);
        }
    }
}
