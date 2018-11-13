/// <summary>
/// 何明军 
/// 2016/4/19
/// 合成界面
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackPackSynthesisUI : SubWnd {

    ///// <summary>
    ///// 当前查看的材料用于下次打开的格子显示
    ///// </summary>
    //protected int curLookMat = 0;
    ///// <summary>
    ///// 当前查看的宠物用于下次打开的格子显示
    ///// </summary>
    //protected int curLookPet = 0;
    ///// <summary>
    ///// 当前查的的宝石用于下次打开的格子显示
    ///// </summary>
    //protected int curLookGem = 0;

	public List<UIButton> btnsType;
	//public List<GameObject> btnsTypeRead;
	public UIButton toOther;
	
	public UIButton btnSOk;
	public UIButton btnSAllOk;
	
	public List<ItemUI> twoItem;
	public List<ItemUI> threeItem;
	public List<ItemUI> fourItem;
	
	public ItemUI sFinished;
	
	public GameObject uIGridItem;
	
	public UILabel sDes;
	
	public UIToggle uiTog;
	
	public UIFxAutoActive[] effectGames;


    private List<int> redList = new List<int>();
	List<ItemUI> curItem;
	int curType = 1;
	bool isStart = false;
	
	void Awake(){
		uIGridItem.SetActive(false);
		list.Add(uIGridItem.GetComponent<SynthesisItemUI>());
	}
	
	void Start(){
		isStart = false;
		StartCoroutine(StartOver());
	}
	
	IEnumerator StartOver(){
		yield return new WaitForEndOfFrame();
		isStart = true;
		ShowItemDes();
	}
	
	#region 创建能合成物品显示UI
	List<SynthesisItemUI> list = new List<SynthesisItemUI>();
	UIGrid uiiGrid;
	void ShowALL(){
//		DestroyAllItem();
		CreateItemUI();
        SetBtnRed();
	}
	
	void CreateItemUI(){
		int i=0;
		int fastIndex = -1;
        //switch (curType)
        //{
        //    case 1:
        //        fastIndex = curLookMat;
        //        break;
        //    case 2:
        //        fastIndex = curLookPet;er
        //        break;
        //    case 3:
        //        fastIndex = curLookGem;
        //        break;
        //}
		List<BlendRef> listRef = ConfigMng.Instance.GetTypeBlend(curType);
		SynthesisItemUI ui =null;
		uiiGrid.maxPerLine = listRef.Count;
		GameObject go = null;
		UIToggle itemTog = null;
		BlendRef refa = null;
		for(;i< listRef.Count;i++){
			refa = listRef[i];
			if(list.Count <= i){
				go = (GameObject)GameObject.Instantiate(uIGridItem);
				go.transform.parent = uIGridItem.transform.parent;
				go.transform.localPosition = Vector3.zero;
				go.transform.localScale = Vector3.one;
				ui = go.GetComponent<SynthesisItemUI>();
				itemTog = go.GetComponent<UIToggle>();
				
				if(ui != null){
					ui.RefData = refa;
					list.Add(ui);
				}
			}else{
				ui = list[i];
				ui.RefData = refa;
				itemTog = ui.gameObject.GetComponent<UIToggle>();
			}
			itemTog.startsActive = false;
			itemTog.value = false;
            EventDelegate.Remove(itemTog.onChange, ShowItemDes);
            EventDelegate.Add(itemTog.onChange, ShowItemDes);
            //UIEventListener.Get(itemTog.gameObject).onClick -= ShowItemDes; 
            //UIEventListener.Get(itemTog.gameObject).onClick += ShowItemDes; 
            //UIEventListener.Get(itemTog.gameObject).parameter = i;

			if(!uiTog.value){
				ui.gameObject.SetActive(true);
				if(fastIndex < 0)fastIndex = i;
			}else{
				ui.gameObject.SetActive(ui.num.enabled);
				if(fastIndex < 0 && ui.gameObject.activeSelf)fastIndex = i;
			}
		}
		
		for(;i<list.Count;i++){
			list[i].GetComponent<UIToggle>().value = false;
			list[i].gameObject.SetActive(false);
		}
		
		if(list.Count > fastIndex && fastIndex >= 0){
			list[fastIndex].GetComponent<UIToggle>().startsActive = true;
			list[fastIndex].GetComponent<UIToggle>().value = true;
		}
		uiiGrid.repositionNow = true;
        int listLenth = ConfigMng.Instance.GetTypeBlend(curType).Count;
        //Debug.Log("  listLenth  :  " + listLenth + "  curLookMat :  " + curLookMat + "    curLookPet  : " + curLookPet + "     curLookGem :  " + curLookGem);
        //switch (curType)
        //{ 
        //    case 1:
        //        if (listLenth > 5 && curLookMat > 0)
        //        {
        //            if (listLenth - curLookMat > 5)
        //            {
        //                SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12 + curLookMat * 90, 0), 10f);
        //            }
        //            else
        //            {
        //                SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12 + (listLenth - 5) * 90, 0), 10f); 
        //            }
        //        } 
        //        else
        //        {
        //            SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12, 0), 10f);
        //        }
        //        break;
        //    case 2:
        //        if (listLenth > 5 && curLookPet > 0)
        //        {
        //            if (listLenth - curLookPet > 5)
        //            {
        //                SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12 + curLookPet * 90, 0), 10f);
        //            }
        //            else
        //            {
        //                SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12 + (listLenth - 5) * 90, 0), 10f);
        //            }
        //        }
        //        else
        //        {
        //            SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12, 0), 10f);
        //        }
        //        break;
        //    case 3 :
        //        if (listLenth > 5 && curLookGem > 0)
        //        {
        //            if (listLenth - curLookGem > 5)
        //            {
        //                SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12 + curLookGem * 90, 0), 10f);
        //            }
        //            else
        //            {
        //                SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12 + (listLenth - 5) * 90, 0), 10f);
        //            }
        //        }
        //        else
        //        {
        //            SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12, 0), 10f);
        //        }
        //        break;
        //}
        SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12, 0), 10f);
	}
	
	void DestroyAllItem(){
		if(list.Count > 0){
			foreach(SynthesisItemUI ui in list){
				ui.GetComponent<UIToggle>().value = false;
				ui.gameObject.SetActive(false);
			}
		}
	}
	#endregion
	
	#region 选中合成物品显示所需信息

    //void ShowItemDes()
    //{
    //    int val = (int)UIEventListener.Get(go).parameter;
    //    switch (curType)
    //    {
    //        case 1:
    //            curLookMat = val;
    //            break;
    //        case 2:
    //            curLookPet = val;
    //            break;
    //        case 3:
    //            curLookGem = val;
    //            break;
    //    }

    //    ShowItemDes();
    //}


	void ShowItemDes(){
		if(!isStart)return;
		SynthesisItemUI refaui = null;
		SynthesisItemUI game = null;
		for(int j=0, max = list.Count;j< max;j++){
			game = list[j];
			if(!game.gameObject.activeSelf)continue;
			UIToggle tog = game.GetComponent<UIToggle>();
			if(tog != null && tog.value){
				refaui = game;
				break;
			}
		}
		if(refaui == null){
			for(int i=0,len = curItem.Count;i<len;i++){
				curItem[i].FillInfo(null);
				curItem[i].gameObject.SetActive(true);
			}
			sFinished.FillInfo(null);
			if(btnSOk != null)UIEventListener.Get(btnSOk.gameObject).parameter  = null;
			if(btnSAllOk != null)UIEventListener.Get(btnSAllOk.gameObject).parameter  = null;
			return;
		}
		CloseItem();
		EquipmentInfo data = new EquipmentInfo(refaui.RefData.itemsEnd[0].eid,refaui.RefData.itemsEnd[0].count,EquipmentBelongTo.PREVIEW);
		sFinished.FillInfo(data);
		
		
		if(refaui.RefData.needItems.Count == twoItem.Count){
			curItem = twoItem;
		}
		
		if(refaui.RefData.needItems.Count == threeItem.Count){
			curItem = threeItem;
		}
		
		if(refaui.RefData.needItems.Count == fourItem.Count){
			curItem = fourItem;
		}
		
		EquipmentInfo curSdesData = null;
		for(int i=0,len = curItem.Count;i<len;i++){
			if(refaui.RefData.needItems.Count > i){
				curItem[i].FillInfo(new EquipmentInfo(refaui.RefData.needItems[i].eid,refaui.RefData.needItems[i].count,EquipmentBelongTo.PREVIEW));
				int num = GameCenter.inventoryMng.GetNumberByType(refaui.RefData.needItems[i].eid);
				if(curItem[i].itemCount != null)curItem[i].itemCount.text = refaui.RefData.needItems[i].count +"/"+((num > i && num >= refaui.RefData.needItems[i].count) ? num.ToString()
					: "[ff2929]"+(num > i ? num : 0)+"[-]");
				curItem[i].gameObject.SetActive(true);
			}else{
				curItem[i].gameObject.SetActive(false);
			}
			if(curSdesData == null){
				curSdesData = new EquipmentInfo(refaui.RefData.needItems[i].eid,refaui.RefData.needItems[i].count,EquipmentBelongTo.PREVIEW);
			}
		}

        int count = 0;
        for (int i = 0, max = refaui.RefData.needItems.Count; i < max; i++)
        {
            count += refaui.RefData.needItems[i].count;
        }
        if (curSdesData != null) sDes.text = curSdesData.ItemName + "x" + count;
		
		if(btnSOk != null)UIEventListener.Get(btnSOk.gameObject).parameter  = refaui;
		if(btnSAllOk != null)UIEventListener.Get(btnSAllOk.gameObject).parameter  = refaui;
	}
	
	void CloseItem(){
		ItemUI item = null;
		for(int i =0;i< twoItem.Count;i++){
			item = twoItem[i];
			if(item != null)item.gameObject.SetActive(false);
		}
		for(int i =0;i< threeItem.Count;i++){
			item = threeItem[i];
			if(item != null)item.gameObject.SetActive(false);
		}
		for(int i =0;i< fourItem.Count;i++){
			item = fourItem[i];
			if(item != null)item.gameObject.SetActive(false);
		}
	}
	#endregion
	
	#region 按钮事件
	void OnClickBtnType(GameObject game){
		int val = (int)UIEventListener.Get(game).parameter; 
		curType = val;
		ShowALL();
		ShowItemDes();
	}
    //合成红点只显示一次，点击过后就消失
    void SetBtnRed()
    {
        int val = curType;//(int)UIEventListener.Get(game).parameter;
        if (redList.Count == 0) return;
        //if (btnsTypeRead[val - 1] != null) btnsTypeRead[val - 1].SetActive(false);
        if (redList.Contains(val))
            redList.Remove(val);
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.SYNTHETIC, (redList.Count > 0));
    }
	void OnClickBtnSOK(GameObject game){
		SynthesisItemUI refaOn  = UIEventListener.Get(game).parameter as SynthesisItemUI;
		if(refaOn != null){
			if(!refaOn.num.enabled){
				GameCenter.messageMng.AddClientMsg(208);
			}else{
				GameCenter.inventoryMng.C2S_SynthesisItem(refaOn.RefData.id);
			}
		}
	}

	void OnClickBtnSAllOK(GameObject game){
		SynthesisItemUI refaOn  = UIEventListener.Get(game).parameter as SynthesisItemUI;
		if(refaOn != null){
			if(!refaOn.num.enabled){
				GameCenter.messageMng.AddClientMsg(208);
			}else{
				GameCenter.inventoryMng.C2S_SynthesisItemAll(refaOn.RefData.id,refaOn.GetNum());
			}
		}
	}
	#endregion
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
        //curLookMat = 0;
        //curLookPet = 0;
        //curLookGem = 0;
		if(uiTog != null){
			uiTog.value = false;
			EventDelegate.Add(uiTog.onChange,SonChange);
		}
		uiiGrid = uIGridItem.transform.parent.GetComponent<UIGrid>();
        //ShowTypeRead();
		ShowALL();
		
		for(int i = 0;i<btnsType.Count;i++){
			if(btnsType[i] != null){
				UIEventListener.Get(btnsType[i].gameObject).onClick  += OnClickBtnType;
                //UIEventListener.Get(btnsType[i].gameObject).onClick += SetBtnRed;
				UIEventListener.Get(btnsType[i].gameObject).parameter = i + 1;
			}
		}

		if(btnSOk != null)UIEventListener.Get(btnSOk.gameObject).onClick  = OnClickBtnSOK;
		if(btnSAllOk != null)UIEventListener.Get(btnSAllOk.gameObject).onClick  = OnClickBtnSAllOK;
		GameCenter.inventoryMng.OnBackpackUpdate += UpDateShowSyn;
		
		if(toOther != null)UIEventListener.Get(toOther.gameObject).onClick  = delegate {
			GameCenter.uIMng.SwitchToUI(GUIType.EQUIPMENTTRAINING);
		};
        GameCenter.mainPlayerMng.isFirstOpenBackSynUI = false;
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
		
		SynthesisItemUI game = null;
		for(int j=0;j< list.Count;j++){
			game = list[j];
			UIToggle tog = game.GetComponent<UIToggle>();
			if(tog != null)EventDelegate.Remove(tog.onChange,ShowItemDes);
		}

		if(uiTog != null){
			EventDelegate.Remove(uiTog.onChange,SonChange);
		}
		GameCenter.inventoryMng.OnBackpackUpdate -= UpDateShowSyn;
	}
	
	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			GameCenter.inventoryMng.OnGetSynthesisResult += GetSynthesisResult;
		}else
		{
			GameCenter.inventoryMng.OnGetSynthesisResult -= GetSynthesisResult;
		}
	}
	
	void GetSynthesisResult(){
		UpDateShowSyn();
		if(curItem.Count == 2){
			if(effectGames.Length > 0)effectGames[0].ShowFx();
		}
		if(curItem.Count == 3){
			if(effectGames.Length > 1)effectGames[1].ShowFx();
		}
		if(curItem.Count == 4){
			if(effectGames.Length > 2)effectGames[2].ShowFx();
		}
	}
    //void ShowTypeRead(){
    //    if (!GameCenter.mainPlayerMng.isFirstOpenBackSynUI) return;
    //    bool[] btnsTypeReadShow = new bool[btnsTypeRead.Count];
		
    //    foreach(BlendRef refa in ConfigMng.Instance.GetBlendRefTable().Values){
    //        //if(btnsTypeReadShow[refa.sort - 1] == null){
    //        //    btnsTypeReadShow[refa.sort - 1] = false;
    //        //}
    //        if(!btnsTypeReadShow[refa.sort - 1] && refa.needItems.Count > 0 &&GameCenter.inventoryMng.GetNumberByType(refa.needItems[0].eid)/(refa.needItems[0].count * refa.needItems.Count) > 0){
    //            btnsTypeReadShow[refa.sort - 1] = true;
    //            redList.Add(refa.sort);
    //        }
    //    }
    //    for(int i =0;i< btnsTypeRead.Count;i++){
    //        btnsTypeRead[i].SetActive(btnsTypeReadShow[i]);
    //    }
    //}
	
	void UpDateShowSyn(){
		if(list.Count > 0){
			SynthesisItemUI data = null;
			SynthesisItemUI game = null;
			UIToggle tog = null;
			for(int j=0, max = list.Count;j< max;j++){
				game = list[j];
				if(game == null || game.RefData.sort != curType)continue;
				game.UpDateShowNum();
				if(!uiTog.value){
					game.gameObject.SetActive(true);
				}else{
					game.gameObject.SetActive(game.num.enabled);
				}
				tog = game.GetComponent<UIToggle>();
				if(game.gameObject.activeSelf){
					if(data == null)data = game;
					if(tog != null && tog.value)data = game;
				}else{
					if(tog != null && tog.value)tog.value = false;
				}
			}
			if(data != null){
				tog = data.GetComponent<UIToggle>();
				if(tog != null && !tog.value)tog.value = true;
			}
			uiiGrid.repositionNow = true;
        //    int listLenth = ConfigMng.Instance.GetTypeBlend(curType).Count; 
        //    switch (curType)
        //    {
        //        case 1:
        //            if (listLenth > 5 && curLookMat > 0)
        //            {
        //                if (listLenth - curLookMat > 5)
        //                {
        //                    SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12 + curLookMat * 90, 0), 10f);
        //                }
        //                else
        //                {
        //                    SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12 + (listLenth - 5) * 90, 0), 10f);
        //                }
        //            }
        //            else
        //            {
        //                SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12, 0), 10f);
        //            }
        //            break;
        //        case 2:
        //            if (listLenth > 5 && curLookPet > 0)
        //            {
        //                if (listLenth - curLookPet > 5)
        //                {
        //                    SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12 + curLookPet * 90, 0), 10f);
        //                }
        //                else
        //                {
        //                    SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12 + (listLenth - 5) * 90, 0), 10f);
        //                }
        //            }
        //            else
        //            {
        //                SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12, 0), 10f);
        //            }
        //            break;
        //        case 3:
        //            if (listLenth > 5 && curLookGem > 0)
        //            {
        //                if (listLenth - curLookGem > 5)
        //                {
        //                    SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12 + curLookGem * 90, 0), 10f);
        //                }
        //                else
        //                {
        //                    SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12 + (listLenth - 5) * 90, 0), 10f);
        //                }
        //            }
        //            else
        //            {
        //                SpringPanel.Begin(uiiGrid.transform.parent.gameObject, new Vector3(72, 12, 0), 10f);
        //            }
        //            break;
        //    }
        } 
		ShowItemDes();
		//ShowTypeRead();
	}
	
	void OnDisable(){
		for(int i = 0;i<btnsType.Count;i++){
			if(btnsType[i] != null)UIEventListener.Get(btnsType[i].gameObject).onClick  -= OnClickBtnType;
            //if (btnsType[i] != null) UIEventListener.Get(btnsType[i].gameObject).onClick -= SetBtnRed;
		}
		
		if(uiTog != null){
			EventDelegate.Remove(uiTog.onChange,SonChange);
		}
	}
	/// <summary>
	/// 监听uiTog组件
	/// </summary>
	void SonChange(){
		SynthesisItemUI data = null;
		SynthesisItemUI game = null;
		UIToggle tog = null;
//		uiiGrid.maxPerLine = 0;
		int j=0;
		for(;j< list.Count;j++){
			game = list[j];
			if(game == null || game.RefData.sort != curType)continue;
			tog = game.GetComponent<UIToggle>();
			if(!game.num.enabled){
				if(tog != null && tog.value)tog.value = false;
				game.gameObject.SetActive(!uiTog.value);
			}
			if(game.gameObject.activeSelf){
				if(data == null)data = game;
				if(tog != null && tog.value)data = game;
			}
		}
		if(data != null){
			tog = data.GetComponent<UIToggle>();
			if(tog != null && !tog.value)tog.value = true;
		} 
        //curLookMat = 0; 
        //curLookPet = 0; 
        //curLookGem = 0;  
		ShowItemDes();
        uiiGrid.repositionNow = true; 
		SpringPanel.Begin(uiiGrid.transform.parent.gameObject,new Vector3(72,12,0),10f);
	}
}
