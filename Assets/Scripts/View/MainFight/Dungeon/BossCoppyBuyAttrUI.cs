//===============================
//作者：邓成
//日期：2016/5/4
//用途：购买BOSS副本属性的界面类
//===============================

using UnityEngine;
using System.Collections;

public class BossCoppyBuyAttrUI : MonoBehaviour {

    public UILabel num;
    public UILabel diamo;

    public GameObject btnAdd;
    public GameObject btnRemove;
    public GameObject btnAddTen;
    public GameObject btnOK;

    int totalNum = 0;
    int curTimes = 0;
    int curNum;
    int CurNum
    {
        get
        {
            return curNum;
        }
        set
        {
            if (totalNum < value)
            {
                curNum = totalNum;
            }
            else
            {
                curNum = value;
            }
            num.text = (curNum*10).ToString();
            diamo.text = GetCost().ToString();
        }
    }

    int GetCost()
    {
        int diamo = 0;
        StepConsumptionRef stepConsumptionRef = null;
        for (int i = 1; i <= CurNum; i++)//最多100%
        {
            stepConsumptionRef = ConfigMng.Instance.GetStepConsumptionRef(i + curTimes);
            if (stepConsumptionRef != null && stepConsumptionRef.bossAttrCost.Count > 0)
                diamo += stepConsumptionRef.bossAttrCost[0].count;
        }
        return diamo;
    }

    public void SetToBuyShow(int _curTimes)
    {
        totalNum = 10 - _curTimes;
        curTimes = _curTimes;//当前购买次数
        CurNum = 1;

        UIEventListener.Get(btnAdd).onClick = delegate(GameObject go)
        {
            CurNum++;
        };
        UIEventListener.Get(btnRemove).onClick = delegate(GameObject go)
        {
            if (CurNum > 1) CurNum--;
        };
        UIEventListener.Get(btnAddTen).onClick = delegate(GameObject go)
        {
            CurNum = totalNum;
        };

        UIEventListener.Get(btnOK).onClick = OnClickToBuy;
    }

    void OnClickToBuy(GameObject games)
    {
        if (CurNum <= 0)
        {
            GameCenter.messageMng.AddClientMsg(242);
            return;
        }
        if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < (ulong)GetCost())
        {
            MessageST mst = new MessageST();
            mst.messID = 137;
            mst.delYes = (x) =>
                {
                    GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else
        {
            GameCenter.bossChallengeMng.C2S_ReqAddBossCoppyData(2, CurNum);
        }
        gameObject.SetActive(false);
    }
}
