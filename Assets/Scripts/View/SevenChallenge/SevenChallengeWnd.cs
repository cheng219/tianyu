//==============================================
//作者：唐源
//日期：2017/4/14
//用途：七日挑战窗口
//==============================================
using UnityEngine;
using st.net.NetBase;
using System.Collections;
using System.Collections.Generic;
public class SevenChallengeWnd : GUIBase
{
    #region UI控件
    public UILabel description;
    public GameObject challenge;
    public SevenChallengeUI[] challengeUI;
    public SevenChallengeSingle SingleUI;
    public UIButton btnClose;
    #endregion
    #region Unity函数
    void Awake()
    {
        mutualExclusion = true;
        FillData();
    }
    #endregion
    #region OnOpen
    protected override void OnOpen()
    {
        base.OnOpen();
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
    #endregion
    #region 事件句柄
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.sevenChallengeMng.updateSevenChallengeData+= RefreshWnd;
            if (btnClose != null)
                UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd;
        }
        else
        {
            GameCenter.sevenChallengeMng.updateSevenChallengeData-= RefreshWnd;
        }
    }
    #endregion
    #region UI控件的响应
    void CloseWnd(GameObject _obj)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }
    #endregion
    #region 窗口界面的刷新

    int CompareSevenDay(seven_day_target_list data1, seven_day_target_list data2)
    {
        if (data1.day_id > data2.day_id)
            return 1;
        else if (data1.day_id < data2.day_id)
            return -1;
        return 0;
    }
    void RefreshWnd()
    {
        List<seven_day_target_list> list = GameCenter.sevenChallengeMng.listInfo;
        list.Sort(CompareSevenDay);
        //Debug.Log(GameCenter.sevenChallengeMng.listInfo.Count);
        if (challengeUI != null && list != null)
        {
            for (int i = 0, count = list.Count; i < count; i++)
            {
                //Debug.Log("(int)list[i].day_id:" + (int)list[i].day_id);
                if (i < challengeUI.Length)
                {
                    challengeUI[i].UpdateUI((int)list[i].day_id, list[i].finish_num, (int)list[i].reward_state);
                    UIEventListener.Get(challengeUI[i].BtnOpenSingle.gameObject).parameter = (int)list[i].day_id;
                    UIEventListener.Get(challengeUI[i].BtnOpenSingle.gameObject).onClick = PreviewChallenge;
                }
            }
            for(int m= list.Count, count= challengeUI.Length;m< count;m++)
            {
                //Debug.Log("m:" + m);
                challengeUI[m].openShow.SetActive(false);
                challengeUI[m].notYetOpen.SetActive(true);
            }
        }
    }

    void PreviewChallenge(GameObject go)
    {
        int index = (int)UIEventListener.Get(go).parameter;
        //Debug.Log("发送七日挑战具体哪一天数据的请求");
        GameCenter.sevenChallengeMng.C2S_ReqSevenChallengeInfo(2, index);
        //Debug.Log("发给服务器的天数:" + (index));
        if (SingleUI != null)
        {
        //    SingleUI.FillRef(index);
            SingleUI.gameObject.SetActive(true);
        }
    }

    public void FillData()
    {
        if (challengeUI != null)
        {
            for(int i = 0, len = challengeUI.Length;i<len;i++)
            {
                challengeUI[i].FillUI(i + 1, 0, 0);
            }
        }
    }
    //public void DelayShow()
    //{
    //    SingleUI.gameObject.SetActive(true);
    //}
    #endregion
}
