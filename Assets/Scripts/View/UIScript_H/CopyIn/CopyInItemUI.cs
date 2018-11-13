/// <summary>
/// 何明军
/// 2016/4/7
/// 副本入口界面
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyInItemUI : MonoBehaviour {

	public GameObject target;
	
	public CopyInItemChildUI[] copyChilds;
	
	public UIGrid grid;
	
	public GameObject btnOK;
	public GameObject btnSweep;
    public UILabel juanZhouNum;//卷轴数量
	public GameObject btnInNum; 
	public GameObject oneShow;
	
	public GameObject btnToTeam;
	
	public UILabel fight;
	public UILabel inNum;
	public UILabel diamoInNum;
	
	public UISprite icon;
	
	public GameObject btnInNumUI;
	
	public GameObject redShow;
	
	public UITexture textIcon;
	
	public UILabel name;
	
    
	CopyGroupRef data;
	public CopyGroupRef ShowCopyData{
		get{
			return data;
		}
		set{
			data = value;
			if(data != null){
				Init();
				ShowCopyItem();
				ShowEquInfo();
			}
		}
	}
	
	bool ISInCopyChlid(int _id){
		CopyRef refd = ConfigMng.Instance.GetCopyRef(_id);
		if(refd == null)return false;
		return refd.lvId <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel;
	}
	
	public void SeleteCopyScene(){
		int curCopyRefID = InCopy();
        UIToggle toggle = copyChilds[curCopyRefID].GetComponent<UIToggle>();
        if (toggle != null)
            toggle.value = true;
		CopyRef copy = ConfigMng.Instance.GetCopyRef(data.copy[curCopyRefID]);
		if(fight != null && copy != null)fight.text = copy.fighting.ToString();
	}
	
	int InCopy(){
		if(data == null)return 0;
		int curCopyRefID = 0;
		for(int i=0,len=copyChilds.Length;i<len;i++){
			if(copyChilds[i] == null)continue;
			if(i < data.copy.Count){
				if(ISInCopyChlid(data.copy[i]))curCopyRefID = i;
			}
		}
		return curCopyRefID;
	}

    int twoCoppyNum = 0;
	void ShowCopyItem(){
//		if(icon != null)icon.spriteName = data.icon; 
        if (oneShow != null) oneShow.SetActive(GameCenter.duplicateMng.CopyType == DuplicateMng.CopysType.ONESCOPY);
        if (btnSweep != null) btnSweep.SetActive(GameCenter.duplicateMng.CopyType == DuplicateMng.CopysType.ONESCOPY);
        if (juanZhouNum != null)
        {
            juanZhouNum.gameObject.SetActive(GameCenter.duplicateMng.CopyType == DuplicateMng.CopysType.ONESCOPY);
            juanZhouNum.text = GameHelper.GetStringWithBagNumber(2210001, 1);  
        }
		CopyInItemDataInfo serData = null;
        if (GameCenter.duplicateMng.CopyDic.TryGetValue(data.id, out serData))
        {
            twoCoppyNum = serData.num;
            if (inNum != null) inNum.text = serData.num > 0 ? serData.num.ToString() : "0";
            if (diamoInNum != null) diamoInNum.text = serData.buyNum > 0 ? serData.buyNum.ToString() : "0";
            if (redShow != null) redShow.SetActive(serData.num > 0);
        }
		
		for(int i=0,len=copyChilds.Length;i<len;i++){
			if(copyChilds[i] == null)continue;
			if(i < data.copy.Count){
				copyChilds[i].CopyRefID = data.copy[i];
				copyChilds[i].SetStar(data.id);
				copyChilds[i].gameObject.SetActive(true);
			}else{
				copyChilds[i].gameObject.SetActive(false);
			}
		}
		ConfigMng.Instance.GetBigUIIcon(data.icon,delegate(Texture2D texture){
			if(textIcon != null)textIcon.mainTexture = texture;
		});
		
		if(name != null)name.text = data.name;
		
		if(btnInNumUI != null)btnInNumUI.SetActive(false);
	}
	void OnCopyChilds(){
		int copyid = 0;
		for(int i=0,len=copyChilds.Length;i<len;i++){
			if(copyChilds[i] == null)continue;
			if(copyChilds[i].GetComponent<UIToggle>().value){
				copyid = copyChilds[i].CopyRefID;
			}
		}
		if(copyid > 0){
			CopyRef copy = ConfigMng.Instance.GetCopyRef(copyid);
			if(fight != null && copy != null)fight.text = copy.fighting.ToString();
		}
	}
	
	List<ItemUI> listItem = new List<ItemUI>();
	void ShowEquInfo(){
		for(int j = grid.transform.childCount - 1;j>=0 ;j--){
			grid.transform.GetChild(j).gameObject.SetActive(false);
		}
		
		ItemUI item = null;
		int i =0;
		for(;i<data.reward.Count;i++){
			if(listItem.Count > i){
				listItem[i].FillInfo(new EquipmentInfo(data.reward[i],EquipmentBelongTo.PREVIEW));
			}else{
				item = UIUtil.CreateItemUIGame(grid.gameObject).GetComponent<ItemUI>();
                if (item != null)
                {
                    item.transform.localScale = Vector3.one;
                    item.FillInfo(new EquipmentInfo(data.reward[i], EquipmentBelongTo.PREVIEW));
                    if (item.itemName != null) item.itemName.enabled = false;
                    listItem.Add(item);
                }
			}
			listItem[i].gameObject.SetActive(true);
		}
		for(;i<listItem.Count;i++){
			listItem[i].gameObject.SetActive(false);
		}
		grid.repositionNow = true;
	}
	
	void OnEnable(){
		UIToggle tog= null;
		GameCenter.duplicateMng.OnCopyItemChange += ShowCopyItem;
		for(int i=0,len=copyChilds.Length;i<len;i++){
			if(copyChilds[i] == null)continue;
			tog = copyChilds[i].GetComponent<UIToggle>();
			if(tog != null)EventDelegate.Add(tog.onChange,OnCopyChilds);
		}
		target.SetActive(false);
		
		Init();
	}
	
	void Init(){
		if(data == null){
			return ;
		}
		int curCopyRefID = InCopy();
		UIToggle tog= null;
		tog = copyChilds[curCopyRefID].GetComponent<UIToggle>();
		//if(tog != null)tog.startsActive = true;注释掉解决选中难度的BUG
		CopyRef copy = ConfigMng.Instance.GetCopyRef(data.copy[curCopyRefID]);
		if(fight != null && copy != null)fight.text = copy.fighting.ToString();
		
		if(GameCenter.duplicateMng.CopyType == DuplicateMng.CopysType.ONESCOPY){
			if(btnOK != null)UIEventListener.Get(btnOK).onClick = delegate {
				for(int i=0,len=copyChilds.Length;i<len;i++){
					tog = copyChilds[i].GetComponent<UIToggle>();
					if(copyChilds[i] != null && tog != null && tog.value){
						CopyInItemDataInfo serData = null;
						//                        Debug.logger.Log("data.id = " + data.id + " , " + GameCenter.endLessTrialsMng.CopyDic.Count);
						if (GameCenter.duplicateMng.CopyDic.TryGetValue(data.id, out serData))
						{
							if (serData.num > 0)
							{
								GameCenter.duplicateMng.C2S_ToCopyItem(data.id, copyChilds[i].CopyRefID);
							}
							else
							{
                                GameCenter.duplicateMng.PopTip(serData, data, copyChilds[i].CopyRefID, false, false);
							}
							return;
						}else{
							Debug.LogError("升级后没有服务端数据过来，找小唔知！");
						}
					}
				}
			};
		}
        if (btnSweep != null) UIEventListener.Get(btnSweep).onClick = OnClickSweep;
           
		if(btnInNumUI != null)UIEventListener.Get(btnInNum).onClick = delegate {
			CopyInNumBuyUI numbuy = btnInNumUI.GetComponent<CopyInNumBuyUI>();
			if(numbuy != null)numbuy.SetToBuyShow(data);
            if (!diamoInNum.text.Equals("0"))
                btnInNumUI.SetActive(true);
            else
                GameCenter.messageMng.AddClientMsg(242);
		};
		
		bool isAtk = data.id == GameCenter.duplicateMng.copyGroupID;
		if(GameCenter.duplicateMng.CopyType == DuplicateMng.CopysType.TWOSCOPY){
			if(btnToTeam != null)UIEventListener.Get(btnToTeam).onClick = delegate {
				if(!GameCenter.teamMng.isInTeam){
					for(int i=0,len=copyChilds.Length;i<len;i++){
						if(copyChilds[i] == null)continue;
						tog = copyChilds[i].GetComponent<UIToggle>();
						if(tog != null && tog.value){
							GameCenter.duplicateMng.C2S_ReqCopyInTeamData(data.id,copyChilds[i].CopyRefID);
							return ;
						}
					}
				}else{

					if(GameCenter.teamMng.isLeader){
						for(int i=0,len=copyChilds.Length;i<len;i++){
							if(copyChilds[i] == null)continue;
							tog = copyChilds[i].GetComponent<UIToggle>();
							if(tog != null && tog.value){
								GameCenter.duplicateMng.C2S_ReqCopyInTeamData(data.id,copyChilds[i].CopyRefID);
								return ;
							}
						}
					}else{
                        //新加个判断解决多人组队进入一次后,队员可以开启多人副本开启界面
                        //(一旦多人副本组队挑战次数大于一次，此判断失效,正确做法应清空GameCenter.endLessTrialsMng.copyGroupID这个数据)
                        if (GameCenter.duplicateMng.CopyTeams.Count > 0 && isAtk && twoCoppyNum > 0)
                        {
							GameCenter.uIMng.SwitchToUI(GUIType.COPYMULTIPLEWND,GUIType.COPYINWND);
							return ;
						}
						GameCenter.messageMng.AddClientMsg(165);
					}
				}
			};
		}

		if(GameCenter.duplicateMng.copyGroupID > 0 && isAtk){
			tog = gameObject.GetComponent<UIToggle>();
			if(tog != null)tog.startsActive = true;
			if(!GameCenter.teamMng.isInTeam)GameCenter.duplicateMng.copyGroupID = 0;
		}else{
			tog = gameObject.GetComponent<UIToggle>();
			if(tog != null)tog.startsActive = false;
		}
	}
	
	void OnDisable(){
		UIToggle tog = null;
		GameCenter.duplicateMng.OnCopyItemChange -= ShowCopyItem;
		for(int i=0,len=copyChilds.Length;i<len;i++){
			if(copyChilds[i] == null)continue;
			tog = copyChilds[i].GetComponent<UIToggle>();
			if(tog != null)EventDelegate.Remove(tog.onChange,OnCopyChilds);
		}
		ConfigMng.Instance.RemoveBigUIIcon(data.icon);
        twoCoppyNum = 0;
	}

    /// <summary>
    /// 点击扫荡副本
    /// </summary>
    void OnClickSweep(GameObject go)
    {
        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel < 51 && GameCenter.vipMng.VipData != null && GameCenter.vipMng.VipData.vLev < 2)
        {
            GameCenter.messageMng.AddClientMsg(396);//1转45以上或VIP2以上
            return;
        }
        for (int i = 0, len = copyChilds.Length; i < len; i++)
        {
            CopyInItemChildUI copyInItemChildUI = copyChilds[i];
            if (copyInItemChildUI != null && copyInItemChildUI.TogValue)
            {
                CopyInItemDataInfo CopyInItemDataInfo = null;
                if (GameCenter.duplicateMng.CopyDic.TryGetValue(data.id, out CopyInItemDataInfo))
                {
                    //                         GameCenter.endLessTrialsMng.CopyDic[data.id];
                    if (CopyInItemDataInfo != null)
                    {
                        int curStar = CopyInItemDataInfo.copyScenes.ContainsKey(copyInItemChildUI.CopyRefID) ?
                            CopyInItemDataInfo.copyScenes[copyInItemChildUI.CopyRefID].star : 0;
                        //if (curStar >= 3)//改为只需通关即可无需3星
                        if (curStar > 0)
                        {
                            GameCenter.duplicateMng.lcopyGroupRef = data;
                            if (CopyInItemDataInfo.num <= 0)
                            {
                                //GameCenter.messageMng.AddClientMsg(244);
                                GameCenter.duplicateMng.PopTip(CopyInItemDataInfo, data, copyInItemChildUI.CopyRefID, true, true);
                            }
                            else
                            {
                                if (GameCenter.duplicateMng.isShowBuySweepItem)
                                { 
                                    if (GameCenter.inventoryMng.GetNumberByType(2210001) > 0)//新增扫荡卷轴消耗
                                    {
                                        GameCenter.endLessTrialsMng.C2S_SweepReward(1, copyInItemChildUI.CopyRefID);
                                    }
                                    else
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
                                                //GameCenter.inventoryMng.C2S_AskBlessWeapon(2210001);//暂用祝福油的快捷购买协议
                                                GameCenter.endLessTrialsMng.C2S_SweepReward(1, copyInItemChildUI.CopyRefID);//点击确定进入扫荡后台判断是否购买
                                            }
                                            else
                                            {
                                                MessageST mst1 = new MessageST();
                                                mst1.messID = 137;
                                                mst1.delYes = delegate
                                                {
                                                    GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                                                };
                                                GameCenter.messageMng.AddClientMsg(mst1);
                                            }
                                        };
                                        GameCenter.messageMng.AddClientMsg(mst);
                                    }
                                }
                                else
                                {
                                    GameCenter.endLessTrialsMng.C2S_SweepReward(1, copyInItemChildUI.CopyRefID);
                                }
                            } 
                        }
                        else
                        {
                            GameCenter.messageMng.AddClientMsg(127);//243提示改为127
                        }
                    }
                }
                else
                {
                    Debug.LogError("升级后没有服务端数据过来，找小唔知！");
                } 
            }
        }
    } 
}
