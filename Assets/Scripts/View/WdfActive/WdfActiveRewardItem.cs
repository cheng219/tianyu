//======================================================
//作者:黄洪兴
//日期:2016/7/13
//用途:精彩活动活动1类型奖励组件
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WdfActiveRewardItem : MonoBehaviour {

    public List<ItemUI> items = new List<ItemUI>();
    public UILabel rewardDes;
    public UILabel nums;
    public GameObject getEffect;//可以获取的特效，暂用在连充豪礼
    public GameObject getBtn;
    public UISpriteEx getEx;
    public GameObject getedBtn;
    public GameObject rechargeBtn;
    public GameObject strongBtn;
    public GameObject strongEquipBtn;
    public GameObject wingBtn;

    public GameObject flowerBtn;//查看鲜花排行 
    public GameObject lastObj;//上届没有得主时隐藏
    public GameObject curObj;//当前
    public UILabel lastGetReward;//上届得主
    public UILabel curPlayer;//当前玩家

    protected  WdfActiveDetailsData info;
    protected  int id;
    protected int severId;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void Refresh(WdfActiveDetailsData _info, WdfActiveTypeData _data)
    {
        if (_info == null)
            return;
        info = _info;
        id = _data.type;
        severId = _data.id;
        for (int i = 0; i < _info.reward_info.Count; i++)
        {
            if (i < items.Count)
            {
                if (items[i] != null)
                {
                    EquipmentInfo eq = _info.reward_info[i];
                    if (eq != null)
                       items[i].FillInfo(eq);
                    items[i].ShowTooltip = true;
                    items[i].gameObject.SetActive(true);
                }
            }          
        }
        if (rewardDes != null && !string.IsNullOrEmpty(_info.desc))
        { 
            rewardDes.text = _info.desc;
            rewardDes.gameObject.SetActive(true);
        }
        if (nums != null)
        {
            if (_data.counter_value < _info.value1)
            {
                nums.text = _data.counter_value.ToString() + "/" + _info.value1.ToString();
            }
            else
            {
                nums.text = _info.value1.ToString() + "/" + _info.value1.ToString();
            }
            nums.gameObject.SetActive(id != 3 && id != 5 && id != 6 && id != 7 && id != 8 && id != 9 && id != 10 && id != 66 && id != 11);
            
        }
        if (getEx != null) getEx.IsGray = UISpriteEx.ColorGray.normal; 
        if (lastObj != null) lastObj.SetActive(id == 8 || id == 9 && _info.lastName != null);
        if (curObj != null) curObj.SetActive(id == 8 || id == 9);
        if (lastGetReward != null)//上届得主
        { 
            lastGetReward.text = _info.lastName;
            lastGetReward.gameObject.SetActive(id == 8 || id == 9);
        }

        if (curPlayer != null)//当前玩家
        {
            curPlayer.text = _info.curName;
            curPlayer.gameObject.SetActive(id == 8 || id == 9);
        }

        if (getBtn != null)
        {
            if(id != 11)UIEventListener.Get(getBtn).onClick = GetReward;
            getBtn.gameObject.SetActive(false);
        }
        if (getEffect != null)
        {
            getEffect.SetActive(false);
        }
        if (getedBtn != null)
        {
            getedBtn.gameObject.SetActive(false);
        }
        if (rechargeBtn != null)
        {
            UIEventListener.Get(rechargeBtn).onClick = GoRecharge;
            rechargeBtn.gameObject.SetActive(false);
        }
        if (strongBtn != null)
        {
            UIEventListener.Get(strongBtn).onClick = GoStrong;
            rechargeBtn.gameObject.SetActive(false);
        }
        if (id == 66)
        { 
            if (_info.reward_times == 0)//已经领取
            {
                if (getedBtn != null) getedBtn.SetActive(true);
            }
            else//没有领取
            {
                if (getBtn != null)
                {
                    getBtn.gameObject.SetActive(true);
                } 
                if (_info.total_reward_times >= _info.reward_times && _info.total_reward_times > 0)//可以领取
                {
                    if (getEx != null) getEx.IsGray = UISpriteEx.ColorGray.normal;
                }
                else//不可领取
                {
                    if (getEx != null) getEx.IsGray = UISpriteEx.ColorGray.Gray;
                }
            }
        }
        else
        {
            if (_info.total_reward_times > _info.reward_times && _info.total_reward_times > 0)
            {
                if (getBtn != null)
                {
                    getBtn.gameObject.SetActive(true);
                }
                if (getEffect != null)
                {
                    getEffect.SetActive(true);
                    if (id == 11)//连充豪礼直接点击物品领取
                    {
                        if (items.Count > 0)
                        {
                            items[0].ShowTooltip = false;
                            UIEventListener.Get(items[0].gameObject).onClick = GetReward;
                        }
                    }
                }
            }
            if (_info.total_reward_times == _info.reward_times && _info.total_reward_times > 0)
            {
                if (getedBtn != null)
                    getedBtn.SetActive(true);
            }
            if (_info.total_reward_times < 1)
            {
                if (id != 5 && id != 6 && id != 7 && id != 8 && id != 9)
                {
                    if (rechargeBtn != null)
                    {
                        rechargeBtn.SetActive(true);
                        if ((id == 10) && _data.counter_value == 1)//已经激活
                        {
                            rechargeBtn.SetActive(false);
                            if (getBtn != null) getBtn.gameObject.SetActive(true);
                            if (getEx != null) getEx.IsGray = UISpriteEx.ColorGray.Gray;
                        }
                    }
                }
                else
                {
                    if (id == 5)
                    {
                        if (strongBtn != null)
                            strongBtn.SetActive(true);
                    }
                    if (id == 6)
                    {
                        if (wingBtn != null)
                        {
                            wingBtn.SetActive(true);
                            UIEventListener.Get(wingBtn).onClick = GoWing;
                        }
                    }
                    if (id == 7)
                    {
                        if (strongEquipBtn != null)
                        {
                            strongEquipBtn.SetActive(true);
                            UIEventListener.Get(strongEquipBtn).onClick = GoStrongEquip;
                        }
                    }
                    if (id == 8 | id == 9)
                    {
                        if (flowerBtn != null)
                        {
                            flowerBtn.SetActive(true);
                            UIEventListener.Get(flowerBtn).onClick = GoFlowerRank;
                        }
                    }
                }
            }
        }

    }



  void GetReward(GameObject _go)
  {
      if (id == 66)//自己构造的登录红利
      {
          GameCenter.weekCardMng.C2S_ReqTakeLoginBonus(info.index);
      }
      else
      {
          GameCenter.wdfActiveMng.C2S_AskActivitysRewards(severId, (int)info.index);
      }
  }

  void GoRecharge(GameObject _go)
  {
      if (id == 10 || id == 66)
      {
#if UNITY_IOS 
             GameCenter.rechargeMng.C2S_RequestRecharge(15);;//android is 4     ios is 15 
#else
          GameCenter.rechargeMng.C2S_RequestRecharge(4);
#endif
      }
      else
      {
          GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
      }
  }

  void GoStrong(GameObject _go)
  {
      GameCenter.littleHelperMng.OpenWndByType(LittleHelpType.STRONG);
  }

  void GoStrongEquip(GameObject _go)
  {
      GameCenter.uIMng.SwitchToUI(GUIType.EQUIPMENTTRAINING);
  }
  void GoWing(GameObject _go)
  {
      GameCenter.uIMng.SwitchToSubUI(SubGUIType.SUBWING);
  }

  void GoFlowerRank(GameObject _go)
  {
      if (GameCenter.mainPlayerMng.FunctionIsOpen(FunctionType.RANKING))
      {
          if (id == 8)
          {
              GameCenter.newRankingMng.curChooseRank = 8;
          }
          if (id == 9)
          {
              GameCenter.newRankingMng.curChooseRank = 7;
          }
          GameCenter.uIMng.SwitchToSubUI(SubGUIType.NEWRANKINGSUBWND);
      }
      else
      {
          GameCenter.messageMng.AddClientMsg(389);
      }
  } 
}
