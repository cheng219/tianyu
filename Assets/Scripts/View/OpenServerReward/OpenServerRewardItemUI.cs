//======================================================
//作者:黄洪兴
//日期:2016/07/14
//用途:开服贺礼奖励组件
//======================================================
using UnityEngine;
using System.Collections;

public class OpenServerRewardItemUI : MonoBehaviour {

    public UILabel rewardName;
    public ItemUI item;
    public UILabel orig_price;
    public UILabel cur_price;
    public UILabel countNum;
    public GameObject buyBtn;




    OpenServerRewardInfoItemData info;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void Refresh(OpenServerRewardInfoItemData _info)
    {
        if (_info == null)
            return;
        info = _info;
        if (rewardName != null)
            rewardName.text = _info.giftName;
        if (item != null)
            item.FillInfo(_info.item_type);
        if (orig_price != null)
        {
            orig_price.text = ConfigMng.Instance.GetUItext(76)+_info.orig_price.ToString();
        }
        if(cur_price!=null)
        {
            cur_price.text = ConfigMng.Instance.GetUItext(77) + _info.cur_price.ToString();
        }
        if (countNum != null)
            countNum.text = _info.max_buy_amount.ToString();
        if (buyBtn != null) UIEventListener.Get(buyBtn).onClick = BuyItem;

    }


    void BuyItem(GameObject _go)
    {
        if ((int)info.max_buy_amount <= 0)
        {
            GameCenter.messageMng.AddClientMsg(167);
            return;
        }
        GameCenter.buyMng.id = (int)info.id;
        GameCenter.buyMng.CurPrice =(int) info.cur_price;
        GameCenter.buyMng.priceType = 1;
        GameCenter.buyMng.OpenBuyWnd(info.item_type, BuyType.OPENSERVER, (int)info.max_buy_amount);

    }









}
