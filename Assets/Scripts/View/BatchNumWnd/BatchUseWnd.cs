//======================================================
//作者:朱素云
//日期:2017/1/23
//用途:批量使用界面
//======================================================
using UnityEngine;
using System.Collections;

public class BatchUseWnd : GUIBase
{

    public GameObject closeBtn;
    public GameObject add;
    public GameObject cut;
    public GameObject maxBtn;
    public GameObject sureUseBtn;
    public ItemUI itemUI;
    public UILabel itemName;
    public UIInput input;
    int num = 1;
    public UILabel itemDes;

    void Awake()
    {

    }
    protected override void OnOpen()
    {
        base.OnOpen();
        if (GameCenter.mercenaryMng.seleteEggToMix != null)
        {
            itemUI.FillInfo(new EquipmentInfo(GameCenter.mercenaryMng.seleteEggToMix.EID, EquipmentBelongTo.PREVIEW));
            itemDes.text = GameCenter.mercenaryMng.seleteEggToMix.Description.Replace("\\n", "\n");
        }
        if (input != null) input.value = "1";
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
            if (input != null) EventDelegate.Add(input.onChange, UpdateInput);
            if (closeBtn != null) UIEventListener.Get(closeBtn).onClick += CloseThis;
            if (add != null) UIEventListener.Get(add).onClick += AddNum;
            if (cut != null) UIEventListener.Get(cut).onClick += CutNum;
            if (maxBtn != null) UIEventListener.Get(maxBtn).onClick += AddMaxNum;
            if (sureUseBtn != null) UIEventListener.Get(sureUseBtn).onClick += UseItem;
        }
        else
        {
            if (input != null) EventDelegate.Remove(input.onChange, UpdateInput);
            if (closeBtn != null) UIEventListener.Get(closeBtn).onClick -= CloseThis;
            if (add != null) UIEventListener.Get(add).onClick -= AddNum;
            if (cut != null) UIEventListener.Get(cut).onClick -= CutNum;
            if (maxBtn != null) UIEventListener.Get(maxBtn).onClick -= AddMaxNum;
            if (sureUseBtn != null) UIEventListener.Get(sureUseBtn).onClick -= UseItem;

        }
    }

    /// <summary>
    /// 刷新数量
    /// </summary>
    void RefreshNum()
    {
        if (itemName != null && GameCenter.mercenaryMng.seleteEggToMix != null)
            itemName.text = GameCenter.mercenaryMng.seleteEggToMix.ItemName;
        if (itemUI != null && GameCenter.mercenaryMng.seleteEggToMix != null)
            itemUI.FillInfo(new EquipmentInfo(GameCenter.mercenaryMng.seleteEggToMix.EID, EquipmentBelongTo.PREVIEW));

    }
    void CloseThis(GameObject obj)
    {
        GameCenter.uIMng.CloseGUI(GUIType.BATCHUSE);
    }
    void UpdateInput()
    {
        if (input == null) return;
        int inputNum = 0;
        EquipmentInfo _equip = GameCenter.mercenaryMng.seleteEggToMix;
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
        if (input != null) input.value = GameCenter.mercenaryMng.seleteEggToMix.StackCurCount.ToString();
    }

    /// <summary>
    /// 确定使用
    /// </summary>
    /// <param name="go"></param>
    void UseItem(GameObject go)
    {
        switch (GameCenter.uIMng.CurOpenType)
        {
            case GUIType.SPRITEANIMAL:
                if (GameCenter.mercenaryMng.curUseEggPetId > 0 && GameCenter.mercenaryMng.seleteEggToMix != null)
                {
                    GameCenter.mercenaryMng.C2S_ReqUseEgg(GameCenter.mercenaryMng.curUseEggPetId, GameCenter.mercenaryMng.seleteEggToMix.InstanceID, num);
                }
                break;
            default:
                break;
        }
        GameCenter.uIMng.CloseGUI(GUIType.BATCHUSE);
    }
}
