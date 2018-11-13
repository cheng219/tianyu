//==================================
//作者：朱素云
//日期：2016/5/4
//用途：结义界面
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class SwornWnd : SubWnd
{
    public UIToggle[] uiTol;
    private int page = 0;

    public GameObject noSworn;
    public GameObject sworn;
    public GameObject task;

    public FriendShipUi swornL;
    public List<SwornInfoUi> swornR;

    public List<SwornTaskUi> taskUi;
    public UIButton teamMakeBtn;
    public UISlider integral;//积分
    public UILabel integralVal;
    public UILabel[] integralLab;
    public UIButton[] box;//积分盒子 
    public GameObject takeAward;
    public UILabel rewardDesLab;
    public ItemUI[] awardsItems;
    public UIButton takeBtn;
    public Transform alreadyTake;//已经领取
    public UIButton closeItemBtn;
    public UIButton ashuBtn;//npc阿叔

    protected SwornData data = null;
    protected List<brothers_list> brothers = new List<brothers_list>();
    protected List<brother_reward_info> isTakeAwared
    {
        get
        {
            return GameCenter.swornMng.isTakeAwared;
        }
    }
    private bool willCloseWnd = false;

    void Awake()
    {
        GameCenter.swornMng.C2S_ReqGetSwornInfo();
        GameCenter.swornMng.C2S_ReqBoxReward();

        if (ashuBtn != null) UIEventListener.Get(ashuBtn.gameObject).onClick = OnClikFinfAshu; 

        for (int i = 0, len = uiTol.Length; i < len; i++)
        {
            if (uiTol[i] != null)
            {
                EventDelegate.Add(uiTol[i].onChange, UITogOnChange);
            }
        }
        //一键组队
        if (teamMakeBtn != null) UIEventListener.Get(teamMakeBtn.gameObject).onClick = OnClickMakeTeam; 
    }

    protected override void OnOpen()
    { 
        base.OnOpen(); 
        Refresh();
        GameCenter.swornMng.OnSwornListUpdata += Refresh; 
    }

    protected override void OnClose()
    {
        base.OnClose();
        willCloseWnd = false;
        GameCenter.swornMng.OnSwornListUpdata -= Refresh; 
    }
    void Update()
    {
        if (willCloseWnd)
        {
            willCloseWnd = false;
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            GameCenter.uIMng.ReleaseGUI(GUIType.MAIL);
        }
    }
    /// <summary>
    /// 一键组队
    /// </summary> 
    void OnClickMakeTeam(GameObject go)
    {
        if (brothers.Count > 0)
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < brothers.Count; i++)
            {
                if (brothers[i].uid != data.playerId)
                {
                    ids.Add(brothers[i].uid);
                }
            }
            GameCenter.teamMng.C2S_TeamInvite(ids);
        }
    }
    /// <summary>
    /// 去找结义大哥结义
    /// </summary> 
    void OnClikFinfAshu(GameObject go)
    {
        NPCAIRef ashu = ConfigMng.Instance.GetNPCAIRefByType(500050);//结义大哥
        if (ashu != null)
        {
            if (GameCenter.curMainPlayer.GoTraceTarget(ashu.scene, ashu.sceneX, ashu.sceneY))
            {
                willCloseWnd = true;
                //GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                //GameCenter.uIMng.ReleaseGUI(GUIType.MAIL);
            }
        }
    }
    void OnDestroy()
    {
        for (int i = 0, len = uiTol.Length; i < len; i++)
        {
            if (uiTol[i] != null)
            {
                EventDelegate.Remove(uiTol[i].onChange, UITogOnChange);
            }
        }
    }

    void UITogOnChange()
    {
        for (int i = 0, len = uiTol.Length; i < len; i++)
        {
            if (uiTol[i] != null && uiTol[i].value)
            {
                page = i;
                Refresh();
                break;
            }
        } 
    }
    /// <summary>
    /// 点击宝箱
    /// </summary> 
    void OnClickTakeReward(GameObject go)
    { 
        int val = (int)UIEventListener.Get(go.gameObject).parameter;
        if (takeAward != null) takeAward.SetActive(true);
        if (isTakeAwared.Count > val && isTakeAwared[val].state == 1)//已经领取
        {
            if (takeBtn != null) takeBtn.gameObject.SetActive(false);
            if (alreadyTake != null) alreadyTake.gameObject.SetActive(true);
        }
        else
        {
            if (takeBtn != null) takeBtn.gameObject.SetActive(true);
            if (alreadyTake != null) alreadyTake.gameObject.SetActive(false);
        }
        if (closeItemBtn != null) UIEventListener.Get(closeItemBtn.gameObject).onClick = delegate { if (takeAward != null)takeAward.SetActive(false); };
        if (rewardDesLab != null) rewardDesLab.text = ConfigMng.Instance.GetUItext(83, new string[1] { (brothers.Count * (val + 1)).ToString() });
        for (int j = 0, max = awardsItems.Length; j < max; j++)
        {
            if (takeBtn != null && takeBtn.gameObject.activeSelf)
            {
                UISpriteEx takeEx = takeBtn.GetComponentInChildren<UISpriteEx>();
                if (data.taskNum < brothers.Count * (val + 1))//没有达到领取条件
                {
                    if (takeEx != null) takeEx.IsGray = UISpriteEx.ColorGray.Gray;
                }
                else
                {
                    if (takeEx != null) takeEx.IsGray = UISpriteEx.ColorGray.normal;
                    UIEventListener.Get(takeBtn.gameObject).onClick = delegate
                    {
                        if (takeAward != null) takeAward.SetActive(false);
                        GameCenter.swornMng.C2S_ReqTakeBoxReward(val + 1);
                    };
                }
            }
            if (data.Reward(val).Count > j) awardsItems[j].FillInfo(new EquipmentInfo(data.Reward(val)[j].eid, data.Reward(val)[j].count, EquipmentBelongTo.PREVIEW));
            else
                awardsItems[j].gameObject.SetActive(false);
        }
    }

    void Refresh()
    {
        if (noSworn != null) noSworn.SetActive(false);
        if (takeAward != null) takeAward.SetActive(false);
        data = GameCenter.swornMng.data; 
        if (data == null)
        {
            if (task != null) task.SetActive(false);
            if (sworn != null)sworn.SetActive(false);
            if (noSworn != null) noSworn.SetActive(true);
            if (page == 1) GameCenter.messageMng.AddClientMsg(503);
            return;
        }
        brothers = data.brothers;
        if (page == 0)//结义
        { 
            if (task != null) task.SetActive(false);
            if (sworn != null) sworn.SetActive(true); 
            swornL.SwornData = data;
            for (int i = 0; i < brothers.Count; i++)
            {
                if (brothers[i].uid == data.playerId)
                {
                    swornR[0].Brother = brothers[i];
                    break;
                }
            }
            for (int i = 1; i < swornR.Count; i++)
            {
                if (brothers.Count > i)
                {
                    swornR[i].Brother = brothers[i]; 
                }
                else
                    swornR[i].gameObject.SetActive(false);
            }
        }
        else//任务
        { 
            RefreshSwornTask();
        }
    }

    void RefreshSwornTask()
    {
        if (sworn != null) sworn.SetActive(false);
        if (task != null) task.SetActive(true);
        for (int i = 0, max = taskUi.Count;i < max; i++)
        {
            if (GameCenter.swornMng.swornTask.Count > i)
                taskUi[i].Task = GameCenter.swornMng.swornTask[i];
        }
        if (integral != null)
        {
            integral.value = (float)data.taskNum / (brothers.Count * 3);
            integral.thumb.transform.localPosition = new Vector3(integral.thumb.transform.localPosition.x, -33);
        }
        if (integralVal != null) integralVal.text = data.taskNum + "/" + brothers.Count * 3;
        if (integralLab.Length > 0)
        {
            for (int i = 0; i < integralLab.Length; i++)
            {
                integralLab[i].text = (brothers.Count * (i + 1)).ToString();
                BoxOpenUi openbox = box[i].GetComponent<BoxOpenUi>();
                if (openbox != null)
                {
                    if (isTakeAwared.Count > i)
                    {
                        if (isTakeAwared[i].state == 1)//已经领取
                        {
                            openbox.show(true);
                        }
                        else
                            openbox.show(false);
                    }
                }
            }
        }
        if (box.Length > 0)
        {
            for (int i = 0; i < box.Length; i++)
            {
                UIEventListener.Get(box[i].gameObject).onClick -= OnClickTakeReward;
                UIEventListener.Get(box[i].gameObject).onClick += OnClickTakeReward;
                UIEventListener.Get(box[i].gameObject).parameter = i;
            }
        } 
    }
}
