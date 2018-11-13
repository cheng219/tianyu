//============================
//作者：邓成
//日期：2017/5/3
//用途：挂机副本显示界面类
//============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HangUpCoppyWnd : SubWnd {
    public UIButton btnGoSecond;
    public UILabel labBtnGoSecond;
    public UILabel labVipLimit;//'VIP2进入'
    public UILabel remainMonsterNum;
    public UILabel labExpNum;

    public GameObject addNumGo;
    public UIButton previewVip;
    public UIButton btnAddNum;
    public UILabel remainAddTimes;
    public UILabel labConsume;
    public UIToggle toggleAutoAdd;
    public UILabel labAutoAdd;
    void Awake()
    {
        if (btnAddNum != null) UIEventListener.Get(btnAddNum.gameObject).onClick = AddMonsterNum;
        if (toggleAutoAdd != null) EventDelegate.Add(toggleAutoAdd.onChange, OnAutoAddChange);
        if (previewVip != null) UIEventListener.Get(previewVip.gameObject).onClick = PreviewVip;
    }

    protected override void OnOpen()
    {
        base.OnOpen();
    }

    protected override void OnClose()
    {
        base.OnClose();
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.activityMng.OnHanguUpCoppyDataUpdateEvent += Refresh;
        }
        else
        {
            GameCenter.activityMng.OnHanguUpCoppyDataUpdateEvent -= Refresh;
        }
    }


    void Refresh()
    { 
        SceneUiType uiType = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType;
        bool inFirst = (uiType == SceneUiType.HANGUPCOPPYFIRSTFLOOR);
        if (labVipLimit != null) labVipLimit.enabled = inFirst && GameCenter.mainPlayerMng.MainPlayerInfo.VipLev < 2;
        if (labBtnGoSecond != null) labBtnGoSecond.text = inFirst ? "进入二层" : "进入一层";
        if (inFirst)
        {
            if (btnGoSecond != null) UIEventListener.Get(btnGoSecond.gameObject).onClick = GoSecond;
        }
        else
        {
            if (btnGoSecond != null) UIEventListener.Get(btnGoSecond.gameObject).onClick = GoFirst;
        }
        if (remainMonsterNum != null) remainMonsterNum.text = GameCenter.activityMng.HangUpCoppyRemainMonsterNum.ToString();
        int expCount = GameCenter.activityMng.HangUpCoppyExpCount;
        if (labExpNum != null) labExpNum.text = expCount <= 10000 ? expCount.ToString() : (expCount/10000 + "W");
        if (remainAddTimes != null) remainAddTimes.text = GameCenter.activityMng.HangUpRemainBuyTimes.ToString();
        if (labConsume != null) labConsume.text = Consume();
        if (labAutoAdd != null) labAutoAdd.enabled = GameCenter.activityMng.IsAutoButMonsterNum;
    }
    bool enough = true;
    EquipmentInfo lackEquip = null;
    string Consume()
    {
        VIPDataInfo info = GameCenter.vipMng.VipData;
        int maxTimes = info != null ? info.HangUpMaxBuyTimes : 0;
        int curTimes = maxTimes - GameCenter.activityMng.HangUpRemainBuyTimes;
        if (curTimes <= 0)
        {
            curTimes = maxTimes;
            Debug.LogError("当前VIP最大次数小于剩余次数,数据异常!");
        }
        Dictionary<int, ItemValue> itemDic = new Dictionary<int, ItemValue>();
        StepConsumptionRef stepConsumptionRef = ConfigMng.Instance.GetStepConsumptionRef(curTimes + 1);
        if (stepConsumptionRef != null && stepConsumptionRef.hangUpCoppyTimesCost.Count > 0)
        {
            for (int j = 0,lengthJ = stepConsumptionRef.hangUpCoppyTimesCost.Count; j < lengthJ; j++)
            {
                ItemValue item = stepConsumptionRef.hangUpCoppyTimesCost[j];
                AddOne(itemDic,item);
            }
        }
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        using (var item = itemDic.GetEnumerator())
        {
            while (item.MoveNext())
            {
                builder.Append(GameHelper.GetStringWithBagNumber(item.Current.Value, out enough, out lackEquip)).Append(" ");
            }
        }
        return builder.ToString();
    }
    void AddOne(Dictionary<int, ItemValue> itemList, ItemValue item)
    {
        if (itemList.ContainsKey(item.eid))
        {
            itemList[item.eid] = new ItemValue(item.eid, itemList[item.eid].count + item.count);
        }
        else
        {
            itemList[item.eid] = item;
        }
    }

    void GoSecond(GameObject go)
    {
        if (GameCenter.mainPlayerMng.MainPlayerInfo.VipLev < 2)
        {
            GameCenter.messageMng.AddClientMsg(154);
            return;
        }
        SceneRef sceneRef = ConfigMng.Instance.GetSceneRef(160012);
        if (sceneRef != null)
        {
            GameCenter.curMainPlayer.GoTraceTarget(160012, sceneRef.in_x, sceneRef.in_z);//一层去二层,取一层的入口
        }
    }
    void GoFirst(GameObject go)
    {
        //GameCenter.activityMng.C2S_FlyHangUpCoppy(160011);
        SceneRef sceneRef = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef;
        if (sceneRef != null)
        {
            GameCenter.curMainPlayer.GoTraceTarget(sceneRef.id, sceneRef.out_x, sceneRef.out_z);//二层去一层,取二层的出口
        }
    }
    void AddMonsterNum(GameObject go)
    {
        if (enough)
        {
            GameCenter.activityMng.C2S_ReqHangUpCoppyData(2);
        }
        else
        {
            MessageST mst = new MessageST();
            mst.messID = 142;
            mst.words = new string[] { lackEquip == null ? string.Empty : (lackEquip.DiamondPrice * lackEquip.StackCurCount).ToString() };
            mst.delYes = (x) =>
                {
                    GameCenter.activityMng.C2S_ReqHangUpCoppyData(2);
                };
            GameCenter.messageMng.AddClientMsg(mst);
        }
    }
    void PreviewVip(GameObject go)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.VIP);
        if (addNumGo != null) addNumGo.SetActive(false);
    }
    void OnAutoAddChange()
    {
        if (toggleAutoAdd != null)
            GameCenter.activityMng.IsAutoButMonsterNum = toggleAutoAdd.value;
        if (labAutoAdd != null) labAutoAdd.enabled = GameCenter.activityMng.IsAutoButMonsterNum;
    }
}
