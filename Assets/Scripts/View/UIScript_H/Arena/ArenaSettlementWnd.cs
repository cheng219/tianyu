
/// <summary>
/// 竞技场结算界面
/// 何明军
/// 2016/4/19
/// </summary>

using UnityEngine;
using System.Collections;

public class ArenaSettlementWnd : GUIBase {

	public GameObject wing;
	public GameObject noWing;
	
	public GameObject btnClose;
	public GameObject btnTo;
	
    public UILabel[] itemLabel;
	public UILabel rank;
	public UILabel upRank;
	
	void Awake(){
		mutualExclusion = true;
		Layer = GUIZLayer.TOPWINDOW;
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
		rank.text = GameCenter.duplicateMng.CopySettlementDataInfo.rank+"";
		upRank.text= GameCenter.duplicateMng.CopySettlementDataInfo.upRank.ToString();
		noWing.SetActive(GameCenter.duplicateMng.CopySettlementDataInfo.state != 1);
		wing.SetActive(GameCenter.duplicateMng.CopySettlementDataInfo.state == 1);


        for (int i = 0, len = itemLabel.Length; i < len; i++)
        {
            if (itemLabel[i] != null)
            {
                if (i < GameCenter.duplicateMng.CopySettlementDataInfo.items.Count)
                {
                    itemLabel[i].text = GameCenter.duplicateMng.CopySettlementDataInfo.items[i].StackCurCount.ToString();
                    itemLabel[i].gameObject.SetActive(true);
                }
                else
                    itemLabel[i].gameObject.SetActive(false);
            }
        }
		
		UIEventListener.Get(btnTo).onClick = delegate {
			GameCenter.littleHelperMng.OpenWndByType(LittleHelpType.STRONG);
		};
		
		if(btnClose != null)UIEventListener.Get(btnClose).onClick = delegate {
			GameCenter.uIMng.SwitchToUI(GUIType.NONE);
		};
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
	}
}
