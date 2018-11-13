//=========================================
//作者：黄洪兴
//日期：2016/04/12
//用途：仙盟商店商品UI的容器
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 商品UI的容器
/// </summary>
public class GuildShopItemContainer : MonoBehaviour {

	/// <summary>
	/// 是否是第一次运行
	/// </summary>
	protected bool firstRun = true;
	/// <summary>
	/// 选择商品的事件
	/// </summary>
	public System.Action<GuildShopItemUI> OnSelectItemEvent = null;
	/// <summary>
	/// 当前选中的商品
	/// </summary>
	protected GuildShopItemUI selectGuildShopItem = null;
	/// <summary>
	/// 商品列表
	/// </summary>
	public List<GuildShopItemInfo> guildShopItemList = new List<GuildShopItemInfo>();
	/// <summary>
	/// 排列
	/// </summary>
	public UIGrid grid;

	public Dictionary<int, GuildShopItemUI> GuildShopItemContainers = new Dictionary<int, GuildShopItemUI>();
	// Use this for initialization
	void Start () {

	}

	/// <summary>
	/// 刷新数据
	/// </summary>
	/// <param name="_skList"></param>
	public void RefreshItems(List<GuildShopItemInfo> _skList)
	{
		guildShopItemList.Clear();
		GuildShopItemContainers.Clear ();
		if (_skList != null)
		{
			for (int i = 0; i < _skList.Count; i++) {
				guildShopItemList.Add(_skList[i]);
			}
		}
		RefreshItems();
	}
	public void RefreshItems(Dictionary<int,GuildShopItemInfo> _Dic)
	{
		guildShopItemList.Clear();
		GuildShopItemContainers.Clear ();
		if (_Dic != null)
		{
            using (var e = _Dic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    guildShopItemList.Add(e.Current.Value);
                }
            }
		}
		RefreshItems();
	}
	/// <summary>
	/// 刷新表现
	/// </summary>
	public void RefreshItems()
	{
		Object guildShopItemprefab = null;
		if (guildShopItemprefab == null)
		{
			guildShopItemprefab = exResources.GetResource(ResourceType.GUI, "Guild/GuildShopItem");
		}
		if (guildShopItemprefab== null)
		{
			GameSys.LogError("找不到预制：Guild/GuildShopItem");
			return ;
		}
        Vector3 V3 = Vector3.zero;
		for(int i = 0;i<guildShopItemList.Count;i++)
		{
            if (!GuildShopItemContainers.ContainsKey(i))
            {
                GameObject obj = Instantiate(guildShopItemprefab) as GameObject;
                Transform parentTransf = this.gameObject.transform;
                obj.transform.parent = parentTransf;
                obj.transform.localPosition = V3;
                obj.transform.localScale = Vector3.one;
                if ((i + 1) % 2 == 0)
                {
                    V3 = new Vector3(0, V3.y - 100, V3.z);
                }
                else
                {
                    V3 = new Vector3(V3.x + 210, V3.y, V3.z);
                }


                GuildShopItemUI shopItemUI = obj.GetComponent<GuildShopItemUI>();
                shopItemUI.FillInfo(guildShopItemList[i]);
                GuildShopItemContainers[i] = shopItemUI;
            }
		}
        guildShopItemprefab = null;
	}
	/// <summary>
	/// 选中一个技能
	/// </summary>
	/// <param name="_itemUI"></param>
	void OnSelectSkillUI(GuildShopItemUI _itemUI)
	{
		//selectSkillUI = _itemUI;
		//if (OnSelectItemEvent != null)
		//	{
		//	OnSelectItemEvent(selectSkillUI);
		//}
	}
}
