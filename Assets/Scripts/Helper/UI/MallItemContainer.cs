//=========================================
//作者：黄洪兴
//日期：2016/4/6
//用途：商城商品UI的容器
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 商城商品UI的容器
/// </summary>
public class MallItemContainer : MonoBehaviour {

	/// <summary>
	/// 是否是第一次运行
	/// </summary>
	protected bool firstRun = true;
	/// <summary>
	/// 选择商品的事件
	/// </summary>
	public System.Action<MallItemUI> OnSelectItemEvent = null;
	/// <summary>
	/// 当前选中的商品
	/// </summary>
	protected MallItemUI selectShopItem = null;
	/// <summary>
	/// 商品列表
	/// </summary>
	public List<MallItemInfo> mallItemList = new List<MallItemInfo>();
	/// <summary>
	/// 排列
	/// </summary>
	public UIGrid grid;

	public Dictionary<int, MallItemUI> MallItemContainers = new Dictionary<int, MallItemUI>();
	// Use this for initialization
	void Start () {

	}

	/// <summary>
	/// 刷新数据
	/// </summary>
	/// <param name="_skList"></param>
	public void RefreshItems(List<MallItemInfo> _List)
	{
		mallItemList.Clear();
		MallItemContainers.Clear ();
		if (_List != null)
		{
			for (int i = 0; i < _List.Count; i++) {
				mallItemList.Add(_List[i]);
			}
		}
		RefreshItems();
	}
	public void RefreshItems(Dictionary<int,MallItemInfo> _Dic)
	{
		mallItemList.Clear();
		MallItemContainers.Clear ();
//		Debug.Log ("重置后的长度为"+mallItemList.Count);
		if (_Dic != null)
		{
            using (var e = _Dic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    mallItemList.Add(e.Current.Value);
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
		Object mallItemprefab = null;
		if (mallItemprefab == null)
		{
			mallItemprefab = exResources.GetResource(ResourceType.GUI, "Mall/MallItem");
		}
		if (mallItemprefab== null)
		{
			GameSys.LogError("找不到预制：Mall/MallItem");
			return ;
		}
		for(int i = 0;i<mallItemList.Count;i++)
		{
		//	Debug.Log ("此时的长度为"+mallItemList.Count+MallItemContainers.Count);
			if (!MallItemContainers.ContainsKey(i))
			{
				GameObject obj = Instantiate(mallItemprefab) as GameObject;
				Transform parentTransf = this.gameObject.transform;
				obj.transform.parent = parentTransf;
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localScale = Vector3.one;
				MallItemUI mallItemUI = obj.GetComponent<MallItemUI>();
				mallItemUI.FillInfo(mallItemList[i]);
				mallItemUI.OnSelectEvent += OnSelectSkillUI;
				MallItemContainers[i] = mallItemUI;
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
			else
			{
				MallItemContainers[i].FillInfo(mallItemList[i]);
			}
		}
        mallItemprefab = null;
		if(grid != null)
		{
			grid.enabled= true;
			grid.Reposition();
		}
	}
	/// <summary>
	/// 选中一个技能
	/// </summary>
	/// <param name="_itemUI"></param>
	void OnSelectSkillUI(MallItemUI _itemUI)
	{
		//selectSkillUI = _itemUI;
		//if (OnSelectItemEvent != null)
		//	{
		//	OnSelectItemEvent(selectSkillUI);
		//}
	}
}
