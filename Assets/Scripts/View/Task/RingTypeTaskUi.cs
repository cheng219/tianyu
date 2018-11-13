//======================================================
//作者:朱素云
//日期:2017/5/3
//用途:环式任务类型ui
//======================================================
using UnityEngine;
using System.Collections;
using st.net.NetBase;

public class RingTypeTaskUi : MonoBehaviour
{ 
    //public UILabel needLev;//做该任务需要的等级
    public GameObject lockObj;//等级未达到没有解锁
    public GameObject data;
    public GameObject finishied;
    public UIButton refreshBtn;
      
    public UILabel prograss;//进度
    public UILabel num;//次数   
    protected int costRefresh = 20;
    public int lev;
    public int taskType = 1;
    protected int restTime;

	// Use this for initialization
	void Start () {
        if (refreshBtn != null) UIEventListener.Get(refreshBtn.gameObject).onClick = RefreshRestNum; 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetData(task_surround_info _info)
    {
        restTime = (int)_info.surplus_refresh_num;
        if (lockObj != null) lockObj.SetActive(GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < lev);
        if (data != null) data.SetActive(GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= lev && _info.finish_num < 10);
        if (finishied != null) finishied.SetActive(GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= lev && _info.finish_num == 10);
        if (prograss != null && data != null && data.activeSelf) prograss.text = _info.finish_num + "/10";
        if (finishied != null && finishied.activeSelf)
        { 
            int allRefreshTime = 0;
            VIPRef vip = ConfigMng.Instance.GetVIPRef(GameCenter.vipMng.VipData != null ? GameCenter.vipMng.VipData.vLev : 0);
            if (vip != null) allRefreshTime = vip.ringRefreshNum;
            StepConsumptionRef consume = ConfigMng.Instance.GetStepConsumptionRef(allRefreshTime - restTime + 1);
            if (consume != null && consume.ringTaskCost.Count > 0)
            {
                costRefresh = consume.ringTaskCost[0].count;
            }
            if (num != null) num.text = _info.surplus_refresh_num.ToString() + "/" + allRefreshTime;
        }
    }  
    /// <summary>
    /// 刷新剩余次数
    /// </summary> 
    void RefreshRestNum(GameObject go)
    {
        if (restTime <= 0)
        {
            MessageST mst = new MessageST();
            mst.messID = 546;
            mst.delYes = (x) =>
            {
                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else
        {
            if (finishied != null && finishied.activeSelf)
            {
                MessageST mst = new MessageST();
                mst.messID = 552;
                mst.words = new string[1] { costRefresh.ToString() };
                mst.delYes = (x) =>
                {
                    GameCenter.taskMng.C2S_ResetRingTask(taskType);
                    GameCenter.taskMng.curRingTaskType = taskType;
                    if (GameCenter.taskMng.GetRingAutoFinish(GameCenter.taskMng.curRingTaskType))
                    {
                        GameCenter.curMainPlayer.StopForNextMove();
                        GameCenter.taskMng.SetRingAutoFinishType(GameCenter.taskMng.curRingTaskType, false);
                    } 
                    GameCenter.uIMng.SwitchToUI(GUIType.RINGTASK);
                };
                GameCenter.messageMng.AddClientMsg(mst);
            }
        }
    }
}
