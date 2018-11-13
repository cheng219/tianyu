//======================================================
//作者:朱素云
//日期:2017/5/4
//用途:多人组队副本非机器人奖励
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyMultipleRoboteRewardUi : MonoBehaviour {

    public ItemUI[] rewards;
    public UISprite getReward;
    public UISprite notGetReward;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetData(List<ItemValue> _rewards, bool _isGet)
    { 
        List<EquipmentInfo> list = new List<EquipmentInfo>();
        for (int i = 0, max = _rewards.Count; i < max; i++)
        {
            list.Add(new EquipmentInfo(_rewards[i].eid, _rewards[i].count, EquipmentBelongTo.PREVIEW));

        }
        for (int i = 0, max = list.Count; i < max; i++)
        {
            if (rewards.Length > i)
            {
                rewards[i].FillInfo(list[i]);
            }
        }
        if (getReward != null) getReward.gameObject.SetActive(_isGet);
        if (notGetReward != null) notGetReward.gameObject.SetActive(!_isGet);
    }

    //public void SetGetState(bool _isGet)
    //{
    //    if (getReward != null) getReward.gameObject.SetActive(_isGet);
    //    if (notGetReward != null) notGetReward.gameObject.SetActive(!_isGet);
    //}
}
