//======================================================
//作者:鲁家旗
//日期:2016.7.11
//用途:获得可以使用的的物品UI提示
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EquipTipUI : MonoBehaviour {
    public UILabel nameLabel;
    public ItemUI item;
    public UIButton btnClose;
    public UIButton clickBtn;
    public UILabel btnName;
    //批量使用物品
    public GameObject batchUseGo;
    public UIButton btnBatchUse;
    public UIInput batchUseNum;
    public UILabel labDes;
    public UIButton btnAdd;
    public UIButton btnReduce;
    public UIButton btnMaxNum;
    public ItemUI batchUseItem;
    public UIButton closeBtn;
    protected bool isClose = false;
    protected List<EquipmentInfo> EquipTip
    {
        get
        {
            return GameCenter.inventoryMng.equipTip;
        }
    }
    protected bool isHide = false;
    void Awake()
    {
        isClose = false;
    }
    public void SetEquipTipInfo(EquipmentInfo _info,int _depath,List<GameObject> _list)
    {
        if (batchUseGo != null) batchUseGo.GetComponent<UIPanel>().depth = _depath + 1;
        if (nameLabel != null) nameLabel.text = _info.ItemName;
        if (item != null) item.FillInfo(_info);
        if(btnName != null)
        {
            if (_info.IsEquip || _info.Family == EquipmentFamily.MOUNTEQUIP)
                btnName.text = ConfigMng.Instance.GetUItext(301);
            else
                btnName.text = ConfigMng.Instance.GetUItext(302);
        }
        if (btnClose != null)
            UIEventListener.Get(btnClose.gameObject).onClick = delegate
            {
                isClose = true;
                if (EquipTip.Contains(_info))
                    EquipTip.Remove(_info);
                _list.Remove(this.gameObject);
                //DestroyImmediate(this.gameObject);
            };
        if (clickBtn != null)
            UIEventListener.Get(clickBtn.gameObject).onClick = delegate
            {
                if(_info.Slot != EquipSlot.None)
                    _info.DoItemAction(ItemActionType.NormalLeft);
                else
                    _info.TryToUseAll();//直接批量使用不弹数量选择界面了
                if (EquipTip.Contains(_info))
                    EquipTip.Remove(_info);
                _list.Remove(this.gameObject);
                if (_info.CanUseBatch && _info.StackCurCount > 1)
                {
                    TreasureHouseWnd wnd = GameCenter.uIMng.GetGui<TreasureHouseWnd>();
                    if (wnd != null)
                    {
                        DestroyImmediate(this.gameObject);
                    }
                    else
                    //OpenBatchUseWnd(_info);
                    {
                        DestroyImmediate(this.gameObject);
                        GameCenter.inventoryMng.CurSelectInventory = _info;
                        //GameCenter.uIMng.GenGUI(GUIType.IMMEDIATEUSE,true);
                        GameCenter.inventoryMng.C2S_UseItems(_info, _info.StackCurCount);//直接全部使用

                    }
                }
                else
                    isClose = true;
                    //DestroyImmediate(this.gameObject);
            };
    }

    /// <summary>
    /// 批量使用物品
    /// </summary>
    /// <param name="_eq"></param>
    void OpenBatchUseWnd(EquipmentInfo _eq)
    {
        int useNum = 0;
        batchUseNum.value = "1";
        if (batchUseGo != null && btnBatchUse != null && batchUseNum != null && int.TryParse(batchUseNum.value, out useNum))
        {
            batchUseGo.SetActive(true);
            UIEventListener.Get(btnBatchUse.gameObject).onClick = (x) =>
            {
                if (int.TryParse(batchUseNum.value, out useNum) && useNum > _eq.StackCurCount)
                {
                    GameCenter.messageMng.AddClientMsg(ConfigMng.Instance.GetUItext(293));
                    return;
                }
                GameCenter.inventoryMng.C2S_UseItems(_eq, useNum);
                batchUseGo.SetActive(false);
                DestroyImmediate(this.gameObject);
            };
        }
        if (batchUseItem != null) batchUseItem.FillInfo(_eq);
        if (labDes != null)
        {
            labDes.text = _eq.Description.Replace("\\n", "\n");
        }
        if (btnAdd != null && batchUseNum != null && int.TryParse(batchUseNum.value, out useNum))
        {
            UIEventListener.Get(btnAdd.gameObject).onClick = (x) =>
            {
                if (useNum < _eq.StackCurCount)
                    batchUseNum.value = (++useNum).ToString();
            };
        }

        if (btnReduce != null && batchUseNum != null && int.TryParse(batchUseNum.value, out useNum))
        {
            UIEventListener.Get(btnReduce.gameObject).onClick = (x) =>
            {
                if (useNum > 0) batchUseNum.value = (--useNum).ToString();
            };
        }
        if (btnMaxNum != null && batchUseNum != null) UIEventListener.Get(btnMaxNum.gameObject).onClick = (x) =>
        {
            if (batchUseNum != null) batchUseNum.value = _eq.StackCurCount.ToString();
            useNum = _eq.StackCurCount;
        };
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate
        {
            DestroyImmediate(this.gameObject);
        };
    }
    void Update()
    {
        if (isClose)
        {
            DestroyImmediate(this.gameObject);
            isClose = false;
        }
        //解决花车巡游开始前如果获得新物品，巡游中该提示一直存在
        if (GameCenter.mainPlayerMng.MainPlayerInfo.IsHide)
        {
            if (!isHide)
            {
                this.gameObject.SetActive(false);
                isHide = true;
            }
        }
    }
}
