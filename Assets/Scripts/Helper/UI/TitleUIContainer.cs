//=========================================
//作者：黄洪兴
//日期：2016/03/22
//用途：称号UI的容器
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 技能UI的容器
/// </summary>
public class TitleUIContainer : MonoBehaviour {

	/// <summary>
	/// 是否是第一次运行
	/// </summary>
	protected bool firstRun = true;
	/// <summary>
	/// 选择称号的事件
	/// </summary>
	public System.Action<TitleUI> OnSelectItemEvent = null;
	/// <summary>
	/// 当前选中的称号
	/// </summary>
	protected TitleUI selectTitleUI = null;
	/// <summary>
	/// 称号列表
	/// </summary>
	public List<TitleInfo> titleList = new List<TitleInfo>();
	/// <summary>
	/// 排列
	/// </summary>
	public UIGrid grid;

	public Dictionary<int, TitleUI> TitleContainers = new Dictionary<int, TitleUI>();
	// Use this for initialization
	void Start () {

	}

	/// <summary>
	/// 刷新数据
	/// </summary>
	/// <param name="_skList"></param>
	public void RefreshItems(List<TitleInfo> _List)
	{
		if (_List == null)
			return;
		if (_List != null)
		{
			titleList.Clear();
            for (int i = 0; i < _List.Count; i++)
            {
                titleList.Add(_List[i]);
            }
		}
		RefreshItems();
	}
	public void RefreshItems(Dictionary<int,TitleInfo> _Dic)
	{
		titleList.Clear();
		if (_Dic != null)
		{
            using (var e = _Dic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    titleList.Add(e.Current.Value);
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
		Object titleprefab = null;
		if (titleprefab== null)
		{
			titleprefab = exResources.GetResource(ResourceType.GUI, "Player_information/Title_Item");

		}
		if (titleprefab == null)
		{
			GameSys.LogError("找不到预制：Player_information/Title_Item");
			return ;
		}
		for(int i = 0;i<titleList.Count;i++)
		{
			if (!TitleContainers.ContainsKey(i))
			{
				GameObject obj = Instantiate(titleprefab) as GameObject;
				Transform parentTransf = this.gameObject.transform;
				obj.GetComponent<UIDragScrollView> ().scrollView = this.gameObject.GetComponentInParent<UIScrollView> ();
				obj.transform.parent = parentTransf;
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localScale = Vector3.one;
				TitleUI titleUI = obj.GetComponent<TitleUI>();
				titleUI.FillInfo(titleList[i]);
				titleUI.OnSelectEvent += OnSelectSkillUI;
				TitleContainers[i] = titleUI;
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
				TitleContainers[i].FillInfo(titleList[i]);
			}
		}
        titleprefab = null;
		if(grid != null)
		{
			grid.Reposition();
		}
	}
	/// <summary>
	/// 升级刷新当前技能单元
	/// </summary>
	/// <param name="_info"></param>
	public void RefreshItem(TitleInfo _info)
	{
		selectTitleUI.FillInfo(_info);
	}
	/// <summary>
	/// 选中一个称号
	/// </summary>
	/// <param name="_itemUI"></param>
	void OnSelectSkillUI(TitleUI _itemUI)
	{
		selectTitleUI = _itemUI;
		if (OnSelectItemEvent != null)
		{
			OnSelectItemEvent(selectTitleUI);
		}
	}
}
