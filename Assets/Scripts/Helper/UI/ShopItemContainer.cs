//=========================================
//作者：黄洪兴
//日期：2016/03/29
//用途：商品UI的容器
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 商品UI的容器
/// </summary>
public class ShopItemContainer : MonoBehaviour {

	/// <summary>
	/// 是否是第一次运行
	/// </summary>
	protected bool firstRun = true;
	/// <summary>
	/// 选择商品的事件
	/// </summary>
	public System.Action<ShopItemUI> OnSelectItemEvent = null;
	/// <summary>
	/// 当前选中的商品
	/// </summary>
	protected ShopItemUI selectShopItem = null;
	/// <summary>
	/// 商品列表
	/// </summary>
	public List<ShopItemInfo> shopItemList = new List<ShopItemInfo>();

	public Dictionary<int, ShopItemUI> ShopItemContainers = new Dictionary<int, ShopItemUI>();
	// Use this for initialization
	void Start () {

	}

	/// <summary>
	/// 刷新数据
	/// </summary>
	/// <param name="_skList"></param>
	public void RefreshItems(List<ShopItemInfo> _skList)
	{
		shopItemList.Clear();
		ShopItemContainers.Clear ();
		if (_skList != null)
		{
			for (int i = 0; i < _skList.Count; i++) {
				shopItemList.Add(_skList[i]);
			}
		}
		RefreshItems();
	}
	public void RefreshItems(Dictionary<int,ShopItemInfo> _Dic)
	{
		shopItemList.Clear();
		ShopItemContainers.Clear ();
		if (_Dic != null)
		{
            using (var e = _Dic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    shopItemList.Add(e.Current.Value);
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
		Object shopItemprefab = null;
		if (shopItemprefab == null)
		{
			shopItemprefab = exResources.GetResource(ResourceType.GUI, "Shop/ShopItem");
		}
		if (shopItemprefab== null)
		{
			GameSys.LogError("找不到预制：Shop/ShopItem");
			return ;
		}
        Vector3 V3 = Vector3.zero;
        for (int i = 0; i < shopItemList.Count; i++)
        {
            if (!ShopItemContainers.ContainsKey(i))
            {
                GameObject obj = Instantiate(shopItemprefab) as GameObject;
                Transform parentTransf = this.gameObject.transform;
                obj.transform.parent = parentTransf;
                obj.transform.localPosition = V3;
                obj.transform.localScale = Vector3.one;
                if ((i + 1) % 2 == 0)
                {
                    V3 = new Vector3(0, V3.y - 140, V3.z);
                }
                else
                {
                    V3 = new Vector3(V3.x + 220, V3.y, V3.z);
                }


                ShopItemUI shopItemUI = obj.GetComponent<ShopItemUI>();
                shopItemUI.FillInfo(shopItemList[i]);
                shopItemUI.OnSelectEvent += OnSelectSkillUI;
                ShopItemContainers[i] = shopItemUI;
                //                if (firstRun && skillList[i].SkillLv>0)
                //                {
                //                    firstRun = false;
                //                    selectSkillUI = skillUI;
                //                    UIToggle tog = obj.GetComponent<UIToggle>();
                //                    tog.value = true;
                //                    if (OnSelectItemEvent != null)
                //                    {
                //                        OnSelectItemEvent(selectSkillUI);
                //                    }
                //                }
            }
        }
	}
	/// <summary>
	/// 选中一个技能
	/// </summary>
	/// <param name="_itemUI"></param>
	void OnSelectSkillUI(ShopItemUI _itemUI)
	{
		//selectSkillUI = _itemUI;
		//if (OnSelectItemEvent != null)
	//	{
		//	OnSelectItemEvent(selectSkillUI);
		//}
	}
}
