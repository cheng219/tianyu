//==============================
//作者：吴江
//日期:2015/6/16
//用途：信息展示管理类
//================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 信息展示管理类 by吴江
/// </summary>
public class ToolTipMng {
    
    /// <summary>
    /// 展示类型 by吴江
    /// </summary>
    public enum TooltipType
	{
	   Equipment,
        Property,
        ShowForMall,
		Access,
		EquipmentCompare,
	}

    /// <summary>
    /// 预制缓存字典 by吴江
    /// </summary>
    protected static Dictionary<TooltipType, GUIBase> tooltipPanelDictionary = new Dictionary<TooltipType, GUIBase>();
	/// <summary>
	/// 是否显示物品热感的模型
	/// </summary>
	public static bool ShowEquipmentModel = true;

    /// <summary>
    /// 根据类型获取预制 by吴江
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    protected static GUIBase GetPanelByType(TooltipType _type)
    {
        if (!tooltipPanelDictionary.ContainsKey(_type) || tooltipPanelDictionary[_type] == null)
        {
            string path = GetPath(_type);
            Object obj = exResources.GetResource(ResourceType.GUI, path);
            if (obj == null)
            {
                GameSys.LogError("找不到UI预制：" + path);
                return null;
            }
            GameObject panel = GameObject.Instantiate(obj) as GameObject;
            panel.transform.parent = GameCenter.uIMng.uIRoot.transform;
            panel.transform.localPosition = new Vector3(0,0,0);
            panel.transform.localScale = new Vector3(1, 1, 1);
            panel.name = obj.name;
            obj = null;
            GUIBase wnd = panel.GetComponent<GUIBase>();
            if (wnd == null)
            {
                GameSys.LogError("找不到<GUIBase>组件！");
                return null;
            }
			if(_type != TooltipType.Access){
				tooltipPanelDictionary[_type] = wnd;
			}else{
				return wnd;
			}
        }
        return tooltipPanelDictionary[_type];
    }

    /// <summary>
    /// 根据类型获取预制路径 by吴江
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    protected static string GetPath(TooltipType _type)
    {
        switch (_type)
        {
            case TooltipType.Equipment:
                return "Item_icon/Item_Wnd";
            case TooltipType.Property:
                return "Tooltip/Panel_MSGBubble";
            case TooltipType.ShowForMall:
                return "Item/ItemDesc_show";
			case TooltipType.Access:
				return "Access/Access";
			case TooltipType.EquipmentCompare:
				return "Item_icon/Item_Compare";
            default:
                return string.Empty;
        }
    }

    /// <summary>
    /// 关闭所有的信息展示 by吴江
    /// </summary>
    public static void CloseAllTooltip()
    {
        foreach (var item in tooltipPanelDictionary.Values)
        {
            if (item != null)
            {
                item.CloseUI();
            }
        }
    }


    /// <summary>
    /// 展示属性信息 by吴江
    /// </summary>
    /// <param name="_triggerUI"></param>
    /// <param name="_tag"></param>
    /// <param name="_value"></param>
    public static void ShowPropertyTooltip(GameObject _triggerUI,ActorPropertyTag _tag, int _value)
    {
        CloseAllTooltip();
        if (_tag == ActorPropertyTag.TOTAL) return;
        GUIBase panel = GetPanelByType(TooltipType.Property);
        if (panel == null) return;

        PropertyTooltip propertyTooltip = panel.GetComponent<PropertyTooltip>();
        if (propertyTooltip == null)
        {
            GameSys.LogError("在预制上找不到组件 ： <EquipmentTooltip>！");
            return;
        }
        propertyTooltip.SetData(_tag, _value, _triggerUI);
        panel.OpenUI();
        SetPropertyTooltipPosition(panel.gameObject, _triggerUI);
    }

    /// <summary>
    /// 设置属性信息展示面板的位置 by吴江
    /// </summary>
    /// <param name="_tooltipObj"></param>
    /// <param name="_triggerObj"></param>
    protected static void SetPropertyTooltipPosition(GameObject _tooltipObj, GameObject _triggerObj)
    {
        Transform tooltipTrans = _tooltipObj.transform;
        Transform tiggerTrans = _triggerObj.transform;

        if (_triggerObj != null)
        {
        //    Bounds triBounds = NGUIMath.CalculateAbsoluteWidgetBounds(_triggerObj.transform);
			Bounds triBounds = _triggerObj.GetComponent<BoxCollider>().bounds;
       //     Bounds descBounds = NGUIMath.CalculateAbsoluteWidgetBounds(tooltipTrans);
			Bounds descBounds = tooltipTrans.GetComponent<BoxCollider>().bounds;
           // float y = tiggerTrans.position.y;

         //   Vector3 pos = GameCenter.cameraMng.uiCamera.WorldToScreenPoint(tiggerTrans.position);
           // float halfScreenWid = Screen.width / 2.0f;
           // float halfScreenHigh = Screen.height / 2.0f;
            //取横坐标差值
            float diffX = (Mathf.Abs(triBounds.max.x - triBounds.min.x) + Mathf.Abs(descBounds.max.x - descBounds.min.x)) / 2.0f;
            //根据对象所在屏幕位置,计算信息面板的横坐标
            float posX = tiggerTrans.position.x - diffX;
            //取竖坐标差值
       //     float diffY = (Mathf.Abs(triBounds.max.y - triBounds.min.y) + Mathf.Abs(descBounds.max.y - descBounds.min.y)) / 2.0f;
            float posY =  tiggerTrans.position.y;


            _tooltipObj.transform.position = new Vector3(posX, posY, tiggerTrans.position.z - 1);

            //以本地坐标修正一次z轴
            _tooltipObj.transform.localPosition = new Vector3(tooltipTrans.localPosition.x, tooltipTrans.localPosition.y, tooltipTrans.localPosition.z);
        }
        else
        {
            _tooltipObj.transform.localPosition = new Vector3(0f, 0f, tooltipTrans.localPosition.z);
        }
    }







    /// <summary>
    /// 展示物品信息  by吴江
    /// </summary>
    /// <param name="_info">装备信息</param>
    /// <param name="_left">左边功能按钮的行为类型</param>
    /// <param name="_middle">中间功能按钮的行为类型</param>
    /// <param name="_right">右边功能按钮的行为类型</param>
    /// <param name="_itemUI">展示界面的碰撞依据。为空的话则是物品控件本身</param>
	public static void ShowEquipmentTooltip(ItemUI _itemUI, ItemActionType _left, ItemActionType _middle, ItemActionType _right,GameObject _tiggerUI = null)
    {
        CloseAllTooltip();
        EquipmentInfo info = _itemUI.EQInfo;
        if (info == null) return;
        GUIBase panel = GetPanelByType(TooltipType.Equipment);
        if (panel == null) return;
        EquipmentTooltip equipmentTooltip = panel.GetComponent<EquipmentTooltip>();
        if (equipmentTooltip == null)
        {
            GameSys.LogError("在预制上找不到组件 ： <EquipmentTooltip>！");
            return;
        }
//		Debug.Log("ShowEquipmentTooltip   热感");
        equipmentTooltip.mSyncTriggerChk = _itemUI.GetComponent<UIToggle>();
        equipmentTooltip.EquipmentInfo = info;
		equipmentTooltip.SetActionBtn(_left, _middle, _right);
        panel.OpenUI();
        SetEquipTooltipPostion(panel.gameObject, _tiggerUI);
    }

	/// <summary>
	/// 展示物品信息  by邓成
	/// </summary>
	/// <param name="_info">装备信息</param>
	/// <param name="_left">左边功能按钮的行为类型</param>
	/// <param name="_middle">中间功能按钮的行为类型</param>
	/// <param name="_right">右边功能按钮的行为类型</param>
	/// <param name="_itemUI">展示界面的碰撞依据。为空的话则是物品控件本身</param>
	public static void ShowEquipmentTooltip(int _itemType)
	{
		CloseAllTooltip();
		EquipmentInfo info = new EquipmentInfo(_itemType,EquipmentBelongTo.PREVIEW);
		if (info == null) return;
		GUIBase panel = GetPanelByType(TooltipType.Equipment);
		if (panel == null) return;
		EquipmentTooltip equipmentTooltip = panel.GetComponent<EquipmentTooltip>();
		if (equipmentTooltip == null)
		{
			GameSys.LogError("在预制上找不到组件 ： <EquipmentTooltip>！");
			return;
		}
		equipmentTooltip.EquipmentInfo = info;
		equipmentTooltip.SetActionBtn(ItemActionType.None,ItemActionType.None,ItemActionType.None);
		panel.OpenUI();
	}

	public static int accessSceneID = 0;
	/// <summary>
	/// 展示物品获取信息  by
	/// </summary>
	public static void ShowEquipmentAccessTooltip(EquipmentInfo info)
	{        
	}


    /// <summary>
    /// 展示物品信息  by吴江
    /// </summary>
    /// <param name="_info">装备信息</param>
    /// <param name="_left">左边功能按钮的行为类型</param>
    /// <param name="_middle">中间功能按钮的行为类型</param>
    /// <param name="_right">右边功能按钮的行为类型</param>
    /// <param name="_itemUI">展示界面的碰撞依据。为空的话则是物品控件本身</param>
	public static void ShowEquipmentTooltip(EquipmentInfo _info, ItemActionType _left, ItemActionType _middle, ItemActionType _right,ItemActionType _other, GameObject _tiggerUI = null)
    {        
        CloseAllTooltip();
        if (_info == null) return;
        GUIBase panel = GetPanelByType(TooltipType.Equipment);
        
        if (panel == null) return;
        EquipmentTooltip equipmentTooltip = panel.GetComponent<EquipmentTooltip>();
        if (equipmentTooltip == null)
        {
            GameSys.LogError("在预制上找不到组件 ： <EquipmentTooltip>！");
            return;
        }
        equipmentTooltip.EquipmentInfo = _info;
		equipmentTooltip.SetActionBtn(_left, _middle, _right,_other);
        panel.OpenUI();        
        //SetEquipTooltipPostion(panel.gameObject, _tiggerUI);  
		if(_info.IsEquip && _info.BelongTo != EquipmentBelongTo.EQUIP)
		{
			EquipmentInfo compareEquip = GameCenter.inventoryMng.GetEquipFromEquipDicBySlot(_info.Slot);
			if(compareEquip != null  && _info.InstanceID != compareEquip.InstanceID)
				ToolTipMng.ShowCompareEquipmentTooltip(compareEquip);
		}
    }

	public static void ShowCompareEquipmentTooltip(EquipmentInfo _info, GameObject _tiggerUI = null)
	{        
		if (_info == null) return;
		GUIBase panel = GetPanelByType(TooltipType.EquipmentCompare);

		if (panel == null) return;
		EquipmentTooltip equipmentTooltip = panel.GetComponent<EquipmentTooltip>();
		if (equipmentTooltip == null)
		{
			GameSys.LogError("在预制上找不到组件 ： <EquipmentTooltip>！");
			return;
		}
		equipmentTooltip.EquipmentInfo = _info;
		equipmentTooltip.SetActionBtn(ItemActionType.None,ItemActionType.None,ItemActionType.None);
		panel.transform.localPosition = new Vector3(0,0,-200);
		panel.OpenUI();        
	}

    /// <summary>
    /// 展示物品信息 by 贺丰
    /// </summary>
    /// <param name="_itemUI"></param>
    /// <param name="_left"></param>
    /// <param name="_middle"></param>
    /// <param name="_right"></param>
    /// <param name="_tiggerUI"></param>
    public static void ShowEquipmentTooltip(ItemUI _itemUI, ItemActionType _left, ItemActionType _middle, ItemActionType _right, ItemActionType _other , GameObject _tiggerUI = null)
    {
        CloseAllTooltip();
        EquipmentInfo info = _itemUI.EQInfo;
        if (info == null) return;
        GUIBase panel = GetPanelByType(TooltipType.Equipment);
        if (panel == null) return;
        EquipmentTooltip equipmentTooltip = panel.GetComponent<EquipmentTooltip>();
        if (equipmentTooltip == null)
        {
            GameSys.LogError("在预制上找不到组件 ： <EquipmentTooltip>！");
            return;
        }
        //		Debug.Log("ShowEquipmentTooltip   热感");
        equipmentTooltip.mSyncTriggerChk = _itemUI.GetComponent<UIToggle>();
        equipmentTooltip.EquipmentInfo = info;
        equipmentTooltip.SetActionBtn(_left, _middle, _right,_other);
        panel.OpenUI();
        //SetEquipTooltipPostion(panel.gameObject, _tiggerUI);
    }
    /// <summary>
    /// 展示商城物品信息  by易睿
    /// </summary>
    /// <param name="_info">装备信息</param>
    /// <param name="_left">左边功能按钮的行为类型</param>
    /// <param name="_middle">中间功能按钮的行为类型</param>
    /// <param name="_right">右边功能按钮的行为类型</param>
    /// <param name="_itemUI">展示界面的碰撞依据。为空的话则是物品控件本身</param>
	public static void ShowMallTooltip(ItemUI _itemUI, ItemActionType access,ItemActionType _left, ItemActionType _middle, ItemActionType _right,GameObject _tiggerUI = null)
    {
        
    }
    /// <summary>
    /// 设置展示面板的位置 by吴江
    /// </summary>
    /// <param name="_tooltipObj"></param>
    /// <param name="_triggerObj"></param>
    protected static void SetEquipTooltipPostion(GameObject _tooltipObj, GameObject _triggerObj)
    {
        Transform tooltipTrans = _tooltipObj.transform;
        Transform tiggerTrans = _triggerObj.transform;
        
        if (_triggerObj != null)
        {
            Bounds triBounds = NGUIMath.CalculateAbsoluteWidgetBounds(_triggerObj.transform);
            Bounds descBounds = NGUIMath.CalculateAbsoluteWidgetBounds(tooltipTrans);
            float lx = tooltipTrans.position.x - descBounds.max.x + triBounds.min.x;
            float rx = tooltipTrans.position.x - descBounds.min.x + triBounds.max.x;
            float y = tooltipTrans.position.y;
            float xMin = descBounds.min.x - descBounds.max.x + triBounds.min.x;

            Vector3 BottomLeftPosition = UICamera.currentCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));            
            if (xMin < BottomLeftPosition.x)
            {                    
               _tooltipObj.transform.position = new Vector3(rx, y, tiggerTrans.position.z - 1);
            }
            else
            {
                _tooltipObj.transform.position = new Vector3(lx, y, tiggerTrans.position.z - 1);
            }
            //以本地坐标修正一次z轴
            _tooltipObj.transform.localPosition = new Vector3(tooltipTrans.localPosition.x, tooltipTrans.localPosition.y, -300f);//解决IOS上,引导和界面层级问题  by邓成
        }
        else
        {
            _tooltipObj.transform.localPosition = new Vector3(0f, 0f, -300f);
        }             
    }
}



