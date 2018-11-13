/// <summary>
/// 何明军
/// 2016/4/19
/// 扫荡界面
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SweepRewardWnd : GUIBase {
	public ItemUI[] items;
	
	public GameObject btnClose;
	public GameObject btnOk;
	public GameObject btnNext;
    public UILabel cost;

    protected float time;
    protected bool isChange = false;
    protected List<ItemValue> itemList;
    protected int expNum = 0;
    protected int coinNum = 0;
    protected int wuxingNum = 0;

    public TweenAlphaAllChild[] tweenAlpha;

    

	void Awake(){
		mutualExclusion = false;
		Layer = GUIZLayer.COVER;

        if (btnNext != null) UIEventListener.Get(btnNext).onClick = SendMsg;
		if(btnClose != null)UIEventListener.Get(btnClose).onClick = delegate {
			GameCenter.uIMng.ReleaseGUI(GUIType.SWEEPCARBON);
		};
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
		if(btnOk != null)UIEventListener.Get(btnOk).onClick = delegate {
			GameCenter.uIMng.ReleaseGUI(GUIType.SWEEPCARBON);
		};
		OnSweepRewardAll();
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
	}
    void SendMsg(GameObject _go)
    {
        CopyInItemDataInfo serData = null;
        if (GameCenter.duplicateMng.lcopyGroupRef != null)
        {
            if (GameCenter.duplicateMng.CopyDic.TryGetValue(GameCenter.duplicateMng.lcopyGroupRef.id, out serData))
            {
                if (GameCenter.endLessTrialsMng.sweepType == EndLessTrialsMng.SweepType.EndLess && !GameCenter.endLessTrialsMng.IsSweepingEndless)
                {
                    return;
                }
                if (serData.num <= 0)
                {
                    GameCenter.duplicateMng.PopTip(serData, GameCenter.duplicateMng.lcopyGroupRef, GameCenter.endLessTrialsMng.sweepCopyID, true, GameCenter.endLessTrialsMng.sweepType == EndLessTrialsMng.SweepType.COPY);
                    return;
                }
                if (GameCenter.endLessTrialsMng.sweepType == EndLessTrialsMng.SweepType.COPY)
                {
                    if (GameCenter.duplicateMng.isShowBuySweepItem)
                    {
                        if (GameCenter.inventoryMng.GetNumberByType(2210001) <= 0)//新增扫荡卷轴消耗 
                        {
                            EquipmentInfo eqinfo = new EquipmentInfo(2210001, EquipmentBelongTo.PREVIEW);
                            MessageST mst = new MessageST();
                            object[] pa = { 1 };
                            mst.pars = pa;
                            mst.delPars = delegate(object[] ob)
                            {
                                if (ob.Length > 0)
                                {
                                    bool b = (bool)ob[0];
                                    if (b)
                                    {
                                        GameCenter.duplicateMng.isShowBuySweepItem = false;
                                    }
                                }
                            };
                            mst.messID = 543;
                            mst.words = new string[1] { eqinfo.DiamondPrice.ToString() };
                            mst.delYes = delegate
                            {
                                if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= eqinfo.DiamondPrice)
                                {
                                    GameCenter.endLessTrialsMng.C2S_SweepReward((int)GameCenter.endLessTrialsMng.sweepType, GameCenter.endLessTrialsMng.sweepCopyID);//点击确定进入扫荡后台判断是否购买
                                }
                                else
                                {
                                    MessageST mst1 = new MessageST();
                                    mst1.messID = 137;
                                    mst1.delYes = delegate
                                    {
                                        GameCenter.uIMng.ReleaseGUI(GUIType.SWEEPCARBON);
                                        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                                    };
                                    GameCenter.messageMng.AddClientMsg(mst1);
                                }
                            };
                            GameCenter.messageMng.AddClientMsg(mst);
                            return;
                        }
                    } 
                }
            }
        }
        GameCenter.endLessTrialsMng.C2S_SweepReward((int)GameCenter.endLessTrialsMng.sweepType, GameCenter.endLessTrialsMng.sweepCopyID);
    }
	void OnSweepRewardAll(){
	    itemList = GameCenter.endLessTrialsMng.sweepListItem;
		if(itemList == null)return ; 
        if (cost != null)
        {
            cost.gameObject.SetActive(GameCenter.endLessTrialsMng.sweepType == EndLessTrialsMng.SweepType.COPY);
            cost.text = GameHelper.GetStringWithBagNumber(2210001, 1);
        }
        isChange = true;
        time = Time.time;
        for (int i = 0; i < tweenAlpha.Length; i++)
        {
            if (tweenAlpha[i] != null)
            {
                tweenAlpha[i].ResetToBeginning();
                tweenAlpha[i].enabled = true;
            }
        }
		for(int i=0,len=items.Length;i<len;i++){
			if(items[i] != null){
				if(itemList.Count > i){
					if (itemList[i].eid == 3)
                    {
                        expNum = 0;
                        //if (items[0].itemCount != null)
                        //    items[i].itemCount.text = itemList[i].count.ToString();
					}
                    else if (itemList[i].eid == 5)
                    {
                        coinNum = 0;
                        //if(items[1].itemCount != null)
                        //    items[i].itemCount.text = itemList[i].count.ToString();
					} 
                    else if (itemList[i].eid == 7)
                    {
                        wuxingNum = 0;
                        //if(items[2].itemCount != null)
                        //    items[i].itemCount.text = itemList[i].count.ToString();
					}
                    else
                    {
						items[i].FillInfo(new EquipmentInfo(itemList[i],EquipmentBelongTo.PREVIEW));
					}
					items[i].gameObject.SetActive(true);
				}else{
					items[i].gameObject.SetActive(false);
				}
			}
		}
	}
    void Update()
    {
        if (isChange)
        {
            if (items[0].itemCount != null)
            {
                if (expNum <= itemList[0].count)
                {
                    expNum += (int)(itemList[0].count / 50);
                }
                items[0].itemCount.text = expNum.ToString();
            }
            if (items[1].itemCount != null)
            {
                if (coinNum <= itemList[1].count)
                {
                    coinNum += (int)(itemList[1].count / 50);
                }
                items[1].itemCount.text = coinNum.ToString();
            }
            if (items[2].itemCount != null)
            {
                if (wuxingNum <= itemList[2].count)
                {
                    wuxingNum += (int)(itemList[2].count / 50);
                }
                items[2].itemCount.text = wuxingNum.ToString();
            }
            
            if (Time.time - time > 1)
            {
                if (expNum != itemList[0].count && items[0].itemCount != null) items[0].itemCount.text = itemList[0].count.ToString();
                if (coinNum != itemList[1].count && items[1].itemCount != null) items[1].itemCount.text = itemList[1].count.ToString();
                if (wuxingNum != itemList[2].count && items[2].itemCount != null) items[2].itemCount.text = itemList[2].count.ToString();
                isChange = false;
            }
        }
    }
	
}
