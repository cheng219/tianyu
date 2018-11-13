//===============================
//作者：邓成
//日期：2016/5/4
//用途：购买BOSS副本挑战次数界面类
//===============================

using UnityEngine;
using System.Collections;

public class BossBuyNumUI : MonoBehaviour {
	public UILabel num;
	public UILabel diamo;
	
	public GameObject btnAdd;
	public GameObject btnRemove;
	public GameObject btnAddTen;
	public GameObject btnOK;
	
	int totalNum = 0;
	int curNum;
	int CurNum{
		get{
			return curNum;
		}
		set{
			if(totalNum < value){
				curNum = totalNum;
			}else{
				curNum = value;
			}
			num.text = curNum.ToString();
            diamo.text = (CurNum * 50).ToString();
		}
	}
	
	public void SetToBuyShow(int remainTimes)
    {
        totalNum = remainTimes;
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
	
	void OnClickToBuy(GameObject games){
		if(CurNum <= 0){
			GameCenter.messageMng.AddClientMsg(242);
			return ;
		}
        if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < (ulong)CurNum * 50)
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
            GameCenter.bossChallengeMng.C2S_ReqAddBossCoppyData(1, CurNum);
        }
        gameObject.SetActive(false);
	}
}
