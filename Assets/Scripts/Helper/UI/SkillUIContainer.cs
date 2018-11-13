//=========================================
//作者：黄洪兴
//日期：2016/03/01
//用途：技能UI的容器
//=========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 技能UI的容器
/// </summary>
public class SkillUIContainer : MonoBehaviour {

    /// <summary>
    /// 是否是第一次运行
    /// </summary>
    protected bool firstRun = true;
    /// <summary>
    /// 选择技能的事件
    /// </summary>
    public System.Action<SkillUI> OnSelectItemEvent = null;
    /// <summary>
    /// 当前选中的技能
    /// </summary>
    protected SkillUI selectSkillUI = null;
    /// <summary>
    /// 技能列表
    /// </summary>
    public List<SkillInfo> skillList = new List<SkillInfo>();
    /// <summary>
    /// 排列
    /// </summary>
	public UIExGrid grid;

	public Dictionary<int, SkillUI> SkillContainers = new Dictionary<int, SkillUI>();
	// Use this for initialization
	void Start () {
	
	}
	
    /// <summary>
    /// 刷新数据
    /// </summary>
    /// <param name="_skList"></param>
    public void RefreshItems(List<SkillInfo> _skList)
    {
        skillList.Clear();
        if (_skList != null)
        {

            for (int i = 0; i < _skList.Count; i++)
            {
                skillList.Add(_skList[i]); 
            }
        }
        RefreshItems();
    }
    public void RefreshItems(Dictionary<int,SkillInfo> _Dic)
    {
        skillList.Clear();
        if (_Dic != null)
        {
            using (var e = _Dic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    skillList.Add(e.Current.Value);
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
		if (this == null)
			return;
		Object skillprefab = null;
        if (skillprefab == null)
        {
			skillprefab = exResources.GetResource(ResourceType.GUI, "Player_information/Single_skillbar");
        }
        if (skillprefab == null)
        {
			GameSys.LogError("找不到预制：Player_information/Single_skillbar");
            return ;
        }
        for(int i = 0;i<skillList.Count;i++)
        {
            if (!SkillContainers.ContainsKey(i))
            {
				GameObject obj = Instantiate(skillprefab) as GameObject;
                Transform parentTransf = this.gameObject.transform;
                obj.transform.parent = parentTransf;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                SkillUI skillUI = obj.GetComponent<SkillUI>();
                skillUI.FillInfo(skillList[i]);
                skillUI.OnSelectEvent += OnSelectSkillUI;
                SkillContainers[i] = skillUI;
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
                SkillContainers[i].FillInfo(skillList[i]);
            }
        }
        if(grid != null)
        {
            grid.Reposition();
        }
    }
    /// <summary>
    /// 升级刷新当前技能单元
    /// </summary>
    /// <param name="_info"></param>
    public void RefreshItem(SkillInfo _info)
    {
        selectSkillUI.FillInfo(_info);
    }
    /// <summary>
    /// 选中一个技能
    /// </summary>
    /// <param name="_itemUI"></param>
    void OnSelectSkillUI(SkillUI _itemUI)
    {
        selectSkillUI = _itemUI;
        if (OnSelectItemEvent != null)
        {
            OnSelectItemEvent(selectSkillUI);
        }
    }
}
