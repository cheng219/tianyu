//==================================
//作者：朱素云
//日期：2016/3/20
//用途：宠物技能ui
//=================================
using UnityEngine;
using System.Collections;

public class PetSkill : MonoBehaviour {

    public UIButton leanerdSkill;//学习后的技能按钮
    public Transform lockedSkill;//还没学习的技能
    public UILabel leanerdSkillName;//学习后的技能名字
    public UISprite leanerdSkillIcon;//技能图片名字 
    public UILabel unlockSkillLab;//解锁所需成长值或资质值
    public UISprite lockSp;
    public NcAutoDestruct effect;

    protected NewPetSkillNumRef skillNumRef;
    public NewPetSkillNumRef SkillNumRef
    {
        get
        {
            return skillNumRef;
        }
        set
        {
            if (value != null)
                skillNumRef = value; 
        }
    }
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
                skillRef = value;
            ShowlearnedInfo();
        }
    }
    /// <summary>
    /// 解锁所需成长值或资质值
    /// </summary>
    public void ShowUnlockSkillInfo()
    {
        if (lockedSkill != null) lockedSkill.gameObject.SetActive(true);
        if (unlockSkillLab != null) unlockSkillLab.gameObject.SetActive(true);
        if (leanerdSkill != null) leanerdSkill.gameObject.SetActive(false);
        if (skillNumRef != null)
        {
            if (skillNumRef.chengZhang <= 0 && skillNumRef.zizhi <= 0)
            {
                if (unlockSkillLab != null) unlockSkillLab.gameObject.SetActive(false);
                if (lockSp != null) lockSp.gameObject.SetActive(false);
            }
            if (skillNumRef.chengZhang > 0)
            {
                if (unlockSkillLab != null) unlockSkillLab.text = ConfigMng.Instance.GetUItext(94, new string[1] { skillNumRef.chengZhang.ToString() });
            }
            if (skillNumRef.zizhi > 0)
            {
                if (unlockSkillLab != null) unlockSkillLab.text = ConfigMng.Instance.GetUItext(95, new string[1] { skillNumRef.zizhi.ToString() });
            }
        }
    }
    public void ShowNoLock()
    {
        if (lockedSkill != null) lockedSkill.gameObject.SetActive(false);
        if (leanerdSkill != null) leanerdSkill.gameObject.SetActive(false);
    }
    /// <summary>
    /// 显示所学技能信息
    /// </summary>
    void ShowlearnedInfo()
    {
        if (skillRef != null)
        {
            if (effect != null) effect.gameObject.SetActive(true);
            if (lockedSkill != null) lockedSkill.gameObject.SetActive(false);
            if (leanerdSkill != null) leanerdSkill.gameObject.SetActive(true);
            if (leanerdSkillName != null) leanerdSkillName.text = skillRef.name;
            if (leanerdSkillIcon != null) leanerdSkillIcon.spriteName = skillRef.icon;
        } 
    }
}
