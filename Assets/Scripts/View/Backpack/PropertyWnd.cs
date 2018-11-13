//==============================================
//作者：邓成
//日期：2016/3/24
//用途：属性窗口
//=================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PropertyWnd : GUIBase 
{
    #region 外部控件
    /// <summary>
    /// 职业
    /// </summary>
    public UILabel Occupation;
    public UILabel Lev;
    public UILabel Name;
	public UILabel fightVal;
	public UILabel slaValue;
	public UILabel slaValueName;
	public UISpriteEx suitPic;
    protected List<StrengthenSuitRef> strengSuitList
    {
        get
        {
            return ConfigMng.Instance.GetStrengSuitRefList(1);
        }
    }
    protected Dictionary<int, EquipmentInfo> playerEqu
    {
        get
        {
            return GameCenter.inventoryMng.GetPlayerEquipList();
        }
    }
    /// <summary>
    /// 图片
    /// </summary>
    public UITexture dressingRoomTexture;

    public ItemUI[] equipItems;
    #endregion
    private PlayerBaseInfo curTargetInfo;
    void Awake()
    {
		layer = GUIZLayer.TOPWINDOW;
		mutualExclusion = false;
    }
    void Start()
    {
		
    }
    protected override void OnOpen()
    {
        base.OnOpen();
		curTargetInfo = GameCenter.curMainPlayer.actorInfo;
        RefreshEquipment();
		RefreshPreview();
        RefreshStar();
		RefreshCount(ActorBaseTag.TOTAL, 0,false);
    }

    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.previewManager.ClearModel();
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
			GameCenter.inventoryMng.OnEquipItemUpdate += RefreshEquipment;
            GameCenter.curMainPlayer.actorInfo.OnBaseUpdate += RefreshCount;
            GameCenter.inventoryMng.OnEquipItemUpdate += RefreshStar;
        }
        else
        {
			GameCenter.inventoryMng.OnEquipItemUpdate -= RefreshEquipment;
            GameCenter.curMainPlayer.actorInfo.OnBaseUpdate -= RefreshCount;
            GameCenter.inventoryMng.OnEquipItemUpdate -= RefreshStar;
        }
    }

    /// <summary>
    /// 关闭热感框
    /// </summary>
    /// <param name="_bool"></param>
    public void SetTip(bool _bool)
    {
        for (int i = 0; i < equipItems.Length; i++)
        {
            if (equipItems[i] != null)
            {
                equipItems[i].ShowTooltip = _bool;
            }
        }
    }
    /// <summary>
    /// 刷新人物预览
    /// </summary>
    void RefreshPreview()
    {
        if (curTargetInfo != null && dressingRoomTexture != null)
        {
            GameCenter.previewManager.TryPreviewSinglePlayer(curTargetInfo, dressingRoomTexture);
        }
    }
    /// <summary>
    /// 刷新身上装备
    /// </summary>
    void RefreshEquipment()
    {
        /////填充已经装备好的数据
        foreach (var item in GameCenter.inventoryMng.EquipDictionary.Values)
        {
            if (equipItems[(int)item.Slot - 1] != null)
            {
				if(item.StackCurCount == 0)
					equipItems[(int)item.Slot - 1].FillInfo(null);
				else
                	equipItems[(int)item.Slot - 1].FillInfo(item);
            }   
        }
        /////填充空白
        for (int i = 0; i < equipItems.Length; i++)
        {
            if (!GameCenter.inventoryMng.EquipDictionary.ContainsKey((EquipSlot)(i + 1)) && equipItems[i] != null)
            {
                equipItems[i].FillInfo(null);
            }
        }
    }
    /// <summary>
    /// 强化套装的星星亮灰显示
    /// </summary>
    void RefreshStar()
    {
        if (playerEqu.Count == 0 || playerEqu.Count < 12)
        {
            suitPic.IsGray = UISpriteEx.ColorGray.Gray;
            return;
        }
        foreach (EquipmentInfo info in playerEqu.Values)
        {
            if (info.UpgradeLv < strengSuitList[0].str_Lev)
            {
                suitPic.IsGray = UISpriteEx.ColorGray.Gray;
                break;
            }
            else
                suitPic.IsGray = UISpriteEx.ColorGray.normal;
        }
    }
    /// <summary>
    /// 刷新顶部数据
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="value"></param>
	protected void RefreshCount(ActorBaseTag tag, ulong value,bool _fromAbility)
    {
        if (Occupation != null)
        {
            Occupation.text = ConfigMng.Instance.GetPlayerConfig(curTargetInfo.Prof).name;
        }
        if (Name != null)
        {
            Name.text = curTargetInfo.Name;
        }
        if (Lev != null)
        {
			Lev.text = GameCenter.curMainPlayer.actorInfo.Level.ToString();
        }
		if(fightVal != null)
			fightVal.text = GameCenter.mainPlayerMng.MainPlayerInfo.FightValue.ToString();
		if(slaValue != null)
			slaValue.text = GameCenter.mainPlayerMng.MainPlayerInfo.KillingValue.ToString();
		if(slaValueName != null)
			slaValueName.text = GameCenter.mainPlayerMng.MainPlayerInfo.KillingValueName;
    }
	void RefreshSlaValue(ActorPropertyTag tag,int val,bool _from)
	{
		if(tag != ActorPropertyTag.SLA)return;

	}
    #region 控件事件
    private void LockToggle(GameObject obj)
    {
        UIToggle tog = obj.GetComponent<UIToggle>();
        if (tog != null)
        {
            tog.value = true;
        }
    }
    #endregion
}
