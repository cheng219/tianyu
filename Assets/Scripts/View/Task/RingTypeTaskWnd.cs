//======================================================
//作者:朱素云
//日期:2017/5/3
//用途:环式任务类型界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class RingTypeTaskWnd : GUIBase
{

    public RingTypeTaskUi[] ringTypes;
    public UIButton closeBtn;

    #region unity

    void Awake()
    {
        layer = GUIZLayer.NORMALWINDOW;
        mutualExclusion = true;
        GameCenter.taskMng.C2S_GetRingTaskPrograss();
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = delegate { 
            GameCenter.uIMng.SwitchToUI(GUIType.NONE); 
        };
    }
    // Use this for initialization
	void Start () { 
        for (int i = 0, max = ringTypes.Length; i < max; i++)
        {
            UIEventListener.Get(ringTypes[i].gameObject).onClick = OnClickRingType;
            UIEventListener.Get(ringTypes[i].gameObject).parameter = ringTypes[i];
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    #endregion


    #region GUIBase

    protected override void OnOpen ()
	{
		base.OnOpen ();
        RefreshTask(TaskType.Ring);
        //RefreshLev(ActorBaseTag.Level, 1 , true);
    }
	protected override void OnClose ()
	{
		base.OnClose ();
	}
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
        if (_bind)
        {
            //GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RefreshLev;
            GameCenter.taskMng.OnTaskGroupUpdate += RefreshTask;
        }
        else
        {
           //GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshLev;
            GameCenter.taskMng.OnTaskGroupUpdate -= RefreshTask;
        }
	}
    #endregion

    #region 刷新

    void RefreshTask(TaskType _type)
    {
        if (_type == TaskType.Ring)
        {
            List<task_surround_info> ringTaskProgress = GameCenter.taskMng.ringTaskProgress;
            for (int i = 0, max = ringTypes.Length; i < max; i++)
            {
                for (int j = 0, len = ringTaskProgress.Count; j < len; j++)
                {
                    if (ringTaskProgress[j].task_sort == ringTypes[i].taskType)
                    { 
                        ringTypes[i].SetData(ringTaskProgress[j]);
                    }
                } 
            }
        }
    }

    //void RefreshLev(ActorBaseTag tag, ulong val, bool state)
    //{
    //    if (tag == ActorBaseTag.Level)
    //    {
    //        for (int i = 0, max = ringTypes.Length; i < max; i++)
    //        {
    //            ringTypes[i].RefreshLev();
    //        }
    //    }
    //}

    #endregion

    #region 事件
    void OnClickRingType(GameObject go)
    {
        RingTypeTaskUi ringTypeTaskUi = (RingTypeTaskUi)UIEventListener.Get(go).parameter;
        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= ringTypeTaskUi.lev)
        { 
            List<task_surround_info> ringTaskProgress = GameCenter.taskMng.ringTaskProgress;
            for (int i = 0, len = ringTaskProgress.Count; i < len; i++)
            {
                if (ringTaskProgress[i].task_sort == ringTypeTaskUi.taskType || ringTaskProgress[i].finish_num == 10)
                {
                    GameCenter.taskMng.curRingTaskType = ringTypeTaskUi.taskType;
                    GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                    if (GameCenter.taskMng.GetRingAutoFinish(GameCenter.taskMng.curRingTaskType))
                    {
                        GameCenter.curMainPlayer.StopForNextMove();
                        GameCenter.taskMng.SetRingAutoFinishType(GameCenter.taskMng.curRingTaskType, false);
                    } 
                    GameCenter.uIMng.SwitchToUI(GUIType.RINGTASK);
                    break;
                }
            } 
        }
    } 
   
    #endregion
}
