//===================
//作者：鲁家旗
//日期：2016/4/15
//用途：藏宝库临时仓库
//===================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TemporaryWarehouse : SubWnd
{
    #region 临时仓库
    public BackpackPageUI backpackPageUI;
    //一键取出
    public UIButton takeOutBtn;
    //整理
    public UIButton arrangeBtn;
    #endregion

    void OnEnable()
    {
        if (takeOutBtn != null)
            UIEventListener.Get(takeOutBtn.gameObject).onClick = OnClickTakeOutBtn;
        if (arrangeBtn != null) 
            UIEventListener.Get(arrangeBtn.gameObject).onClick = OnClickArrangeBtn;
    }
    /// <summary>
    /// 打开窗口的时候
    /// </summary>
    protected override void OnOpen()
    {
        base.OnOpen();
        //GameCenter.uIMng.GenGUI(GUIType.BACKPACKWND, true);
        GameCenter.inventoryMng.OpenBackpack(ItemShowUIType.NORMALBAG);
        InitProgress();
    }
    /// <summary>
    /// 关闭窗口的时候
    /// </summary>
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.uIMng.ReleaseGUI(GUIType.BACKPACKWND);
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.treasureHouseMng.OnHouseUpdate += UpdateAllStorgeItems;
            //GameCenter.treasureHouseMng.OnStorageItemUpdate += UpdateStorageItems;
        }
        else
        {
            GameCenter.treasureHouseMng.OnHouseUpdate -= UpdateAllStorgeItems;
            //GameCenter.treasureHouseMng.OnStorageItemUpdate -= UpdateStorageItems;
        }
    }

    #region 控件事件
    /// <summary>
    /// 一键取出
    /// </summary>
    void OnClickTakeOutBtn(GameObject go)
    {
        List<int> idlist = new List<int>();
        foreach (EquipmentInfo info in GameCenter.treasureHouseMng.StorageDictionary.Values)
        {
            idlist.Add(info.InstanceID);
        }
        GameCenter.treasureHouseMng.C2S_ReqTakeOutHouse(idlist,true);
    }
    /// <summary>
    /// 整理临时仓库
    /// </summary>
    void OnClickArrangeBtn(GameObject go)
    {
        GameCenter.treasureHouseMng.C2S_ReqArrageHouse();
    }
    /// <summary>
    /// 整理背包
    /// </summary>
    void OnClickArrangeBakeBtn(GameObject go)
    {
        GameCenter.inventoryMng.C2S_ArrangeBag();
    }
    #endregion

    public int SortEquipment(EquipmentInfo eq1, EquipmentInfo eq2)
    {
        if (eq1.Postion > eq2.Postion)
            return 1;
        if (eq1.Postion < eq2.Postion)
            return -1;
        return 0;
    }

    #region 临时仓库

    void InitProgress()
    {
        List<EquipmentInfo> backpackItems = new List<EquipmentInfo>(GameCenter.treasureHouseMng.RealStorageDictionary.Values);
        backpackItems.Sort(SortEquipment);
        if (backpackPageUI != null) backpackPageUI.Init(1, backpackItems);
    }

    protected void RefreshItems()
    {
        List<EquipmentInfo> backpackItems = new List<EquipmentInfo>(GameCenter.treasureHouseMng.RealStorageDictionary.Values);
        backpackItems.Sort(SortEquipment);
        if (backpackPageUI != null) backpackPageUI.UpdateItems(backpackItems);
    }
    /// <summary>
    /// 刷新所有物品
    /// </summary>
    protected void UpdateAllStorgeItems()
    {
        RefreshItems();
    }
    /// <summary>
    /// 仓库更新单个数据
    /// </summary>
    protected void UpdateStorageItems(int pos, EquipmentInfo eq)
    {
        RefreshItems();
    }
    #endregion
}
