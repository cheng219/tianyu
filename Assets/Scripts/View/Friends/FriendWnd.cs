//==================================
//作者：朱素云
//日期：2016/4/10
//用途：仙友界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendWnd : SubWnd
{
    #region 数据
    /// <summary>
    /// 标记要显示的界面（0 好友 1 附近的人 2 黑名单 3 仇人 4 查找好友 5 管理好友）
    /// </summary>
    private int page = 0;
    /// <summary>
    /// 控制界面显示的Tog
    /// </summary>
    public List<UIToggle> uiTog;
    /// <summary>
    /// 好友
    /// </summary>
    public OthersItemUi friendItems;
    /// <summary>
    /// 好友管理列表ui
    /// </summary> 
    public GameObject manage;
    /// <summary>
    /// 查找好友界面
    /// </summary>
    public FindFriendUi findFriendUi; 
    /// <summary>
    /// 附近没有人
    /// </summary>
    public GameObject nobody;
    /// <summary>
    /// 没有好友
    /// </summary>
    public GameObject nofriend;
    /// <summary>
    /// 各种操作界面
    /// </summary>
    public List<OperateUi> operateWindow = new List<OperateUi>(); 
    /// <summary>
    /// 当前操作Ui
    /// </summary>
    protected OperateUi curOperateUi;
    /// <summary>
    /// 好友管理仙友ui
    /// </summary>
    protected FDictionary friendItemUiList = new FDictionary(); 
    protected List<FriendsInfo> list
    {
        get
        {
            return GameCenter.friendsMng.friendList;
        }
    } 
    public UILabel noTuiSong;
    public UIScrollView Scro;
    public UIScrollView ComendScro;
    private UIPanel panel;
    private float itemWidth = 430;
    private float itemHeiht = -130;

    private bool isCreateItems = false;


    #region 好友管理
    /// <summary>
    /// 反选删除
    /// </summary>
    public UIButton deleteNochoose;
    /// <summary>
    /// 删除选中
    /// </summary>
    public UIButton deleteChoose;
    /// <summary>
    /// 删除所有
    /// </summary>
    public UIButton deleteAll;
    /// <summary>
    /// 将选中的添加到黑名单
    /// </summary>
    public UIButton addChooseToBlack;
    /// <summary>
    /// 全部取消
    /// </summary>
    public UIButton cancelAll;

    protected FDictionary FriendsDic
    {
        get
        {
            return GameCenter.friendsMng.friendsDic;
        }
    }
    protected List<int> ChoosedList
    {
        get
        {
            return GameCenter.friendsMng.chooseList;
        }
    }
    #endregion

    #endregion

    #region unity
    void Awake()
    { 
        if(Scro != null)panel = Scro.GetComponent<UIPanel>(); 
        friendItems.gameObject.SetActive(false);
    }
    void Start()
    {
        page = 0;
        if (panel != null) panel.SetRect(42, -44, 886, 410); 
        GameCenter.friendsMng.C2S_ReqFriendsList(1);  
    }
    void Update()
    {
        if (isCreateItems)
        {
            isCreateItems = false;
            CreateFriendItem();
        }
    }
    #endregion

    #region 刷新 
    /// <summary>
    /// 创建列表
    /// </summary>
    void CreateFriendItem()
    {   
        GameCenter.friendsMng.isAddInAdvice = false;
        findFriendUi.gameObject.SetActive(false);
        manage.SetActive(false);
        nofriend.SetActive(false);
        nobody.SetActive(false);
        noTuiSong.gameObject.SetActive(false); 
        if (curOperateUi != null) curOperateUi.gameObject.SetActive(false); 
        switch ((FriendType)page)
        {
            case FriendType.FRIEND:
                GameCenter.friendsMng.findFriend = null;
                if (list.Count <= 0)
                {
                    nofriend.SetActive(true);  
                    return;
                }
                break;
            case FriendType.NEARBY: 
                GameCenter.friendsMng.findFriend = null;
                GameCenter.friendsMng.GetNearbyPlayer(); 
                if (list.Count <= 0)
                { 
                    nobody.SetActive(true); 
                    return;
                }
                break;
            case FriendType.BLACKLIST:
            case FriendType.ENEMY: 
                GameCenter.friendsMng.findFriend = null;
                if (list.Count <= 0)
                { 
                    return;
                }
                break;
            case FriendType.COMMEND:
                { 
                    GameCenter.friendsMng.isAddInAdvice = true;
                    findFriendUi.gameObject.SetActive(true);
                    findFriendUi.ShowTuiSong();
                    if (GameCenter.friendsMng.findFriend != null)//如果有查找的右边显示查找的
                    {
                        findFriendUi.FriendsData = GameCenter.friendsMng.findFriend;
                    }
                    else
                    {
                        if (findFriendUi.FriendsData != null)
                        {
                            findFriendUi.FriendsData = null; 
                        }
                        else
                            findFriendUi.ShowFindInfo();
                    }
                    if (list.Count <= 0)
                    { 
                        GameCenter.friendsMng.isAddInAdvice = false;
                        noTuiSong.gameObject.SetActive(true);
                        return;
                    }  
                }
                break;
            case FriendType.MANAGEFRIEND:
                ChoosedList.Clear();
                GameCenter.friendsMng.findFriend = null;
                manage.SetActive(true); 
                if (list.Count <= 0)
                { 
                    return;
                } 
                break;
            default:
                break;
        } 
        OthersItemUi itemUi = null; 
        for (int i = 0,max = list.Count; i < max; i++)
        {
            if (!friendItemUiList.ContainsKey(list[i].configId))
            {
                GameObject go = null;
                go = (GameObject)GameObject.Instantiate(friendItems.gameObject);
                itemUi = go.GetComponent<OthersItemUi>();
                if (page == (int)FriendType.COMMEND)
                { 
                    go.transform.parent = ComendScro.transform; 
                    go.transform.localPosition = new Vector3(30, i * itemHeiht - 36);  
                }
                else if (page == (int)FriendType.MANAGEFRIEND)
                {
                    go.transform.parent = friendItems.transform.parent; 
                    if (itemUi != null)
                    {
                        itemUi.transform.localPosition = new Vector3(i % 2 * itemWidth + 30, i / 2 * itemHeiht); 
                    }
                }
                else
                {
                    go.transform.parent = friendItems.transform.parent;
                    go.transform.localPosition = new Vector3(i % 2 * itemWidth + 30, i / 2 * itemHeiht); 
                } 
                if (itemUi != null)
                {
                    if (itemUi.sendFlowerBtn != null) itemUi.sendFlowerBtn.SetActive(page == 0);
                    if (itemUi.intimacyLab != null) itemUi.intimacyLab.SetActive(page == 0 || page == 4);
                    if (itemUi.sameScrDes != null) itemUi.sameScrDes.SetActive(page == 1);
                    if (itemUi.placeLab != null) itemUi.placeLab.gameObject.SetActive(page == 3 && list[i].IsOnline);
                    if (itemUi.chooseTol != null) itemUi.chooseTol.gameObject.SetActive(page == 5); 

                    itemUi.FriendsData = list[i];
                    friendItemUiList[list[i].configId] = itemUi;
                    UIEventListener.Get(itemUi.gameObject).onClick -= OnClickFriendItem;
                    UIEventListener.Get(itemUi.gameObject).onClick += OnClickFriendItem;
                    UIEventListener.Get(itemUi.gameObject).parameter = itemUi; 
                }
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
            }
            else
            {
                itemUi = friendItemUiList[list[i].configId] as OthersItemUi;
                if (itemUi != null)
                { 
                    if (page == (int)FriendType.COMMEND)
                    { 
                        itemUi.transform.parent = ComendScro.transform; 
                        itemUi.transform.localPosition = new Vector3(30, i * itemHeiht - 36);  
                    }
                    else if (page == (int)FriendType.MANAGEFRIEND)
                    {
                        itemUi.transform.parent = friendItems.transform.parent;
                        itemUi.transform.localPosition = new Vector3(i % 2 * itemWidth + 30, i / 2 * itemHeiht); 
                    }
                    else
                    {
                        itemUi.transform.parent = friendItems.transform.parent;
                        itemUi.transform.localPosition = new Vector3(i % 2 * itemWidth + 30, i / 2 * itemHeiht);  
                    }

                    if (itemUi.sendFlowerBtn != null) itemUi.sendFlowerBtn.SetActive(page == 0);
                    if (itemUi.intimacyLab != null) itemUi.intimacyLab.SetActive(page == 0 || page == 4);
                    if (itemUi.sameScrDes != null) itemUi.sameScrDes.SetActive(page == 1);
                    if (itemUi.placeLab != null) itemUi.placeLab.gameObject.SetActive(page == 3 && list[i].IsOnline);
                    if (itemUi.chooseTol != null) itemUi.chooseTol.gameObject.SetActive(page == 5); 
                    itemUi.gameObject.SetActive(true);
                    itemUi.FriendsData = list[i];
                    friendItemUiList[list[i].configId] = itemUi; 
                    UIEventListener.Get(itemUi.gameObject).onClick -= OnClickFriendItem;
                    UIEventListener.Get(itemUi.gameObject).onClick += OnClickFriendItem;
                    UIEventListener.Get(itemUi.gameObject).parameter = itemUi; 
                    itemUi.transform.localScale = Vector3.one;
                } 
            }
        } 

        if (manage != null && manage.activeSelf)
        {
            if (deleteNochoose != null) UIEventListener.Get(deleteNochoose.gameObject).onClick = OnClickDeleteNoChoose;
            if (deleteChoose != null) UIEventListener.Get(deleteChoose.gameObject).onClick = delegate
            {
                if (ChoosedList.Count > 0)
                {
                    MessageST mst = new MessageST();
                    mst.messID = 383;
                    mst.delYes = delegate
                    {
                        GameCenter.friendsMng.C2S_ReqOperateFriend(FriendOperation.DELETEFRIEND, ChoosedList);
                    };
                    GameCenter.messageMng.AddClientMsg(mst);
                }
            };
            if (deleteAll != null) UIEventListener.Get(deleteAll.gameObject).onClick = OnClickDeleteAll;
            if (addChooseToBlack != null) UIEventListener.Get(addChooseToBlack.gameObject).onClick = delegate
            {
                GameCenter.friendsMng.C2S_ReqOperateFriend(FriendOperation.ADDBALCKLIST, ChoosedList);
            };
            if (cancelAll != null) UIEventListener.Get(cancelAll.gameObject).onClick = OnClickCancleAll;
        }
    }


    /// <summary>
    /// 刷新列表
    /// </summary>
    void OnFriendListUpdate()
    {
        foreach (OthersItemUi ui in friendItemUiList.Values)
        {
            ui.gameObject.SetActive(false);  
            ui.sp.gameObject.SetActive(false);
        }
        if (operateWindow.Count > page) curOperateUi = operateWindow[page];
        isCreateItems = true;
    }

    #endregion


    #region 界面管理

    protected override void OnOpen()
    {
        base.OnOpen();
        for (int i = 0, len = uiTog.Count; i < len; i++)
        {
            if (uiTog[i] != null)
            {
                UIEventListener.Get(uiTog[i].gameObject).onClick = ClickToggleEvent;
            }
        }
        CloseOperateUi();
        OnFriendListUpdate(); 
        GameCenter.friendsMng.OnFriendsDicUpdata += OnFriendListUpdate; 
    }
    protected UIToggle lastChangeToggle = null;
    protected void ClickToggleEvent(GameObject go)
    {
        UIToggle toggle = go.GetComponent<UIToggle>();
        if (toggle != lastChangeToggle)
        {
            UITogOnChange();
        }
        if (toggle != null && toggle.value) lastChangeToggle = toggle;
    }
    void CloseOperateUi()
    {
        for (int i = 0; i < operateWindow.Count; i++)
        {
            if (operateWindow[i] != null) operateWindow[i].gameObject.SetActive(false);
        } 
    }
    protected override void OnClose()
    {
        base.OnClose();
        isCreateItems = false;
        GameCenter.friendsMng.OnFriendsDicUpdata -= OnFriendListUpdate; 
    }
    void OnDisable()
    {
        GameCenter.friendsMng.OnFriendsDicUpdata -= OnFriendListUpdate;
    }

    #endregion 

    #region 控件事件

    void UITogOnChange()
    {
        for (int i = 0, len = uiTog.Count; i < len; i++)
        {
            if (uiTog[i] != null && uiTog[i].value)
            {
                page = i;
                if (page != (int)FriendType.COMMEND)
                {
                    if (page == (int)FriendType.MANAGEFRIEND)
                    {
                        panel.SetRect(42, 1, 886, 336); 
                    }
                    else
                    {
                        panel.SetRect(42, -44, 886, 410);
                    } 
                    Scro.SetDragAmount(0, 0, false);
                }
                else
                {
                    ComendScro.SetDragAmount(0, 0, false);
                }
                if (page <= 4) GameCenter.friendsMng.C2S_ReqFriendsList(page + 1);
                else
                    GameCenter.friendsMng.C2S_ReqFriendsList(1);
                break;
            }
        }
        OnFriendListUpdate();
    }
 
    /// <summary>
    /// 点击列表，显示操作按钮
    /// </summary> 
    void OnClickFriendItem(GameObject go)
    { 
        if (page < 5)
        {
            OthersItemUi friend = (OthersItemUi)UIEventListener.Get(go).parameter;
            UIToggle tog = friend.GetComponent<UIToggle>();
            if (tog != null) friend.sp.gameObject.SetActive(tog.value);
            GameCenter.friendsMng.curFriend = GameCenter.friendsMng.GetFriendsInfoById(friend.FriendsData.configId);
            if (operateWindow.Count > page) curOperateUi = operateWindow[page];
            curOperateUi.gameObject.SetActive(true);
            curOperateUi.transform.localPosition = friend.transform.position - new Vector3(0, 100);
            if (page == 3) curOperateUi.isTransfer = true;
            else
                curOperateUi.isTransfer = false; 

            if (curOperateUi != null)
                curOperateUi.show();
        }
    } 

    void OnClickDeleteNoChoose(GameObject go)
    {
        List<int> choosed = new List<int>();
        foreach (OthersItemUi ui in friendItemUiList.Values)
        {
            if (ui.chooseTol != null && ui.chooseTol.gameObject.activeSelf && !ui.chooseTol.value)
            {
                choosed.Add(ui.FriendsData.configId);
            }
            if (ui.chooseTol != null && ui.chooseTol.gameObject.activeSelf)
            {
                ui.chooseTol.value = !ui.chooseTol.value;
            }
        }
        if (choosed.Count > 0)
        {
            MessageST mst = new MessageST();
            mst.messID = 383;
            mst.delYes = delegate
            {
                GameCenter.friendsMng.C2S_ReqOperateFriend(FriendOperation.DELETEFRIEND, choosed);
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
    }

    void OnClickDeleteAll(GameObject go)
    {
        if (GameCenter.friendsMng.friendsUid.Count > 0)
        {
            MessageST mst = new MessageST();
            mst.messID = 383;
            mst.delYes = delegate
            {
                GameCenter.friendsMng.C2S_ReqOperateFriend(FriendOperation.DELETEFRIEND, GameCenter.friendsMng.friendsUid);
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
    }

    void OnClickCancleAll(GameObject go)
    {
        foreach (OthersItemUi ui in friendItemUiList.Values)
        {
            ui.chooseTol.value = false;
            ChoosedList.Remove(ui.FriendsData.configId);
            //ui.sp.gameObject.SetActive(false);
        }
    }
    #endregion
}
