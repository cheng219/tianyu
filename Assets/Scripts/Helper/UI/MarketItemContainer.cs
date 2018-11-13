//=========================================
//作者：黄洪兴
//日期：2016/04/12
//用途：市场拍卖物品的容器
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 市场拍卖物品的容器
/// </summary>
public class MarketItemContainer : MonoBehaviour {

    
	/// <summary>
	/// 是否是我的拍卖
	/// </summary>
	public  bool isMyMarketItem = false;
	/// <summary>
	/// 选择市场拍卖物品的事件
	/// </summary>
	public System.Action<MarketItemUI> OnSelectItemEvent = null;
	/// <summary>
	/// 拍卖的物品
	/// </summary>
	public List<MarketItemInfo> marketItemList = new List<MarketItemInfo>();

	public Dictionary<int, MarketItemUI> MarketItemContainers = new Dictionary<int,MarketItemUI>();
	// Use this for initialization
	void Start () {
	}




    /// <summary>
    /// 刷新数据
    /// </summary>
    /// <param name="_skList"></param>
    public void RefreshItems(Dictionary<int, List<MarketItemInfo>> _skList)
    {
        marketItemList.Clear();
        MarketItemContainers.Clear();
        if (_skList != null)
        {
            using (var e=_skList.GetEnumerator())
            {
                while(e.MoveNext())
                {
                    for (int i = 0; i < e.Current.Value.Count; i++)
                    {
                        marketItemList.Add(e.Current.Value[i]);
                    }

                }

            }
        }
        RefreshItems();
    }


	/// <summary>
	/// 刷新数据
	/// </summary>
	/// <param name="_skList"></param>
	public void RefreshItems(List<MarketItemInfo> _skList)
	{
		marketItemList.Clear();
        MarketItemContainers.Clear();
		if (_skList != null)
		{
			for (int i = 0; i < _skList.Count; i++) {
				marketItemList.Add(_skList[i]);
			}
		}
		RefreshItems();
	}
	public void RefreshItems(Dictionary<int,MarketItemInfo> _Dic)
	{
		marketItemList.Clear();
        MarketItemContainers.Clear();
		if (_Dic != null)
		{
            using (var e = _Dic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    marketItemList.Add(e.Current.Value);
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
		Object marketItemprefab = null;
		if (marketItemprefab == null)
		{
			if (isMyMarketItem) {
				marketItemprefab = exResources.GetResource (ResourceType.GUI, "Market/MyMarketItem");
			} else {
				marketItemprefab = exResources.GetResource (ResourceType.GUI, "Market/MarketItem");
			}
		}
		if (marketItemprefab== null)
		{
			GameSys.LogError("找不到预制：Market/MarketItem或者Market/MyMarketItem");
			return ;
		}
		Vector3 V3 = Vector3.zero;
		for(int i = 0;i<marketItemList.Count;i++)
		{
			if (!MarketItemContainers.ContainsKey(i))
			{
				GameObject obj = Instantiate(marketItemprefab) as GameObject;
				Transform parentTransf = this.gameObject.transform;
				obj.transform.parent = parentTransf;
				obj.transform.localPosition = V3;
				obj.transform.localScale = Vector3.one;
					V3 = new Vector3 (V3.x, V3.y - 125, V3.z);
				MarketItemUI marketItemUI = obj.GetComponent<MarketItemUI>();
				marketItemUI.FillInfo(marketItemList[i]);
				//marketTypeUI.OnSelectEvent += OnSelectSkillUI;
				MarketItemContainers[i] =marketItemUI;
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
        marketItemprefab = null;
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="_itemUI"></param>
	void OnSelectSkillUI(MarketItemUI _itemUI)
	{
		//selectSkillUI = _itemUI;
		//if (OnSelectItemEvent != null)
		//	{
		//	OnSelectItemEvent(selectSkillUI);
		//}
	}
}
