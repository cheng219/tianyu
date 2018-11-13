//======================================================
//作者:朱素云
//日期:20175/4
//用途:仙盟捐献类型
//======================================================
using UnityEngine;
using System.Collections;

public class GuildDonateTypeUi : MonoBehaviour {

    public UILabel nameLab;
    public UIButton donateBtn;
    public UILabel rewardDes;
    public UISprite costSp;
    public UILabel costLab;
    protected GuildDonateRef guildDonateRef;
    protected ItemValue itemCost = null;


	// Use this for initialization
	void Start () {

        if (donateBtn != null) UIEventListener.Get(donateBtn.gameObject).onClick = delegate
        {
            if (GameCenter.guildMng.restDonateTimes <= 0)
            {
                GameCenter.messageMng.AddClientMsg(558);
                return;
            }
            if (itemCost != null && itemCost.eid == 18)
            {
                MessageST msg = new MessageST();
                msg.messID = 562;
                msg.words = new string[1] { itemCost .count.ToString()};
                msg.delYes = delegate
                {
                    if (guildDonateRef != null)
                    {
                        GameCenter.guildMng.C2S_GuildDonate(guildDonateRef.id);
                    }
                };
                GameCenter.messageMng.AddClientMsg(msg); 
            }
            else
            {
                if (guildDonateRef != null)
                {
                    GameCenter.guildMng.C2S_GuildDonate(guildDonateRef.id);
                }
            }
        };
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetData(GuildDonateRef _guildDonateRef)
    {
        guildDonateRef = _guildDonateRef;

        if (_guildDonateRef != null)
        {
            if (nameLab != null) nameLab.text = _guildDonateRef.donationName;
            if (rewardDes != null) rewardDes.text = _guildDonateRef.des.Replace("\\n", "\n"); 
            if (_guildDonateRef.cost.Count > 0)
            {
                if (costSp != null)
                {
                    //EquipmentInfo eqinfo = new EquipmentInfo(_guildDonateRef.cost[0].eid, EquipmentBelongTo.PREVIEW);
                    costSp.spriteName = GameHelper.GetCoinIconByType(_guildDonateRef.cost[0].eid);
                    if (_guildDonateRef.cost[0].eid == 18) itemCost = _guildDonateRef.cost[0];
                    costSp.MakePixelPerfect();
                }
                if (costLab != null) costLab.text = _guildDonateRef.cost[0].count.ToString();
            }
        }
    }
}
