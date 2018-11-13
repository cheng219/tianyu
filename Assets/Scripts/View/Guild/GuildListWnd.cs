//==================================
//作者：邓成
//日期：2016/4/18
//用途：公会列表界面类
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildListWnd : GUIBase {
	public GameObject haveGuildGo;
	public GameObject noGuildGo;
    public UIButton createGuildBtn;
	public UIButton oneKeyApply;
	public UIButton backGuildMain;
	public UILabel labGuildRank;
	public UIToggle toggleSeeCanJoin;

	public UIButton closeCheckBtn;
	public UIGrid parent;
	public UIScrollView scrollView;

	public Vector4 positionInfo = new Vector4(0,165,0,-60);
    
    #region 创建公会
    public UIInput createName;
	public UIInput createNotice;
    public UIButton diamondCreateBtn;
	public UIButton thingCreateBtn;
    #endregion

    Dictionary<int, GuildItem> allitems = new Dictionary<int, GuildItem>();
    List<GuildData> curList = new List<GuildData>();
    public int MAXNUM = 30;//申请界面最大公会数量暂定为30；
	bool onlyShowCanJoin = false;
	bool OnlyShowCanJoin
	{
		get{return onlyShowCanJoin;}
		set
		{
			if(onlyShowCanJoin != value)
			{
				onlyShowCanJoin = value;
				ShowGuildList();
			}
		}
	}

    //bool isSearch = false;


    protected int curPage = 1;
    protected int CurPage
    {
        get { return curPage; }
        set
        {
//            curPage = value;
//            int num = 0;
//            int count = GameCenter.guildMng.GuildDic.Count;
//            num = count % MAXNUM == 0 ? count / MAXNUM : (count / MAXNUM + 1);
//            if (value <= 1)
//                formerBtn.gameObject.SetActive(false);
//            else
//                formerBtn.gameObject.SetActive(true);
//            if (value >= num)
//                nextBtn.gameObject.SetActive(false);
//            else
//                nextBtn.gameObject.SetActive(true);
        }
    }

    void Awake()
    {
        mutualExclusion = true;
        layer = GUIZLayer.NORMALWINDOW;
		if(closeCheckBtn != null)UIEventListener.Get(closeCheckBtn.gameObject).onClick = CloseWnd;
		if(diamondCreateBtn != null)UIEventListener.Get(diamondCreateBtn.gameObject).onClick = CreateGuild;
		if(thingCreateBtn != null)UIEventListener.Get(thingCreateBtn.gameObject).onClick = ThingCreateGuild;
		if(backGuildMain != null)UIEventListener.Get(backGuildMain.gameObject).onClick = BackGuildMain;
		if(oneKeyApply != null)UIEventListener.Get(oneKeyApply.gameObject).onClick = OneKeyApply;
		if(createName != null)EventDelegate.Add(createName.onChange,OnChangeName);
		if(createNotice != null)EventDelegate.Add(createNotice.onChange,OnChangeNotice);
        //UIEventListener.Get(nextBtn.gameObject).onClick = ClickNext; 
        CurPage = 1;
        
    }
    protected override void OnOpen()
    {
        base.OnOpen();
		InitWnd();
		GameCenter.guildMng.C2S_GuildList(1);
    }
    protected override void OnClose()
    {
        base.OnClose();
		if(createName != null)createName.value = string.Empty;
		if(createNotice != null)createNotice.value = string.Empty;
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (toggleSeeCanJoin != null) EventDelegate.Add(toggleSeeCanJoin.onChange, SenCanJoin);
            GameCenter.guildMng.OnGetGuildListEvent += ShowGuildList;
			GameCenter.mainPlayerMng.MainPlayerInfo.onGuildNameUpdate += CloseWndByNameUpdate;
        }
        else
        {
            if (toggleSeeCanJoin != null) EventDelegate.Remove(toggleSeeCanJoin.onChange, SenCanJoin);
            GameCenter.guildMng.OnGetGuildListEvent -= ShowGuildList;
			GameCenter.mainPlayerMng.MainPlayerInfo.onGuildNameUpdate -= CloseWndByNameUpdate;
        }
    }
	void OnChangeName()
	{
		if(createName != null)
		{
			string text = GameCenter.loginMng.FontHasCharacter(createName.label.bitmapFont,createName.value);
			if(!string.IsNullOrEmpty(text))
			{
				createName.value = text;
				GameCenter.messageMng.AddClientMsg(300);
			}
		}
	}
	void OnChangeNotice()
	{
		if(createNotice != null)
		{
			string text = GameCenter.loginMng.FontHasCharacter(createNotice.label.bitmapFont,createNotice.value);
			if(!string.IsNullOrEmpty(text))
			{
				createNotice.value = text;
				GameCenter.messageMng.AddClientMsg(300);
			}
		}
	}
	void CloseWndByNameUpdate(string guildName)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}
	void InitWnd()
	{
		bool haveGuild = GameCenter.mainPlayerMng.MainPlayerInfo.HavaGuild;
		if(haveGuildGo != null)
			haveGuildGo.SetActive(haveGuild);
		if(noGuildGo != null)
			noGuildGo.SetActive(!haveGuild);
		if(haveGuild)
		{
			if(labGuildRank != null && GameCenter.guildMng.MyGuildInfo != null)
				labGuildRank.text = GameCenter.guildMng.MyGuildInfo.GuildRank.ToString();
		}
	}
	void SenCanJoin()
	{
		if(toggleSeeCanJoin != null)
			OnlyShowCanJoin = toggleSeeCanJoin.value;
	}
    void Refresh()
    {
        for (int i = 0; i < allitems.Count; i++)
        {
            if (allitems[i] != null)
                allitems[i].gameObject.SetActive(false);
        }
        curList.Sort(GuildItemCompare.Instance);
        for (int i = 0; i < curList.Count; i++)
        {
            if (!allitems.ContainsKey(i))
            {
				GuildItem guildItem = GuildItem.CreateNew(parent.transform);
                guildItem.SetData(curList[i] as GuildData);
                allitems[i] = guildItem;
            }
            else
            {
                allitems[i].SetData(curList[i] as GuildData);
            }
			//allitems[i].transform.localPosition = new Vector3(positionInfo.x,positionInfo.y+positionInfo.w*i,-1);
            allitems[i].gameObject.SetActive(true);
        }   
		parent.repositionNow = true;
		if(scrollView != null)
			scrollView.SetDragAmount(0,0,false);
    }

	void OneKeyApply(GameObject go)
	{
		List<int> guildList = new List<int>();
		foreach(GuildData data in curList)
		{
			if(data.canJoin)
				guildList.Add(data.guildId);
		}
		GameCenter.guildMng.C2S_JoinGuild(guildList);
	}
    /// <summary>
    /// 手动分页
    /// </summary>
    void ShowGuildList()
    {
        //curDic.Clear();
        curList.Clear();
        //int index = 0;
		Dictionary<int,GuildData> dic = GameCenter.guildMng.GuildDic;
		foreach (GuildData item in dic.Values)
        {
            //if (index < (CurPage - 1) * MAXNUM) continue;
           //if (index >= CurPage * MAXNUM) break;
            //curDic.Add(item, dic[item]);
			if(OnlyShowCanJoin)
			{
				if(item.canJoin)
					curList.Add(item);
			}else
			{
				curList.Add(item);
			}
        }
        Refresh();
    }
    #region 控件事件
    /// <summary>
    /// 点击创建
    /// </summary>
    /// <param name="go"></param>
    void CreateGuild(GameObject go)
    {
        if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < 50)
		{
			MessageST mst = new MessageST();
			mst.messID = 137;
			mst.delYes = (x)=>
			{
				GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
			};
			GameCenter.messageMng.AddClientMsg(mst);
			return;
		}
        if (createName != null && createNotice != null)
        {
            if (!GameCenter.loginMng.FontHasAllCharacter(createName.label.bitmapFont, createName.value) || !GameCenter.loginMng.FontHasAllCharacter(createNotice.label.bitmapFont, createNotice.value) || GameCenter.loginMng.CheckName(createName.value))
            {
                GameCenter.messageMng.AddClientMsg(490);
                return;
            }
            GameCenter.guildMng.C2S_CreateNewGuild(createName.value, createNotice.value, 2);
        }
    }
	void ThingCreateGuild(GameObject go)
	{
        if (createName != null && createNotice != null)
        {
            if (!GameCenter.loginMng.FontHasAllCharacter(createName.label.bitmapFont, createName.value) || !GameCenter.loginMng.FontHasAllCharacter(createNotice.label.bitmapFont, createNotice.value)||GameCenter.loginMng.CheckName(createName.value))
            {
                GameCenter.messageMng.AddClientMsg(490);
                return;
            }
            GameCenter.guildMng.C2S_CreateNewGuild(createName.value, createNotice.value, 1);
        }
	}
    /// <summary>
    /// 前一页
    /// </summary>
    /// <param name="go"></param>
    void ClickFormer(GameObject go)
    {
        CurPage -= 1;
        ShowGuildList();    
    }
    /// <summary>
    /// 下一页
    /// </summary>
    /// <param name="go"></param>
    void ClickNext(GameObject go)
    {
        CurPage += 1;
        ShowGuildList(); 
    }
	/// <summary>
	/// 返回公会界面
	/// </summary>
	/// <param name="go">Go.</param>
	void BackGuildMain(GameObject go)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.GUILDMAIN);
	}
    #endregion
	void CloseWnd(GameObject go)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}

    /// <summary>
    /// 排序
    /// </summary>
    public class GuildItemCompare : IComparer<GuildData>
    {
        static GuildItemCompare instance;
        public static GuildItemCompare Instance
        {
            get
            {
                if (instance == null) instance = new GuildItemCompare();
                return instance;
            }
        }
        public GuildItemCompare() { }
        public int Compare(GuildData _x, GuildData _y)
        {
            if (_x != null && _y != null)
            {
                int ret = _x.guildRank.CompareTo(_y.guildRank);
                if (ret != 0) return ret;
                return 0;
            }
            else
            {
                Debug.LogError("比较数据为空，比较失败");
                return 0;
            }
        }
    } 
}
