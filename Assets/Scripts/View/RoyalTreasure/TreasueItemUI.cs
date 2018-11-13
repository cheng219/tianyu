//======================================================
//作者:鲁家旗
//日期:2017/1/19
//用途:宝箱单个UI
//======================================================
using UnityEngine;
using System.Collections;
using System;
public class TreasueItemUI : MonoBehaviour 
{
    public GameObject vipGo;
    public UIButton goRecharge;
    public GameObject haveItemGo;
    public GameObject notHaveItemGo;
    public UILabel nameLabel;
    public UISprite treasureSp;

    public GameObject timeCountDown;
    public UILabel timeCount;
    public UITimer timer;
    public GameObject timeGo;

    public UIButton openBtn;
    public UIButton rightAwayBtn;//加速开启
    public UIButton quickenBtn;//立即开启
    public UIButton getRewardBtn;

    public UILabel dimanoNum;
    public UILabel rewardBeiShu;

    int needGold = 0;//加速开启所需元宝
    float beishu = 0;//当前时间段的奖励倍数
    protected RoyalTreaureData data;
    protected RoyalBoxRef refData;
    void Start()
    {
        //激活宝箱
        if (openBtn != null) UIEventListener.Get(openBtn.gameObject).onClick = delegate
        {
            if(data != null)GameCenter.royalTreasureMng.C2S_ReqActiveRoyalBox(data.ID);
        };
        //加速开启 领取奖励
        if (rightAwayBtn != null) UIEventListener.Get(rightAwayBtn.gameObject).onClick = delegate
        {
            if (data != null) PopTip(true);
        };
        //立即开启 领取奖励
        if (quickenBtn != null) UIEventListener.Get(quickenBtn.gameObject).onClick = delegate
        {
            if (data != null) PopTip(false);
        };
        //普通开启 领取奖励
        if (getRewardBtn != null) UIEventListener.Get(getRewardBtn.gameObject).onClick = delegate
        {
            if (data != null)
            {
                GameCenter.royalTreasureMng.curGetRewardBoxData = data;
                GameCenter.royalTreasureMng.C2S_ReqGetRoyalReward(data.ID, 1);
            }
        };
        //点击成为Vip
        if (goRecharge != null) UIEventListener.Get(goRecharge.gameObject).onClick = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
        };
        //展示物品信息
        if (haveItemGo != null) UIEventListener.Get(haveItemGo).onClick = delegate
        {
            if (data != null) ToolTipMng.ShowEquipmentTooltip(new EquipmentInfo(data.ItemID, EquipmentBelongTo.PREVIEW), ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None);
        };
        //展示物品信息
        if (vipGo != null) UIEventListener.Get(vipGo).onClick = delegate
        {
            ToolTipMng.ShowEquipmentTooltip(new EquipmentInfo(7000004, EquipmentBelongTo.PREVIEW), ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None);
        };
    }

    public void RefreshTreasureItem(RoyalTreaureData _data)
    { 
        if (haveItemGo != null) haveItemGo.SetActive(_data != null);
        if (notHaveItemGo != null) notHaveItemGo.SetActive(_data == null);
        if (vipGo != null)
        {
            vipGo.SetActive(GameCenter.vipMng.VipData.vLev < 2);
            if (notHaveItemGo != null) notHaveItemGo.SetActive(_data == null && !vipGo.activeSelf);
        }

        if (_data != null)
        { 
            data = _data;
            refData = ConfigMng.Instance.GetRoyalBoxRef(_data.ItemID);
            if (timeCountDown != null) timeCountDown.SetActive(_data.curState);
            if (timeGo != null) timeGo.SetActive(!_data.curState);
            //当没有宝箱在开启时，显示开启宝箱按钮
            if (openBtn != null) openBtn.gameObject.SetActive(!GameCenter.royalTreasureMng.isOpeningBox && _data.curState);
            if (rightAwayBtn != null) rightAwayBtn.gameObject.SetActive(!_data.curState);
            //当有宝箱在开启时，另外的宝箱显示立即领取按钮
            if (quickenBtn != null) quickenBtn.gameObject.SetActive(GameCenter.royalTreasureMng.isOpeningBox && _data.curState);

            if (getRewardBtn != null) getRewardBtn.gameObject.SetActive(false);
            //宝箱未开启
            if (_data.curState && refData != null)
            {
                //Debug.Log("开启时间="+ refData.time);
                float hours = (refData.time / 3600.0f);
                //Debug.Log("开启时间:小时="+hours);
                if (timeCount != null) timeCount.text = hours.ToString("0.00").TrimEnd('.','0');
                int mins = (int)(refData.time / 60.0f);
                needGold = (int)(mins /*/ 3.0f*/);
                beishu = 2;
            }
            //宝箱正在开启状态
            if (!_data.curState)
            {
                if (timer != null&&GameCenter.royalTreasureMng.timeGap!=0.0)
                {
                    timer.StartIntervalTimer((int)(_data.restTime));
                    timer.onTimeOut = delegate
                    {
                        if (rightAwayBtn != null) rightAwayBtn.gameObject.SetActive(false);
                        if (getRewardBtn != null) getRewardBtn.gameObject.SetActive(true);
                    };
                    InvokeRepeating("SetDimano", 180.0f, 180.0f);
                }
                if(dimanoNum != null)
                {
                    //double temp =Convert.ToDouble(GameCenter.royalTreasureMng.timeGap + _data.restTime)/60.0f;
                    needGold =  (int)Math.Ceiling(Convert.ToDouble((GameCenter.royalTreasureMng.timeGap + _data.restTime- Time.realtimeSinceStartup) / 60.0f));
                    //needGold = (int)(mins/3.0f);
                    if (needGold == 0) needGold = 1;
                    dimanoNum.text = needGold.ToString();
                }
                if (rewardBeiShu != null)
                { 
                    if(refData != null) 
                    {
                        bool isInStepOne = _data.restTime >= refData.time * 0.8;
                        bool isInStepTwo = _data.restTime >= refData.time * 0.5 && _data.restTime < refData.time * 0.8;
                        bool isInStepThree = _data.restTime >= 0 && _data.restTime <refData.time * 0.5;
                        beishu = isInStepOne ? 2 : (isInStepTwo ? 1.5f : (isInStepThree ? 1 : 1));
                        rewardBeiShu.text = beishu.ToString();
                    }
                }
            }

            if (nameLabel != null && _data.RoyalTreasueInfo != null) nameLabel.text = _data.RoyalTreasueInfo.ItemStrColor + _data.RoyalTreasueInfo.ItemName;
            if (treasureSp != null && _data.RoyalTreasueInfo != null) treasureSp.spriteName = refData.notOpenIcon;
        }
    }
    /// <summary>
    /// 动态刷新元宝
    /// </summary>
    void SetDimano()
    {
        if (needGold >= 1)
        {
            //needGold = needGold - 1;
            if (dimanoNum != null) dimanoNum.text = needGold.ToString();
        }
    }
    /// <summary>
    /// 动态刷新倍数
    /// </summary>
    protected bool isRefshBeishu = true;
    protected bool isneedRefsh = true;
    float restartTime = 0;
    void Update()
    {
       //if (data != null && refData != null&&!data.curState)
       // {
       //     needGold = (int)(timer.GetCurrentTime() / 60.0f);
       //     dimanoNum.text = needGold.ToString();
       //     Debug.Log((int)(timer.GetCurrentTime() / 60.0f));
       // }
        if (data != null && refData != null && !data.curState && data.restTime != 0)//开启的宝箱才刷新
        {
            if(GameCenter.royalTreasureMng.timeGap!=0.0f)
            {
                //Debug.Log(data.restTime - Time.realtimeSinceStartup);
                needGold = (int)(Math.Ceiling(Convert.ToDouble((GameCenter.royalTreasureMng.timeGap+data.restTime - Time.realtimeSinceStartup) / 60.0f)));
                SetDimano();
            }
            if (isRefshBeishu)
            {
                restartTime += Time.deltaTime;
                if (data.restTime - restartTime < refData.time * 0.8)
                {
                    isRefshBeishu = false;
                    beishu = 1.5f;
                    if (rewardBeiShu != null) rewardBeiShu.text = beishu.ToString();
                }
            }
            else if (isneedRefsh)
            {
                restartTime += Time.deltaTime;
                if (data.restTime - restartTime < refData.time * 0.5)
                {
                    isneedRefsh = false;
                    restartTime = 0;
                    beishu = 1.0f;
                    if (rewardBeiShu != null) rewardBeiShu.text = beishu.ToString();
                }
            }
        }
    }
    /// <summary>
    /// 加速领奖弹出提示
    /// </summary>
    void PopTip(bool _isRightAway)
    {
        MessageST msg = new MessageST();
        msg.messID = 501;
        msg.words = new string[2] { needGold.ToString(), beishu.ToString() };
        msg.delYes = delegate
        {
            if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= (ulong)needGold)
            {
                GameCenter.royalTreasureMng.curGetRewardBoxData = data;
                GameCenter.royalTreasureMng.C2S_ReqGetRoyalReward(data.ID, 2);
                //加速开启宝箱(取消推送)
                if (_isRightAway)
                {
                    GameCenter.messageMng.CancelPushInfo(11);
                }
            }
            else
            {
                MessageST mst = new MessageST();
                mst.messID = 137;
                mst.delYes = delegate
                {
                    // 跳转到充值界面
                    GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                };
                GameCenter.messageMng.AddClientMsg(mst);
            }
        };
        GameCenter.messageMng.AddClientMsg(msg);
    }
}
