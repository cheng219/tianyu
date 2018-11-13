//==============================================
//作者：黄洪兴
//日期：2016/3/1
//用途：技能学习界面
//=================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 技能学习界面
/// </summary>
public class SkillUpgradesWnd : SubWnd
{
    #region UI控件对象
	public UILabel skillModel;
	public SkillUIContainer skillContainer;
    public SkillUIContainer bdSkillContainer;
	public SkillUIContainer useSkillContainer;
	public SkillUIContainer showSkillContainer;
	public List<SkillInfo> useSkills;
	public Dictionary<int,SkillInfo> skillDic;
	public UILabel showSkillName;
	public UILabel showSkillLv;
	public UILabel showSkillState;
	public UILabel showSkillDes;
    public UILabel TheSkillMagic;
    public UILabel skillColdTime;
    public GameObject endLessObj;
    public GameObject needObj;
    public UILabel upSkillLabel;

//	public SkillUI curSkill;
//	public List<SkillUI> curSkillList;

//	public List<SkillUI> SkillList;
    public GameObject SkillObj;
  // public GameObject MasteryObj;
   // public GameObject VaillantObj;
    /// <summary>
    /// 当前技能描述
    /// </summary>
    public UILabel TheSkillDes;
    /// <summary>
    /// 下一级技能描述
    /// </summary>
    public UILabel NextSkillDes;
    public GameObject NextSkillDesObj;
    /// <summary>
    /// 升级等级需求
    /// </summary>
    public UILabel LvUpLvNeed;
    /// <summary>
    /// 升级金钱需求
    /// </summary>
    public UILabel LvUpCoin;
    /// <summary>
    /// 升级悟性需求
    /// </summary>
    public UILabel LvUpPow;
    /// <summary>
    /// 升级技能按钮
    /// </summary>
    public UIButton UpdateSkill;
	/// <summary>
	/// 拖动技能时的图片
	/// </summary>
	public GameObject moveSkill;
	#endregion
    /// <summary>
    /// 当前技能引用
    /// </summary>
   // public SkillInfo curskillinfo;

	/// <summary>
	/// 起始拖动技能的技能槽
	/// </summary>
	protected GameObject dropSkillObj;
	/// <summary>
	/// 标识对技能不同的操作
	/// </summary>
	protected bool press=false;
	protected bool release=false;
	protected bool dropSkill=false;
	protected bool dropedSkill=false;

	protected float starTime;
	protected float nowTime;
	protected Vector2 starPoint;
	protected Vector2 nowPoint;
	protected float distance=0;
    protected int configID = 0;
    protected bool coinEnough
    {
        get
        {
            if (nextSkill == null)
                return true;
           return (ulong)ConfigMng.Instance.GetSkillMainLvRef(nextSkill.SkillID, nextSkill.SkillLv).learnCoin <= GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
        }
    }
    protected bool skillresEnough
    {
        get
        {
            if (nextSkill == null)
                return true;
            return ConfigMng.Instance.GetSkillMainLvRef(nextSkill.SkillID, nextSkill.SkillLv).learnSp <= GameCenter.mainPlayerMng.MainPlayerInfo.SkillRes;
        }
    }
    protected bool levEnough
    {
        get
        {
            if (nextSkill == null)
                return true;
            return ConfigMng.Instance.GetSkillMainLvRef(nextSkill.SkillID, nextSkill.SkillLv).learnLv <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel;
        }
    }
    protected SkillInfo nextSkill;
    void Awake() 
    {
		//curskillinfo = GameCenter.skillMng.CurSkillInfo;
        type = SubGUIType.SKILLUPWND;
		useSkillContainer.RefreshItems (GameCenter.skillMng.useSkills);
        bdSkillContainer.RefreshItems(GameCenter.skillMng.GetSkillBySkillType(2));
		skillContainer.RefreshItems (GameCenter.skillMng.GetSkillBySkillType(1));
		if (GameCenter.skillMng.CurSkillInfo == null)
        {
            if (skillContainer.skillList.Count > 0)
            {
                GameCenter.skillMng.CurSkillInfo = skillContainer.skillList[0];
            }
            else
            {
                if (bdSkillContainer.skillList.Count > 0)
                {
                    GameCenter.skillMng.CurSkillInfo = bdSkillContainer.skillList[0];
                }
            }
		}
		List < SkillInfo > list= new List<SkillInfo> ();
		list.Add (GameCenter.skillMng.CurSkillInfo);
		showSkillContainer.RefreshItems (list);
		showSkillContainer.gameObject.GetComponentInChildren<BoxCollider> ().enabled =false;
		for (int j = 0; j <skillContainer.SkillContainers.Count; j++) {
            UIEventListener.Get(skillContainer.SkillContainers[j].gameObject).onPress -= PressSkill;
			UIEventListener.Get (skillContainer.SkillContainers[j].gameObject).onPress += PressSkill;
			skillContainer.SkillContainers[j].isEnable = true;
		}

        for (int j = 0; j < bdSkillContainer.SkillContainers.Count; j++)
        {
            UIEventListener.Get(bdSkillContainer.SkillContainers[j].gameObject).onPress -= PressSkill;
            UIEventListener.Get(bdSkillContainer.SkillContainers[j].gameObject).onPress += PressSkill;
            bdSkillContainer.SkillContainers[j].isEnable = true;
        }

		for (int n = 0; n < useSkillContainer.SkillContainers.Count; n++) {
           //UIEventListener.Get(useSkillContainer.SkillContainers[n].gameObject).onDrop -= DropSkill;
            UIEventListener.Get(useSkillContainer.SkillContainers[n].gameObject).onPress -= PressSkill;
			//UIEventListener.Get (useSkillContainer.SkillContainers [n].gameObject).onDrop += DropSkill;
			UIEventListener.Get (useSkillContainer.SkillContainers [n].gameObject).onPress += PressSkill;
		}
        if (UpdateSkill != null) UIEventListener.Get(UpdateSkill.gameObject).onClick -= OnClickUpdateBtn;
        if (UpdateSkill != null) UIEventListener.Get(UpdateSkill.gameObject).onClick += OnClickUpdateBtn;
        if (endLessObj != null) UIEventListener.Get(endLessObj).onClick = GoToEndLess;
        nextSkill = new SkillInfo(GameCenter.skillMng.CurSkillInfo.SkillID, GameCenter.skillMng.CurSkillInfo.SkillLv + 1);
       // skillContainer.OnSelectItemEvent += RefreshDes;
    }
	void DropSkill(GameObject _go, GameObject _drop)
	{
		if (_drop!=null&&_drop.GetComponent<SkillUI> ()!=null&& _drop.GetComponent<SkillUI> ().skillinfo!=null)
        {
            if (_drop.GetComponent<SkillUI>().skillinfo.isEnable && _drop.GetComponent<SkillUI>().skillinfo.SkillType!=3)
            {
                ChangeSkill(_go, _drop);
                RefreshSkill();
                dropedSkill = true;
            }
		}
	}
	void PressSkill(GameObject _go, bool _flag)
	{
		dropSkillObj = _go;
		if (_flag&&_go.GetComponent<SkillUI> ().skillinfo!=null) {
			distance = 0;
			dropedSkill = false;
			GameCenter.skillMng.CurSkillInfo = _go.GetComponent<SkillUI> ().skillinfo;
			RefreshDes ();
                moveSkill.GetComponent<UISprite>().spriteName = dropSkillObj.GetComponent<SkillUI>().itemIcon.spriteName;
                starTime = Time.time;
                starPoint = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
				press = true;

		} 
		if(!_flag){
			press = false;
			moveSkill.SetActive (false);
		}
		RefreshSkill ();
		RefreshDes ();
	}		


    protected override void OnOpen()
    {
        base.OnOpen();
		//Debug.Log ("进入技能事件");

		GameCenter.skillMng.OnUpdateSkillList += UpdateOneSkill;
        GameCenter.skillMng.OnChangeSkill += UpDateSkillPoint;
		GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += UpdateLvToSkill;
        GameCenter.skillMng.C2S_SkillReq();
        GameCenter.mainPlayerMng.OnBaseValueChange += RereshRes;
		RefreshSkill ();
		RefreshDes ();
    }
    protected override void OnClose()
    {
        base.OnClose();
		GameCenter.skillMng.OnUpdateSkillList -= UpdateOneSkill;
        GameCenter.skillMng.OnChangeSkill -= UpDateSkillPoint;
		GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= UpdateLvToSkill;
        if (skillContainer.SkillContainers.Count > 0)
            GameCenter.skillMng.CurSkillInfo = skillContainer.SkillContainers[0].skillinfo;
        else
        { 
            if(bdSkillContainer.SkillContainers.Count > 0)
                GameCenter.skillMng.CurSkillInfo = bdSkillContainer.SkillContainers[0].skillinfo;
        }
        GameCenter.mainPlayerMng.OnBaseValueChange -= RereshRes;

    }

	void RereshRes()
	{
		if (LvUpCoin != null)
		{
            if (coinEnough)
            {
                LvUpCoin.text = nextSkill.LearnCoin + "/" + (GameCenter.mainPlayerMng.MainPlayerInfo.BindCoinCount + GameCenter.mainPlayerMng.MainPlayerInfo.UnBindCoinCount).ToString();
			}
			else{
                LvUpCoin.text = nextSkill.LearnCoin + "/" + "[FF0000]" + (GameCenter.mainPlayerMng.MainPlayerInfo.BindCoinCount + GameCenter.mainPlayerMng.MainPlayerInfo.UnBindCoinCount).ToString();
			}
			//Debug.Log("此时技能ID为" + GameCenter.skillMng.CurSkillInfo.SkillID.ToString() + "等级" + GameCenter.skillMng.CurSkillInfo.SkillLv.ToString());
		}
		if (LvUpPow != null)
		{
            if (skillresEnough)
            {
                LvUpPow.text = nextSkill.LearnSp + "/" + GameCenter.mainPlayerMng.MainPlayerInfo.SkillRes.ToString();
			} else {
                LvUpPow.text = nextSkill.LearnSp + "/" + "[FF0000]" + GameCenter.mainPlayerMng.MainPlayerInfo.SkillRes.ToString();
			}
		}
	}

    Vector3 mousePos = Vector3.zero;
    Vector3 targetPos = Vector3.zero;
	void Update()
	{
        //把技能图标拖动功能去掉   by zsy
		//Debug.Log (curskillinfo.SkillName);
        //nowTime = Time.time;
        //nowPoint = new Vector2 (Input.mousePosition.x/Screen.width,Input.mousePosition.y/Screen.height);
        //if (press&&nowTime - starTime > 0.15f)
        //{
        //    if (dropSkillObj != null)
        //    {
        //        if (dropSkillObj.GetComponent<SkillUI>().skillinfo.SkillType == 3 || !dropSkillObj.GetComponent<SkillUI>().skillinfo.isEnable)
        //            return;
        //    }
        //    mousePos = Input.mousePosition;
        //    mousePos.x = Mathf.Clamp01(mousePos.x / Screen.width);
        //    mousePos.y = Mathf.Clamp01(mousePos.y / Screen.height);
        //    mousePos.z = -0.5f;
        //    targetPos = GameCenter.cameraMng.uiCamera.ViewportToWorldPoint(mousePos);
        //    moveSkill.transform.position = new Vector3(targetPos.x, targetPos.y, 0);
        //    moveSkill.SetActive (true);
        //    distance = Vector2.Distance (starPoint, nowPoint);
        //}
        //if (!press &&!dropedSkill&&dropSkillObj!=null&&distance>0.1f) {
        //    if (!dropSkillObj.GetComponent<SkillUI> ().isEnable) 
        //    {
        //        dropSkillObj.GetComponent<SkillUI> ().FillInfo (null);
        //        dropedSkill = true;
        //        RefreshSkill();
        //        onChangeUseSkill();
        //    }
        //    distance = 0;
        //}
			
	}
	void ChangeSkill(GameObject _go,GameObject _target)
	{
		if (_target.GetComponent<SkillUI> () != null && _go.GetComponent<SkillUI> () != null && _target.GetComponent<SkillUI> ().skillinfo != null&&_target.GetComponent<SkillUI> ().skillinfo.isEnable) {
			if (_target.GetComponent<SkillUI> ().isEnable) {
				for (int i = 0; i < useSkillContainer.SkillContainers.Count; i++) {
					if (useSkillContainer.SkillContainers [i].skillinfo == _target.GetComponent<SkillUI> ().skillinfo) {
						useSkillContainer.SkillContainers [i].FillInfo (null);
					}
				}
				_go.GetComponent<SkillUI> ().FillInfo (_target.GetComponent<SkillUI> ().skillinfo);
			} else {
				SkillInfo info = _go.GetComponent<SkillUI> ().skillinfo;
				_go.GetComponent<SkillUI> ().FillInfo (_target.GetComponent<SkillUI> ().skillinfo);
				_target.GetComponent<SkillUI> ().FillInfo (info);
			}
		}
			onChangeUseSkill ();
			RefreshSkill ();
	}


    void UpDateSkillPoint()
    {
        useSkillContainer.RefreshItems(GameCenter.skillMng.useSkills);
        RefreshSkill();
        RefreshDes();

    }


    /// <summary>
    /// 升级导致的刷新表现
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="value"></param>
	private void UpdateLvToSkill(ActorBaseTag tag,ulong value,bool _fromAbility)
    {
        if (tag == ActorBaseTag.Level)
        {
            RefreshSkill(); 
            RefreshNeed();
        }
    }
    /// <summary>
    /// 技能改变
    /// </summary>
    void UpdateOneSkill()
    {
		useSkillContainer.RefreshItems (GameCenter.skillMng.useSkills);
		skillContainer.RefreshItems (GameCenter.skillMng.GetSkillBySkillType(1));
        bdSkillContainer.RefreshItems(GameCenter.skillMng.GetSkillBySkillType(2));
		List < SkillInfo > list= new List<SkillInfo> ();
		list.Add (GameCenter.skillMng.CurSkillInfo);
		showSkillContainer.RefreshItems (list);
		
        RefreshSkill();
		RefreshDes ();
        //curskillinfo = GameCenter.skillMng.CurSkillInfo;
        GameCenter.skillMng.SetSkillConfig(configID);
    }

    /// <summary>
    /// 刷新技能描述
    /// </summary>
    void RefreshDes()
    {

        if (GameCenter.skillMng.CurSkillInfo != null)
        {
            nextSkill = new SkillInfo(GameCenter.skillMng.CurSkillInfo.SkillID, GameCenter.skillMng.CurSkillInfo.SkillLv + 1);
            if (showSkillContainer != null) showSkillContainer.SkillContainers[0].FillInfo(GameCenter.skillMng.CurSkillInfo);
            if (showSkillName != null) showSkillName.text = GameCenter.skillMng.CurSkillInfo.SkillName;
            if (showSkillLv != null) showSkillLv.text = GameCenter.skillMng.CurSkillInfo.SkillLv.ToString() + ConfigMng.Instance.GetUItext(288);
            if (GameCenter.skillMng.CurSkillInfo.isEnable)
            {
                bool b = false;
                for (int i = 0; i < GameCenter.skillMng.useSkills.Count; i++)
                {
                    if (GameCenter.skillMng.useSkills[i] == null)
                        continue;
                    if (GameCenter.skillMng.useSkills[i].SkillID == GameCenter.skillMng.CurSkillInfo.SkillID)
                    {
                        b = true;
                    }
                }
                if (b)
                {
                    showSkillState.text = ConfigMng.Instance.GetUItext(92);
                }
                else
                {
                    showSkillState.text = ConfigMng.Instance.GetUItext(91); ;
                }
            }
            else
            {
                showSkillState.text = ConfigMng.Instance.GetUItext(93); ;
            }

            if (TheSkillDes != null)
            {
                if (GameCenter.skillMng.CurSkillInfo!=null)
                TheSkillDes.text = GameCenter.skillMng.CurSkillInfo.SkillDes;
            }
            if (nextSkill == null)
            {
                NextSkillDesObj.SetActive(false);
            }
            else
            {
                if (nextSkill.SkillDes == null)
                {
                    NextSkillDesObj.SetActive(false);
                }
                else
                {
                    if (NextSkillDes != null)
                    {
                        NextSkillDesObj.SetActive(true);
                        NextSkillDes.text = nextSkill.SkillDes;
                    }

                }
            }

            if (LvUpLvNeed != null)
            {
                LvUpLvNeed.text = ConfigMng.Instance.GetLevelDes(nextSkill.LearnLv);
                if (!nextSkill.LevEnough)
                {
                    LvUpLvNeed.text = "[FF0000]" + ConfigMng.Instance.GetLevelDes(nextSkill.LearnLv);
                }
            }
            if (LvUpCoin != null)
            {
                if (nextSkill.CoinEnough)
                {
                    LvUpCoin.text = nextSkill.LearnCoin + "/" + (GameCenter.mainPlayerMng.MainPlayerInfo.BindCoinCount + GameCenter.mainPlayerMng.MainPlayerInfo.UnBindCoinCount).ToString();
				}
				else{
                    LvUpCoin.text = nextSkill.LearnCoin + "/" + "[FF0000]" + (GameCenter.mainPlayerMng.MainPlayerInfo.BindCoinCount + GameCenter.mainPlayerMng.MainPlayerInfo.UnBindCoinCount).ToString();
				}
                //Debug.Log("此时技能ID为" + GameCenter.skillMng.CurSkillInfo.SkillID.ToString() + "等级" + GameCenter.skillMng.CurSkillInfo.SkillLv.ToString());
            }
            if (LvUpPow != null)
            {
                if (nextSkill.ResEnough)
                {
                    LvUpPow.text= nextSkill.LearnSp+ "/" + GameCenter.mainPlayerMng.MainPlayerInfo.SkillRes.ToString();
				} else {
                    LvUpPow.text = nextSkill.LearnSp + "/" + "[FF0000]" + GameCenter.mainPlayerMng.MainPlayerInfo.SkillRes.ToString();
				}
            }


            if (TheSkillMagic!=null)
            {
                TheSkillMagic.text = GameCenter.skillMng.CurSkillInfo.mpNeed.ToString();
            }

            if (skillColdTime != null)
            {
                skillColdTime.text = GameCenter.skillMng.CurSkillInfo.CD.ToString();
                if (GameCenter.skillMng.CurSkillInfo.CD > 0)
                {
                    skillColdTime.transform.parent.gameObject.SetActive(true); 
                }
                else
                {
                    skillColdTime.transform.parent.gameObject.SetActive(false);
                }
            }

			if (skillModel != null) 
			{
				if (GameCenter.skillMng.CurSkillInfo.SkillType == 3) {
					skillModel.text = ConfigMng.Instance.GetUItext(344);
				}
				else{
					skillModel.text = ConfigMng.Instance.GetUItext(345);
				}
			}

            if (needObj != null)
                needObj.SetActive(!GameCenter.skillMng.CurSkillInfo.isFullLevel);
            if (upSkillLabel != null)
                upSkillLabel.text = GameCenter.skillMng.CurSkillInfo.isFullLevel ? ConfigMng.Instance.GetUItext(120) : ConfigMng.Instance.GetUItext(346);
            if (UpdateSkill != null)
            {
                BoxCollider box = UpdateSkill.gameObject.GetComponent<BoxCollider>();
                if (box != null)
                    box.enabled=!GameCenter.skillMng.CurSkillInfo.isFullLevel;
            }

        }




    }
    /// <summary>
    /// 刷新需求
    /// </summary>
    void RefreshNeed()
    {
//        if (LvUpNeed != null)
//        {
//            UpdateSkill.gameObject.SetActive(false);
//            if (curskillinfo.SkillLv < curskillinfo.SkillLevelLimit)
//            {
//                UpdateSkill.gameObject.SetActive(true);
//                UpdateSkill.enabled = false;
//                SkillMainLvRef curRef = ConfigMng.Instance.GetSkillMainLvRef(curskillinfo.SkillID, curskillinfo.SkillLv + 1);
//                if (curRef.learnLv > GameCenter.mainPlayerMng.MainPlayerInfo.Level)
//                {
//                    LvUpNeed.text = "角色等级： " + "[FF0000]" + curRef.learnLv + "[-]级\n";
//                }
//                else
//                {
//                    LvUpNeed.text = "角色等级： " + curRef.learnLv + "[-]级\n";
//                }
//                if (curRef.learnCoin > 0)
//                {
//                    LvUpCoin.text = curRef.learnCoin.ToString();
//                }
//                if (curRef.learnSp > 0)
//                {
//                    LvUpNeed.text = LvUpNeed.text + "技能点：" + curRef.learnSp + "\n";
//                }
//
//            }
//            else
//            {
//                LvUpNeed.text = string.Empty;
//            }
//        }
    }
    /// <summary>
    /// 刷新技能表现和数据
    /// </summary>
    void RefreshSkill()
    {
		if (skillContainer == null || useSkillContainer == null || bdSkillContainer == null)
			return;
        nextSkill = new SkillInfo(GameCenter.skillMng.CurSkillInfo.SkillID, GameCenter.skillMng.CurSkillInfo.SkillLv + 1);
		//Debug.Log ("刷新技能界面");
       // useSkillContainer.RefreshItems(GameCenter.skillMng.useSkills);
		for (int i = 0; i < skillContainer.skillList.Count; i++) {
			for (int j = 0; j < useSkillContainer.skillList.Count; j++) {
                if (skillContainer.SkillContainers[i].skillinfo != null && useSkillContainer.SkillContainers[j].skillinfo!=null)
                {
                    if (skillContainer.SkillContainers[i].skillinfo.SkillID == useSkillContainer.SkillContainers[j].skillinfo.SkillID)
                    {
                        skillContainer.SkillContainers[i].isUse = true;
                        //Debug.Log ("第"+i+"个技能为使用状态");
                        j = useSkillContainer.skillList.Count;
                    }
				} else {
					skillContainer.SkillContainers [i].isUse = false;
				}
			}
			skillContainer.SkillContainers[i].RefreshSkill ();

		}

        for (int i = 0; i < bdSkillContainer.skillList.Count; i++)
        {
            for (int j = 0; j < useSkillContainer.skillList.Count; j++)
            {
                if (bdSkillContainer.SkillContainers[i].skillinfo != null && useSkillContainer.SkillContainers[j].skillinfo != null)
                {
                    if (bdSkillContainer.SkillContainers[i].skillinfo.SkillID == useSkillContainer.SkillContainers[j].skillinfo.SkillID)
                    {
                        bdSkillContainer.SkillContainers[i].isUse = true;
                        //Debug.Log ("第"+i+"个技能为使用状态");
                        j = useSkillContainer.skillList.Count;
                    }
                }
                else
                {
                    bdSkillContainer.SkillContainers[i].isUse = false;
                }
            }
            bdSkillContainer.SkillContainers[i].RefreshSkill();

        }

		for (int i = 0; i < useSkillContainer.SkillContainers.Count; i++) {
			useSkillContainer.SkillContainers[i].RefreshSkill ();
		}
    }
    #region 控件事件
    /// <summary>
    /// 切换功能
    /// </summary>
    /// <param name="obj"></param>
    void OnCheckChange(GameObject obj)
    {
//        if (SkillObj == null || MasteryObj == null || VaillantObj == null)
//        {
//            return;
//        }
    }
    void GoToEndLess(GameObject obj)
    {
        if (GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.ENDLESSTRIAL))
        {
            GameCenter.uIMng.SwitchToUI(GUIType.ENDLESSWND);
        }
        else
        {
            GameCenter.messageMng.AddClientMsg(406);
        }
        //        if (SkillObj == null || MasteryObj == null || VaillantObj == null)
        //        {
        //            return;
        //        }
    }

    /// <summary>
    /// 升级技能
    /// </summary>
    /// <param name="obj"></param>
    void OnClickUpdateBtn(GameObject obj)
    {
        if (nextSkill != null)
        {

            if (coinEnough && skillresEnough&&levEnough)
            {
                if (GameCenter.skillMng.CurSkillInfo != null)
                {
                    GameCenter.skillMng.C2S_SkillUp(GameCenter.skillMng.CurSkillInfo.SkillID);
                    //Debug.Log("发送升级技能ID" + GameCenter.skillMng.CurSkillInfo.SkillID);
                }
            }
            else
            {
                if (!levEnough)
                {
                    
                    GameCenter.messageMng.AddClientMsg(13);
                }

                else if (!coinEnough)
                {
                    //string[] str = { ConfigMng.Instance.GetEquipmentRef(6).name };
                    GameCenter.messageMng.AddClientMsg(155);
                }
                else if(!skillresEnough)
                {
                    //string[] str = { ConfigMng.Instance.GetEquipmentRef(7).name };
                    GameCenter.messageMng.AddClientMsg(230);
                }
            }
        }
		//GameCenter.uIMng.SwitchToUI (GUIType.GUILDFIGHT);
    }
	void onChangeUseSkill()
	{
		List<int> list=new List<int>();

        using (var e = useSkillContainer.SkillContainers.GetEnumerator())
        {

            while (e.MoveNext())
            {
                if (e.Current.Value.skillinfo != null && e.Current.Value.skillinfo.isEnable)
                {
                    list.Add(e.Current.Value.skillinfo.SkillID);
                }
                else
                {
                    list.Add(0);
                }
            }
        }



        //foreach (var item in useSkillContainer.SkillContainers) {
        //    if (item.Value.skillinfo != null&&item.Value.skillinfo.isEnable) {
        //        list.Add (item.Value.skillinfo.SkillID);
        //    }
        //    else{
        //        list.Add (0);
        //    }
        //}
//
//
//
//		for (int i = 0; i < useSkills.Count; i++) {
//			if (useSkillContainer.skillList [i] != null) {
//				list.Add (useSkillContainer.skillList [i].SkillID);
//			} else {
//				list.Add (0);
//			}
//		}
		GameCenter.skillMng.C2S_changeSkill (list);
	}



    #endregion
    
}
