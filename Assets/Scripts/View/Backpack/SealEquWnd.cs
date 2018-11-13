//======================================================
//作者:鲁家旗
//日期:2016/10/26
//用途:深渊封印
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SealEquWnd : SubWnd
{
    #region 数据
    public UILabel[] bindLabel;
    public UILabel goldLabel;
    public UILabel consumeLabel;
    public UIButton bindBtn;
    public UIButton cancelBindBtn;
    public UIToggle quickTog;
    public GameObject goldGo;
    public GameObject shenGo;

    public UIPanel itemsPanel;
    public ItemUI curChooseItem;
    public UIToggle toggleEquip;
    public UIToggle toggleBag;
    public Vector4 positionInfo = new Vector4(0, 0, 0, 90);
    protected Dictionary<int, EquipmentInfo> tempItems = new Dictionary<int, EquipmentInfo>();
    protected Dictionary<int, GameObject> allGoItems = new Dictionary<int, GameObject>();
    protected EquipmentInfo curEquInfo;
    protected EquipmentInfo consumeInfo;
    #endregion    
    
    protected override void OnOpen()
    {
        base.OnOpen();
        toggleEquip.value = true;
        consumeInfo = new EquipmentInfo(4000013, EquipmentBelongTo.PREVIEW);
        GetEquipFromEqu();
        ShowItems();
        curEquInfo = null;
        RefreshLeft(curEquInfo);
        if (bindBtn != null) UIEventListener.Get(bindBtn.gameObject).onClick = delegate
          {
              if (GameCenter.inventoryMng.GetNumberByType(4000013) >= 1)
              {
                  if (curEquInfo != null)
                      GameCenter.inventoryMng.C2S_AskSealEqu(curEquInfo.InstanceID, 1);
              }
              else
              {
                  if (quickTog.value)
                  { 
                    //TODO快捷购买
                  }
                  else
                    GameCenter.messageMng.AddClientMsg(141);
              }
          };
        if (cancelBindBtn != null) UIEventListener.Get(cancelBindBtn.gameObject).onClick = delegate
        {
            if (GameCenter.inventoryMng.GetNumberByType(4000013) >= 1)
            {
                if (curEquInfo != null)
                    GameCenter.inventoryMng.C2S_AskSealEqu(curEquInfo.InstanceID, 2);
            }
            else
            {
                if (quickTog.value)
                {
                    //TODO快捷购买
                }
                else
                    GameCenter.messageMng.AddClientMsg(141);
            }
        };
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
            if (toggleEquip != null) UIEventListener.Get(toggleEquip.gameObject).onClick = OnChangeEquipList;
            if (toggleBag != null) UIEventListener.Get(toggleBag.gameObject).onClick = OnChangeEquipList;
            if (quickTog != null) EventDelegate.Add(quickTog.onChange, OnChangeQuickTog);
            GameCenter.inventoryMng.OnGetSealEquResult += RefreshLeft;
            if (shenGo != null) UIEventListener.Get(shenGo).onClick = delegate {
                ToolTipMng.ShowEquipmentTooltip(consumeInfo, ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None);
            };
            
        }
        else
        {
            if (toggleEquip != null) UIEventListener.Get(toggleEquip.gameObject).onClick = null;
            if (toggleBag != null) UIEventListener.Get(toggleBag.gameObject).onClick = null;
            if (quickTog != null) EventDelegate.Remove(quickTog.onChange, OnChangeQuickTog);
            GameCenter.inventoryMng.OnGetSealEquResult -= RefreshLeft;
            if (shenGo != null) UIEventListener.Get(shenGo).onClick = null;
        }
    }
    /// <summary>
    /// 是否快捷购买
    /// </summary>
    void OnChangeQuickTog()
    {
        goldGo.SetActive(quickTog.value);
    }
    /// <summary>
    /// 切换身上和背包按钮
    /// </summary>
    void OnChangeEquipList(GameObject go)
    {
        itemsPanel.GetComponent<UIScrollView>().ResetPosition();
        if (toggleEquip.value)
        {
            GetEquipFromEqu();
        }
        else if (toggleBag.value)
        {
            GetEquipFromBag();
        }
        ShowItems();
    }
    /// <summary>
    /// 获得身上的所有装备
    /// </summary>
    void GetEquipFromEqu()
    {
        tempItems.Clear();
        Dictionary<int, EquipmentInfo> backDic = GameCenter.inventoryMng.GetPlayerEquipList();
        using (var e = backDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                EquipmentInfo info = e.Current.Value;
                if (info.Slot != EquipSlot.None && info.StackCurCount != 0)
                    tempItems[info.InstanceID] = info;
            }
        }
    }
    /// <summary>
    /// 获得背包中的所有装备
    /// </summary>
    void GetEquipFromBag()
    {
        tempItems.Clear();
        Dictionary<int, EquipmentInfo> backDic = GameCenter.inventoryMng.GetBackpackEquipDic();
        using (var e = backDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                EquipmentInfo info = e.Current.Value;
                if (info.Slot != EquipSlot.None && info.StackCurCount != 0)
                    tempItems[info.InstanceID] = info;
            }
        }
    }
    #region 右侧逻辑
    /// <summary>
    /// 显示右边的装备信息
    /// </summary>
    void ShowItems()
    {
        HideAllGoItems();
        int index = 0;
        using (var e = tempItems.GetEnumerator())
        {
            while (e.MoveNext())
            {
                EquipmentInfo info = e.Current.Value;
                if (info == null) continue;
                GameObject go = null;
                if (!allGoItems.ContainsKey(info.InstanceID))
                {
                    go = Instantiate(exResources.GetResource(ResourceType.GUI, "Backpack/DecomposeItem")) as GameObject;
                    allGoItems[info.InstanceID] = go;
                }
                go = allGoItems[info.InstanceID];
                go.SetActive(true);
                go.transform.parent = itemsPanel.transform;
                go.transform.localPosition = new Vector3(positionInfo.x, positionInfo.y - positionInfo.w * index, -1);
                go.transform.localScale = Vector3.one;
                DecomposeItemUI bagItemUI = go.GetComponent<DecomposeItemUI>();
                go = null;
                bagItemUI.SetData(info, ChooseBagItem);
                index++;
            }
        }
    }
    void HideAllGoItems()
    {
        using (var e = allGoItems.GetEnumerator())
        {
            while (e.MoveNext())
            {
                GameObject go = e.Current.Value;
                if (go != null) go.SetActive(false);
            }
        }
    }
    void ChooseBagItem(GameObject go)
    {
        curEquInfo  = (EquipmentInfo)UIEventListener.Get(go).parameter;
        RefreshLeft(curEquInfo);
    }
    #endregion

    #region 左侧逻辑
    
    /// <summary>
    /// 刷新左边的信息
    /// </summary>
    void RefreshLeft(EquipmentInfo _info)
    {
        if (_info != null) curEquInfo = _info;
        if (curEquInfo != null)
            curChooseItem.FillInfo(new EquipmentInfo(curEquInfo, EquipmentBelongTo.PREVIEW));
        else
            curChooseItem.FillInfo(null);
        if (curEquInfo != null)
        {
            if (bindLabel[0] != null) bindLabel[0].gameObject.SetActive(curEquInfo.IsBind);
            if (bindLabel[1] != null) bindLabel[1].gameObject.SetActive(!curEquInfo.IsBind);
            if (bindBtn != null) bindBtn.gameObject.SetActive(!curEquInfo.IsBind);
            if (cancelBindBtn != null) cancelBindBtn.gameObject.SetActive(curEquInfo.IsBind);
        }
        else
        {
            if (bindLabel[0] != null) bindLabel[0].gameObject.SetActive(false);
            if (bindLabel[1] != null) bindLabel[1].gameObject.SetActive(false);
        }
        int num = GameCenter.inventoryMng.GetNumberByType(4000013);
        if (consumeLabel != null) consumeLabel.text = "1/" + (num >= 1 ? num.ToString() :"[ff0000]" + num);
        if (goldLabel != null) goldLabel.text = consumeInfo.DiamondPrice + "/" + GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount;
    }
    #endregion
}
