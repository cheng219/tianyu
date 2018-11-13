//=========================================
//作者：黄洪兴
//日期：2016/04/12
//用途：仙盟技能UI的容器
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 仙盟技能UI的容器
/// </summary>
public class GuildSkillContainer : MonoBehaviour {

	/// <summary>
	/// 是否是第一次运行
	/// </summary>
	protected bool firstRun = true;
	/// <summary>
	/// 选择商品的事件
	/// </summary>
	public System.Action<GuildSkillUI> OnSelectItemEvent = null;
	/// <summary>
	/// 当前选中的商品
	/// </summary>
	protected GuildSkillUI selectGuildShopSkill = null;
	/// <summary>
	/// 商品列表
	/// </summary>
	public List<GuildSkillInfo> guildSkillList = new List<GuildSkillInfo>();
	/// <summary>
	/// 排列
	/// </summary>
	public UIGrid grid;

	public Dictionary<int, GuildSkillUI> GuildSkillContainers = new Dictionary<int, GuildSkillUI>();
	// Use this for initialization
	void Start () {

	}

	/// <summary>
	/// 刷新数据
	/// </summary>
	/// <param name="_skList"></param>
	public void RefreshItems(List<GuildSkillInfo> _skList)
	{
		guildSkillList.Clear();
		GuildSkillContainers.Clear ();
		if (_skList != null)
		{
			for (int i = 0; i < _skList.Count; i++) {
				guildSkillList.Add(_skList[i]);
			}
		}
		RefreshItems();
	}
	public void RefreshItems(Dictionary<int,GuildSkillInfo> _Dic)
	{
		guildSkillList.Clear();
		GuildSkillContainers.Clear ();
		if (_Dic != null)
		{
            using (var e = _Dic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    guildSkillList.Add(e.Current.Value);
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
		Object guildSkillprefab = null;
		if (guildSkillprefab == null)
		{
			guildSkillprefab = exResources.GetResource(ResourceType.GUI, "Guild/GuildSkillItem");
		}
		if (guildSkillprefab== null)
		{
			GameSys.LogError("找不到预制：Guild/GuildSkillItem");
			return ;
		}
		Vector3 V3 = Vector3.zero;
		for(int i = 0;i<guildSkillList.Count;i++)
		{
			if (!GuildSkillContainers.ContainsKey(i))
			{
				GameObject obj = Instantiate(guildSkillprefab) as GameObject;
				Transform parentTransf = this.gameObject.transform;
				obj.transform.parent = parentTransf;
				obj.transform.localPosition = V3;
				obj.transform.localScale = Vector3.one;
				if ((i+1) % 2 == 0) {
					V3 = new Vector3 (0, V3.y - 130, V3.z);
				} else {
					V3 = new Vector3 (V3.x+270,V3.y,V3.z);
				}


				GuildSkillUI guildSkillUI = obj.GetComponent<GuildSkillUI>();
				guildSkillUI.skillMark = i;
				guildSkillUI.FillInfo(guildSkillList[i]);
				guildSkillUI.OnSelectEvent += OnSelectSkillUI;
				GuildSkillContainers[i] = guildSkillUI;
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
        guildSkillprefab = null;
	}
	/// <summary>
	/// 选中一个技能
	/// </summary>
	/// <param name="_itemUI"></param>
	void OnSelectSkillUI(GuildSkillUI _itemUI)
	{
		//selectSkillUI = _itemUI;
		//if (OnSelectItemEvent != null)
		//	{
		//	OnSelectItemEvent(selectSkillUI);
		//}
	}
}
