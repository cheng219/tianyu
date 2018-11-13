//==================================
//作者：黄洪兴
//日期：2016/4/15
//用途：下载奖励界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DownloadBonusWnd : GUIBase {
	/// <summary>
	/// 进度条
	/// </summary>
	public UISlider progressBar;
	/// <summary>
	/// 奖励
	/// </summary>
	public List<ItemUI> rewards;
	/// <summary>
	/// 领取奖励按钮
	/// </summary>
	public UIButton getRewardBtn;
    /// <summary>
    /// 开始下载按钮
    /// </summary>
    public UIButton loadStartBtn;
    /// <summary>
    /// 下载中
    /// </summary>
    public UIButton loadOnBtn;
    /// <summary>
    /// 下载完成
    /// </summary>
    public UIButton loadOverBtn;

	public GameObject CloseBtn;

	//MainPlayerInfo mainPlayerInfo = null;

	void Awake()
	{
		if (CloseBtn != null)
			UIEventListener.Get (CloseBtn).onClick = delegate {BtnClose ();};
		mutualExclusion = true;
        if (getRewardBtn != null) UIEventListener.Get(getRewardBtn.gameObject).onClick = OnClickGetRewardBtn;
	}
	protected override void OnOpen()
	{
		base.OnOpen();
		RefreshItems ();
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
		}
		else
		{
		}
	}


	void RefreshItems()
	{
        List<ItemValue> rewradList = ConfigMng.Instance.GetDownloadBonusRef(1).reward;
        for (int i = 0, max = rewradList.Count; i < max; i++)
        {
            if (rewards.Count > i)
            {
                rewards[i].FillInfo(new EquipmentInfo(rewradList[i].eid, rewradList[i].count, EquipmentBelongTo.PREVIEW));
            }
            else
            {
                rewards[i].gameObject.SetActive(false);
            }
        }
        loadStartBtn.gameObject.SetActive(GameCenter.downloadBonusMng.curDownLoadProType == DownLoadProType.LOADSTART);
        loadOnBtn.gameObject.SetActive(GameCenter.downloadBonusMng.curDownLoadProType == DownLoadProType.LOADON);
        loadOverBtn.gameObject.SetActive(GameCenter.downloadBonusMng.curDownLoadProType == DownLoadProType.LOADOVER); 
	}

	void RefreshRight()
	{


	}



	void BtnClose(){
		GameCenter.uIMng.SwitchToUI(GUIType.NONE);
	}



	void SellItem(GameObject obj)
	{
		//	GameCenter.buyMng.OpenBuyWnd (GUIType.SHOPWND);	
	}
    /// <summary>
    /// 领取奖励
    /// </summary> 
    void OnClickGetRewardBtn(GameObject go)
    {
        //GameCenter.downloadBonusMng.C2S_ReqGetDownReward();
    }
}
