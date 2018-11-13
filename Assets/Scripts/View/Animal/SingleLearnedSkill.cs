//==================================
//作者：朱素云
//日期：2016/3/28
//用途：宠物学习了的技能ui
//=================================
using UnityEngine;
using System.Collections;

public class SingleLearnedSkill : MonoBehaviour 
{
    public UILabel skillName;//学习后的技能名字
    public UISprite skillIcon;//技能图片名字  
    protected NewPetSkillRef skillRef;
    public NewPetSkillRef SkillRef
    {
        get
        {
            return skillRef;
        }
        set
        {
            if (value != null)
			{
				skillRef = value;
            	ShowlearnedInfo();
			}else
			{
				ShowEmptySkill();
			}
        }
    }
    public static SingleLearnedSkill CeateNew(int _index, int _id,GameObject gameParent)
    { 
        Object prefab = null;
        if (prefab == null)
        {
            prefab = exResources.GetResource(ResourceType.GUI, "SpiritAnimal/skill");
        }
        if (prefab == null)
        {
            GameSys.LogError("找不到已学技能预制");
            return null;
        }
        GameObject obj = Instantiate(prefab) as GameObject;
        obj.transform.parent = gameParent.transform; 
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        prefab = null;
        SingleLearnedSkill itemUI = obj.GetComponent<SingleLearnedSkill>();
        if (itemUI == null)
        {
            GameSys.LogError("预制上找不到组件：<SingleLearnedSkill>");
            return null;
        }
        itemUI.SkillRef = ConfigMng.Instance.GetPetSkillRef(_id); 
        return itemUI;
    }
    /// <summary>
    /// 创造被动技能
    /// </summary> 
    public static SingleLearnedSkill CeateBDSkill(int _index, int _id, GameObject gameParent)
    {
        NewPetSkillRef petSkillRef = ConfigMng.Instance.GetPetSkillRef(_id);
        if (petSkillRef != null)
        {
            if (petSkillRef.beidong == 2 || petSkillRef.beidong == 3 || petSkillRef.beidong == 4)
            { 
                 return CeateNew(_index, _id, gameParent); 
            }
        }
        return null;
    } 
    void ShowlearnedInfo()
    {
        if (SkillRef != null)
        {
            if (skillName != null) skillName.text = SkillRef.name;
            if (skillIcon != null)
            {
                skillIcon.enabled = true;
                skillIcon.spriteName = SkillRef.icon;
            }
        }
    }
	void ShowEmptySkill()
	{
		if(skillName != null)skillName.text = "";
		if(skillIcon != null)skillIcon.enabled = false;
	}
}
