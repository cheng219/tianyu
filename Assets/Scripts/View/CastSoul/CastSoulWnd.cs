//==================================
//作者：黄洪兴
//日期：2016/4/1
//用途：铸魂主界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CastSoulWnd: GUIBase {

    public UIFxAutoActive critEffect;
    public UIFxAutoActive soulOneEffect;
    public UIFxAutoActive soulTwoEffect;
    public UIFxAutoActive soulThreeEffect;
    public UIFxAutoActive soulFourEffect;
    public UIFxAutoActive soulFiveEffect;
    public UITexture BjTexture;
	public GameObject closeBtn;
	public GameObject commonCastSoul;
	public GameObject advancedCastSoul;
	public UIToggle[] souls = new UIToggle[5];
    public UISpriteEx[] soulsprite = new UISpriteEx[5];
    public UILabel[] soulsNameLab;
	public UILabel awardDes;
	public UILabel commonCastSoulNum;
	public UILabel advancedCastSoulNum;
	public UILabel commonCastSoulConsume;
	public UILabel advancedCastSoulConsume;
	//public ItemUI rewardItem;
	private int curSoul=1;
    int soulID=0;
	EquipmentInfo reward;

    VIPRef VIPref;
    public UILabel tog4Lock;//35级解锁
    public UILabel tog5Lock;//45级解锁

    public UISlider castSoulPro;//铸魂进度
    public UILabel castSoulProLab;//进度说明
    public ItemUI soulRewardItem;//奖励物品
    public UIButton getRewardBtn;//领取奖励
    public UISpriteEx getRewardEx;
    public GameObject canget; 
    public GameObject alreadyget;
    public ItemUI[] items;
    public UISprite redRemind;

    #region UNITY

    void Awake()
	{
		mutualExclusion = true;
        if (getRewardBtn != null) UIEventListener.Get(getRewardBtn.gameObject).onClick = OnClickGetReward;
        GameCenter.castSoulMng.C2S_AskSoulNum();
	}
	protected override void OnOpen()
	{
		base.OnOpen();
        RefreshReward();
        FDictionary rewardsTable = ConfigMng.Instance.GetCastSoulRefTable();
        for (int i = 0; i < items.Length; i++)
        {
            if (rewardsTable.Count > i)
            {
                items[i].transform.parent.gameObject.SetActive(true);
                CastSoulRef reward = ConfigMng.Instance.GetCastSoulRef(i + 1);
                if (reward != null)
                {
                    if (reward.normalItem.Count > 0)
                    {
                        EquipmentInfo eq = new EquipmentInfo(reward.normalItem[0].eid, reward.normalItem[0].count, EquipmentBelongTo.PREVIEW);
                        items[i].FillInfo(eq);
                        items[i].itemName.text = eq.ItemName + " X " + reward.normalItem[0].count;
                    }
                }
            }
            else
            {
                items[i].transform.parent.gameObject.SetActive(false);
            }
        }  
		
        RefreshNeedLev(35);
        RefreshNeedLev(45);
        ConfigMng.Instance.GetBigUIIcon("Pic_zh_bg", SetTexture);
        VIPref = GameCenter.vipMng.VipData.RefData;
	} 

	protected override void OnClose()
	{
		base.OnClose(); 
		ConfigMng.Instance.RemoveBigUIIcon("Pic_zh_bg");
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
			for (int i = 0; i < souls.Length; i++)
			{
				EventDelegate.Add(souls[i].onChange, ChooseSoulByType);
			}
            GameCenter.castSoulMng.UpdateSoulNum += ShowSoulEffect;
			GameCenter.castSoulMng.UpdateSoulNum += ChooseSoulByType;
            GameCenter.castSoulMng.OnCastSoulCrit += ShowCritEffect;
            GameCenter.castSoulMng.UpdateSoulReward += RefreshReward;
			if(closeBtn!=null)UIEventListener.Get(closeBtn).onClick += CloseThis;
			if(commonCastSoul!=null)UIEventListener.Get(commonCastSoul).onClick += CommonCastSoul;
			if(advancedCastSoul!=null)UIEventListener.Get(advancedCastSoul).onClick += AdvancedCastSoul;
		}
		else
		{
			for (int i = 0; i < souls.Length; i++)
			{
				EventDelegate.Remove(souls[i].onChange, ChooseSoulByType);
			}
            if (closeBtn != null) UIEventListener.Get(closeBtn).onClick -= CloseThis;
            if (commonCastSoul != null) UIEventListener.Get(commonCastSoul).onClick -= CommonCastSoul;
            if (advancedCastSoul != null) UIEventListener.Get(advancedCastSoul).onClick -= AdvancedCastSoul;
			GameCenter.castSoulMng.UpdateSoulNum -= ChooseSoulByType;
            GameCenter.castSoulMng.OnCastSoulCrit -= ShowCritEffect;
            GameCenter.castSoulMng.UpdateSoulNum -= ShowSoulEffect;
            GameCenter.castSoulMng.UpdateSoulReward -= RefreshReward;
		}
	}

    #endregion 

    #region 控件事件

    void SetTexture(Texture2D _texture)
    {
        if (BjTexture != null)
            BjTexture.mainTexture = _texture;
    }

    void OnClickGetReward(GameObject go)
    {
        if (getRewardEx.IsGray == UISpriteEx.ColorGray.normal)
        {
            GameCenter.castSoulMng.C2S_AskSoulReward();
        }
    }
		
	void CloseThis(GameObject obj)
	{
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
//		Debug.Log ("关闭铸魂界面");
	}
	void CommonCastSoul(GameObject obj)
	{
        if (LevRemind()) return;
        if (GameCenter.castSoulMng.CommonCastSoulNum[soulID] > 0)
        {
            GameCenter.castSoulMng.C2S_CastSoul(curSoul, 1);
//            Debug.Log("发送普通铸魂协议");
        }
        else
        {
            GameCenter.messageMng.AddClientMsg(168);
        }
		
	}
	void AdvancedCastSoul(GameObject obj)
	{
        if (LevRemind()) return;
        if (GameCenter.castSoulMng.AdvancedCastSoulNum[soulID] > 0)
        {
            GameCenter.castSoulMng.C2S_CastSoul(curSoul, 2);
        }
        else
        {
            GameCenter.messageMng.AddClientMsg(168);
        }
//		Debug.Log ("发送高级铸魂协议");
	}

    #endregion

    #region 刷新

    protected bool LevRemind()
    {
        if (curSoul == 4)
        {
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < 35)
            { 
                GameCenter.messageMng.AddClientMsg(13);
                return true;
            }
        }
        if (curSoul == 5)
        {
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < 45)
            { 
                GameCenter.messageMng.AddClientMsg(13);
                return true;
            }
        }
        return false;
    }

    void ShowSoulEffect()
    {
        switch (curSoul)
        {
            case 1:
                if (soulOneEffect != null)
                    soulOneEffect.ReShowFx();
                break;
            case 2:
                if (soulTwoEffect != null)
                    soulTwoEffect.ReShowFx();
                break;
            case 3:
                if (soulThreeEffect != null)
                    soulThreeEffect.ReShowFx();
                break;
            case 4:
                if (soulFourEffect != null)
                    soulFourEffect.ReShowFx();
                break;
            case 5:
                if (soulFiveEffect != null)
                    soulFiveEffect.ReShowFx();
                break;
            default:
                break;
        }

    }


    void ShowCritEffect()
    {
       
        if (critEffect != null)
        {
            critEffect.ReShowFx();
        }


    }


	void ChooseSoulByType()
	{
		int _type = 0;
		for (int i = 0; i < souls.Length; i++)
		{
			if (souls[i].value)
			{
				_type = i+1;
				break;
			}
		}
       
        //string st = ConfigMng.Instance.GetUItext(69);
		switch (_type) {
            case 1:
                {
                    awardDes.text = ConfigMng.Instance.GetUItext(69, new string[] { ConfigMng.Instance.GetUItext(70) }); curSoul = 1; RefreshSoulNum(curSoul); 
                    break;
                }
            case 2:
                {
                    awardDes.text = ConfigMng.Instance.GetUItext(69, new string[] { ConfigMng.Instance.GetUItext(71) }); curSoul = 2; RefreshSoulNum(curSoul);
                    break;
                }
            case 3:
                {
                    awardDes.text = ConfigMng.Instance.GetUItext(69, new string[] { ConfigMng.Instance.GetUItext(72) }); curSoul = 3; RefreshSoulNum(curSoul);
                    break;
                }
            case 4:
                { 
                    awardDes.text = ConfigMng.Instance.GetUItext(69, new string[] { ConfigMng.Instance.GetUItext(73) }); curSoul = 4; RefreshSoulNum(curSoul);
                    break;
                }
            case 5:
                { 
                    awardDes.text = ConfigMng.Instance.GetUItext(69, new string[] { ConfigMng.Instance.GetUItext(74) }); curSoul = 5; RefreshSoulNum(curSoul);
                    break;
                }
		default:	break;
		}

        RefreshSprite(_type);


	}

    /// <summary>
    /// 刷新进度奖励
    /// </summary>
    void RefreshReward() 
    { 
        CastsoulRewardRef castsoulRewardRef = ConfigMng.Instance.GetcastsoulRewardRef(GameCenter.castSoulMng.CurSoulRewardId + 1);

        if (castsoulRewardRef == null)
        {
            int dex = ConfigMng.Instance.GetCastsoulRewardRefTable().Count;
            castsoulRewardRef = ConfigMng.Instance.GetcastsoulRewardRef(dex);//领取完毕
            canget.SetActive(false);
            alreadyget.SetActive(true);
            if (redRemind != null) redRemind.gameObject.SetActive(false);
            castSoulPro.value = 1;
            if (castsoulRewardRef != null)
            {
                castSoulProLab.text = castsoulRewardRef.num + "/" + castsoulRewardRef.num;
                getRewardEx.IsGray = UISpriteEx.ColorGray.Gray;
                ItemValue item = castsoulRewardRef.reward;
                soulRewardItem.FillInfo(new EquipmentInfo(item.eid, item.count, EquipmentBelongTo.PREVIEW));
            }
            return;
        }
        if (castsoulRewardRef != null)
        { 
            canget.SetActive(true);
            alreadyget.SetActive(false);
            if (GameCenter.castSoulMng.CurSoulNum >= castsoulRewardRef.num)
            {
                castSoulPro.value = 1;
                castSoulProLab.text = castsoulRewardRef.num + "/" + castsoulRewardRef.num;
                getRewardEx.IsGray = UISpriteEx.ColorGray.normal;
                if (redRemind != null) redRemind.gameObject.SetActive(true);
            }
            else
            {
                castSoulPro.value = (float)GameCenter.castSoulMng.CurSoulNum / castsoulRewardRef.num;
                castSoulProLab.text = GameCenter.castSoulMng.CurSoulNum + "/" + castsoulRewardRef.num;
                getRewardEx.IsGray = UISpriteEx.ColorGray.Gray;
                if (redRemind != null) redRemind.gameObject.SetActive(false);
            }
            if (castsoulRewardRef.reward != null)
            {
                ItemValue item = castsoulRewardRef.reward; 
                soulRewardItem.FillInfo(new EquipmentInfo(item.eid, item.count, EquipmentBelongTo.PREVIEW)); 
            }
        }
        else
        {  
            getRewardEx.IsGray = UISpriteEx.ColorGray.Gray;
            castSoulPro.value = 0;
            castSoulProLab.text = "0/0";
            if (redRemind != null) redRemind.gameObject.SetActive(false);
        }
    }


    void RefreshSprite(int _id)
    {
        for (int i = 0; i < soulsprite.Length; i++)
        { 
            if (i + 1 == _id)
            {
                soulsprite[i].IsGray = UISpriteEx.ColorGray.normal;
                if (soulsNameLab.Length > i) soulsNameLab[i].gameObject.SetActive(true);
            }
            else
            {
                soulsprite[i].IsGray = UISpriteEx.ColorGray.Gray;
                if (soulsNameLab.Length > i) soulsNameLab[i].gameObject.SetActive(false);
            }
            
        }


    }

    void RefreshNeedLev(int _needLev)
    {
        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < _needLev)
        { 
            if (_needLev == 45) tog5Lock.gameObject.SetActive(true);
            if (_needLev == 35) tog4Lock.gameObject.SetActive(true);
        }
        else
        {
            if (_needLev == 45) tog5Lock.gameObject.SetActive(false);
            if (_needLev == 35) tog4Lock.gameObject.SetActive(false);
        }
    }


	void RefreshSoulNum(int _soulID, bool islock = false)
	{
        if (VIPref == null)
            return;
        UIwenbenRef TextRef = ConfigMng.Instance.GetUITextRef(68);
        soulID = _soulID;
        if (GameCenter.castSoulMng.CommonCastSoulNum.ContainsKey(_soulID))
        {
            if (TextRef != null)
                commonCastSoulNum.text = TextRef.text + GameCenter.castSoulMng.CommonCastSoulNum[_soulID].ToString();
        }
        if (GameCenter.castSoulMng.AdvancedCastSoulNum.ContainsKey(_soulID))
        {
            if(TextRef!=null)
                advancedCastSoulNum.text = TextRef.text + GameCenter.castSoulMng.AdvancedCastSoulNum[_soulID].ToString();
        }
		reward = new EquipmentInfo (ConfigMng.Instance.GetCastSoulRef (_soulID).normalItem [0].eid,
            ConfigMng.Instance.GetCastSoulRef (_soulID).normalItem [0].count,
            EquipmentBelongTo.PREVIEW);
		//rewardItem.FillInfo (reward);
        if (commonCastSoulConsume != null)
        {
            int castsoulnum=0;
            if (VIPref != null)
            {
                castsoulnum = VIPref.cast_soul_num - GameCenter.castSoulMng.CommonCastSoulNum[_soulID] + 1;
                CastSoulTimeRef Ref=ConfigMng.Instance.GetCastSoulTimeRef(castsoulnum);
                if (Ref != null)
                {
                    if ((ulong)Ref.giftMoney[0].count <= GameCenter.mainPlayerMng.MainPlayerInfo.RealYuanCount)
                    {
                        commonCastSoulConsume.text = Ref.giftMoney[0].count.ToString() + "/" + GameCenter.mainPlayerMng.MainPlayerInfo.RealYuanCount.ToString();
                    }
                    else
                    {
                        commonCastSoulConsume.text = Ref.giftMoney[0].count.ToString() + "/" + "[FF0000]" + GameCenter.mainPlayerMng.MainPlayerInfo.RealYuanCount.ToString();
                    }
                }
            }
        }
        if (advancedCastSoulConsume != null)
        {
            int castsoulnum = 0;
            if (VIPref != null)
            {
                castsoulnum = VIPref.cast_soul_num - GameCenter.castSoulMng.AdvancedCastSoulNum[_soulID] + 1;
                CastSoulTimeRef Ref = ConfigMng.Instance.GetCastSoulTimeRef(castsoulnum);
                if (Ref != null)
                {
                    if ((ulong)Ref.specialMoney[0].count <= GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)
                    {
                        advancedCastSoulConsume.text = Ref.specialMoney[0].count.ToString() + "/" + GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount.ToString();
                    }
                    else
                    {
                        advancedCastSoulConsume.text = Ref.specialMoney[0].count.ToString() + "/" + "[FF0000]" + GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount.ToString();
                    }
                }
            }
        }
    }

    #endregion

} 
