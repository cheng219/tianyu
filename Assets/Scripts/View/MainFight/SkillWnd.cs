//==============================================
//作者：黄洪兴
//日期：2016/6/27
//用途：技能释放栏
//=================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillWnd : GUIBase
{
    /// <summary>
    /// 技能按钮
    /// </summary>
    public SkillItem[] SkillButton = new SkillItem[4];
    /// <summary>
    /// 攻击按钮
    /// </summary>
    public GameObject attackButton;
	public UISprite attackIcon;
    /// <summary>
    /// 切换攻击目标按钮
    /// </summary>
    public GameObject ChangeAttButton;
    /// <summary>
    /// 挂机按钮
    /// 特效加入，按钮改成toggle -by ms
    /// </summary>
    public GameObject autoAttackBtn;

    public GameObject mountBtn;

    public GameObject effect;

    public UIButton rideBtn;

    /// <summary>
    /// 药品不足提示按钮
    /// </summary>
    public GameObject drugBtn;

    /// <summary>
    /// 系统设置按钮
    /// </summary>
    public GameObject settingBtn;
	/// <summary>
	/// 退出场景按钮(熔恶之地)
	/// </summary>
	public GameObject btnExit;

    /// <summary>
    /// 主城隐藏
    /// </summary>
    public GameObject cityDisplay;
    /// <summary>
    /// 翅膀技能
    /// </summary>
    public GameObject wingSkillGo;
    /// <summary>
    /// 翅膀技能图片
    /// </summary>
    public UISprite wingSkillSp;
    public UIFxAutoActive wingFx;
    /// <summary>
    /// 新技能获得动画
    /// </summary>
    public TweenPosition NewSkillGetAnimation;

    public UIFxAutoActive newSkillEffect;

    public UISprite newSkillSprite;

    Vector3 AnimationPoint=Vector3.zero;
    void Awake()
    {   
		if(btnExit != null)UIEventListener.Get(btnExit.gameObject).onClick = ClickExit;
        if (rideBtn != null) UIEventListener.Get(rideBtn.gameObject).onClick = delegate { GameCenter.uIMng.SwitchToUI(GUIType.SPRITEANIMAL); };
        if (NewSkillGetAnimation != null)
        {
            AnimationPoint = NewSkillGetAnimation.transform.localPosition;
            NewSkillGetAnimation.AddOnFinished(ShowEffect);
        }
        if (mountBtn != null)
        {
            UIEventListener.Get(mountBtn).onClick -= OnClickMountBtn;
            UIEventListener.Get(mountBtn).onClick += OnClickMountBtn;
        }
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        if (drugBtn != null)
            drugBtn.SetActive(false);
        if (GameCenter.curGameStage.SceneType == SceneType.CITY && cityDisplay != null)
        {
            cityDisplay.gameObject.SetActive(false);
        }
		SceneUiType sceneUiType = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType;
		switch(sceneUiType)
		{
		case SceneUiType.RONGELAND:
		case SceneUiType.GUILDFIRE:
		case SceneUiType.GUILDPROTECT:
        case SceneUiType.BATTLEFIGHT:
		case SceneUiType.UNDERBOSS:
		case SceneUiType.LIRONGELAND:
		case SceneUiType.GUILDSIEGE:
        case SceneUiType.HANGUPCOPPYFIRSTFLOOR:
        case SceneUiType.HANGUPCOPPYSECONDFLOOR:
			if(btnExit != null)btnExit.gameObject.SetActive(true);
			break;
        case SceneUiType.GUILDWAR:
            if(btnExit != null)btnExit.gameObject.SetActive(true);
			break;
		default:
			if(btnExit != null)btnExit.gameObject.SetActive(false);
			break;
		}
        RefreshSkill();
        SetSysOpenFun();
        OnAutoFightStateUpdate(GameCenter.curMainPlayer.IsInAutoFight);
    }

    protected override void OnClose()
    {
        base.OnClose();
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (SkillButton[0] != null) UIEventListener.Get(SkillButton[0].gameObject).onClick += OnClickSkillBtn0;
            if (SkillButton[1] != null) UIEventListener.Get(SkillButton[1].gameObject).onClick += OnClickSkillBtn1;
            if (SkillButton[2] != null) UIEventListener.Get(SkillButton[2].gameObject).onClick += OnClickSkillBtn2;
            if (SkillButton[3] != null) UIEventListener.Get(SkillButton[3].gameObject).onClick += OnClickSkillBtn3;
            if (attackButton != null) UIEventListener.Get(attackButton.gameObject).onClick += OnClickAttackBtn;
            if (ChangeAttButton != null) UIEventListener.Get(ChangeAttButton.gameObject).onClick += OnClickChangeAttBtt;
            if (autoAttackBtn != null) UIEventListener.Get(autoAttackBtn.gameObject).onClick += OnClickAutoAttBtn;
            if (settingBtn != null) UIEventListener.Get(settingBtn).onClick += OnClickSettingBtn;
            if (drugBtn != null) UIEventListener.Get(drugBtn).onClick += OnClickDrugBtn;
            GameCenter.skillMng.OnUpdateSkillList += RefreshSkill;
			GameCenter.skillMng.OnChangeSkill += RefreshSkill;
            GameCenter.shopMng.ShowDrugBtn += ShowDrugBtn;
            PlayerAutoFightFSM.OnAutoFightStateUpdate += OnAutoFightStateUpdate;
			PlayerAutoDartFSM.OnAutoDartStateUpdate += OnAutoFightStateUpdate;
            GameCenter.abilityMng.OnPassiveWingSkillTrigger += RefreshWingSkillSp;
            GameCenter.skillMng.OnPlayNewSkillGetAnimation = NewSkillGetAnimationStart;
//            GameCenter.mainPlayerMng.UpdateOpenFun += SysFunOpen;
        }
        else
        {
            if (SkillButton[0] != null) UIEventListener.Get(SkillButton[0].gameObject).onClick -= OnClickSkillBtn0;
            if (SkillButton[1] != null) UIEventListener.Get(SkillButton[1].gameObject).onClick -= OnClickSkillBtn1;
            if (SkillButton[2] != null) UIEventListener.Get(SkillButton[2].gameObject).onClick -= OnClickSkillBtn2;
            if (SkillButton[3] != null) UIEventListener.Get(SkillButton[3].gameObject).onClick -= OnClickSkillBtn3;
            if (attackButton != null) UIEventListener.Get(attackButton.gameObject).onClick -= OnClickAttackBtn;
            if (ChangeAttButton != null) UIEventListener.Get(ChangeAttButton.gameObject).onClick -= OnClickChangeAttBtt;
            if (autoAttackBtn != null) UIEventListener.Get(autoAttackBtn.gameObject).onClick -= OnClickAutoAttBtn;
            if (settingBtn != null) UIEventListener.Get(settingBtn).onClick -= OnClickSettingBtn;
            if (drugBtn != null) UIEventListener.Get(drugBtn).onClick -= OnClickDrugBtn;
            GameCenter.skillMng.OnUpdateSkillList -= RefreshSkill;
            GameCenter.shopMng.ShowDrugBtn -= ShowDrugBtn;
            PlayerAutoFightFSM.OnAutoFightStateUpdate -= OnAutoFightStateUpdate;
			PlayerAutoDartFSM.OnAutoDartStateUpdate -= OnAutoFightStateUpdate;
			GameCenter.skillMng.OnChangeSkill -= RefreshSkill;
            GameCenter.abilityMng.OnPassiveWingSkillTrigger -= RefreshWingSkillSp;
            GameCenter.skillMng.OnPlayNewSkillGetAnimation -= NewSkillGetAnimationStart;
//            GameCenter.mainPlayerMng.UpdateOpenFun -= SysFunOpen;
        }
    }


    /// <summary>
    /// 技能更新刷新技能栏
    /// </summary>
    void RefreshSkill()
    { 
        for (int i = 0, max = SkillButton.Length; i < max; i++)
        {
            SkillButton[i].FillInfo(null, false);
        }
		if(SkillButton != null)
		{
            List<SkillInfo> list = GameCenter.skillMng.GetShowSkill(); 
			for (int i = 0,max=SkillButton.Length; i < max; i++) 
			{
				if(SkillButton[i] == null)break;
                //bool isCanUse = GameCenter.skillMng.useSkills.Count > i;
                ////SkillInfo skill = GameCenter.skillMng.useSkills.Count > i ?GameCenter.skillMng.useSkills[i] : null;
                //SkillInfo skill = GameCenter.skillMng.useSkills[i];
                //int ID = skill == null ?0:skill.SkillID;
                //if (GameCenter.skillMng.abilityDic.ContainsKey(ID) && skill != null)
                //{
                //    if (SkillButton[i].skillInfo == null || SkillButton[i].skillInfo.SkillID != skill.SkillID || skill.SkillLv != skill.SkillLv)
                //    {
                //        SkillButton[i].AddCDTime(GameCenter.skillMng.abilityDic[ID]);
                //        GameCenter.skillMng.abilityDic[ID].HasConfirmSkill -= SkillButton[i].EnterCooling;
                //        GameCenter.skillMng.abilityDic[ID].HasConfirmSkill += SkillButton[i].EnterCooling;
                //        SkillButton[i].FillInfo(skill, isCanUse);
                //    }
                //}else
                //{  
                //    SkillButton[i].FillInfo(skill, isCanUse);
                //}  
                SkillInfo skill = list.Count > i ? list[i] : null;
                int ID = skill == null ? 0 : skill.SkillID; 
                if (GameCenter.skillMng.abilityDic.ContainsKey(ID) && skill != null)
                { 
                    if (SkillButton[i].skillInfo == null || SkillButton[i].skillInfo.SkillID != skill.SkillID || skill.SkillLv != skill.SkillLv)
                    {  
                        GameCenter.skillMng.abilityDic[ID].HasConfirmSkill -= SkillButton[i].EnterCooling;
                        GameCenter.skillMng.abilityDic[ID].HasConfirmSkill += SkillButton[i].EnterCooling;
                        SkillButton[i].FillInfo(skill,true);
                        SkillButton[i].AddCDTime(GameCenter.skillMng.abilityDic[ID]);
                    }
                }
                else
                { 
                    SkillButton[i].FillInfo(skill,false);
                }
			}
		}
		if(attackIcon != null)
		{
			string attackIconName = string.Empty;
			switch(GameCenter.mainPlayerMng.MainPlayerInfo.Prof)
			{
			case 1:
				attackIconName = "B_J_Z_26";
				break;
			case 2:
				attackIconName = "B_J_Z_27";
				break;
			case 3:
				attackIconName = "B_J_Z_28";
				break;
			}
			attackIcon.spriteName = attackIconName;
		}
    }
    /// <summary>
    /// 技能按钮0响应
    /// </summary>
    /// <param name="obj"></param>
    void OnClickSkillBtn0(GameObject obj)
    {
        OnClickSkillBtnActon(0);
    }
    /// <summary>
    /// 技能按钮响应
    /// </summary>
    /// <param name="obj"></param>
    void OnClickSkillBtn1(GameObject obj)
    {
        OnClickSkillBtnActon(1);
    }
    /// <summary>
    /// 技能按钮响应
    /// </summary>
    /// <param name="obj"></param>
    void OnClickSkillBtn2(GameObject obj)
    {
        OnClickSkillBtnActon(2);
    }
    /// <summary>
    /// 技能按钮响应
    /// </summary>
    /// <param name="obj"></param>
    void OnClickSkillBtn3(GameObject obj)
    {
        OnClickSkillBtnActon(3);
    } 
    public void OnClickSkillBtnActon(int _btnIndex)
    {

        if (PlayerInputListener.isDragingRockerItem)//如果在拖动摇杆移动不能释放技能 add by黄洪兴
        {
            return;
        }
        if (!GameCenter.curMainPlayer.inputListener.CheckLock())
        {
            GameCenter.messageMng.AddClientMsg(58);
            return; //如果是锁定操作状态，那么不能使用技能 add by吴江
        }
        //测试技能 
        if (!SkillButton[_btnIndex].HadLearn)
        {
            GameCenter.messageMng.AddClientMsg(42);
            return;
        }
        if (!SkillButton[_btnIndex].HadMp)
        {
            GameCenter.messageMng.AddClientMsg(43);
            return;
        }
        if (SkillButton[_btnIndex].IsDown != true)
        {
            if (GameCenter.curMainPlayer.isRigidity) return;
			if (SkillButton [_btnIndex].skillInfo == null)
				return;
			int ID = SkillButton [_btnIndex].skillInfo.SkillID;
            //AbilityInstance curInstance = GameCenter.curMainPlayer.curTryUseAbility;
            AbilityInstance thisInstance = GameCenter.skillMng.abilityDic[ID];
			if (GameCenter.curMainPlayer.IsSilent && thisInstance.thisSkillMode == SkillMode.CLIENTSKILL)
			{
				GameCenter.messageMng.AddClientMsg(59);
				return; //如果是沉默状态，那么不能使用技能 add by吴江
			}
            //if (curInstance != null && curInstance == thisInstance && curInstance.TargetActor == GameCenter.curMainPlayer.CurTarget && !curInstance.HasServerConfirm)
            //{
            //    return;
            //}
            GameCenter.curMainPlayer.CancelCommands();
            thisInstance.ResetResult(GameCenter.curMainPlayer.CurTarget as SmartActor);
            thisInstance.SetActor(GameCenter.curMainPlayer, GameCenter.curMainPlayer.CurTarget as SmartActor);
			GameCenter.curMainPlayer.TryUseAbility(thisInstance, true);
//            if (GameCenter.curMainPlayer.AttakType != MainPlayer.AttackType.AUTOFIGHT)
//            {
//                GameCenter.curMainPlayer.GoNormalFight();
//            }
        }
    }

    /// <summary>
    /// 攻击按钮响应
    /// </summary>
    /// <param name="obj"></param>
    protected void OnClickAttackBtn(GameObject _obj)
    {
		if (PlayerInputListener.isDragingRockerItem)//如果在拖动摇杆移动不能释放技能 add by黄洪兴
		{
			return;
		}
        //测试技能
        if (GameCenter.curMainPlayer.isRigidity || GameCenter.curMainPlayer.IsProtecting) return;
		if (GameCenter.curMainPlayer.CurTarget == null)
			GameCenter.curMainPlayer.TryGetColosestAttackTarget(RelationType.AUTOMATEDATTACKS, 10.0f);
		if(GameCenter.curMainPlayer.CurTarget != null&&(GameCenter.curMainPlayer.CurTarget as SmartActor==true))
			GameCenter.curMainPlayer.HitTargetOnce((SmartActor)GameCenter.curMainPlayer.CurTarget);
    }
    /// <summary>
    /// 切换攻击目标按钮响应
    /// </summary>
    /// <param name="obj"></param>
    protected void OnClickChangeAttBtt(GameObject _obj)
    {
        GameCenter.curMainPlayer.ChangeTarget();
    }


    protected void OnClickAutoAttBtn(GameObject _obj)
    {
        GameCenter.curMainPlayer.CancelCommands();
		if (GameCenter.curMainPlayer.CurFSMState != MainPlayer.EventType.AI_DART_CTRL && GameCenter.curMainPlayer.CurFSMState != MainPlayer.EventType.AI_FIGHT_CTRL)
        {
            GameCenter.curMainPlayer.GoAutoFight();
        }
        else
        {
			GameCenter.curMainPlayer.ExitAIFight();
        }
    }

    protected void OnAutoFightStateUpdate(bool _state)
    {
        effect.SetActive(_state);
        if (autoAttackBtn != null)
        {
            UIToggle tog = autoAttackBtn.GetComponent<UIToggle>();
            if (tog != null)
            {
                tog.value = _state;
            }
        }
    }

    protected void OnClickMountBtn(GameObject _go)
    {
        if (GameCenter.newMountMng.curMountInfo != null)
        {
            GameCenter.newMountMng.C2S_ReqRideMount(GameCenter.newMountMng.curMountInfo.IsRiding ? ChangeMount.DOWNRIDE : ChangeMount.RIDEMOUNT, GameCenter.newMountMng.curMountInfo.ConfigID, MountReqRideType.AUTO);
        }
    }
    protected void OnClickSettingBtn(GameObject _obj)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.SYSTEMSETTING);
    }

    protected void OnClickDrugBtn(GameObject _obj)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.DRUGLACKWND);
        if (drugBtn != null)
            drugBtn.SetActive(false);
    }


    void ShowDrugBtn()
    {
        //Debug.Log("ShowDrugBtn ");
        if (drugBtn != null)
        {
            if(GameCenter.shopMng.ShowedHPDrugBtn || GameCenter.shopMng.ShowedMPDrugBtn)
                drugBtn.SetActive(true);
            else
                drugBtn.SetActive(false);
        }


    }

    /// <summary>
    /// 设置功能
    /// </summary>
    protected void SetSysOpenFun()
    {
//		if(mountBtn != null)mountBtn.gameObject.SetActive(GameCenter.mainPlayerMng.IsOpenFun(FunctionType.Mount));
    }
    /// <summary>
    /// 功能开启
    /// </summary>
//    protected void SysFunOpen(int ID)
//    {
//        FunctionType type = (FunctionType)ConfigMng.Instance.GetSysOpenRef(ID).sysId;
//        if (type == FunctionType.Entourage)
//        {
//            mountBtn.gameObject.SetActive(true);
//        }
//    }
    /// <summary>
    /// 打开npc对话框时隐藏
    /// </summary>
    /// <param name="_state"></param>
    void SetThisActive(bool _state)
    {
        this.gameObject.SetActive(_state);
    } 

	void ClickExit(GameObject go)
	{
	//	GameCenter.endLessTrialsMng.C2S_OutCopy();
        int sceneId = GameCenter.mainPlayerMng.MainPlayerInfo.SceneID;
        if (sceneId == 100021 || sceneId == 100023 || sceneId == 160011 || sceneId == 160012)
		    GameCenter.curMainPlayer.GoTraceTarget(100001,72,66);//自动寻路到长安
        else
			GameCenter.duplicateMng.OutCopyWnd();
	}

    protected Dictionary<int, float> wingSkillTipTime = new Dictionary<int, float>();
    /// <summary>
    /// 刷新翅膀技能 by鲁家旗
    /// </summary>
    /// <param name="_skillId"></param>
    void RefreshWingSkillSp(int _skillId)
    {
        if(wingSkillGo != null)
            wingSkillGo.SetActive(true);
        SkillMainConfigRef skill = ConfigMng.Instance.GetSkillMainConfigRef(_skillId);
        if (skill != null && wingSkillSp != null)
            wingSkillSp.spriteName = skill.skillIcon;
        SkillLvDataRef skillLv = ConfigMng.Instance.GetSkillLvDataRef(_skillId,1);
        if (!wingSkillTipTime.ContainsKey(_skillId) || (skillLv != null && Time.time - wingSkillTipTime[_skillId] > skillLv.cd))
        {
            MessageST mst = new MessageST();
            mst.messID = 368;
            mst.words = new string[2] { GameCenter.wingMng.CurUseWingInfo != null ? GameCenter.wingMng.CurUseWingInfo.WingName : string.Empty, skill.skillName };
            GameCenter.messageMng.AddClientMsg(mst);
            wingSkillTipTime[_skillId] = Time.time;
        }
        if (wingFx != null)
            wingFx.ShowFx();
        CancelInvoke("HideSkillSp");
        Invoke("HideSkillSp", 3.5f);
    }
    void HideSkillSp()
    {
        if (wingSkillGo != null)
            wingSkillGo.SetActive(false);
    }


    void NewSkillGetAnimationStart(int _id)
    {
        if (NewSkillGetAnimation != null)
        {
            for (int i = 0; i < SkillButton.Length; i++)
            {
                if (SkillButton[i].skillInfo!=null&&SkillButton[i].skillInfo.SkillID == _id)
                {
                    NewSkillGetAnimation.transform.localPosition = AnimationPoint;
                    if (newSkillSprite != null)
                        newSkillSprite.spriteName = SkillButton[i].skillInfo.SkillIcon;
                    NewSkillGetAnimation.gameObject.SetActive(true);
                    NewSkillGetAnimation.to = SkillButton[i].gameObject.transform.parent.transform.localPosition;
                    NewSkillGetAnimation.from = AnimationPoint;
                    NewSkillGetAnimation.ResetToBeginning();
                    NewSkillGetAnimation.PlayForward();
                }
            }
        }

    }


    void ShowEffect()
    {

        if (newSkillEffect != null)
            newSkillEffect.ShowFx(NewSkillGetAnimationEnd);
    }

    void NewSkillGetAnimationEnd()
    {
        NewSkillGetAnimation.gameObject.SetActive(false);
    }




}
