//======================================================
//作者:何明军
//日期:2016/6/28
//用途:转生中技能UI
//======================================================
using UnityEngine;
using System.Collections;

public class ReinSkillUI : MonoBehaviour {

	public UISprite Icon;
	public UILabel name;
	public UILabel lev;
	public UILabel des;
	public UILabel mp;
	public UILabel typeName;
	public GameObject isNull;
	public GameObject NoNull;
	SkillInfo info =null;
	public SkillInfo CurSkillInfo{
		get{
			return info;
		}
		set{
			info = value;
			ShowSkill();
		}
	}
	
	void ShowSkill(){
		if(isNull != null)isNull.SetActive(info == null);
		if(NoNull != null)NoNull.SetActive(info != null);
		
		if(info == null)return ;
		if(typeName != null)typeName.text = info.CurSkillTypeName;
		if(Icon != null)Icon.spriteName = info.SkillIcon;
		if(name != null)name.text = ConfigMng.Instance.GetUItext(359) + info.SkillName;
		if(lev != null)lev.text = info.SkillLv.ToString();
		if(des != null)des.text = info.SkillDes;
		if(mp != null)mp.text = info.mpNeed.ToString();
	}
	
	void OnEnable(){
		ShowSkill();
	}
}
