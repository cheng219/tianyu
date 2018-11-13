//======================================================
//作者:朱素云
//日期:2017/4/5
//用途:抽奖奖励物品
//======================================================
using UnityEngine;
using System.Collections;

public class WdfLotteryRewardItem : MonoBehaviour
{

    protected ItemUI item;
    public UILabel basicDes;//基础
    public UILabel advanceDes;//进阶
    public UISprite advanceSp;//进阶背景
    public TweenScale tween;
    public Transform parent;

	// Use this for initialization
	void Start () { 
        if (tween == null) tween = this.GetComponent<TweenScale>(); 
	}

    void OnDisable()
    {
         
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetData(st.net.NetBase.lucky_wheel_reward_info _info)
    {
        if (parent != null && item == null) item = ItemUI.CreatNew(parent, Vector3.zero, Vector3.one);
        if (item != null)
        {
            item.FillInfo(new EquipmentInfo((int)_info.item_type,(int)_info.amount, EquipmentBelongTo.PREVIEW));
        }
        if (basicDes != null) basicDes.gameObject.SetActive(_info.wheel_type == 1);
        if (advanceDes != null) advanceDes.gameObject.SetActive(_info.wheel_type == 2);
        if (advanceSp != null) advanceSp.gameObject.SetActive(_info.wheel_type == 2); 
    }
}
