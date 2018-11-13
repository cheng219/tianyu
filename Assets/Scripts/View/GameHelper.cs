//==================================
//作者：邓成
//日期：2016/4/12
//用途：游戏辅助类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class GameHelper{

	public static bool haveDontDestroyRingEffect = false;
    public static string secondComfirmText;
    public static string loadText;
    public static string netFaildText;
    public static string loadNetFaildText;
	#region 物品
	/// <summary>
	/// 根据物品需求数量及背包已有数量，获取显示字符串(eg:初级强化石 10/30)
	/// </summary>
	public static string GetStringWithBagNumber(int _equipEid,ulong _number)
	{
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		ulong count = (ulong)GameCenter.inventoryMng.GetNumberByType(_equipEid);
		switch(_equipEid)
		{
		case 5:
                count = GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
			break;
		case 6:
            count = GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
			break;
		default://金币等物品不显示名字
            builder.Append("[u]").Append(GameHelper.GetEquipNameByType(_equipEid)).Append("[/u]").Append(" ");
			break;
		}
		builder.Append(_number);
		if(count < _number)
			builder.Append("[ff0000]");
		builder.Append("/").Append(count);
		if(count < _number)
			builder.Append("[-]");
		return builder.ToString();
	}
	/// <summary>
	/// 根据物品需求数量及背包已有数量，获取显示字符串(eg:初级强化石 10/[ff0000]0[-])
	/// </summary>
	public static string GetStringWithBagNumber(ItemValue _itemValue)
	{
		ulong number = (ulong)_itemValue.count;
		int equipEid = _itemValue.eid;
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		ulong count = (ulong)GameCenter.inventoryMng.GetNumberByType(equipEid);
		switch(equipEid)
		{
		case 5:
                count = GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
			break;
		case 6:
            count = GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
			break;
		default://金币等物品不显示名字
            builder.Append("[u]").Append(GameHelper.GetEquipNameByType(equipEid)).Append("[/u]").Append(" ");
			break;
		}
		builder.Append(number);
		if(count < number)
			builder.Append("[ff0000]");
		builder.Append("/").Append(count);
		if(count < number)
			builder.Append("[-]");
		return builder.ToString();
	}
	/// <summary>
	/// 根据物品需求数量及背包已有数量，获取显示字符串(eg:初级强化石 10/[ff0000]0[-]) out材料是否足够
	/// </summary>
	public static string GetStringWithBagNumber(ItemValue _itemValue,out bool enough)
	{
		ulong number = (ulong)_itemValue.count;
		int equipEid = _itemValue.eid;
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
        ulong count = (ulong)GameCenter.inventoryMng.GetNumberByType(equipEid);
		switch(equipEid)
		{
		case 5:
                count = GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
			break;
		case 6:
            count = GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
			break;
		default://金币等物品不显示名字
            builder.Append("[u]").Append(GameHelper.GetEquipNameByType(equipEid)).Append("[/u]").Append(" ");
			break;
		}
		builder.Append(number);
		enough = (count >= number);
		if(count < number)
			builder.Append("[ff0000]");
		builder.Append("/").Append(count);
		if(count < number)
			builder.Append("[-]");
		return builder.ToString();
	}
	/// <summary>
	/// 根据物品需求数量及背包已有数量，获取显示字符串(eg:初级强化石 10/[ff0000]0[-]) out材料是否足够 out 缺少的材料
	/// </summary>
	public static string GetStringWithBagNumber(ItemValue _itemValue,out bool enough,out EquipmentInfo lackEquip)
	{
        ulong number = (ulong)_itemValue.count;
		int equipEid = _itemValue.eid;
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
        ulong count = (ulong)GameCenter.inventoryMng.GetNumberByType(equipEid);
		switch(equipEid)
		{
		case 5:
                count = GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
			break;
		case 6:
            count = GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
			break;
		default://金币等物品不显示名字
			builder.Append("[u]").Append(GameHelper.GetEquipNameByType(equipEid)).Append("[/u]").Append(" ");
			break;
		}
		builder.Append(number);
		enough = (count >= number);
		lackEquip = (enough == false)?new EquipmentInfo(equipEid,(int)(number -count),EquipmentBelongTo.NONE):null;
		if(count < number)
			builder.Append("[ff0000]");
		builder.Append("/").Append(count);
		if(count < number)
			builder.Append("[-]");
		return builder.ToString();
	}
	/// <summary>
	/// 根据物品type获取带颜色的文字显示
	/// </summary>
	public static string GetEquipNameByType(int type)
	{
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		EquipmentInfo equip = new EquipmentInfo(type,EquipmentBelongTo.NONE);
		if(equip != null)
		{
			builder.Append(equip.ItemStrColor).Append("[url=").Append(type).Append("]").Append(equip.ItemName).Append("[/url][-]");
		}
		return builder.ToString();
	}
	#endregion

	#region  获取途径
	/// <summary>
	/// 构造链接字符串(eg：[url=XXX]YYY[u],在UILabel中只显示"YYY"，但可获得“”XXX的内容)
	/// </summary>
	public static string GetUrlString(string _urlDes,string _urlText)
	{
		StringBuilder builder = new StringBuilder();
		builder.Append("[url=").Append(_urlText).Append("]").Append(_urlDes).Append("[/url]");
		return builder.ToString();
	}
	/// <summary>
	/// 构造链接字符串(eg：[url=1|XXX|YYY]YYY[u],在UILabel中只显示"xxx"，但可获得“”1|XXX|YYY的内容)
	/// </summary>
	public static string GetUrlString(string _urlDes)
	{
		StringBuilder builder = new StringBuilder();
		string[] urlText = _urlDes.Split('|');
		string url = string.Empty;
		if(_urlDes.Contains("[u]"))
		{
			url = _urlDes.Replace("[u]",string.Empty);
		}
		if(url.Contains("[/u]"))
		{
			url = url.Replace("[/u]",string.Empty);
		}
		//Debug.Log("url:"+url);
		builder.Append("[url=").Append(url).Append("]").Append(urlText.Length>1?urlText[1]:string.Empty).Append("[/url]");
		return builder.ToString();
	}
	/// <summary>
	/// 点击链接文字
	/// </summary>
	public static void SetUrlCallback(UILabel _lab,Action<string> _callUrl)
	{
		string url = _lab.GetUrlAtPosition(UICamera.lastWorldPosition);
		BoxCollider box = _lab.GetComponent<BoxCollider>();
		if(box == null)
		{
			box = _lab.gameObject.AddComponent<BoxCollider>();
			box.size = new Vector3(_lab.localSize.x,_lab.localSize.y,0);
		}
		UIEventListener.Get(_lab.gameObject).onClick =(x)=>
		{
			if(!string.IsNullOrEmpty(url))
			{
				if(_callUrl != null)_callUrl(url);
			}
		};
		Debug.Log(url);
	}
	/// <summary>
	/// 设置一个UILabel链接至UI界面
	/// </summary>
	public static void SetUrlToUI(UILabel _lab,string _urlDes,string _urlText)
	{
		if(_lab != null)_lab.text = GameHelper.GetUrlString(_urlDes,_urlText);
		GameHelper.SetUrlCallback(_lab,(x)=>
			{
				try
				{
					GUIType uiType = (GUIType)Enum.Parse(Type.GetType("GUIType"),(string)x,true);
					if(uiType != GUIType.NONE)
					{
						if(uiType == GUIType.NEWMALL)
						{
							GameCenter.newMallMng.OpenMall();
						}else if(uiType == GUIType.SHOPWND)
						{
							GameCenter.shopMng.OpenShopWnd();
						}else
						{
							GameCenter.uIMng.SwitchToUI(uiType);
						}
					}
				}catch(System.Exception e)
				{
					try
					{
						SubGUIType subUiType = (SubGUIType)Enum.Parse(Type.GetType("SubGUIType"),(string)x);
						GameCenter.uIMng.SwitchToSubUI(subUiType);
					}catch(System.Exception e2)
					{
						GameCenter.messageMng.AddClientMsg("配置错误");
					}
				}
			});
	}


    /// <summary>
    /// 根据界面id设置一个UILabel链接至UI界面
    /// </summary>
    public static void SetUrlToUIByWndId(UILabel _lab, string _urlDes, string _urlText)
    {
        if (_lab != null) _lab.text = GameHelper.GetUrlString(_urlDes, _urlText);
        
        GameHelper.SetUrlCallback(_lab, (x) =>
        { 
            int wndId = int.Parse(x);
            if (GameCenter.inventoryMng != null)
            {
                GameCenter.inventoryMng.SkipWndById(wndId);
            }
            else
            {
                Debug.Log(" inventoryMng 为null, 不能使用跳转界面功能");
            }
        });
    }
	#endregion

	#region 等级套装
	public static string GetLevelSuitDes(EquipmentInfo info)
	{
		if(!info.isAttackSuit && !info.isDefenseSuit)return string.Empty;
		StringBuilder builder = new StringBuilder();
		string defaultStr = info.isAttackSuit?ConfigMng.Instance.GetUItext(262):ConfigMng.Instance.GetUItext(263);
		int suitType = (info.isAttackSuit?1:2);
		int suitCount = 0;//套装满足数量
		if(info.Quality >= 4 && info.UseReqLevel >= 20)
		{
			EquipmentSetRef setRef = ConfigMng.Instance.GetEquipmentSetRefByLv(info.UseReqLevel,info.Quality,suitType);
			if(setRef != null)
			{
				Dictionary<int,EquipmentInfo> equipList = GameCenter.inventoryMng.GetPlayerEquipList();
				builder.Append(ConfigMng.Instance.GetUItext(264));//第一行
				foreach (EquipmentInfo equipmentInfo in equipList.Values) {
					int tempType = equipmentInfo.isAttackSuit?1:2;
					if(tempType == suitType && equipmentInfo.Quality == info.Quality && equipmentInfo.UseReqLevel >= info.UseReqLevel)
					{
						suitCount++;
						defaultStr = defaultStr.Replace("[a1a1a1]"+equipmentInfo.SlotName+"[-]",(equipmentInfo.ItemStrColor+equipmentInfo.SlotName+"[-]"));
					}
				}
				builder.Append("[eedeb0]").Append(setRef.des).Append("(").Append(suitCount).Append("/6)[-]").Append("\n");//第二行
				builder.Append(defaultStr).Append("\n");//第三行
				string attrName1 = ConfigMng.Instance.GetAttributeTypeName(setRef.three_attr.tag);

				builder.Append((suitCount>=3&&suitCount<6)?info.ItemStrColor:"[a1a1a1]");//颜色
				builder.Append("3件  ").Append(attrName1).Append("+").Append(setRef.three_attr.value);
				builder.Append((suitCount>=6?ConfigMng.Instance.GetUItext(265):string.Empty));
				builder.Append("[-]").Append("\n");//颜色 //第四行

				string attrName2 = ConfigMng.Instance.GetAttributeTypeName(setRef.six_attr.tag);

				builder.Append((suitCount >= 6)?info.ItemStrColor:"[a1a1a1]");//颜色
				builder.Append("6件  ").Append(attrName2).Append("+").Append((float)setRef.six_attr.value/100f).Append("%");
				builder.Append("[-]").Append("\n");//颜色  //第五行
			}
		}
		return builder.ToString();
	}
	#endregion

	#region 属性
	/// <summary>
	/// 获取属性的显示
	/// </summary>
	public static string GetAttributeNameAndValue(List<AttributePair> attrList)
	{
		StringBuilder builder = new StringBuilder();
		string atkStr =ConfigMng.Instance.GetUItext(266);
		string defStr = ConfigMng.Instance.GetUItext(267);
		bool haveAtk = false;
		bool haveDef = false;
		for (int i = 0,max=attrList.Count; i < max; i++) 
		{
			AttributePair attr = attrList[i];
			switch(attr.tag)
			{
			case ActorPropertyTag.ATKUP:
				atkStr = atkStr.Replace("#1",attr.value.ToString());
				haveAtk = true;
				break;
			case ActorPropertyTag.ATKDOWN:
				atkStr = atkStr.Replace("#0",attr.value.ToString());
				haveAtk = true;
				break;
			case ActorPropertyTag.DEFUP:
				defStr = defStr.Replace("#1",attr.value.ToString());
				haveDef = true;
				break;
			case ActorPropertyTag.DEFDOWN:
				defStr = defStr.Replace("#0",attr.value.ToString());
				haveDef = true;
				break;
			default:
				builder.Append(ConfigMng.Instance.GetAttributeTypeName(attr.tag));
				builder.Append(":").Append(attr.value);
				if(i<max-1 || haveAtk || haveDef)builder.Append("\n");
				break;
			}
		}
		if(haveAtk)builder.Append(atkStr);
		if(haveDef)builder.Append(defStr);
		return builder.ToString();
	}

	/// <summary>
	/// 获取属性的显示
	/// </summary>
	public static string GetAttributeWithStrengthValue(List<AttributePair> attrList,EquipmentInfo info)
	{
		StringBuilder builder = new StringBuilder();
        string atkStr = ConfigMng.Instance.GetUItext(266);
        string defStr = ConfigMng.Instance.GetUItext(267);
		bool haveAtk = false;
		bool haveDef = false;
		for (int i = 0,max=attrList.Count; i < max; i++) 
		{
			AttributePair attr = attrList[i];
			switch(attr.tag)
			{
			case ActorPropertyTag.ATKUP:
				atkStr = atkStr.Replace("#1",attr.value.ToString());
				haveAtk = true;
				break;
			case ActorPropertyTag.ATKDOWN:
				atkStr = atkStr.Replace("#0",attr.value.ToString());
				haveAtk = true;
				break;
			case ActorPropertyTag.DEFUP:
				defStr = defStr.Replace("#1",attr.value.ToString());
				haveDef = true;
				break;
			case ActorPropertyTag.DEFDOWN:
				defStr = defStr.Replace("#0",attr.value.ToString());
				haveDef = true;
				break;
			default:
				builder.Append(ConfigMng.Instance.GetAttributeTypeName(attr.tag));
				builder.Append(":").Append(attr.value);
				int upgradeVal = info.GetStrengthValueByTag(attr.tag);
				if(upgradeVal != 0)builder.Append("    [00ff00]+").Append(upgradeVal).Append("[-]");
				if(i<max-1 || haveAtk || haveDef)builder.Append("\n");
				break;
			}
		}
		int atk = info.GetStrengthValueByTag(ActorPropertyTag.ATK);
		int def = info.GetStrengthValueByTag(ActorPropertyTag.DEF);
		if(haveAtk)builder.Append(atkStr);
		if(atk != 0)builder.Append("    [00ff00]+").Append(atk).Append("[-]");
		if(haveDef)builder.Append(defStr);
		if(def != 0)builder.Append("    [00ff00]+").Append(def).Append("[-]");
		return builder.ToString();
	}

    /// <summary>
    /// 获取属性的显示
    /// </summary>
    public static string GetAttributeNameAndValue(List<AttributePair> attrList, List<AttributePair> nextAttrList)
    {
        StringBuilder builder = new StringBuilder();
        if (attrList.Count != nextAttrList.Count)
        {
            Debug.LogError("强化前后的属性不一致!无法对比");
            return string.Empty;
        }
        for (int i = 0, max = attrList.Count; i < max; i++)
        {
            AttributePair attr = attrList[i];
            AttributePair nextAttr = nextAttrList[i];
            builder.Append(ConfigMng.Instance.GetAttributeTypeName(attr.tag));
            builder.Append(":+").Append(attr.value);
            builder.Append("  [00ff00]+").Append(nextAttr.value - attr.value).Append("[-]"); 
            if (i < max - 1) builder.Append("\n");
        }
        return builder.ToString();
    }
	#endregion

	#region 时间处理
	/// <summary>
	/// 获取最近时间状态(1天前、一周前),recentTime为后台传过来的秒数
	/// </summary>
	public static string GetRecentTimeStr(int recentTime)
	{
		DateTime oldTime = new DateTime (1970,1,1);
		DateTime dateTime = ToChinaTime(DateTime.Now);
		int seconds = (int)dateTime.Subtract (oldTime).TotalSeconds;
		int diffSeconds = seconds - recentTime;
		//Debug.Log ("diffSeconds:"+diffSeconds);
		if (diffSeconds <= 3600) {
			return "一小时内";
		} else if (diffSeconds <= 12 * 60 * 60) {
			return "半天内";
        }
        else if (diffSeconds <= 24 * 3600)
        {
            return "一天内";
        }
        else if (diffSeconds <= 2 * 24 * 3600)
        {
            return "一天前";
        }
        else if (diffSeconds <= 3 * 24 * 3600)
        {
            return "两天前";
        }
        else if (diffSeconds <= 4 * 24 * 3600)
        {
            return "三天前";
        }
        else if (diffSeconds <= 5 * 24 * 3600)
        {
            return "四天前";
        }
        else if (diffSeconds <= 6 * 24 * 3600)
        {
            return "五天前";
        }
        else if (diffSeconds <= 7 * 24 * 3600)
        {
            return "六天前";
        }
        else
        {
            return "一周前";
        }
	}
	#endregion

	#region 货币类型
	public static string GetCoinIconByType(int coinType)
	{
		switch(coinType)
		{
		case 1:
			return "Icon_gold";
		case 2:
			return "Icon_zhibi";
        case 5:
            return "Icon_tongbisuo";
        case 18:
            return "Icon_bangyb_small";
		}
		return string.Empty;
	}
	#endregion

	#region 等级显示
	public static string GetLevelStr(int _level)
	{
		AttributeRef attributeRef = ConfigMng.Instance.GetAttributeRef(_level > 0 ? _level : 1);
		if(attributeRef.reborn > 0){
			return ConfigMng.Instance.GetUItext(12,new string[2]{attributeRef.reborn.ToString(),attributeRef.display_level.ToString()});
		}else{
			return ConfigMng.Instance.GetUItext(13,new string[1]{attributeRef.display_level.ToString()});
		}
		//return "";
	}
	#endregion

	#region 时间
	public static DateTime ToChinaTime(DateTime _dateTime)
	{
//		try
//		{
//			TimeZoneInfo chinaZone = TimeZoneInfo.FindSystemTimeZoneById("中国标准时间");
//			DateTime time = TimeZoneInfo.ConvertTime(_dateTime,chinaZone);
//			Debug.Log(time.ToLongTimeString());
//			return time;
//		}catch(Exception e)
//		{
//			Debug.Log(e.ToString());
//		}
		return _dateTime.AddSeconds(8*60*60);
	}

	#endregion

    #region
    /// <summary>
    /// MD5加密字符串
    /// </summary>
    /// <param name="_param"></param>
    /// <returns></returns>
    public static string SignString(string _param)
    {
        byte[] result = Encoding.Default.GetBytes(_param);    
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] output = md5.ComputeHash(result);
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < output.Length; i++)
            builder.Append(output[i].ToString("x2"));
        return builder.ToString().ToLower();
    }
    #endregion
}
