//===================================
//作者：吴江
//日期:2015/6/16
//用途：物品信息展示界面
//=====================================

using UnityEngine;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 物品信息展示界面  by吴江
/// </summary>
public class EquipmentTooltip : GUIBase
{

	#region 宠物及时装(可预览模型)
	/// <summary>
	/// 带预览UI的
	/// </summary>
	public GameObject itemAnimal;
	public UITexture itemPreview;
	public UILabelAndBack animalAccess;

	public UISprite animalBackground;
	public GameObject animalBottomGo;
	#endregion

	#region 装备
	/// <summary>
	/// 装备UI
	/// </summary>
    public GameObject itemEquip;
	public UILabelAndBack[] labDes;
	public UIScrollView scrollView;
	/// <summary>
	/// 物品职业
	/// </summary>
	public UILabel itemProf;
	/// <summary>
	/// 物品部位
	/// </summary>
	public UILabel itemSlot;
	public UILabel itemGS;
	public UISprite gsUp;
	public UISprite gsDown;

	public UISprite background;
	public GameObject bottomGo;
	#endregion

    #region 坐骑装备
    public GameObject mountEquip;
    public UILabel itemFamily;
    /// <summary>
    /// 物品部位
    /// </summary>
    public UILabel mountEquipSlot;
    public UILabel mountEquipGS;
    #endregion

    #region 其他
    /// <summary>
	/// 材料等物品
	/// </summary>
    public GameObject itemOther;
	public UILabelAndBack otherAccess;
	#endregion

    
	#region 共用
	/// <summary>
	/// 名字和强化等级
	/// </summary>
	public UILabel[] itemName;
	/// <summary>
	/// 需求等级
	/// </summary>
	public UILabel[] needLev;
	public UISprite[] icon;
	/// <summary>
	/// 品质描述
	/// </summary>
	public UISprite[] qualityIcon;
	public UILabel[] itemMoney;
	public UILabel[] itemDetails; 

	public GameObject leftButton;
	public GameObject middleButton;
	public GameObject rightButton;
	public GameObject accessButton;

	public UILabel leftButtonText;
	public UILabel middleButtonText;
	public UILabel rightButtonText;
	public UILabel accessButtonText;

	public GameObject btnUpView;
	public GameObject btnDownView;
	#endregion
	/// <summary>
	/// 最大的中间区域高度
	/// </summary>
	public int maxMiddleHeight = 330;
	/// <summary>
	/// 上面和底部固定占用高度
	/// </summary>
	public int topAndBottomHeight = 220;

    [HideInInspector]
    public UIToggle mSyncTriggerChk;

    protected EquipmentInfo curEquipInfo = null;

	void Awake()
	{
		mutualExclusion = false;
		layer = GUIZLayer.COVER;
		if(btnUpView != null)UIEventListener.Get(btnUpView.gameObject).onClick = UpView;
		if(btnDownView != null)UIEventListener.Get(btnDownView.gameObject).onClick = DownView;
	}
    protected override void OnOpen()
    {
        
    }

	protected override void OnClose ()
	{
		base.OnClose ();
	}

	void OnEnable()
    {
       
        GameCenter.inventoryMng.OnUpdateDetailItem += GetItemDetail;
	}

	void OnDisable()
	{
        if (mSyncTriggerChk != null)
        {
            mSyncTriggerChk.value = false;
            mSyncTriggerChk = null;
        }
        GameCenter.inventoryMng.OnUpdateDetailItem -= GetItemDetail;
		ToolTipMng.CloseAllTooltip();//同时关闭比较界面
	}

    void OnDestroy()
    {
        GameCenter.inventoryMng.OnUpdateDetailItem -= GetItemDetail;
    }

	void UpView(GameObject go)
	{
		if(scrollView != null)
			scrollView.SetDragAmount(0,0,false);
	}
	void DownView(GameObject go)
	{
		if(scrollView != null)
			scrollView.SetDragAmount(1,1,false);
	}

    EquipmentInfo GetCurEquipInfo(GameObject go)
	{
        return curEquipInfo;
	}
	
	void SetButtonActive(GameObject go, bool state)
	{
		if(go != null) NGUITools.SetActiveSelf(go,state);
	}

    public EquipmentInfo EquipmentInfo
	{
		get
		{
            return curEquipInfo;
		}
		set
		{
            //if (curEquipInfo != value)  //不管是不是同一个都刷新
            //{
                curEquipInfo = value;
                Refresh();
            //}
		}
	}
	
    /// <summary>
    /// 整体刷新 by吴江
    /// </summary>
	void Refresh()
	{
        RefreshBasic();
        if (itemEquip != null) itemEquip.SetActive(false);
        if (itemOther != null) itemOther.SetActive(false);
        if (itemAnimal != null) itemAnimal.SetActive(false);
        if (mountEquip != null) mountEquip.SetActive(false);
		if (curEquipInfo.IsEquip)
        {
			if(itemEquip != null)itemEquip.SetActive(true);
            RefreshProperty();
			if(labDes != null)ShowDes();
        }else if(curEquipInfo.Family == EquipmentFamily.MOUNTEQUIP)
        {
            if (mountEquip != null) mountEquip.SetActive(true);
            if (itemFamily != null) itemFamily.text = curEquipInfo.FamilyName;
            if (mountEquipSlot != null) mountEquipSlot.text = curEquipInfo.SlotName;
        }
        else if(curEquipInfo.Family == EquipmentFamily.PET || curEquipInfo.Family == EquipmentFamily.COSMETIC)
        {
			if(itemAnimal != null)itemAnimal.SetActive(true);
			//int [] equipArr = new int[1]{curEquipInfo.EID};
			Dictionary<EquipSlot,EquipmentInfo> equipDic = new Dictionary<EquipSlot, EquipmentInfo>();
			equipDic[curEquipInfo.Slot] = curEquipInfo;
			if(itemPreview != null && ToolTipMng.ShowEquipmentModel)
			{
				if(curEquipInfo.Family == EquipmentFamily.COSMETIC)
				{
                    //if(curEquipInfo.NeedProf == GameCenter.mainPlayerMng.MainPlayerInfo.Prof)
                    //{
                    //    GameCenter.previewManager.TryPreviewSinglePlayer(GameCenter.mainPlayerMng.MainPlayerInfo,itemPreview,equipDic,ActorAnimFSM.EventType.Movie);
                    //}else 
                    if(curEquipInfo.NeedProf != 0)
					{
						PlayerBaseInfo playerInfo = new PlayerBaseInfo(curEquipInfo.NeedProf,0);
						GameCenter.previewManager.TryPreviewSinglePlayer(playerInfo,itemPreview,equipDic);
					}else
					{
						Debug.LogError("时装数据错误,NeedProf:"+curEquipInfo.NeedProf);
					}
				}else
				{
					int petID = 0;
					if(int.TryParse(curEquipInfo.ActionArg,out petID))
					{
						GameCenter.previewManager.TryPreviewSingelEntourage(petID,itemPreview);
					}
						//itemPreview.StartLoad(petID,NGUI3DType.Entourage,equipArr);
				}	
			}
			if(animalAccess != null)
			{
                if (curEquipInfo != null && !curEquipInfo.EquipGetAddress.Equals("0"))
                {
                    animalAccess.SetEquipmentInfo(curEquipInfo);
                    animalAccess.SetUIAccess(curEquipInfo.EquipGetAddress);
                }
                else
                    animalAccess.SetTextData("");
			}
			if(itemPreview != null)itemPreview.enabled = ToolTipMng.ShowEquipmentModel;
			if(animalBackground != null)
				animalBackground.height = ToolTipMng.ShowEquipmentModel?560:400;
			if(animalBottomGo != null)
				animalBottomGo.transform.localPosition = ToolTipMng.ShowEquipmentModel?new Vector3(-12,0,0):new Vector3(-12,160,0);
		}else
		{
			if(itemOther != null)itemOther.SetActive(true);
			if(otherAccess != null)
			{
                if (curEquipInfo != null && !curEquipInfo.EquipGetAddress.Equals("0"))
                {
                    otherAccess.SetEquipmentInfo(curEquipInfo);
                    otherAccess.SetUIAccess(curEquipInfo.EquipGetAddress);
                }
                else
                    otherAccess.SetTextData("");
			}
		}
	}
	void ShowDes()
	{
		int height = 0;
		StringBuilder builder = null;
		for (int i = 0; i < labDes.Length; i++) 
		{
			if(labDes[i] == null)return;
			labDes[i].gameObject.SetActive(true);
			labDes[i].transform.localPosition = new Vector3(37,140 - height,0);
			switch(i)
			{
			case 0://战力级幸运值
				builder = new StringBuilder();
				if(curEquipInfo.Slot != EquipSlot.None)
				{
					builder.Append(ConfigMng.Instance.GetUItext(226)).Append(curEquipInfo.GS);
					EquipmentInfo slotOne = GameCenter.inventoryMng.GetEquipFromEquipDicBySlot(curEquipInfo.Slot);
					if(slotOne != null && curEquipInfo.BelongTo != EquipmentBelongTo.EQUIP && slotOne.GS != curEquipInfo.GS)
					{
						builder.Append((slotOne.GS < curEquipInfo.GS)?"[00ff00]    +":"[ff0000]    -").Append(Mathf.Abs((float)(slotOne.GS - curEquipInfo.GS))).Append("[-]");
						if(gsUp != null)gsUp.enabled =( slotOne.GS < curEquipInfo.GS);
						if(gsDown != null)gsDown.enabled =( slotOne.GS > curEquipInfo.GS);
					}else
					{
						if(gsUp != null)gsUp.enabled =false;
						if(gsDown != null)gsDown.enabled =false;
					}
					if(curEquipInfo.Family == EquipmentFamily.WEAPON)
						builder.Append("\n").Append(ConfigMng.Instance.GetUItext(227)).Append(curEquipInfo.LuckyLv);
				}
				height += labDes[i].SetTextData(builder.ToString());
				break;
			case 1://基础属性
				builder = new StringBuilder();
				if(curEquipInfo.Slot != EquipSlot.None)
				{
					if(curEquipInfo.AttributePairList.Count > 0)builder.Append(ConfigMng.Instance.GetUItext(228)).Append("\n");
					builder.Append(GameHelper.GetAttributeWithStrengthValue(curEquipInfo.AttributePairList,curEquipInfo));
				}
				height += labDes[i].SetTextData(builder.ToString());
				break;
			case 2://洗练属性
				builder = new StringBuilder();
				if(curEquipInfo.Slot != EquipSlot.None)
				{
					if(curEquipInfo.EquOne != 0)
                        builder.Append(ConfigMng.Instance.GetUItext(229)).Append("\n").Append(ConfigMng.Instance.GetValueStringByID(curEquipInfo.EquOne));
					if(curEquipInfo.EquTwo != 0)
						builder.Append("         ").Append(ConfigMng.Instance.GetValueStringByID(curEquipInfo.EquTwo)).Append("\n");
					if(curEquipInfo.EquThree != 0)
						builder.Append(ConfigMng.Instance.GetValueStringByID(curEquipInfo.EquThree));
					if(curEquipInfo.EquFour != 0)
						builder.Append("         ").Append(ConfigMng.Instance.GetValueStringByID(curEquipInfo.EquFour));
				}
				height += labDes[i].SetTextData(builder.ToString());
				break;
			case 3://镶嵌属性
				builder = new StringBuilder();
				if(curEquipInfo.Slot != EquipSlot.None && curEquipInfo.Family != EquipmentFamily.GEM)
				{
					if(curEquipInfo.HasInlayGem)
					{
                        builder.Append(ConfigMng.Instance.GetUItext(230)).Append("\n");
						int index = 0;
						using(var e = curEquipInfo.InlayGemDic.GetEnumerator())
						{
							while(e.MoveNext())
							{
								st.net.NetBase.pos_des pos_des = e.Current.Value;
								if(pos_des.type != 0)
								{
									EquipmentInfo info = new EquipmentInfo(pos_des.type,EquipmentBelongTo.NONE);
									builder.Append(info.ItemName).Append(" ");
									if(info.AttributePairList.Count > 0)
									{
										string attrName = ConfigMng.Instance.GetAttributeTypeName(info.AttributePairList[0].tag);
										builder.Append(attrName).Append(":").Append(info.AttributePairList[0].value);
									}
									index++;
									//if(index < curEquipInfo.InlayGemDic.Count && index%2 != 0)builder.Append("    ");
									//if(index < curEquipInfo.InlayGemDic.Count && index%2 == 0)builder.Append("\n");
									if(index < curEquipInfo.InlayGemDic.Count)builder.Append("\n");
								}
							}
						}
					}
				}
				height += labDes[i].SetTextData(builder.ToString());
				break;
			case 4://套装属性
				if(curEquipInfo != null && curEquipInfo.Slot != EquipSlot.None)
				{
					builder = new StringBuilder();
					builder.Append(GameHelper.GetLevelSuitDes(curEquipInfo));
					height += labDes[i].SetTextData(builder.ToString());
				}else
					height += labDes[i].SetTextData("");
				break;
			case 5://获取途径
				if(curEquipInfo != null && !curEquipInfo.EquipGetAddress.Equals("0") )
				{
                    labDes[i].SetEquipmentInfo(curEquipInfo);
					height += labDes[i].SetUIAccess(curEquipInfo.EquipGetAddress);
				}else
				{
					height += labDes[i].SetTextData("");
				}
				break;
			}
		}
		if(background != null)
		{
			if(height < maxMiddleHeight)
				background.height = height+topAndBottomHeight;
			else
				background.height = maxMiddleHeight+topAndBottomHeight;
			if(bottomGo != null)
				bottomGo.transform.localPosition = new Vector3(0,height > maxMiddleHeight?0:maxMiddleHeight-height,0);
		}
		if(btnUpView != null)btnUpView.gameObject.SetActive(height > maxMiddleHeight);
		if(btnDownView != null)btnDownView.gameObject.SetActive(height > maxMiddleHeight);
	}

    /// <summary>
    /// 刷新基本信息，名字和图片以及品质
    /// </summary>
    protected void RefreshBasic()
    {
        if (curEquipInfo == null) return;
        if (itemName != null)
        {
			for (int i = 0,max=itemName.Length; i < max; i++) {
				if(itemName[i] != null)itemName[i].text = curEquipInfo.ItemStrColor + curEquipInfo.ItemName+ (curEquipInfo.UpgradeLv > 0?("  +" + curEquipInfo.UpgradeLv):"");
			}
        }
        if (icon != null)
		{
			for (int i = 0,max=icon.Length; i < max; i++) {
				if(icon[i] != null)icon[i].spriteName = curEquipInfo.IconName;
			}
		}
		if(qualityIcon != null)
		{
			for (int i = 0,max=qualityIcon.Length; i < max; i++) {
				if(qualityIcon[i] != null)qualityIcon[i].color = curEquipInfo.ItemColor;
			}
		}
        if (itemMoney != null)
		{
			for (int i = 0,max=itemMoney.Length; i < max; i++) {
				if(itemMoney[i] != null)
				{
					itemMoney[i].text = curEquipInfo.CanSell?(curEquipInfo.Price).ToString():ConfigMng.Instance.GetUItext(193);
				}
			}
		}
        if (needLev != null)
        {
			for (int i = 0,max=needLev.Length; i < max; i++) {
				if(needLev[i] != null)
				{
                    if (curEquipInfo.Family != EquipmentFamily.MOUNTEQUIP)
                    {
                        string lvStr = ConfigMng.Instance.GetLevelDes(curEquipInfo.UseReqLevel);
                        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= curEquipInfo.UseReqLevel)
                            needLev[i].text = "[00FF00]" + lvStr;
                        else
                            needLev[i].text = "[FF0000]" + lvStr;
                    }
                    else
                    {
                        needLev[i].text = curEquipInfo.UpgradeLv.ToString();//坐骑装备的等级,实际为强化等级
                    }
				}
			}
        }
		if(itemDetails != null)
		{
			for (int i = 0,max=itemDetails.Length; i < max; i++) {
				if(itemDetails[i] != null)
				{
					StringBuilder builder = new StringBuilder();
					builder.Append(curEquipInfo.Description.Replace("\\n","\n")).Append("\n");
					if(curEquipInfo.Family == EquipmentFamily.GEM)
					{
						for (int j = 0,maxJ=curEquipInfo.AttributePairList.Count; j < maxJ; j++) 
						{
							string attrName = ConfigMng.Instance.GetAttributeTypeName(curEquipInfo.AttributePairList[j].tag);
							string value = curEquipInfo.AttributePairList[j].value.ToString();
							builder.Append("[00ff00]").Append(attrName).Append("   +").Append(value);
							if(j < maxJ-1)builder.Append("\n[-]");
						}
					}
					if(curEquipInfo.Family == EquipmentFamily.TOKEN)
					{
						List<AttributePair> attrPair = GameCenter.coupleMng.GetTokenAttrByItem(curEquipInfo.EID);//信物初始属性
						for (int j = 0,maxJ=attrPair.Count; j < maxJ; j++) 
						{
							string attrName = ConfigMng.Instance.GetAttributeTypeName(attrPair[j].tag);
							int value = attrPair[j].value;//初始属性
                            int addVal = GameCenter.coupleMng.GetTokenAttrValueByTag(attrPair[j].tag);//获得的属性
                            string val = string.Empty;
                            if (GameCenter.uIMng.CurOpenType == GUIType.MARRIAGE)
                            {
                                val = value.ToString();
                            }
                            else
                            {
                                val = (value + addVal).ToString();
                            }

                            builder.Append("[00ff00]").Append(attrName).Append("  ").Append(val);
							if(j < maxJ-1)builder.Append("\n[-]");
						}
					}
                    if (curEquipInfo.Family == EquipmentFamily.MOUNTEQUIP)//坐骑装备属性
                    {
                        if (mountEquipGS != null) mountEquipGS.text = curEquipInfo.GS.ToString();
                        for (int j = 0, maxJ = curEquipInfo.StrengthValue.Count; j < maxJ; j++)
                        {
                            string attrName = ConfigMng.Instance.GetAttributeTypeName(curEquipInfo.StrengthValue[j].tag);
                            string value = curEquipInfo.StrengthValue[j].value.ToString();
                            builder.Append("[00ff00]").Append(attrName).Append("   +").Append(value);
                            if (j < maxJ - 1) builder.Append("\n[-]");
                        }
                    }
					//builder.Append((curEquipInfo.IsBind?"[ff0000]已绑定[-]":"[00ff00]未绑定[-]"));
					itemDetails[i].text = builder.ToString();
				}
			}
		}
    }

    /// <summary>
    /// 获得详细数据
    /// </summary>
    protected void GetItemDetail(EquipmentInfo _info)
    {
        EquipmentInfo = _info;
    }

    /// <summary>
    /// 刷新属性信息，以及对比信息
    /// </summary>
    protected void RefreshProperty()
    {
        if (curEquipInfo == null) return;
        if (itemGS != null)
        { 
//            装备基础战力 +
//装备基础战力 * 强化加成比例+装备基础战力*加星加成比例 + 
//附加属性战力*加星加成比例 + 

            itemGS.text = curEquipInfo.GS.ToString();

        }
//        if (itemLev != null)
//			itemLev.text = curEquipInfo.ItemStrColor +  (curEquipInfo.UpgradeLv > 0?curEquipInfo.UpgradeLv.ToString():"");
        if (itemProf!=null)
        { 
			PlayerConfig playerConfig = ConfigMng.Instance.GetPlayerConfig(curEquipInfo.NeedProf);
			bool selfEquip = curEquipInfo.CheckClass(GameCenter.mainPlayerMng.MainPlayerInfo.Prof);
			string colorStr = selfEquip?"[00ff00]{0}[-]":"[ff0000]{0}[-]";
			itemProf.text = playerConfig == null?string.Format(colorStr,ConfigMng.Instance.GetUItext(194)):string.Format(colorStr,playerConfig.name);
        }
		if(itemSlot != null)
		{
			itemSlot.text = curEquipInfo.SlotName;
		}
    }

    /// <summary>
    /// 随机属性静态显示
    /// </summary>
    public StringBuilder StaticExtraAttr(int _curNum) 
    {
        StringBuilder strBuild = new StringBuilder(512);

        return strBuild;
    }


    /// <summary>
    /// 显示某个指定的功能按钮 by吴江
    /// </summary>
    /// <param name="button"></param>
    /// <param name="buttonLabel"></param>
    /// <param name="buttonText"></param>
    /// <param name="onClick"></param>
    void ShowButton(GameObject button, UILabel buttonLabel, string buttonText, UIEventListener.VoidDelegate onClick)
    {
        if (buttonText == string.Empty || onClick == null) return;
        if (buttonLabel != null)
        {
            buttonLabel.text = buttonText;
//			gamesBtn.Add(buttonLabel);
        }
        if (button != null)
        {
//			UIEventListener listener =  leftButton.GetComponent<UIEventListener>();
//			if(listener!= null)Destroy(listener);
            UIEventListener buttonListener = UIEventListener.Get(button);
            buttonListener.parameter = curEquipInfo;
			buttonListener.onClick = onClick;
//            buttonListener.onClick += Close;
            NGUITools.SetActiveSelf(button, true);
//			if(buttonText == "选 择")NGUIDebug.Log("Name" + buttonText);
            //UIButton btn = button.GetComponent<UIButton>();
            //if(btn != null){
            //    UISprite bgSperite = btn.tweenTarget.GetComponent<UISprite>();
            //    if(bgSperite != null){
            //        if(buttonText == "销 毁" || buttonText == "分 解"){
            //            bgSperite.spriteName = "Pic_anniu_hong";
            //        }else{
            //            bgSperite.spriteName = "Button_Normal_Buttonyellow";
            //        }
            //    }
            //}
        }
    }

    public void SetActionBtn(ItemActionType _left, ItemActionType _middle, ItemActionType _right,ItemActionType _other)
    {
        HideAllBtn();
        //		gamesBtn.Clear();
        if (curEquipInfo == null) return;

        string name = curEquipInfo.GetItemActionName(_left);
        if (name != string.Empty)
        {
            ShowButton(leftButton, leftButtonText, name, x1 =>
            {
                //				if(_left == ItemActionType.SelectAdd){
                //					InventoryUtility.TryToSelect(this.curEquipInfo);
                //				}else{
                //					curEquipInfo.DoItemAction(_left);
                //				}
                curEquipInfo.DoItemAction(_left);
                Close(leftButton.gameObject);
            });
        }
        name = curEquipInfo.GetItemActionName(_middle);
        if (name != string.Empty)
        {
            ShowButton(middleButton, middleButtonText, name, x2 =>
            {
//                				if(name == "强 化"){
//                					InventoryUtility.Strengthen(this.curEquipInfo);
//                				}else{
//                					if(_middle == ItemActionType.SelectAdd){
//                						InventoryUtility.TryToSelect(this.curEquipInfo);
//                					}else{
//                						curEquipInfo.DoItemAction(_middle);
//                					} 
                curEquipInfo.DoItemAction(_middle);
                Close(middleButton.gameObject);
                //				}
            });
        }
        name = curEquipInfo.GetItemActionName(_right);
        if (name != string.Empty)
        {
            ShowButton(rightButton, rightButtonText, name, x3 =>
            {
                //				if(_right == ItemActionType.SelectAdd){
                //					InventoryUtility.TryToSelect(this.curEquipInfo);
                //				}else{
                //					curEquipInfo.DoItemAction(_right);
                //				} 
                curEquipInfo.DoItemAction(_right);
                Close(rightButton.gameObject);
            });
        } 
        name = curEquipInfo.GetItemActionName(_other);
        if (name != string.Empty)
        {
            ShowButton(accessButton, accessButtonText, name, x4 =>
            {
                //				if(_right == ItemActionType.SelectAdd){
                //					InventoryUtility.TryToSelect(this.curEquipInfo);
                //				}else{
                //					curEquipInfo.DoItemAction(_right);
                //				} 
                curEquipInfo.DoItemAction(_other);
                Close(accessButton.gameObject);
            });
        }
    }


    /// <summary>
    /// 设置功能按钮 by吴江
    /// </summary>
    /// <param name="_left"></param>
    /// <param name="_middle"></param>
    /// <param name="_right"></param>
	public void SetActionBtn(ItemActionType _left,ItemActionType _middle,ItemActionType _right)
    {
        HideAllBtn();
//		gamesBtn.Clear();
        if(curEquipInfo == null) return;

		string name = curEquipInfo.GetItemActionName(_left);
        if (name != string.Empty)
        {
			ShowButton(leftButton, leftButtonText, name, x1 => {
//				if(_left == ItemActionType.SelectAdd){
//					InventoryUtility.TryToSelect(this.curEquipInfo);
//				}else{
//					curEquipInfo.DoItemAction(_left);
//				}
				curEquipInfo.DoItemAction(_left);
				Close(leftButton.gameObject);
			});   
        }
        name = curEquipInfo.GetItemActionName(_middle);
        if(name != string.Empty)
        {
			ShowButton(middleButton, middleButtonText, name, x2 => { 
//				if(name == "强 化"){
//					InventoryUtility.Strengthen(this.curEquipInfo);
//				}else{
//					if(_middle == ItemActionType.SelectAdd){
//						InventoryUtility.TryToSelect(this.curEquipInfo);
//					}else{
//						curEquipInfo.DoItemAction(_middle);
//					} 
					curEquipInfo.DoItemAction(_middle);
					Close(middleButton.gameObject);
//				}
			});   
        }
		name = curEquipInfo.GetItemActionName(_right);
        if (name != string.Empty)
        {
			ShowButton(rightButton, rightButtonText, name, x3 => { 
//				if(_right == ItemActionType.SelectAdd){
//					InventoryUtility.TryToSelect(this.curEquipInfo);
//				}else{
//					curEquipInfo.DoItemAction(_right);
//				} 
				curEquipInfo.DoItemAction(_right);
				Close(rightButton.gameObject);
			});  
        }
    }

    protected void HideAllBtn()
    {
        if (leftButton != null){
			NGUITools.SetActiveSelf(leftButton, false);
		}
        if (middleButton != null){
			NGUITools.SetActiveSelf(middleButton, false);
		}
        if (rightButton != null){
			NGUITools.SetActiveSelf(rightButton, false);
		}
		if (accessButton != null){
			NGUITools.SetActiveSelf(accessButton, false);
		}
    }

    public void Close(GameObject _go)
    {
        ToolTipMng.CloseAllTooltip();             
    }


	
    /// <summary>
    /// 获取绑定状态描述语句  TO DO:文字获取要改成读配置，不能用硬编码  by吴江
    /// </summary>
    /// <param name="_eq"></param>
    /// <returns></returns>
	public static string GetBindWord(EquipmentInfo _eq)
	{
        return string.Empty;
	}


}

