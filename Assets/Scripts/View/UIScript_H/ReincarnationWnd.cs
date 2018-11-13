/// <summary>
///  何明军
/// 2016/4/19
/// 转生界面
/// </summary>
using UnityEngine;
using System.Collections;

public class ReincarnationWnd : SubWnd {

	public UILabel curReinNum;
	public UILabel rExp;
	public UILabel rLev;
	public UIButton btnOK;
	//public UIButton btnRein;
	
	public ReincarnationItemUI curRein;
	public ReincarnationItemUI nextRein;
	
	public GameObject target;
	//public UILabel curExp;
	//public UILabel curFix;
	public UILabel proExp;
	public UILabel proFix;
	//public UILabel coins;
	
	public UILabel curNum;
	
	public ItemUI[] items;
	//public UILabel[] itemsCoin;
	public UIButton btnOne;//使用普通神元丹
	public UIButton btnTwo;//使用高级神元丹
	public UIButton btnFix;//经验兑换
    public UILabel fullLev;
    public UISpriteEx reinEx;
    public UITexture xiuweiTex;
    public ItemUI expItem;
    public GameObject fullLevEffect;
	
	MainPlayerInfo MainInfo{
		get{
			return GameCenter.mainPlayerMng.MainPlayerInfo;
		}
	}
	
	void ReinShow(ActorBaseTag tag, ulong y,bool da){
		AttributeRef refData = ConfigMng.Instance.GetAttributeRef(MainInfo.CurLevel);
		if(refData == null)return ;
        curReinNum.text = refData.reborn + ConfigMng.Instance.GetUItext(360) + " → " + (refData.reborn + 1) + ConfigMng.Instance.GetUItext(360);
		
		SuperLifeRef data = ConfigMng.Instance.GetSuperLifeRef(refData.reborn);
		SuperLifeRef datar = ConfigMng.Instance.GetSuperLifeRef(refData.reborn + 1);
        float posy = -128;
        float val = 0;
		if(datar != null){
            val = MainInfo.ReliveRes;
            if (data != null && data.superExp != 0) posy = -126 + (val / data.superExp >= 1 ? 1 : val / data.superExp) * 134;
            reinEx.IsGray = UISpriteEx.ColorGray.normal;
            fullLev.gameObject.SetActive(false);
			rLev.text = ConfigMng.Instance.GetLevelDes(data.need_lev);
			rLev.color = data.need_lev > MainInfo.CurLevel ? Color.red : Color.white;
			curRein.SetContent(data);
			nextRein.SetContent(data,datar);
			nextRein.gameObject.SetActive(true); 
            rExp.text = data.superExp + "/" + (data.superExp > MainInfo.ReliveRes ? "[ff0000]" + MainInfo.ReliveRes + "[-]" : MainInfo.ReliveRes.ToString());
            xiuweiTex.transform.localPosition = new Vector3(xiuweiTex.transform.localPosition.x, posy, 0); 
            expItem.FillInfo(new EquipmentInfo(3, GameCenter.inventoryMng.GetNumberByType(3), EquipmentBelongTo.PREVIEW));//经验
            fullLevEffect.SetActive(val / data.superExp >= 1);
            if (expItem.itemName != null) expItem.itemName.enabled = false;
        }else{
			curRein.SetContent(data); 
            nextRein.SetContent(data); 
            fullLev.gameObject.SetActive(true);
            rLev.gameObject.SetActive(false);
            rExp.gameObject.SetActive(false);
            reinEx.IsGray = UISpriteEx.ColorGray.Gray;
            fullLevEffect.SetActive(false);
		}
		
		//curExp.text = MainInfo.CurExp.ToString();
		//curFix.text = MainInfo.ReliveRes.ToString();
		if(data != null){
            string needexp = data.needExp.ToString();
            if (data.needExp / 10000 > 1) needexp = ((float)data.needExp / 10000) + "万";
            proExp.text = needexp;
			proFix.text = "+"+data.buySuperExp;
			//coins.text = data.needGold.ToString();
//			MallRef mall = null;
			for(int i=0,len=items.Length;i<len;i++){
				if(items[i] != null && data.items.Count > i){
//					mall = ConfigMng.Instance.GetMallRef(datar.items[i]);
//					if(mall != null){
                    EquipmentInfo Iteminfo = new EquipmentInfo(data.items[i], GameCenter.inventoryMng.GetNumberByType(data.items[i]), EquipmentBelongTo.PREVIEW);
					items[i].FillInfo(Iteminfo);
					//itemsCoin[i].text = Iteminfo.DiamondPrice.ToString();
					if(items[i].itemName != null)items[i].itemName.enabled = false;
                    if (GameCenter.inventoryMng.GetNumberByType(data.items[i]) <= 0)
                    {
                        items[i].itemCount.gameObject.SetActive(true);
                        if(items[i].itemCount != null)items[i].itemCount.text = "0";
                    }
//					}
				}
			}
		}else{
			proExp.text = "0";
			proFix.text = "+0";
			//coins.text = "0";
		}
		curNum.text = MainInfo.reinNum.ToString();
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
		AttributeRef refData = ConfigMng.Instance.GetAttributeRef(MainInfo.CurLevel);
		if(refData == null)return ;
		SuperLifeRef datar = ConfigMng.Instance.GetSuperLifeRef(refData.reborn);
		UIEventListener.Get(btnOne.gameObject).onClick = delegate {
			if(datar.items.Count > 0){
                if (GameCenter.inventoryMng.GetNumberByType(datar.items[0]) > 0)//有普通神元丹直接使用
                {
                    GameCenter.mainPlayerMng.C2S_ReinState(2);
                }
                else //没有神元丹购买神元丹并使用
                { 
                    EquipmentInfo Iteminfo = new EquipmentInfo(datar.items[0], 1, EquipmentBelongTo.PREVIEW);
                    if (Iteminfo.DiamondPrice > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)
                    { 
                        MessageST mst = new MessageST();
                        mst.messID = 137;
                        mst.delYes = delegate
                        {
                            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                        }; 
                        GameCenter.messageMng.AddClientMsg(mst); 
                    }
                    else
                    { 
                        if (!GameCenter.mainPlayerMng.ShowUseNormalGoldPillTip)
                        {
                            GameCenter.mainPlayerMng.C2S_ReinState(2);
                        }
                        else
                        { 
                            MessageST mst = new MessageST();
                            object[] pa = { 1 };
                            mst.pars = pa;
                            mst.delPars = delegate(object[] ob)
                            {
                                if (ob.Length > 0)
                                {
                                    bool b = (bool)ob[0];
                                    if (b)
                                        GameCenter.mainPlayerMng.ShowUseNormalGoldPillTip = false;
                                }
                            };
                            mst.messID = 494;
                            mst.words = new string[2] { Iteminfo.DiamondPrice.ToString(), Iteminfo.ItemName }; 
                            mst.delYes = delegate
                            {
                                GameCenter.mainPlayerMng.C2S_ReinState(2);
                            };
                            GameCenter.messageMng.AddClientMsg(mst);
                        }
                    }
                } 
			} 
		};
		UIEventListener.Get(btnTwo.gameObject).onClick = delegate {
            if (datar.items.Count > 1)
            {
                if (GameCenter.inventoryMng.GetNumberByType(datar.items[1]) > 0)//有普通神元丹直接使用
                {
                    GameCenter.mainPlayerMng.C2S_ReinState(3);
                }
                else //没有神元丹购买神元丹并使用
                {
                    EquipmentInfo Iteminfo = new EquipmentInfo(datar.items[1], 1, EquipmentBelongTo.PREVIEW);
                    if (Iteminfo.DiamondPrice > GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount)
                    {
                        MessageST mst = new MessageST();
                        mst.messID = 137;
                        mst.delYes = delegate
                        {
                            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                        };
                        GameCenter.messageMng.AddClientMsg(mst); 
                    }
                    else
                    {
                        if (!GameCenter.mainPlayerMng.ShowUseSeniorGoldPillTip)
                        {
                            GameCenter.mainPlayerMng.C2S_ReinState(3);
                        }
                        else
                        {
                            MessageST mst = new MessageST();
                            object[] pa = { 1 };
                            mst.pars = pa;
                            mst.delPars = delegate(object[] ob)
                            {
                                if (ob.Length > 0)
                                {
                                    bool b = (bool)ob[0];
                                    if (b)
                                        GameCenter.mainPlayerMng.ShowUseSeniorGoldPillTip = false;
                                }
                            };
                            mst.messID = 494;
                            mst.words = new string[2] { Iteminfo.DiamondPrice.ToString(), Iteminfo.ItemName };
                            mst.delYes = delegate
                            {
                                GameCenter.mainPlayerMng.C2S_ReinState(3);
                            };
                            GameCenter.messageMng.AddClientMsg(mst);
                        }
                    }
                }
            } 
		};
		UIEventListener.Get(btnOK.gameObject).onClick = delegate {
			if(datar.superExp > MainInfo.ReliveRes){
				GameCenter.messageMng.AddClientMsg(203);
				return ;
			}
			GameCenter.mainPlayerMng.C2S_ReinState(1);
		};
		UIEventListener.Get(btnFix.gameObject).onClick = delegate {
			if(datar == null){
				GameCenter.messageMng.AddClientMsg(220);
				return ;
			}
            //if(MainInfo.reinNum <= 0){
            //    GameCenter.messageMng.AddClientMsg(206);
            //    return ;
            //}
			if(MainInfo.CurExp < (ulong)datar.needExp){
				GameCenter.messageMng.AddClientMsg(205);
				return ;
			}
			GameCenter.mainPlayerMng.C2S_ReinState(4);
		};
        //UIEventListener.Get(btnRein.gameObject).onClick = delegate {
        //    if(datar == null){
        //        GameCenter.messageMng.AddClientMsg(220);
        //        return ;
        //    }
        //    target.SetActive(true);
        //};
		ReinShow(ActorBaseTag.CurMP,1,false);
		MainInfo.OnBaseUpdate += ReinShow;
        GameCenter.inventoryMng.OnBackpackUpdate += Refresh;
	}

    void Refresh()
    {
        ReinShow(ActorBaseTag.CurMP, 1, false);
    }

	protected override void OnClose ()
	{
		base.OnClose ();
		MainInfo.OnBaseUpdate -= ReinShow;
        GameCenter.inventoryMng.OnBackpackUpdate -= Refresh;
	}
	
	void OnDestroy(){
		MainInfo.OnBaseUpdate -= ReinShow;
        GameCenter.inventoryMng.OnBackpackUpdate -= Refresh;
	} 
}
