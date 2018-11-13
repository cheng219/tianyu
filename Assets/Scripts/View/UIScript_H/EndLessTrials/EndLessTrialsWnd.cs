/// <summary>
/// 何明军
/// 2016/4/19
/// 无尽挑战系统
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndLessTrialsWnd : GUIBase {

    public UIButton resetBtn;//重置按钮
    public UILabel restRefreshTime;//剩余次数
    public UILabel noRestTime;//没有剩余次数 
    public GameObject haveRestTime;
    public UILabel resetCostLab;
    protected int restCostNum = 20;

	public GameObject btnClose;
	public GameObject btnLeft;
	public GameObject btnRigth;
	
	public GameObject btnSweeping;
	
	public GameObject itemEty;
	
	public GameObject lineEty;
	
	public UILabel totalStar;
	public UILabel nameText;
	
	public EndLessTrialsItemUI itemOpen;
	public EndLessTrialsStarUI starOpen;
	
	public EndLessTrialsStarUI stars;
    //public GameObject[] startsRedPoint;
    public UITexture textIcon;
	
	EndLessTrialsMng EndMng{
		get{
			return GameCenter.endLessTrialsMng;
		}
	}

    protected int curChapterId = 0;
	// Use this for initialization
	void Awake () {
        GameCenter.endLessTrialsMng.C2S_EndList();
		mutualExclusion = true;
		Layer = GUIZLayer.TOPWINDOW;
		itemEty.SetActive(false);
		lineEty.SetActive(false);
		UIEventListener.Get(btnSweeping).onClick = delegate {
			if(!EndMng.IsSweepingEndless){
				return ;
			}
			GameCenter.endLessTrialsMng.C2S_SweepReward(2,0);
		};
		
		UIEventListener.Get(btnClose).onClick = delegate {
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
		};
		UIEventListener.Get(btnLeft).onClick = delegate {
			if(ConfigMng.Instance.GetChapterRefData(EndMng.CurChapterID - 1) != null){
				EndMng.CurChapterID--;
			}
            GameCenter.endLessTrialsMng.CountRedPoint();
        };
		UIEventListener.Get(btnRigth).onClick = delegate {
			ChapterRef refData = ConfigMng.Instance.GetChapterRefData(EndMng.CurChapterID + 1);
			if(refData != null && EndMng.GetItemDataCurFront(refData.allLevels[0])){
				EndMng.CurChapterID++;
			}else{
				GameCenter.messageMng.AddClientMsg(179);
			}
            GameCenter.endLessTrialsMng.CountRedPoint();
        };
        curChapterId = EndMng.CurChapterID;
        if (resetBtn != null) UIEventListener.Get(resetBtn.gameObject).onClick = ClickResetBtn; 
	}

    protected int GetResetCost()
    {
        int restTime = GameCenter.endLessTrialsMng.RestRefreshTime;
        int allRefreshTime = 0;
        VIPRef vip = ConfigMng.Instance.GetVIPRef(GameCenter.vipMng.VipData != null ? GameCenter.vipMng.VipData.vLev : 0);
        if (vip != null) allRefreshTime = vip.endlessNum;
        StepConsumptionRef consume = ConfigMng.Instance.GetStepConsumptionRef(allRefreshTime - restTime + 1);
        if (consume != null && consume.endlessResetCost.Count > 0)
        {
            restCostNum = consume.endlessResetCost[0].count;
            return consume.endlessResetCost[0].count;
        }
        else
        {
            Debug.Log("阶梯消费表找不到id :  " + (allRefreshTime - restTime + 1) + " 的数据，无尽重置的剩余次数 ：" + restTime + " , 当前vip等级获得的总次数 ：" + allRefreshTime);
        }
        return 20;
    }

    /// <summary>
    /// 刷新剩余次数
    /// </summary>
    void ShowRestTime()
    {
        if (restRefreshTime != null) restRefreshTime.text = GameCenter.endLessTrialsMng.RestRefreshTime.ToString();
        if (resetCostLab != null) resetCostLab.text = GetResetCost().ToString();
        if (noRestTime != null)
        {
            noRestTime.gameObject.SetActive(GameCenter.endLessTrialsMng.RestRefreshTime <= 0);
            if (noRestTime.gameObject.activeSelf)
            {  
                if (GameCenter.vipMng.VipData == null || (ConfigMng.Instance.GetEndlessNumVIPLevByCurVip() > GameCenter.vipMng.VipData.vLev))
                {
                    noRestTime.text = "达到VIP" + ConfigMng.Instance.GetEndlessNumVIPLevByCurVip() + "获得次数";
                }
                else
                {
                    noRestTime.text = "您今日没有重置次数了！";
                }
            }
        }
        if (haveRestTime != null) haveRestTime.SetActive(GameCenter.endLessTrialsMng.RestRefreshTime > 0);
    }
	
	void ShowChapter(){ 
		btnLeft.SetActive(ConfigMng.Instance.GetChapterRefData(EndMng.CurChapterID - 1) != null);
		btnRigth.SetActive(ConfigMng.Instance.GetChapterRefData(EndMng.CurChapterID +1) != null);
        ShowRestTime();
		foreach(EndLessTrialsItemUI go in listEndItem){
			go.gameObject.SetActive(false);
		}
		foreach(GameObject go in listLine){
			go.SetActive(false);
		}
		ChapterRef dataList = ConfigMng.Instance.GetChapterRefData(EndMng.CurChapterID);
		if(dataList == null)return ;
		CreateLine();
		CreateItem(dataList);
		nameText.text = dataList.name;
		//totalStar.text = EndMng.GetTotalStar().ToString();
		
		ConfigMng.Instance.GetBigUIIcon(dataList.icon,delegate(Texture2D texture){
			if(textIcon != null)textIcon.mainTexture = texture;
		});
	}
	
	List<EndLessTrialsItemUI> listEndItem = new List<EndLessTrialsItemUI>();
	void CreateItem(ChapterRef dataList){ 
		CheckPointRef refData = null;
		int i =0,len = 0;
		GameObject go = null;
		EndLessTrialsItemUI endLess = null;
		for(i=0,len=dataList.allLevels.Count;i<len;i++){
			refData = ConfigMng.Instance.GetCheckPointRef(dataList.allLevels[i]);
			if(refData == null)continue;
			if(listEndItem.Count <= i){
				go = (GameObject)GameObject.Instantiate(itemEty);
				go.transform.parent = lineEty.transform.parent;
				go.transform.localPosition = new Vector3(refData.coordinate.x,refData.coordinate.y,-1f);
				go.transform.localScale = Vector3.one;
				go.name = "EndLessTrialsItem"+refData.id;
				endLess = go.GetComponent<EndLessTrialsItemUI>();
				endLess.SetEndItem(refData);
				UIEventListener.Get(go).onClick = OnClickEndItem;
				UIEventListener.Get(go).parameter = refData;
				listEndItem.Add(endLess);
				go.SetActive(true);
			}else{
				listEndItem[i].gameObject.name = "EndLessTrialsItem"+refData.id;
				listEndItem[i].SetEndItem(refData);
				listEndItem[i].transform.localPosition = new Vector3(refData.coordinate.x,refData.coordinate.y,-1f);
				UIEventListener.Get(listEndItem[i].gameObject).onClick = OnClickEndItem;
				UIEventListener.Get(listEndItem[i].gameObject).parameter = refData;
				listEndItem[i].gameObject.SetActive(true);
			}
		} 
	}

    void ShowBoxReward()
    { 
        ChapterReward chapterReward = null;
        curChapterId = 1;
        ChapterRef ChapterRef = ConfigMng.Instance.GetChapterRefData(curChapterId);
        chapterReward = GetReward(ChapterRef);
        while (isGetReward(ChapterRef) && curChapterId < ConfigMng.Instance.GetChapterRefTable().Count)
        {
            ++curChapterId;
            ChapterRef = ConfigMng.Instance.GetChapterRefData(curChapterId);
            chapterReward = GetReward(ChapterRef);
        }

        if (stars != null)
        {
            if (chapterReward != null)
            {
                stars.gameObject.SetActive(true);
                stars.SetStar(chapterReward, curChapterId); 
                if (stars != null)
                {
                    UIEventListener.Get(stars.gameObject).onClick -= OnClickStars;
                    UIEventListener.Get(stars.gameObject).onClick += OnClickStars;
                    UIEventListener.Get(stars.gameObject).parameter = chapterReward;
                }
            }
            else
            {
                stars.gameObject.SetActive(false);
            }
        }

        //ChapterRef dataList = ConfigMng.Instance.GetChapterRefData(EndMng.CurChapterID);
        //if (dataList == null) return;
        //bool isGetAll = true;
        //for (int i = 0, len = dataList.rewardData.Count; i < len; i++)
        //{
        //    if (!GameCenter.endLessTrialsMng.GetStarReward(GameCenter.endLessTrialsMng.CurChapterID, dataList.rewardData[i].starNum))//显示还没领取的
        //    {
        //        if (stars != null)
        //        {
        //            stars.gameObject.SetActive(true);
        //            stars.SetStar(dataList.rewardData[i]);
        //            if (stars != null)
        //            {
        //                UIEventListener.Get(stars.gameObject).onClick -= OnClickStars;
        //                UIEventListener.Get(stars.gameObject).onClick += OnClickStars;
        //                UIEventListener.Get(stars.gameObject).parameter = dataList.rewardData[i];
        //            }
        //        }
        //        isGetAll = false;
        //        break;
        //    }
        //}
        //if (isGetAll)
        //{
        //    if (stars != null) stars.gameObject.SetActive(false);
        //}
    }

    protected bool isGetReward(ChapterRef _ChapterRef)
    { 
        for (int i = 0, len = _ChapterRef.rewardData.Count; i < len; i++)
        {
            if (!GameCenter.endLessTrialsMng.GetStarReward(curChapterId, _ChapterRef.rewardData[i].starNum))//显示还没领取的
            {
                return false;
            }
        }
        return true;
    }

    protected ChapterReward GetReward(ChapterRef _ChapterRef)
    { 
        for (int i = 0, len = _ChapterRef.rewardData.Count; i < len; i++)
        {
            if (!GameCenter.endLessTrialsMng.GetStarReward(curChapterId, _ChapterRef.rewardData[i].starNum))//显示还没领取的
            {
                return _ChapterRef.rewardData[i];
            }
        }
        return null;
    }
	
	void OnClickStars(GameObject games){
		ChapterReward chapterReward = UIEventListener.Get(games).parameter as ChapterReward;
		starOpen.SetStar(chapterReward, curChapterId);
		starOpen.gameObject.SetActive(true);
	}
	
	void OnClickEndItem(GameObject games){
		CheckPointRef refData = UIEventListener.Get(games).parameter as CheckPointRef;
		itemOpen.SetEndItem(refData);
		itemOpen.gameObject.SetActive(true);
	}
	
	List<GameObject> listLine = new List<GameObject>();
	void CreateLine(){
		LineRef dataList = ConfigMng.Instance.GetLineRef(EndMng.CurChapterID);
		if(dataList == null)return ;
		int i =0,len = 0;
		GameObject go = null;
		for(i=0,len=dataList.Line.Count;i<len;i++){
//		foreach(Lines data in dataList.Line){
			if(listLine.Count <= i){
				go = (GameObject)GameObject.Instantiate(lineEty);
				go.transform.parent = lineEty.transform.parent;
				listLine.Add(go);
			}else{
				go = listLine[i];
			}
			go.transform.localPosition = dataList.Line[i].coordinate;
			go.transform.localRotation = new Quaternion(dataList.Line[i].rotate.x,dataList.Line[i].rotate.y,dataList.Line[i].rotate.y,0);
			go.transform.localScale = Vector3.one;
			go.GetComponent<UISprite>().spriteName = dataList.Line[i].icon;
			go.SetActive(true);
			i++;
		}
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
		
		ShowChapter();
		EndMng.OnCurChapterUpdate += ShowChapter;
        EndMng.OnRestRefreshTimeUpdate += ShowRestTime;
        EndMng.OnCurChapterStarUpdate += ShowBoxReward;
		//GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ENDLESSTRIAL,false);
        //每次打开检测一下红点
        GameCenter.endLessTrialsMng.CountRedPoint();
        ShowBoxReward();
    }
	
	protected override void OnClose ()
	{
		base.OnClose ();
		EndMng.OnCurChapterUpdate -= ShowChapter;
        EndMng.OnRestRefreshTimeUpdate -= ShowRestTime;
        EndMng.OnCurChapterStarUpdate -= ShowBoxReward;
        //for(int i=0,len=stars.Length;i<len;i++){
        //    UIEventListener.Get(stars[i].gameObject).onClick -= OnClickStars;
        //}
		
		if(textIcon != null)textIcon.mainTexture = null;
		ChapterRef dataList = ConfigMng.Instance.GetChapterRefData(EndMng.CurChapterID);
		ConfigMng.Instance.RemoveBigUIIcon(dataList.icon);
        GameCenter.endLessTrialsMng.CountRedPoint();
    }

    void ClickResetBtn(GameObject go)
    {
        if (GameCenter.endLessTrialsMng.RestRefreshTime <= 0)
        {
            if (GameCenter.vipMng.VipData == null || (ConfigMng.Instance.GetEndlessNumVIPLevByCurVip() > GameCenter.vipMng.VipData.vLev))
            {
                MessageST mst = new MessageST();
                mst.messID = 556;
                mst.words = new string[1] { ConfigMng.Instance.GetEndlessNumVIPLevByCurVip().ToString() };
                GameCenter.messageMng.AddClientMsg(mst);
            }
            else 
            {
                GameCenter.messageMng.AddClientMsg(557);
            }
            return;
        }
        if (GameCenter.endLessTrialsMng.isShowEndlessResetTip)
        {
            MessageST mst = new MessageST();
            object[] pa = { 1 };
            mst.pars = pa;
            mst.delPars = delegate(object[] ob)
            {
                if (ob.Length > 0)
                {
                    bool b = (bool)ob[0];
                    if (b)
                        GameCenter.endLessTrialsMng.isShowEndlessResetTip = false;
                }
            };
            mst.messID = 555;
            mst.words = new string[1] { restCostNum.ToString()};
            mst.delYes = (x) =>
            {
                EndMng.C2S_ResetEndlessInfo();
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else
        {
            EndMng.C2S_ResetEndlessInfo();
        }
    }
    ///// <summary>
    ///// 检测红点
    ///// </summary>
    //void CheckRedPoint()
    //{
    //    int num = startsRedPoint.Length;
    //    for (int i = 0, len = startsRedPoint.Length; i < len; i++)
    //    {
    //        if (startsRedPoint[i].activeSelf)
    //        {
    //            //Debug.Log("stars[i].readPoint.active=" + stars[i].readPoint.gameObject.activeSelf);
    //            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ENDLESSTRIAL, true);
    //        }
    //        else
    //        {
    //            num--;
    //            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.ENDLESSTRIAL, num > 0 ? true : false);
    //        }
    //    }
    //}
}
