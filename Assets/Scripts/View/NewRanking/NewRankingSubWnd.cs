//====================
//作者：鲁家旗
//日期：2016/4/19
//用途：排行榜界面类
//====================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NewRankingSubWnd : SubWnd
{
    #region 控件数据
    public UIToggle[] uiTogs;
    public UIButton flowerBtn;
    public UIButton friendBtn;
    public GameObject rankItemGo;

    public UILabel titleLabel;//标题描述

    public UILabel myRank;//自己排名
    public UILabel myNoRank;//暂未上榜
    public UILabel nameLabel;//名字
    public UILabel myDataLabel;//自身数据
    public UILabel guildLabel;//工会数据
    protected Dictionary<int, NewRankingInfo> rankDic
    { 
        get
        {
            return GameCenter.newRankingMng.RankingDic;
        }
    }
    protected List<GameObject> rankItemList = new List<GameObject>();
    protected List<NewRankingInfo> infoList = new List<NewRankingInfo>();
    public UIScrollView scrollView;
    protected int curPage = 1;//分页请求，当前第几页
    protected int rankType = 1;//第几个榜
    protected int currtY = 167;
    public UITexture curTargetTex;
    protected PlayerBaseInfo curTargetInfo;
    protected bool isChangeTog = false;
    protected int myDataOne = 0;
    protected int myDataTwo = 0;
    protected bool isCreate = false;
    #endregion

    void Awake()
    {
        //送花
        if (flowerBtn != null) UIEventListener.Get(flowerBtn.gameObject).onClick = delegate
        {
            if (infoList.Count != 0)
            {
                if (GameCenter.newRankingMng.CurChooseRankPlayerId != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                {
                    GameCenter.friendsMng.sendFlowerToOne = GameCenter.newRankingMng.CurChooseRankPlayerId;
                    //送花的入口类型
                    GameCenter.friendsMng.SendFlowerType = rankType;
                    GameCenter.uIMng.GenGUI(GUIType.SENDFLOWER, true);
                }
                else
                {
                    GameCenter.messageMng.AddClientMsg(411);
                }
            }
            else
                GameCenter.messageMng.AddClientMsg(412);
        };
        //交友
        if (friendBtn != null) UIEventListener.Get(friendBtn.gameObject).onClick = delegate
        {
            if (infoList.Count != 0)
            {
                if (GameCenter.newRankingMng.CurChooseRankPlayerId == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
                {
                    GameCenter.messageMng.AddClientMsg(413);
                }
                else
                    GameCenter.friendsMng.C2S_AddFriend(GameCenter.newRankingMng.CurChooseRankPlayerId);
            }
            else
                GameCenter.messageMng.AddClientMsg(412);
        };
        if (scrollView != null)
            scrollView.onDragFinished += OnDragFinished;
        if (rankItemGo != null) rankItemGo.SetActive(false);
    }
    void Update()
    {
        if (isCreate)
        {
            InitToggles(null);
            isCreate = false;
        }
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        for (int i = 0; i < uiTogs.Length; i++)
        {
            if (uiTogs[i] != null) UIEventListener.Get(uiTogs[i].gameObject).onClick -= InitToggles;
            if (uiTogs[i] != null) UIEventListener.Get(uiTogs[i].gameObject).onClick += InitToggles;
        }
        if (uiTogs[GameCenter.newRankingMng.curChooseRank] != null)
        {
            uiTogs[GameCenter.newRankingMng.curChooseRank].value = true;
            GameCenter.newRankingMng.curChooseRank = 0;
        }
        isCreate = true;
    }
    
    protected override void OnClose()
    {
        base.OnClose();
    }
    void OnDestroy()
    {
        GameCenter.previewManager.ClearModel();
        for (int i = 0; i < uiTogs.Length; i++)
        {
            if (uiTogs[i] != null) UIEventListener.Get(uiTogs[i].gameObject).onClick -= InitToggles;
        }
        if (scrollView != null)
            scrollView.onDragFinished -= OnDragFinished;
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.newRankingMng.OnRankingUpdate += Refresh;
            GameCenter.previewManager.OnGotCurAskRankPlayerInfo += RefreshModel;
            GameCenter.previewManager.OnGotCurAskPlayerInfo += RefreshPetModel;
        }
        else
        {
            GameCenter.newRankingMng.OnRankingUpdate -= Refresh;
            GameCenter.previewManager.OnGotCurAskRankPlayerInfo -= RefreshModel;
            GameCenter.previewManager.OnGotCurAskPlayerInfo -= RefreshPetModel;
        }
    }
    void Refresh()
    {
        CreateRankItem();
        RefreshRank();
        RefreshMyData();
    }
    /// <summary>
    /// 分页请求
    /// </summary>
    void OnDragFinished()
    {
        Vector3 constraint = scrollView.panel.CalculateConstrainOffset(scrollView.bounds.min, scrollView.bounds.min);
        if (constraint.y <= 1f)
        {
            curPage++;
            GameCenter.newRankingMng.C2S_ReqGetRank(rankType, curPage);
        }
    }
    /// <summary>
    /// 切换榜
    /// </summary>
    void InitToggles(GameObject go)
    {
        if (rankItemList.Count != 0) rankItemList[0].GetComponent<UIToggle>().value = true;
        if (curPage != 1)   curPage = 1;
        for (int i = 0; i < uiTogs.Length; i++)
        {
            if (uiTogs[i].value)
            {
                isChangeTog = true;
                RefreshTitleInfo(i);
                infoList.Clear();
                //发送请求协议
                GameCenter.newRankingMng.C2S_ReqGetRank(i+1,curPage);
                //第几个榜
                rankType = i + 1;
                if (scrollView != null) scrollView.ResetPosition();
            }
        }
        if (uiTogs[2].value || uiTogs[3].value)
        {
            if (flowerBtn != null) flowerBtn.gameObject.SetActive(false);
            if (friendBtn != null) friendBtn.gameObject.SetActive(false);
        }
        else
        {
            if (flowerBtn != null) flowerBtn.gameObject.SetActive(true);
            if (friendBtn != null) friendBtn.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 创建数据格
    /// </summary>
    void  CreateRankItem()
    {
        if (rankItemList.Count < curPage * 20)
        {
            using (var data = rankDic.GetEnumerator())
            {
                while (data.MoveNext())
                {
                    GameObject go = Instantiate(rankItemGo) as GameObject;
                    go.transform.parent = rankItemGo.transform.parent;
                    if (curPage == 1) go.GetComponent<UIToggle>().startsActive = true;
                    currtY = currtY - 55;
                    go.transform.localPosition = new Vector3(-228, currtY, 0);
                    go.transform.localScale = Vector3.one;
                    rankItemList.Add(go);
                }
            }
        }
    }
    void RefreshTitleInfo(int _rankType)
    {
        if (titleLabel != null)
        {
            switch (_rankType)
            {
                case 0:
                    titleLabel.text = ConfigMng.Instance.GetUItext(108);
                    break;
                case 1:
                    titleLabel.text = ConfigMng.Instance.GetUItext(107);                     
                    break;
                case 2:
                    titleLabel.text = ConfigMng.Instance.GetUItext(109);
                    break;
                case 3:
                    titleLabel.text = ConfigMng.Instance.GetUItext(110);
                    break;
                case 4:
                    titleLabel.text = ConfigMng.Instance.GetUItext(111);
                    break;
                case 5:
                    titleLabel.text = ConfigMng.Instance.GetUItext(112);
                    break;
                case 6:
                    titleLabel.text = ConfigMng.Instance.GetUItext(113);
                    break;
                case 7:
                    titleLabel.text = ConfigMng.Instance.GetUItext(114);
                    break;
                case 8:
                    titleLabel.text = ConfigMng.Instance.GetUItext(115);
                    break; 
                case 9:
                    titleLabel.text = ConfigMng.Instance.GetUItext(116);
                    break;
                case 10:
                    titleLabel.text = ConfigMng.Instance.GetUItext(117);
                    break;
            }
        }
    }
    /// <summary>
    /// 刷新排行榜数据
    /// </summary>
    void RefreshRank()
    {
        if (rankDic.Count == 0) curPage--;
        using (var data = rankDic.GetEnumerator())
        {
            while (data.MoveNext())
            {
                if (!infoList.Contains(data.Current.Value))
                    infoList.Add(data.Current.Value);
            }
        }
        for (int i = 0; i < rankItemList.Count; i++)
        {
            if (i >= infoList.Count)
                rankItemList[i].SetActive(false);
            else
            {
                rankItemList[i].SetActive(true);
                RankingItem rankingItem = rankItemList[i].GetComponent<RankingItem>();
                if (rankingItem != null)
                {
                    rankingItem.rankNumLabel.text = (i + 1).ToString();
                    rankingItem.Refresh(infoList[i],rankType);
                }
            }
        }
        if (infoList.Count == 0)//没有玩家的排行榜不显示模型
        {
            GameCenter.previewManager.ClearModel();
        }
        if (infoList.Count != 0 && isChangeTog)
        {
            GameCenter.newRankingMng.CurChooseRankPlayerId = infoList[0].PlayerId;
            GameCenter.previewManager.C2S_ReqGetInfo(infoList[0].PlayerId, rankType == 6 ? 1 : 0);
            isChangeTog = false;
        }
    }
    /// <summary>
    /// 刷新自身数据
    /// </summary>
    void RefreshMyData()
    {
        myDataOne = GameCenter.newRankingMng.myValue1;
        myDataTwo = GameCenter.newRankingMng.myValue2;
        //是否上榜
        if (GameCenter.newRankingMng.myRank <= 100 && GameCenter.newRankingMng.myRank != 0 && myRank != null)
        {
            myRank.gameObject.SetActive(true);
            myNoRank.gameObject.SetActive(false);
            myRank.text = GameCenter.newRankingMng.myRank.ToString();
            if (nameLabel != null) nameLabel.gameObject.SetActive(true);
            if (nameLabel != null) nameLabel.text = GameCenter.mainPlayerMng.MainPlayerInfo.Name;
        }
        else
        {
            myRank.gameObject.SetActive(false);
            myNoRank.gameObject.SetActive(true);
            if (nameLabel != null) nameLabel.gameObject.SetActive(false);
        }
        if (myDataLabel != null)
        {
            guildLabel.gameObject.SetActive(false);
            myDataLabel.gameObject.SetActive(true);
            switch (rankType - 1)
            {
                case 0:
                    myDataLabel.text = myDataOne.ToString();
                    break;
                case 1:
                    myDataLabel.text = ConfigMng.Instance.GetLevelDes(myDataOne);
                    break;
                case 2:
                    if (myDataOne != 0)
                    {
                        myDataLabel.text = myDataOne.ToString();
                    }
                    else
                        myDataLabel.gameObject.SetActive(false);
                    break;
                case 3:
                    if (myDataOne != 0)
                    {
                        RidePropertyRef rideRef = ConfigMng.Instance.GetMountPropertyRef(myDataOne);
                        myDataLabel.text = rideRef == null ? string.Empty : rideRef.name;
                    }
                    else
                        myDataLabel.gameObject.SetActive(false);
                    break;
                case 4:
                    if (myDataOne != 0)
                    {
                        CheckPointRef chapRef = ConfigMng.Instance.GetCheckPointRef(myDataOne);
                        myDataLabel.text = chapRef == null ? string.Empty : chapRef.name;
                    }
                    else
                        myDataLabel.gameObject.SetActive(false);
                    break;
                case 5:
                    if (nameLabel != null) nameLabel.text = GameCenter.mainPlayerMng.MainPlayerInfo.GuildName;
                    if (myDataOne != 0 && myDataTwo != 0)
                    {
                        myDataLabel.gameObject.SetActive(false);
                        guildLabel.gameObject.SetActive(true);
                        guildLabel.text = myDataOne + "      " + myDataTwo;
                    }
                    else
                    {
                        guildLabel.gameObject.SetActive(false);
                        myDataLabel.gameObject.SetActive(false);
                    }
                    break;
                case 6:
                    if (myDataOne != 0)
                    {
                        WingRef wingRef = ConfigMng.Instance.GetWingRef(myDataOne, myDataTwo);
                        myDataLabel.text = wingRef == null ? string.Empty : wingRef.name;
                    }
                    else
                        myDataLabel.gameObject.SetActive(false);
                    break;
                default:
                    if (myDataOne != 0)
                        myDataLabel.text = myDataOne.ToString();
                    else
                        myDataLabel.gameObject.SetActive(false);
                    break;
            }
        }
    }
    /// <summary>
    /// 刷新模型
    /// </summary>
    void RefreshModel()
    {
        curTargetInfo = GameCenter.previewManager.CurAskPlayerInfo; 
        //宠物榜和坐骑榜没有送花按钮
        if (uiTogs[2].value || uiTogs[3].value && curTargetInfo != null)
        {
            GameCenter.previewManager.C2S_AskOpcPetPreview(GameCenter.newRankingMng.CurChooseRankPlayerId);
        }
        else
        {
            if (curTargetTex != null && curTargetInfo != null)
            {
                //curTargetTex.width = 446;
                //curTargetTex.height = 439;
                GameCenter.previewManager.TryPreviewSinglePlayer(curTargetInfo, curTargetTex);
            }
        }
    }
    /// <summary>
    /// 刷新宠物和坐骑模型
    /// </summary>
    void RefreshPetModel()
    {
        if (uiTogs[2].value && curTargetInfo.CurPetInfo != null && curTargetTex != null)
        {
            //curTargetTex.width = 270;
            //curTargetTex.height = 300;
            GameCenter.previewManager.TryPreviewSingelEntourage(curTargetInfo.CurPetInfo, curTargetTex);
        }
        else if (uiTogs[3].value && curTargetInfo.CurMountInfo != null && curTargetTex != null)
        {
            //curTargetTex.width = 400;
            //curTargetTex.height = 400;
            GameCenter.previewManager.TryPreviewMount(curTargetInfo.CurMountInfo, curTargetTex);
        }
    }
}
