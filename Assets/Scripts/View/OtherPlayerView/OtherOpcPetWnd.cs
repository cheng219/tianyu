//==============================================
//作者：邓成
//日期：2016/4/28
//用途：查看其他玩家宠物信息窗口
//=================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class OtherOpcPetWnd : SubWnd {

	#region 宠物
	public UILabel labPetName;
	public UILabel labQuality;
	public UILabel labGrowth;
	public UILabel labPetLv;
	public UILabel labPetValue;
	public UITexture petTexture;
    private UIGrid grid;
    public SingleLearnedSkill singleSkill;
    private FDictionary petSkillList = new FDictionary(); 
    public UISpriteEx[] tianHunStarList;//天魂星星
    public UISpriteEx[] diHunStarList;//地魂星星
    public UISpriteEx[] mingHunStarList;//命魂星星 
    public UILabel[] propertyInfoVal; 
	#endregion
	#region 坐骑
	public UILabel mountName;
	public UITexture mountTexture; 
    public UILabel[] curPropertyLab;
    public GameObject effect4;//坐骑特效
    public GameObject effect7;
    public UISpriteEx []starSp;
	#endregion

	/// <summary>
	/// 当前人物数据
	/// </summary>
	private PlayerBaseInfo curTargetInfo;

	protected override void OnOpen ()
	{ 
		base.OnOpen ();
        if (singleSkill != null)
        {
            singleSkill.gameObject.SetActive(false);
            grid = singleSkill.transform.parent.GetComponent<UIGrid>();
        }
        RefreshAll();
	}
	protected override void OnClose ()
	{
		base.OnClose ();
        if (petTexture != null) petTexture.mainTexture = null;
        if (mountTexture != null) mountTexture.mainTexture = null;
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.previewManager.OnGotCurAskPlayerInfo += RefreshAll;
		}else
		{
			GameCenter.previewManager.OnGotCurAskPlayerInfo -= RefreshAll;
		}
	}
	void RefreshAll()
	{
		curTargetInfo = GameCenter.previewManager.CurAskPlayerInfo; 
		if(curTargetInfo == null)return;
		ShowPetInfo();
		ShowMountInfo();
	}

	void ShowPetInfo()
	{
        if (labPetName != null) labPetName.text = curTargetInfo.CurPetInfo == null ? ConfigMng.Instance.GetUItext(332) : curTargetInfo.CurPetInfo.PetName;
		if(curTargetInfo.CurPetInfo==null)
		{
			ShowNoPet();
			return;
		}
        AttributeRef attributeRef = ConfigMng.Instance.GetAttributeRef((int)curTargetInfo.Level);
        if (attributeRef.reborn > 0)
        {
            if (labPetLv != null) labPetLv.text = attributeRef.display_level.ToString();
        }
        else
        {
            if (labPetLv != null) labPetLv.text = curTargetInfo.Level.ToString();
        } 
		//if(labPetLv != null)labPetLv.text = curTargetInfo.Level.ToString();
		if(labGrowth != null)labGrowth.text = curTargetInfo.CurPetInfo.GrowUp.ToString();
		if(labPetValue != null)labPetValue.text = curTargetInfo.CurPetInfo.Power.ToString();
		if(labQuality != null)labQuality.text = curTargetInfo.CurPetInfo.Aptitude.ToString();
		if(petTexture != null)
		{
            GameCenter.previewManager.TryPreviewSingelEntourage(curTargetInfo.CurPetInfo, petTexture);
		}
        ShowSkill();
        if (singleSkill != null) singleSkill.gameObject.SetActive(false);
        if (tianHunStarList.Length > 0) ShowStar(tianHunStarList, curTargetInfo.CurPetInfo.Tian_soul);
        if (diHunStarList.Length > 0) ShowStar(diHunStarList, curTargetInfo.CurPetInfo.Di_soul);
        if (mingHunStarList.Length > 0) ShowStar(mingHunStarList, curTargetInfo.CurPetInfo.Life_soul);
        if (propertyInfoVal.Length >= 9) 
        {
            ShowPropertyVal(propertyInfoVal[0], propertyInfoVal[1], propertyInfoVal[2], curTargetInfo.CurPetInfo.PropertyList);
            ShowPropertyVal(propertyInfoVal[3], propertyInfoVal[4], propertyInfoVal[5], curTargetInfo.CurPetInfo.GrowUpPropertyList);
            ShowPropertyVal(propertyInfoVal[6], propertyInfoVal[7], propertyInfoVal[8], curTargetInfo.CurPetInfo.SoulPropertyList);  
        }
	}

	void ShowMountInfo()
	{
		if(mountName != null)mountName.text = (curTargetInfo.CurMountInfo != null)?curTargetInfo.CurMountInfo.Name:ConfigMng.Instance.GetUItext(333);
        if (curTargetInfo.CurMountInfo == null)
        {
            if (propertyInfoVal != null)
            { 
                for (int j = 0, max = curPropertyLab.Length; j < max; j++)
                {
                    curPropertyLab[j].text = "0";
                } 
            }
            ShowNoMount();
            return;
        }
		if(mountTexture != null)
		{
            GameCenter.previewManager.TryPreviewSingelMount(curTargetInfo.CurMountInfo, mountTexture); 
		} 
        RidePropertyRef mountPropertyRefData = ConfigMng.Instance.GetMountPropertyRef(curTargetInfo.CurMountInfo.Lev > 0 ? curTargetInfo.CurMountInfo.Lev : 1);
        if (propertyInfoVal != null)
        {
            if (mountPropertyRefData != null)
            {
                for (int j = 0, max = curPropertyLab.Length; j < max; j++)
                {
                    curPropertyLab[j].text = mountPropertyRefData.attr[j].ToString();
                }
            }
        }
        ShowStar((curTargetInfo.CurMountInfo.Lev-1) % 9);
	}
	void ShowNoPet()
	{
		if(labPetLv != null)labPetLv.text = "0";
		if(labGrowth != null)labGrowth.text = "0";
		if(labPetValue != null)labPetValue.text = "0";
		if(labQuality != null)labQuality.text = "0";
        if (singleSkill != null) singleSkill.gameObject.SetActive(false);
        foreach (SingleLearnedSkill ui in petSkillList.Values)
        {
            ui.gameObject.SetActive(false);
        }
        if (propertyInfoVal != null)
        {
            for (int i = 0, max = propertyInfoVal.Length; i < max; i++)
            {
                propertyInfoVal[i].text = "0";
            }
        }
        if (tianHunStarList != null) ShowNoStar(tianHunStarList);
        if (diHunStarList != null) ShowNoStar(diHunStarList);
        if (mingHunStarList != null) ShowNoStar(mingHunStarList);
	}
    void ShowNoStar(UISpriteEx[] _starList)
    {
        StarTypeRef starTypeRef = ConfigMng.Instance.GetStarTypeRef(1);
        if (starTypeRef != null)
        {
            for (int i = 0, max = _starList.Length; i < max; i++)
            {
                _starList[i].spriteName = starTypeRef.icon;
                _starList[i].IsGray = UISpriteEx.ColorGray.Gray;
            }
        } 
    }
    void ShowStar(UISpriteEx[] _starList, int _lev)
    {
        for (int val = 0; val < 32; val += 8)
        {
            StarTypeRef lastStarTypeRef = ConfigMng.Instance.GetStarTypeRef((val / 8 > 0) ? (val / 8) : 1);
            StarTypeRef starTypeRef = ConfigMng.Instance.GetStarTypeRef(val / 8 + 1);
            if (_lev > val && _lev <= val + 8 && lastStarTypeRef != null && starTypeRef != null)
            {
                for (int i = 0; i < _lev - val; i++)
                {
                    _starList[i].spriteName = starTypeRef.icon;
                    _starList[i].IsGray = UISpriteEx.ColorGray.normal;
                }
                for (int i = _lev - val; i < 8; i++)
                {
                    _starList[i].IsGray = UISpriteEx.ColorGray.normal;
                    if (val / 8 > 0) _starList[i].spriteName = lastStarTypeRef.icon;
                    else _starList[i].IsGray = UISpriteEx.ColorGray.Gray;
                }
            }
        }
    }
    void ShowPropertyVal(UILabel _propertyLab1, UILabel _propertyLab12, UILabel _propertyLab13, List<pet_property_list> _propertyList)
    { 
        for (int j = 0; j < _propertyList.Count; j++)
        {
            if (_propertyList[j].type == (int)PetProperty.PETATT)
                _propertyLab1.text = _propertyList[j].num.ToString();
            if (_propertyList[j].type == (int)PetProperty.PETHIT)
                _propertyLab12.text = _propertyList[j].num.ToString();
            if (_propertyList[j].type == (int)PetProperty.PETCRI)
                _propertyLab13.text = _propertyList[j].num.ToString();
        }
    }
    void ShowSkill()
    {
        List<uint> SkillList = curTargetInfo.CurPetInfo.SkillList; 
        foreach (SingleLearnedSkill ui in petSkillList.Values)
        {
            ui.gameObject.SetActive(false);
        }
        if (grid != null)
        {
            grid.maxPerLine = SkillList.Count;
            SingleLearnedSkill skill = null;
            for (int i = 0, max = SkillList.Count; i < max; i++)
            {
                int id = (int)SkillList[i];
                if (!petSkillList.ContainsKey(id))
                {
                    if (singleSkill != null)
                    {
                        GameObject go = (GameObject)GameObject.Instantiate(singleSkill.gameObject);
                        go.transform.parent = singleSkill.transform.parent;
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localScale = Vector3.one;
                        skill = go.GetComponent<SingleLearnedSkill>();
                        if (skill != null)
                        {
                            petSkillList[id] = skill;
                            skill.SkillRef = ConfigMng.Instance.GetPetSkillRef((int)id);
                        }
                        go.SetActive(true);
                    }
                }
                else
                {
                    SingleLearnedSkill petskill = petSkillList[id] as SingleLearnedSkill;
                    petskill.SkillRef = ConfigMng.Instance.GetPetSkillRef((int)id);
                    petskill.transform.localPosition = new Vector3();
                    petskill.gameObject.SetActive(true);
                }
            }
            grid.repositionNow = true;
        }
    }


    /// <summary>
    /// 坐骑等级的显示
    /// </summary>
    void ShowStar(int _lev)
    {
        if (effect4 != null) effect4.SetActive(false);
        if (effect7 != null) effect7.SetActive(false);  
        for (int i = 0, max = starSp.Length; i < max; i++)
        {
            if (i <= _lev)
            {
                if (i == 3 && effect4 != null) effect4.SetActive(true);
                if (i == 6 && effect7 != null) effect7.SetActive(true);
                starSp[i].IsGray = UISpriteEx.ColorGray.normal;
            }
            else
            {
                starSp[i].IsGray = UISpriteEx.ColorGray.Gray;
            }
        }
    }

    void ShowNoMount()
    {
        if (effect4 != null) effect4.SetActive(false);
        if (effect7 != null) effect7.SetActive(false);
        for (int i = 0, max = starSp.Length; i < max; i++)
        { 
           starSp[i].IsGray = UISpriteEx.ColorGray.Gray; 
        }
    }
}

