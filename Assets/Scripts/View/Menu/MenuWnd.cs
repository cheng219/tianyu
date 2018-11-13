//==============================================
//作者：邓成
//日期：2016/3/3
//用途：菜单窗口
//=================================================

using UnityEngine;
using AnimationOrTween;
using System.Collections;

public class MenuWnd : SubWnd
{
    
    #region UI控件对象
    /// <summary>
    /// 角色按钮
    /// </summary>
    public UIButton PlayerButton;
	/// <summary>
	/// 背包按钮
	/// </summary>
	public UIButton PacketButton;
	/// <summary>
	/// 锻造按钮
	/// </summary>
	public UIButton StrengthButton;
	/// <summary>
	/// 宠物按钮
	/// </summary>
	public UIButton petButton;
	/// <summary>
	/// 仙盟按钮
	/// </summary>
	public UIButton GuildButton;
	/// <summary>
	/// 社交按钮
	/// </summary>
	public UIButton CampButton;
	/// <summary>
	/// 排行榜按钮
	/// </summary>
	public UIButton rankButton;
	/// <summary>
	/// 法宝按钮
	/// </summary>
	public UIButton magicButton;
    #endregion

    protected override void OnOpen()
    {
        base.OnOpen();
        //GameCenter.curMainPlayer.CancelCommands();
        //ActiveAnimation.Play(this.GetComponent<Animation>(), "Menu_dong", Direction.Forward, EnableCondition.DoNothing, DisableCondition.DoNotDisable);

    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
//            GameCenter.mainPlayerMng.UpdateOpenFun += SysOpenFunction;
        }
        else
        {
//            GameCenter.mainPlayerMng.UpdateOpenFun -= SysOpenFunction;
        }

    }

    void Awake()
    {
        if (PlayerButton != null) UIEventListener.Get(PlayerButton.gameObject).onClick = OnClickPlayerBtn;
        if (PacketButton != null) UIEventListener.Get(PacketButton.gameObject).onClick = OnClickPacketBtn;
		if (StrengthButton != null) UIEventListener.Get(StrengthButton.gameObject).onClick = OnClickStrengthBtn;
        if (GuildButton != null) UIEventListener.Get(GuildButton.gameObject).onClick = OnClickGuildBtn;
        if (CampButton != null) UIEventListener.Get(CampButton.gameObject).onClick = OnClickCampBtn;
        if (rankButton != null) UIEventListener.Get(rankButton.gameObject).onClick = OnClickRankBtn; 
        if (petButton != null) UIEventListener.Get(petButton.gameObject).onClick = OnClickPetBtn;
        if (magicButton != null) UIEventListener.Get(magicButton.gameObject).onClick = OnClickMagicButton;
    }
       

    #region 控件事件
    /// <summary>
    /// 单击角色按钮
    /// </summary>
    /// <param name="obj"></param>
    void OnClickPlayerBtn(GameObject obj)
    {
		
        GameCenter.uIMng.SwitchToUI(GUIType.INFORMATION);
    }
    /// <summary>
    /// 单击背包按钮
    /// </summary>
    /// <param name="obj"></param>
    void OnClickPacketBtn(GameObject obj)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.BACKPACK);
    }
	/// <summary>
	/// 单击强化按钮
	/// </summary>
	/// <param name="obj"></param>
	void OnClickStrengthBtn(GameObject obj)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.EQUIPMENTTRAINING);
	}
    /// <summary>
    /// 单击工会按钮
    /// </summary>
    /// <param name="obj"></param>
    void OnClickGuildBtn(GameObject obj)
    {
		if (GameCenter.mainPlayerMng.MainPlayerInfo.HavaGuild == false)
        {
			GameCenter.uIMng.SwitchToUI(GUIType.GUILDLIST);//没有公会时
        }
        else
        {
            GameCenter.uIMng.SwitchToUI(GUIType.GUILDMAIN);//有公会时
        }
    }

    void OnClickMagicButton(GameObject obj)
	{
        GameCenter.uIMng.SwitchToUI(GUIType.MAGICWEAPON);
	}

	




    void OnClickPetBtn(GameObject obj)
    {
		GameCenter.uIMng.SwitchToUI(GUIType.SPRITEANIMAL);
    }

    /// <summary>
    /// 单击社交按钮
    /// </summary>
    /// <param name="obj"></param>
    void OnClickCampBtn(GameObject obj)
    {
		GameCenter.uIMng.SwitchToUI(GUIType.MAIL);
    }
    /// <summary>
    /// 单击空白区域
    /// </summary>
    /// <param name="obj"></param>
    void OnClickBlankSpaceBtn(GameObject obj)
    {
        //ActiveAnimation.Play(this.GetComponent<Animation>(), "Menu_dong", Direction.Reverse, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
        GameCenter.uIMng.SwitchToUI(GUIType.LITTLEMAP);
        GameCenter.uIMng.SwitchToUI(GUIType.TASK);
        GameCenter.uIMng.SwitchToUI(GUIType.MAINFIGHT);
    }
    /// <summary>
    /// 点击排行榜按钮
    /// </summary>
    /// <param name="_obj"></param>
    void OnClickRankBtn(GameObject _obj)
    {
        //GameCenter.uIMng.SwitchToUI(GUIType.NEWRANKING);
        if (GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.RANKING))
        {
            GameCenter.uIMng.SwitchToSubUI(SubGUIType.NEWRANKINGSUBWND);
        }
        else
        {
            GameCenter.uIMng.SwitchToSubUI(SubGUIType.ACHIEVEMENT);
        }
    }
		
    #endregion

    /// <summary>
    /// 设置功能
    /// </summary>
    protected void SetSysOpenFun()
    {

    }
    /// <summary>
    /// 功能开启
    /// </summary>
    protected void SysOpenFunction(int ID)
    {
//        FunctionType type = (FunctionType)ConfigMng.Instance.GetSysOpenRef(ID).sysId;
//        switch (type)
//        {
//            case FunctionType.Forging:
//                SmithyButton.SetOpenState(true);
//                break;
//            case FunctionType.Mount:
//                mountButton.SetOpenState(true);
//                break;
//            case FunctionType.Pet:
//                petButton.SetOpenState(true);
//                break;
//            case FunctionType.Entourage:
//                SoldiersButton.SetOpenState(true);
//                break;
//            case FunctionType.Bag:
//                PacketButton.SetOpenState(true);
//                break;
//            case FunctionType.Rand:
//                rankButton.SetOpenState(true);
//                break;
//            case FunctionType.Relic:
//                relicButton.SetOpenState(true);
//                break;
//            case FunctionType.Role:
//                PlayerButton.SetOpenState(true);
//                break;
//            case FunctionType.Setting:
//                SettingButton.SetOpenState(true);
//                break;
//            case FunctionType.Skill:
//                SkillButton.SetOpenState(true);
//                break;
//            case FunctionType.Task:
//                TaskButton.SetOpenState(true);
//                break;
//            case FunctionType.Union:
//                GuildButton.SetOpenState(true);
//                break;
//            case FunctionType.Camp:
//                CampButton.SetOpenState(true);
//                break;
//            default:
//                break;
//        }
    }
}
