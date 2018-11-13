//==================================
//作者：唐源
//日期：2017/2/9
//用途：打开立即使用的弹窗
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ImmediateUseWnd : GUIBase{

    #region UI控件
    public UIButton btnBatchUse;
    public UIInput batchUseNum;
    public UILabel labDes;
    public UIButton btnAdd;
    public UIButton btnReduce;
    public UIButton btnMaxNum;
    public ItemUI batchUseItem;
    public UIButton closeBtn;
    #endregion
    #region unity函数
    void Awake()
    {
        layer = GUIZLayer.NORMALWINDOW;
        mutualExclusion = false;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    #endregion
    protected override void OnOpen()
    {
        base.OnOpen();
        RefreshImmediateUseWnd();
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
    #region 事件句柄
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (closeBtn != null)
                UIEventListener.Get(closeBtn.gameObject).onClick += Close;
        }
        else
        {
            UIEventListener.Get(closeBtn.gameObject).onClick -= Close;
        }
    }
    #endregion
    #region 控件事件
    /// <summary>
    /// 关闭窗口
    /// </summary>
    void Close(GameObject _obj)
    {
        GameCenter.uIMng.ReleaseGUI(GUIType.IMMEDIATEUSE);
    }
    #endregion
    #region 界面刷新
    void RefreshImmediateUseWnd()
    {
        EquipmentInfo _eq = GameCenter.inventoryMng.CurSelectInventory;
        if (_eq != null)
        {
            int useNum = 0;
            if (btnBatchUse != null && batchUseNum != null && int.TryParse(batchUseNum.value, out useNum))
            {
                batchUseNum.value = "1";
                UIEventListener.Get(btnBatchUse.gameObject).onClick = (x) =>
                {
                    if (int.TryParse(batchUseNum.value, out useNum) && useNum > _eq.StackCurCount)
                    {
                        GameCenter.messageMng.AddClientMsg(ConfigMng.Instance.GetUItext(293));
                        return;
                    }
                    if (GameCenter.mercenaryMng.isOpenMixWndAndUseEgg)
                    {
                        if (GameCenter.mercenaryMng.curUseEggPetId > 0)
                        {
                            GameCenter.mercenaryMng.C2S_ReqUseEgg(GameCenter.mercenaryMng.curUseEggPetId, _eq.InstanceID, useNum);
                        }
                    }
                    else
                    {
                        GameCenter.inventoryMng.C2S_UseItems(_eq, useNum);
                    }
                    GameCenter.uIMng.ReleaseGUI(GUIType.IMMEDIATEUSE);
                };
            }
            if (batchUseItem != null) batchUseItem.FillInfo(_eq);
            if (labDes != null) labDes.text = _eq.Description.Replace("\\n", "\n");
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
        }
        else
        {
            Debug.LogError("当前选中的物品为空");
        }

    }
    #endregion
}
