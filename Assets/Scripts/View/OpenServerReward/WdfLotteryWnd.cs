//======================================================
//作者:朱素云
//日期:2017/4/5
//用途:抽奖界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class WdfLotteryWnd : SubWnd
{

    #region 控件 
    public UILabel curAllCount;//奖池数量
    public UITimer remainTime;//活动时间 
    public UIButton recordLoterry;//抽奖记录 
    public UIButton loterryOnceUseDiamond;//抽一次
    public UIButton loterryTenTimes;//抽十次
    public GameObject firstRecharge;
    public UILabel firstprice;
    public UILabel price1;
    public UILabel price2;

    public WdfRotateItemUi basisUi;//基础转盘
    public WdfLotteryReward basicLotteryResult;
    public WdfRotateItemUi advanceUi;//进阶转盘
    public WdfLotteryReward advanceLotteryResult;

    protected WdfLotteryData loteryInfo;

    public GameObject recordGo;
    public UIButton closeRecord;
    protected List<RewardRecord> RewardRecordList = new List<RewardRecord>();
    public GameObject  rewardDrid;
    public GameObject item;
   
    #endregion

    #region unity
     

    protected override void OnOpen()
    {
        base.OnOpen();
        Refresh();
        GameCenter.openServerRewardMng.IsRotateOver = false;
        GameCenter.openServerRewardMng.isRotate = false;
        GameCenter.openServerRewardMng.OnLotteryDataUpdate += Refresh;
        GameCenter.openServerRewardMng.OnLotteryResultUpdate += RefreshResult;
        GameCenter.openServerRewardMng.OnRotateOverUpdate += AfterRotateOpenResult;
        GameCenter.openServerRewardMng.OnLotteryRecordUpdate += RefreshReWardRecord;
        if (loteryInfo != null) loteryInfo.OnRewardCountUpdate += RefreshRewardCount;
    }
    protected override void OnClose()
    {
        base.OnClose(); 
        GameCenter.openServerRewardMng.OnLotteryDataUpdate -= Refresh;
        GameCenter.openServerRewardMng.OnLotteryResultUpdate -= RefreshResult;
        GameCenter.openServerRewardMng.OnRotateOverUpdate -= AfterRotateOpenResult;
        GameCenter.openServerRewardMng.OnLotteryRecordUpdate -= RefreshReWardRecord;
        if (loteryInfo != null) loteryInfo.OnRewardCountUpdate -= RefreshRewardCount;
    }
     
	// Use this for initialization
	void Start () {
         
        if (loterryOnceUseDiamond != null)
        {
            UIEventListener.Get(loterryOnceUseDiamond.gameObject).onClick = LoterryOnceUseDiamond;
        }
        if (loterryTenTimes != null)
        {
            UIEventListener.Get(loterryTenTimes.gameObject).onClick = LoterryTenTimes;
        }
        if (recordLoterry != null) UIEventListener.Get(recordLoterry.gameObject).onClick = OnClickRecordLoterry;
        if (closeRecord != null) UIEventListener.Get(closeRecord.gameObject).onClick = delegate { recordGo.SetActive(false); }; 
	}
	  
    #endregion

    #region 事件
    /// <summary>
    /// 奖池数量刷新
    /// </summary>
    void RefreshRewardCount()
    { 
        if (loteryInfo != null && curAllCount != null) curAllCount.text = loteryInfo.allRewarCount.ToString();
        if (price2 != null) price2.text = loteryInfo.price2.ToString();
        if (firstprice != null) firstprice.text = ((loteryInfo.price2) / 2).ToString();
        if (firstRecharge != null) firstRecharge.SetActive(!loteryInfo.isLotteriedTenTimes);
    }
    void Refresh()
    { 
        loteryInfo = GameCenter.openServerRewardMng.lotteryData;
        if (this == null)
            return;
        if (loteryInfo == null)
            return; 
        this.gameObject.SetActive(true);
        if (basicLotteryResult != null) basicLotteryResult.gameObject.SetActive(false);
        if (advanceLotteryResult != null) advanceLotteryResult.gameObject.SetActive(false);
        if (remainTime != null)
        {
            remainTime.StartIntervalTimer(loteryInfo.restTime);
            remainTime.onTimeOut = (x) =>
            {
                GameCenter.openServerRewardMng.C2S_AskLotteryInfo(); 
            };
        }
        if (price1 != null)price1.text = loteryInfo.price1.ToString(); 

        RefreshRewardCount();

        if (loteryInfo.basicReward.Count > 0)
        {
            if (basisUi != null)
            {
                basisUi.ResetData();
                basisUi.SetReward(loteryInfo.basicReward); 
            } 
        }
        if (loteryInfo.advanceReward.Count > 0)
        {
            if (advanceUi != null)
            {
                advanceUi.ResetData();
                advanceUi.SetReward(loteryInfo.advanceReward, true); 
            }
        }
    }

    void RefreshResult()
    {
        if (advanceUi != null)
        {
            advanceUi.ResetData();
        }
        if (basisUi != null)
        {
            basisUi.ResetData();
        }
        List<lucky_wheel_reward_info> lottryResult = GameCenter.openServerRewardMng.lottryResult;
        if (lottryResult.Count > 1)//十抽结果
        { 
            if (advanceLotteryResult != null)
            {
                GameCenter.openServerRewardMng.isRotate = false;
                advanceLotteryResult.gameObject.SetActive(true);
                advanceLotteryResult.SetData(lottryResult);
            }
        }
        else//一抽结果
        {
            if (basisUi != null)
            {
                basisUi.ResetData();
                if (lottryResult.Count > 0)
                {
                    basisUi.BegainRotate(lottryResult[0], lottryResult[0].wheel_type == 2);
                }
            }
        }
    }

    /// <summary>
    /// 抽一次转完后打开结果
    /// </summary>
    void AfterRotateOpenResult()
    { 
        if (GameCenter.openServerRewardMng.IsRotateOver)
        { 
            if (GameCenter.openServerRewardMng.lottryResult.Count > 0)
            {
                if (GameCenter.openServerRewardMng.lottryResult[0].wheel_type == 2)//奖励一次进阶转盘
                {
                    if (advanceUi != null)
                    { 
                        advanceUi.BegainRotate(GameCenter.openServerRewardMng.lottryResult[0]);
                        GameCenter.openServerRewardMng.ReSetResult();
                    }
                }
                else//获得普通转盘奖励
                {
                    if (basicLotteryResult != null)
                    {
                        GameCenter.openServerRewardMng.isRotate = false;
                        basicLotteryResult.gameObject.SetActive(true);
                        basicLotteryResult.SetData(GameCenter.openServerRewardMng.lottryResult);
                    }
                }
            }
            else
            {
                if (basicLotteryResult != null)
                {
                    basicLotteryResult.gameObject.SetActive(true);
                    basicLotteryResult.SetData(GameCenter.openServerRewardMng.lottryResultOne);
                }
            }
        } 
    }


    /// <summary>
    /// 销毁预制
    /// </summary>
    void Destroyitem()
    {
        if (rewardDrid != null)
        {
            rewardDrid.transform.DestroyChildren();
        }
    }

    /// <summary>
    /// 刷新抽奖记录
    /// </summary>
    void RefreshReWardRecord()
    {
        RewardRecordList.Clear();
        Destroyitem(); 
        List<lucky_wheel_record> recordList = GameCenter.openServerRewardMng.lottryRecord; 
        if (item != null)
        {
            for (int i = 0, max = recordList.Count; i < max; i++)
            { 
                EquipmentInfo info = new EquipmentInfo((int)recordList[i].item_type, (int)recordList[i].amount, EquipmentBelongTo.PREVIEW);
                GameObject go = GameObject.Instantiate(item);
                go.transform.parent = rewardDrid.transform;
                go.transform.localPosition = new Vector3(0, 40 * i , 0);
                go.transform.localScale = Vector3.one;
                go.gameObject.SetActive(true);
                RewardRecord record = go.GetComponent<RewardRecord>();
                string str1 = recordList[i].name;
                string str2 = info.ItemStrColor + info.ItemName + "x" + recordList[i].amount;
                int[] arrayId = { i, info.EID };
                //Debug.Log("玩家的名字：=  " + GameCenter.treasureHouseMng.playerName[i]);
                //string str2 = ConfigMng.Instance.GetUItext(22)
                if (arrayId != null && !string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2))
                {
                    record.FillInfo(str1, str2, arrayId); 
                    RewardRecordList.Add(record);
                }
            }
        }
        else
        {
            Debug.LogError("名为 item 的预制为空");
        }
        //rewardDrid.repositionNow = true;
    } 
     
    void LoterryOnceUseDiamond(GameObject _go)
    {
        if (GameCenter.openServerRewardMng.isRotate) return;
        if (advanceUi != null)
        {
            advanceUi.ResetData();
        }
        if (basisUi != null)
        {
            basisUi.ResetData();
        }
        GameCenter.openServerRewardMng.C2S_AskLottery(1);
    }
    void LoterryTenTimes(GameObject _go)
    {
        if (GameCenter.openServerRewardMng.isRotate) return;
        if (advanceUi != null)
        {
            advanceUi.ResetData();
        }
        if (basisUi != null)
        {
            basisUi.ResetData();
        }
        GameCenter.openServerRewardMng.C2S_AskLottery(2);
    }
    void OnClickRecordLoterry(GameObject _go)
    { 
        GameCenter.openServerRewardMng.C2S_AskLotteryRecord();
        recordGo.SetActive(true); 
    }
    #endregion
}
