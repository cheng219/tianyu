//=====================================
//作者:黄洪兴
//日期:2016/4/14
//用途:仙盟技能组件
//========================================


using UnityEngine;
using System.Collections;
/// <summary>
/// 仙盟技能UI组件
/// </summary>
public class GuildSkillUI : MonoBehaviour
{
	public int skillMark=0;
	#region 控件数据
	/// <summary>
	/// 技能名字
	/// </summary>
	public UILabel skillName;
	/// <summary>
	/// 技能的图片
	/// </summary>
	public UISprite skillIcon;
	/// <summary>
	/// 技能等级
	/// </summary>
	public UILabel skillLev;
	/// <summary>
	/// 贡献消耗
	/// </summary>
	public UILabel cost;
	/// <summary>
	/// 选中图片
	/// </summary>
	public UISprite checkmark;
	public GameObject uPObj;
	public GameObject chooseObj;
	#endregion 
	#region 数据
	/// <summary>
	/// 选择事件
	/// </summary>
	public System.Action<GuildSkillUI> OnSelectEvent = null;
	/// <summary>
	/// 当前填充的数据
	/// </summary>
	public GuildSkillInfo guildSkillinfo;
	//	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
	#endregion
	// Use this for initialization
	void Start () {
		UIEventListener.Get (uPObj).onClick -= UpSkill;
		UIEventListener.Get (uPObj).onClick += UpSkill;
		UIEventListener.Get (chooseObj).onClick -= ChooseSkill;
		UIEventListener.Get (chooseObj).onClick += ChooseSkill;
		//		UIEventListener.Get(useBtn).onClick -= UseTitle;
		//		UIEventListener.Get(useBtn).onClick += UseTitle;

	}

	void UpSkill(GameObject obj)
	{
        GameCenter.guildSkillMng.CurSkillMark = skillMark;
        if (GameCenter.guildSkillMng.OnCurSkillUpdate != null)
            GameCenter.guildSkillMng.OnCurSkillUpdate();
		GameCenter.guildSkillMng.C2S_AskSkillUp (guildSkillinfo.ID);
		Debug.Log ("点击升级技能");
		//		GameCenter.titleMng.ChooseTitle = titleinfo;
		//		GameCenter.titleMng.UpdateTitle ();

	}

	void ChooseSkill(GameObject obj)
	{
		GameCenter.guildSkillMng.CurSkillMark = skillMark;
		if (GameCenter.guildSkillMng.OnCurSkillUpdate != null)
			GameCenter.guildSkillMng.OnCurSkillUpdate ();
		//		if (titleinfo.IsOwn) {
		//			if (GameCenter.titleMng.CurUseTitle == titleinfo) {
		//
		//				GameCenter.titleMng.C2S_UseTitle (titleinfo.ID, 0);
		//			} else {
		//				GameCenter.titleMng.C2S_UseTitle (titleinfo.ID, 1);
		//			}
		//		} else {
		//			GameCenter.messageMng.AddClientMsg ("该称号未获得");
		//		}
		//		GameCenter.titleMng.ChooseTitle = titleinfo;
		//		GameCenter.titleMng.UpdateTitle ();
		//
	}


	/// <summary>
	/// 填充数据
	/// </summary>
	/// <param name="_info"></param>
	public void FillInfo(GuildSkillInfo _info)
	{
		if (_info == null) {
			guildSkillinfo = null;
			return;
		} 
		else {
			guildSkillinfo = _info;

			//			oldSkillinfo = skillinfo;
		}
		RefreshShopItem ();
	}
	/// <summary>
	/// 刷新表现
	/// </summary>
	public void RefreshShopItem()
	{
		if (guildSkillinfo == null)
			return;
		if (skillName != null)
			skillName.text = guildSkillinfo.Name;
		if (skillIcon != null)
			skillIcon.spriteName = guildSkillinfo.Icon;
		if (skillLev != null)
			skillLev.text = guildSkillinfo.Lev.ToString () + ConfigMng.Instance.GetUItext(288);
		if (cost != null)
			cost.text = guildSkillinfo.Cost.ToString ();
		if (checkmark != null)
			checkmark.gameObject.SetActive (skillMark==GameCenter.guildSkillMng.CurSkillMark);
        if (guildSkillinfo.Des == ConfigMng.Instance.GetUItext(120))
        {
            if (uPObj != null)
            {
                uPObj.SetActive(false);
            }

        }   
		//		Debug.Log ("此时角色穿戴的称号为"+GameCenter.titleMng.CurUseTitle.ID);
		//		if (titleinfo != null) {
		//			titleName.text = titleinfo.Name;
		//			chooseObj.SetActive (isChoose);
		//			useObj.SetActive (isUse);
		//		} else {
		//
		//		}

	}
}
