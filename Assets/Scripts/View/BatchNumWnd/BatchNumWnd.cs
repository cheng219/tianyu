//==================================
//作者：黄洪兴
//日期：2016/6/23
//用途：数量选择面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BatchNumWnd : GUIBase
{
    public GameObject closeBtn;
    public GameObject add;
    public GameObject cut;
    public GameObject maxBtn;
    public GameObject confirmBtn;
    public ItemUI itemUI;
    public UILabel itemName;
    public UIInput input;
    int num=1;
    void Awake()
    {

    }
    protected override void OnOpen()
    {
        base.OnOpen();
        if (GameCenter.tradeMng.CurTradeItemEQ!= null)
            itemUI.FillInfo(new EquipmentInfo(GameCenter.tradeMng.CurTradeItemEQ.EID,EquipmentBelongTo.PREVIEW));
        if (input!=null)input.value = "1";
        num = 1;
        RefreshNum();
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
            if (input != null)EventDelegate.Add(input.onChange, UpdateInput);
            if (closeBtn != null) UIEventListener.Get(closeBtn).onClick += CloseThis;
            if (add != null) UIEventListener.Get(add).onClick += AddNum;
            if (cut != null) UIEventListener.Get(cut).onClick += CutNum;
            if (maxBtn != null) UIEventListener.Get(maxBtn).onClick += AddMaxNum;
            if (confirmBtn != null) UIEventListener.Get(confirmBtn).onClick += BuyItem;
        }
        else
        {
            if (input != null) EventDelegate.Remove(input.onChange, UpdateInput);
            if (closeBtn != null) UIEventListener.Get(closeBtn).onClick -= CloseThis;
            if (add != null) UIEventListener.Get(add).onClick -= AddNum;
            if (cut != null) UIEventListener.Get(cut).onClick -= CutNum;
            if (maxBtn != null) UIEventListener.Get(maxBtn).onClick -= AddMaxNum;
            if (confirmBtn != null) UIEventListener.Get(confirmBtn).onClick -= BuyItem;

        }
    }

    /// <summary>
    /// 刷新数量
    /// </summary>
    void RefreshNum()
    {
        if (itemName != null && GameCenter.tradeMng.CurTradeItemEQ!=null)
            itemName.text = GameCenter.tradeMng.CurTradeItemEQ.ItemName;
        if (itemUI != null && GameCenter.tradeMng.CurTradeItemEQ != null)
            itemUI.FillInfo(new EquipmentInfo(GameCenter.tradeMng.CurTradeItemEQ.EID, EquipmentBelongTo.PREVIEW));

    }
    void CloseThis(GameObject obj)
    {
        GameCenter.uIMng.CloseGUI(GUIType.BATCHNUM);
    }
    void UpdateInput()
    {
        if (input == null) return;
        int inputNum = 0;
        EquipmentInfo _equip = GameCenter.tradeMng.CurTradeItemEQ;
        if (int.TryParse(input.value, out inputNum))
        {
            if (inputNum > _equip.StackCurCount)
            {
                inputNum = _equip.StackCurCount;
                input.value = _equip.StackCurCount.ToString();
            }
            if (inputNum <= 0)
            {
                inputNum = 1;
                input.value = "1";
            }
        }
        else
        {
            inputNum = 1;
            input.value = "1";
        }
        num = inputNum;
    }
    void AddNum(GameObject obj)
    {
        int inputNum = 0;
        if (input != null && int.TryParse(input.value, out inputNum))
        {
            input.value = (inputNum + 1).ToString();
        }
    }
    void CutNum(GameObject obj)
    {
        int inputNum = 0;
        if (input != null && int.TryParse(input.value, out inputNum))
        {
            input.value = (inputNum - 1).ToString();
        }
    }
    void AddMaxNum(GameObject obj)
    {
        if (input != null) input.value = GameCenter.tradeMng.CurTradeItemEQ.StackCurCount.ToString();
    }
    void BuyItem(GameObject obj)
    {
        switch (GameCenter.uIMng.CurOpenType)
        {
            case GUIType.TRADE:
                GameCenter.tradeMng.AddTradeItem(num);
                break;
            default:
                break;
        }
        GameCenter.uIMng.CloseGUI(GUIType.BATCHNUM);
    }

}
