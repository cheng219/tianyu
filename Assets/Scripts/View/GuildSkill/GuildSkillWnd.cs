//==================================
//作者：黄洪兴
//日期：2016/4/12
//用途：仙盟技能界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildSkillWnd : GUIBase {

    /// <summary>
    /// 贡献
    /// </summary>
    public UILabel contribute;

	public UILabel need2;
    public UILabel need2Label;
	public UILabel need1;
    public UILabel need1Label;
	/// <summary>
	/// 下一等级技能说明	
	/// </summary>
	public UILabel nextDes;
	/// <summary>
	/// 下一等级
	/// </summary>
	public UILabel nextLv;
	/// <summary>
	/// 当前技能说明
	/// </summary>
	public UILabel thisDes;
	/// <summary>
	/// 当前等级
	/// </summary>
	public UILabel thisLv;
	/// <summary>
	/// 技能图标
	/// </summary>
	public UISprite skillIcon;
	/// <summary>
	/// 技能名字
	/// </summary>
	public UILabel skillName;
	/// <summary>
	/// 技能容器
	/// </summary>
	public GuildSkillContainer ItemContainer;
	public GameObject CloseBtn;
	public GameObject grid;

	//MainPlayerInfo mainPlayerInfo = null;

	void Awake()
	{
		if (CloseBtn != null)
			UIEventListener.Get (CloseBtn).onClick = delegate {BtnClose ();};
		mutualExclusion = true;
	}
	protected override void OnOpen()
	{
		GameCenter.guildSkillMng.C2S_AskGuildSkillList ();
		base.OnOpen(); 
		RefreshItems ();
	}
	protected override void OnClose()
	{
		base.OnClose();
        //GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.GUILDSKILL, false);
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
			GameCenter.guildSkillMng.OnSkillListUpdate += RefreshItems;
			GameCenter.guildSkillMng.OnCurSkillUpdate += RefreshItems;
            GameCenter.mainPlayerMng.OnBaseValueChange += RefreshContribute;
		}
		else
		{
			GameCenter.guildSkillMng.OnSkillListUpdate -= RefreshItems;
			GameCenter.guildSkillMng.OnCurSkillUpdate -= RefreshItems;
            GameCenter.mainPlayerMng.OnBaseValueChange -= RefreshContribute;
		}
	}


    void RefreshContribute()
    {
        if (contribute != null)
        {
            contribute.text = GameCenter.mainPlayerMng.MainPlayerInfo.GuildContribution.ToString();
            //Debug.Log("刷新技能贡献度为" + GameCenter.mainPlayerMng.MainPlayerInfo.GuildContribution);
        }
    }


	void RefreshItems()
	{
        DestroyItem();
		ItemContainer.RefreshItems(GameCenter.guildSkillMng.GuildSkillDic);
		RefreshRight();
        RefreshContribute();
	}

	void DestroyItem()
	{
        if (grid != null)
        {
            grid.transform.DestroyChildren();
        }
	}
	void RefreshRight()
	{
		if (GameCenter.guildSkillMng.CurGuildSkill == null)
			return;
		if(skillName!=null)skillName.text=GameCenter.guildSkillMng.CurGuildSkill.Name+ConfigMng.Instance.GetUItext(289);
        if (skillIcon != null) skillIcon.spriteName = GameCenter.guildSkillMng.CurGuildSkill.Icon;// Debug.Log("技能图标的名字" + GameCenter.guildSkillMng.CurGuildSkill.Icon);
		if(thisLv!=null)thisLv.text=GameCenter.guildSkillMng.CurGuildSkill.Lev.ToString();
		if(thisDes!=null)thisDes.text=ConfigMng.Instance.GetUItext(290)+GameCenter.guildSkillMng.CurGuildSkill.Name+GameCenter.guildSkillMng.CurGuildSkill.Attrs[0].value.ToString()+"点";
		if(nextLv!=null)nextLv.text=GameCenter.guildSkillMng.CurGuildSkill.Lev+1>=100?"":(GameCenter.guildSkillMng.CurGuildSkill.Lev+1).ToString();
        if (nextDes != null)
        {
            GuildSkillRef Ref = ConfigMng.Instance.GetGuildSkillRef(GameCenter.guildSkillMng.CurGuildSkill.ID+1);
            if (GameCenter.guildSkillMng.CurGuildSkill.Des == ConfigMng.Instance.GetUItext(120))
            {
                nextDes.text = ConfigMng.Instance.GetUItext(120);
                if (need1 != null)
                {
                    need1.gameObject.SetActive(false);
                }
                if (need2 != null)
                {
                    need2.gameObject.SetActive(false);
                }
            }
            else
            {
                string[] words = { Ref.attrs[0].value.ToString() };
                nextDes.text = UIUtil.Str2Str(GameCenter.guildSkillMng.CurGuildSkill.Des, words);
            }
        }
        //GuildSkillRef nextRef = ConfigMng.Instance.GetGuildSkillRef(GameCenter.guildSkillMng.CurGuildSkill.ID+1);
            if (need1 != null)
            {
                need1.text = GameCenter.guildSkillMng.CurGuildSkill.Need1.ToString() + ConfigMng.Instance.GetUItext(288);
                if (GameCenter.guildMng.MyGuildInfo.GuildLv < GameCenter.guildSkillMng.CurGuildSkill.Need1)
                {
                    need1.text = "[FF0000]" + GameCenter.guildSkillMng.CurGuildSkill.Need1.ToString() + ConfigMng.Instance.GetUItext(288);
                    if (need1Label != null)
                        need1Label.text = ConfigMng.Instance.GetUItext(291);
                }
                else
                {
                    
                    if (need1Label != null)
                        need1Label.text = ConfigMng.Instance.GetUItext(292);
                }
            //if (need2 != null)
            //{
            //    need2.text = nextRef.need2.ToString();
            //    if (GameCenter.mainPlayerMng.MainPlayerInfo.GuildContribution < nextRef.need2)
            //    {
            //        need2.text = "[FF0000]" + nextRef.need2.ToString();
            //        if (need2Label != null)
            //            need2Label.text = "[FF0000]仙盟贡献:";
            //    }
            //    else
            //    {
            //        if (need2Label != null)
            //            need2Label.text = "仙盟贡献:";
            //    }
            //}
        }
		
	}



	void BtnClose(){
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        GameCenter.uIMng.SwitchToUI(GUIType.GUILDMAIN);
	}



	void SellItem(GameObject obj)
	{
		//	GameCenter.buyMng.OpenBuyWnd (GUIType.SHOPWND);	
	}


}
