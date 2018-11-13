//=========================================
//作者：黄洪兴
//日期：2016/04/12
//用途：市场大分页的容器
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 市场大分页的容器
/// </summary>
public class MarketTypeContainer : MonoBehaviour {

	/// <summary>
	/// 是否是第一次运行
	/// </summary>
	protected bool firstRun = true;
	/// <summary>
	/// 选择市场大分页的事件
	/// </summary>
	public System.Action<MarketTypeUI> OnSelectItemEvent = null;
	/// <summary>
	/// 当前选中的大分页
	/// </summary>
	protected MarketTypeUI selectMarketType = null;
	/// <summary>
	/// 分页列表
	/// </summary>
	public List<MarketTypeInfo> marketTypeList = new List<MarketTypeInfo>();

    public List<MarketTypeUI> MarketTypeContainers = new List<MarketTypeUI>();
	// Use this for initialization
	void Start () {

	}

	/// <summary>
	/// 刷新数据
	/// </summary>
	/// <param name="_skList"></param>
	public void RefreshItems(List<MarketTypeInfo> _skList)
	{
		marketTypeList.Clear();
		MarketTypeContainers.Clear ();
		if (_skList != null)
		{
			for (int i = 0; i < _skList.Count; i++) {
				marketTypeList.Add(_skList[i]);
			}
		}
		RefreshItems();
	}
	public void RefreshItems(Dictionary<int,MarketTypeInfo> _Dic)
	{
		marketTypeList.Clear();
		MarketTypeContainers.Clear ();
		if (_Dic != null)
		{
            using (var e = _Dic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    marketTypeList.Add(e.Current.Value);
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
		Object marketTypeprefab = null;
		if (marketTypeprefab == null)
		{
			marketTypeprefab = exResources.GetResource(ResourceType.GUI, "Market/MarketType");
		}
		if (marketTypeprefab== null)
		{
			GameSys.LogError("找不到预制：Market/MarketType");
			return ;
		}
		Vector3 V3 = Vector3.zero;
        MarketTypeContainers.Clear();
		for(int i = 0;i<marketTypeList.Count;i++)
		{
				GameObject obj = Instantiate(marketTypeprefab) as GameObject;
				Transform parentTransf = this.gameObject.transform;
				obj.transform.parent = parentTransf;
				obj.transform.localPosition = V3;
				obj.transform.localScale = Vector3.one;
				if (!marketTypeList [i].ShowPage) {
					V3 = new Vector3 (V3.x, V3.y - 57, V3.z);
				} else {
					V3 = new Vector3 (V3.x, V3.y - (marketTypeList [i].Pagename.Count+1) * 57, V3.z);
				}
				MarketTypeUI marketTypeUI = obj.GetComponent<MarketTypeUI>();
                marketTypeUI.typeID = i + 1;
				marketTypeUI.FillInfo(marketTypeList[i]);
				//marketTypeUI.OnSelectEvent += OnSelectSkillUI;
				MarketTypeContainers.Add(marketTypeUI);
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
        marketTypeprefab = null;
	}


    public void UpdateItems(int _id,bool _b)
    {
        if (MarketTypeContainers.Count>=_id)
        {
            if(marketTypeList.Count>_id-1)
            {
                float f = (marketTypeList[_id - 1].Pagename.Count) * 57;
                if (_b)
                {
                    for (int i = 0; i < MarketTypeContainers.Count; i++)
                    {
                        if (i >= _id)
                        {
                            Vector3 V = MarketTypeContainers[i].transform.localPosition;
                            MarketTypeContainers[i].transform.localPosition = new Vector3(V.x, V.y - f, V.z);
                        }
                        MarketTypeContainers[i].FillInfo(marketTypeList[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < MarketTypeContainers.Count; i++)
                    {
                        if (i >= _id)
                        {
                            Vector3 V = MarketTypeContainers[i].transform.localPosition;
                            MarketTypeContainers[i].transform.localPosition = new Vector3(V.x, V.y + f, V.z);
                        }
                        MarketTypeContainers[i].FillInfo(marketTypeList[i]);
                    }
                }

            }

        }

    }


	/// <summary>
	/// 选中一个技能
	/// </summary>
	/// <param name="_itemUI"></param>
	void OnSelectSkillUI(MarketTypeUI _itemUI)
	{
		//selectSkillUI = _itemUI;
		//if (OnSelectItemEvent != null)
		//	{
		//	OnSelectItemEvent(selectSkillUI);
		//}
	}
}
