//======================================================
//作者:何明军
//日期:16/6/17
//用途:ping值与当前时间
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainTimeWnd : GUIBase {
    public UISprite[] pingSp;
	//public UILabel pingVal;
	public UITimer gameTime;
	public UILabel DoubleHitNumLabel;
    public UILabel BigDoubleHitNumLabel;
	public GameObject DoubleHitObj;
    public UISprite progressSprite;
    #region 功能预告
    public GameObject XBtnObj;
    public GameObject NewFunctionHintsObj;
    public GameObject NewFunctionHintsBtn;
    public UISprite NewFunctionHintsSprite;
    public UILabel NewFunctionHintsDes;
    public GameObject NewFunctionHintsTextObj;
    public UILabel NewFunctionHintsText;
    private List<int> NewFunctionHintsSteps = new List<int>();
    public UILabel title;
    public GameObject oldItem;
    public UIButton getRewardBtn;
    public UIGrid itemGird;
    private Dictionary<int, GameObject> itemDic = new Dictionary<int, GameObject>();
    public GameObject redPoint;//新增红点 by 唐源
    /// <summary>
    /// 奇缘入口
    /// </summary>
    public OnMiacleclick MiracleEnter;
    #endregion

    float _lastTime;
	int doubleHitNum;
	
	void Awake(){
        //InvokeRepeating("RefeshDoubleHitState", 1.0f, 0.05f);
		RefeshServerTime();
		ShowPing();
        if (oldItem != null) oldItem.SetActive(false);
	}
	
	void RefeshServerTime(){
		if(gameTime != null){
			gameTime.StartCurServerTimer(GameCenter.instance.ServerTimePointSecond);
			gameTime.OnBeforeDawn = delegate {
				GameCenter.activityMng.InTheMorningUpdateData();
			};
		}
	}
	
	void ShowPing(){
		//pingVal.text = GameCenter.instance.PingTime + "ms";
        if (GameCenter.instance.NetDelayTime <= GameCenter.pingSmooth)
        {
			//pingVal.color = Color.green;
            if (pingSp[0] != null) pingSp[0].enabled = true;
            if (pingSp[1] != null) pingSp[1].enabled = false;
            if (pingSp[2] != null) pingSp[2].enabled = false;
        }
        else if (GameCenter.instance.NetDelayTime <= GameCenter.pingOrdinary)
        {
			//pingVal.color = Color.yellow;
            if (pingSp[0] != null) pingSp[0].enabled = false;
            if (pingSp[1] != null) pingSp[1].enabled = true;
            if (pingSp[2] != null) pingSp[2].enabled = false;
		}else{
			//pingVal.color = Color.red;
            if (pingSp[0] != null) pingSp[0].enabled = false;
            if (pingSp[1] != null) pingSp[1].enabled = false;
            if (pingSp[2] != null) pingSp[2].enabled = true;
		}
	}

    protected override void OnOpen()
    {
        base.OnOpen();
        ShowMiracleAccess();
    }
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
            NewFunctionHintsSteps = ConfigMng.Instance.GetAllNewFunctionHintsSteps();
            RefreshNewFunctionHints(TaskType.Main);
            if (XBtnObj != null)
                UIEventListener.Get(XBtnObj).onClick += CloseText;
            if (NewFunctionHintsBtn != null)
                UIEventListener.Get(NewFunctionHintsBtn).onClick += ShowText;
            GameCenter.taskMng.OnTaskGroupUpdate += RefreshNewFunctionHints;
			GameCenter.loginMng.OnPingChange += ShowPing;
			GameCenter.abilityMng.OnContinuCountUpdate += RefeshDoubleHit;
			GameCenter.OnServerTimeUpdate += RefeshServerTime;
            if (getRewardBtn != null)
                UIEventListener.Get(getRewardBtn.gameObject).onClick += OnClickGetReward;
            GameCenter.mainPlayerMng.UpdateFunctionReward += RefreshNewFunctionHints;
            GameCenter.miracleMng.OnMiracleDataUpdateEvent += ShowMiracleAccess;
		}
		else
		{
            if (XBtnObj != null)
                UIEventListener.Get(XBtnObj).onClick -= CloseText;
            if (NewFunctionHintsBtn != null)
                UIEventListener.Get(NewFunctionHintsBtn).onClick -= ShowText;
            GameCenter.taskMng.OnTaskGroupUpdate -= RefreshNewFunctionHints;
			GameCenter.loginMng.OnPingChange -= ShowPing;
			GameCenter.abilityMng.OnContinuCountUpdate -= RefeshDoubleHit;
			GameCenter.OnServerTimeUpdate -= RefeshServerTime;
            if (getRewardBtn != null)
                UIEventListener.Get(getRewardBtn.gameObject).onClick -= OnClickGetReward;
            GameCenter.mainPlayerMng.UpdateFunctionReward -= RefreshNewFunctionHints;
            GameCenter.miracleMng.OnMiracleDataUpdateEvent -= ShowMiracleAccess;
		}
	}



    void CloseText(GameObject _obj)
    {
        if (NewFunctionHintsTextObj != null)
            NewFunctionHintsTextObj.SetActive(false);

    }

    void ShowText(GameObject _obj)
    {
        //Debug.Log("11111111111");
        //if (NewFunctionHintsTextObj != null)
        //    NewFunctionHintsTextObj.SetActive(!NewFunctionHintsTextObj.activeSelf);
        GameCenter.uIMng.SwitchToUI(GUIType.NEWREWARDPREVIEW);
    }

    void Update()
    {
        if (isHitStar)
        {
            if (Time.time - _lastTime < 3)
            {
                if (DoubleHitNumLabel != null)
                {
                    DoubleHitNumLabel.text = doubleHitNum.ToString();
                    BigDoubleHitNumLabel.text = doubleHitNum.ToString();
                    if (isNewHit)
                    {
                        BigDoubleHitNumLabel.gameObject.SetActive(true);
                        TweenScale s = BigDoubleHitNumLabel.transform.GetComponent<TweenScale>();
                        if(s!=null)
                        s.PlayReverse();
                        isNewHit = false;
                    }
                }
                if (DoubleHitObj != null) DoubleHitObj.SetActive(true);
                if (progressSprite != null) progressSprite.fillAmount = (1 - (Time.time - _lastTime) / 3);
                if (Time.time - _lastTime < 0.05)
                {
                    
                    //if ((int)(120 - (100 * (Time.time - _lastTime) / 0.05)) > 20)
                    //{
                    //    BigDoubleHitNumLabel.fontSize = (int)(120 - (100 * (Time.time - _lastTime) / 0.05));
                    //}
                    //else
                    //{
                    //    BigDoubleHitNumLabel.gameObject.SetActive(false);
                    //}
                }
                else
                {
                    BigDoubleHitNumLabel.gameObject.SetActive(false);
                }

            }
            else
            {
                doubleHitNum = 0;
                if (DoubleHitObj != null)
                {
                    DoubleHitObj.SetActive(false);
                    isHitStar = false;

                }
            }
        }
    }

    bool isHitStar=false;
    bool isNewHit = false;
	//void RefeshDoubleHitState()
	//{
        //if (Time.time - _lastTime < 2)
        //{
        //    if (DoubleHitNum != null)
        //    {
        //        DoubleHitNum.text = doubleHitNum.ToString();
        //        if (isNewHit)
        //        {
        //            DoubleHitNum.fontSize = 40;
        //            isNewHit = false;
        //        }
        //        else
        //        {
        //            DoubleHitNum.fontSize = 20;
        //        }
        //    }
        //    if (DoubleHitObj != null) DoubleHitObj.SetActive(true);
        //    if (progressSprite != null) progressSprite.fillAmount = (1 - (Time.time - _lastTime) / 2);
        //}
        //else
        //{
        //    doubleHitNum = 0;
        //    if (DoubleHitObj != null)
        //    {
        //        DoubleHitObj.SetActive(false);

        //    }
        //}
	//}

    void ShowNewFunctionHints(NewFunctionHintsRef _ref,bool _canGetReward)
    { 
        if (NewFunctionHintsObj != null)
            NewFunctionHintsObj.SetActive(true);
        if (NewFunctionHintsDes != null)
        {
            string st = _ref.des.Replace("\\n", "\n");
            NewFunctionHintsDes.text = st;
        }
        if (NewFunctionHintsText != null)
            NewFunctionHintsText.text = _ref.text;
        if (NewFunctionHintsSprite != null)
        {
            NewFunctionHintsSprite.spriteName = _ref.Icon;
            NewFunctionHintsSprite.MakePixelPerfect();
        }
        if (title != null) title.text = _ref.title;
        if (itemGird != null) itemGird.maxPerLine = _ref.reward.Count;
        for (int i = 0; i < _ref.reward.Count; i++)
        {
            ItemValue item = _ref.reward[i];
            GameObject go = null;
            if (!itemDic.ContainsKey(item.eid))
            {
                go = Instantiate(oldItem) as GameObject;
                itemDic[item.eid] = go;
            }
            else
                go = itemDic[item.eid];
            go.transform.parent = oldItem.transform.parent;
            go.transform.localPosition = Vector3.zero; 
            go.transform.localScale = Vector3.one;
            ItemUI itemUI = go.GetComponent<ItemUI>();
            if (itemUI != null)
                itemUI.FillInfo(new EquipmentInfo(item.eid,item.count,EquipmentBelongTo.PREVIEW));
            go.SetActive(true);
        }
        if (itemGird != null) itemGird.repositionNow = true;
        
        if (getRewardBtn != null)
        {
            UISpriteEx spEx = getRewardBtn.GetComponentInChildren<UISpriteEx>();
            if (spEx != null)
            {
                if (_canGetReward)
                {
                    spEx.IsGray = UISpriteEx.ColorGray.normal;
                    redPoint.SetActive(true);//新功能预告增加红点 By 唐源
                }
                else
                {
                    spEx.IsGray = UISpriteEx.ColorGray.Gray;
                    redPoint.SetActive(false);//新功能预告增加红点 By 唐源
                }
            }
        }
    }
    void OnClickGetReward(GameObject _go)
    {
        GameCenter.mainPlayerMng.C2S_ReqGetFuncReward(GameCenter.mainPlayerMng.curGetRewardStep);
    }

    void RefreshNewFunctionHints(TaskType _type)
    { 
        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef == null)
            return;
        if (_type == TaskType.Main && GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.sort == SceneType.SCUFFLEFIELD)
        {
            NewFunctionHintsRef curFunctionRef = ConfigMng.Instance.GetNewFunctionHintsById(GameCenter.mainPlayerMng.curGetRewardStep); 
            if (curFunctionRef != null)
            {
                //if (GameCenter.taskMng.HaveMainTask(1, curFunctionRef.Step) &&
                if (GameCenter.uIMng.CurOpenType != GUIType.NPCDIALOGUE && GameCenter.uIMng.CurOpenType != GUIType.NEWFUNCTIONTIPUI)
                {
                    bool canGetReward = GameCenter.taskMng.CurTaskCanGetReward(1, curFunctionRef.Step);
                    ShowMiracleAccess();
                    ShowNewFunctionHints(curFunctionRef, canGetReward);
                    return;
                }
            }
            if (NewFunctionHintsObj != null)
                NewFunctionHintsObj.SetActive(false);
            if (MiracleEnter != null)
                MiracleEnter.gameObject.SetActive(false);

        }
    }



  public   void ShowObj(bool _b)
    {
        if (_b)
        {
            if(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.sort == SceneType.SCUFFLEFIELD)
            {
                NewFunctionHintsRef curFunctionRef = ConfigMng.Instance.GetNewFunctionHintsById(GameCenter.mainPlayerMng.curGetRewardStep);
                if (curFunctionRef != null)
                {
                    //if (GameCenter.taskMng.HaveMainTask(1, curFunctionRef.Step))
                    //{
                    if (GameCenter.uIMng.CurOpenType != GUIType.NPCDIALOGUE && GameCenter.uIMng.CurOpenType != GUIType.NEWFUNCTIONTIPUI)
                        {
                            bool canGetReward = GameCenter.taskMng.CurTaskCanGetReward(1, curFunctionRef.Step);
                            ShowNewFunctionHints(curFunctionRef, canGetReward);
                            return;
                        }
                    //}
                }
                if (NewFunctionHintsObj != null)
                    NewFunctionHintsObj.SetActive(false);
                if (MiracleEnter != null)
                    MiracleEnter.gameObject.SetActive(false);
            }
        }
        else
        {
            if (NewFunctionHintsObj != null)
                NewFunctionHintsObj.SetActive(false);
            if (MiracleEnter != null)
                MiracleEnter.gameObject.SetActive(false);
        }


    }



	void RefeshDoubleHit(float _time)
	{
		_lastTime = _time;
		doubleHitNum++;
        isNewHit = true;
        isHitStar = true;
	}

    void ShowMiracleAccess()
    {
        AccessState accessState = GameCenter.miracleMng.miracleStatus;
        if (GameCenter.sceneMng.EnterSucceed && accessState != AccessState.ACHIEVE && accessState != AccessState.NONE)
        {
            if (MiracleEnter != null) MiracleEnter.OpenUI(); 
        }
        if (accessState == AccessState.ACHIEVE && accessState != AccessState.NONE && MiracleEnter.gameObject.activeSelf)
        {
            if (MiracleEnter != null) MiracleEnter.CloseUI(); 
        }
    }

    public void ShowMiracleAccess(bool _show)
    {
        if (MiracleEnter != null)
        {
            AccessState accessState = GameCenter.miracleMng.miracleStatus;
            if (_show && accessState != AccessState.ACHIEVE && accessState != AccessState.NONE)
            {
                MiracleEnter.OpenUI();
            }
            if (!_show && accessState == AccessState.ACHIEVE && accessState != AccessState.NONE)
            {
                MiracleEnter.CloseUI();
            }
        }
    }
}
