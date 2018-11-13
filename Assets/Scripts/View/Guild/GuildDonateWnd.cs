//==================================
//作者：邓成
//日期：2016/4/15
//用途：仙盟捐献界面类
//=================================

using UnityEngine;
using System.Collections;

public class GuildDonateWnd : SubWnd { 
    //public UIInput inputCoin;
    //public UIInput inputDiamond; 
	//public UIToggle toggleMessage;
	public UILabel labCoin;
	public UILabel labDiamond;
    public UILabel restTime;
    public GuildDonateTypeUi[] types;
	public UIButton btnClose;
	void Awake()
	{ 
		if(btnClose != null)UIEventListener.Get(btnClose.gameObject).onClick = CloseWnd; 
	}
	protected override void OnOpen()
	{
		base.OnOpen();
        ShowDonateType();
		Refresh();
        RefreshRestTime();
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
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += Refresh;
            GameCenter.guildMng.OnGuildDonateTimeEvent += RefreshRestTime;
		}
		else
		{ 
			GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= Refresh;
            GameCenter.guildMng.OnGuildDonateTimeEvent -= RefreshRestTime;
		}
	}
	void CloseWnd(GameObject go)
	{
		this.CloseUI();
	} 
	void Refresh(ActorBaseTag _tag,ulong _value,bool state)
	{
		if(_tag == ActorBaseTag.BindDiamond || _tag == ActorBaseTag.Diamond || _tag == ActorBaseTag.BindCoin || _tag == ActorBaseTag.UnBindCoin)
			Refresh();
	}
    void Refresh()
    { 
        if (labCoin != null) labCoin.text = GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCountText;
        if (labDiamond != null) labDiamond.text = GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount.ToString(); 
    }
    void RefreshRestTime()
    {
        if (restTime != null) restTime.text = GameCenter.guildMng.restDonateTimes.ToString();
    }
    void ShowDonateType()
    {
        FDictionary dic = ConfigMng.Instance.GetGuildDonateRefTable();
        int i = 0;
        foreach (GuildDonateRef donate in dic.Values)
        {
            if (types.Length > i)
            {
                types[i].SetData(donate);
                ++i;
            }
        }
    }
    //void CoinChange()
    //{
    //    int coinNum = 0;
    //    if(inputCoin != null && int.TryParse(inputCoin.value,out coinNum))
    //    {
    //        if(coinNum > GameCenter.guildMng.DonateLimitCoin)
    //            inputCoin.value = GameCenter.guildMng.DonateLimitCoin.ToString();
    //    }
    //}
    //void DiamondChange()
    //{
    //    int diamondNum = 0;
    //    if(inputDiamond != null && int.TryParse(inputDiamond.value,out diamondNum))
    //    {
    //        if(diamondNum > GameCenter.guildMng.DonateLimitDiamond)
    //            inputDiamond.value = GameCenter.guildMng.DonateLimitDiamond.ToString();
    //    }
    //}
}
