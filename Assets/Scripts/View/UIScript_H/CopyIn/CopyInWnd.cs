/// <summary>
/// 何明军
/// 2016/4/7
/// 副本入口界面
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyInWnd : GUIBase {

	public GameObject btnClose;
	
	public GameObject copyItemety;
	public UITable table;
	
	public UIToggle[] tog;
	public GameObject[] togRed;
//	public CopyMultipleUI multiple;
	
	List<CopyInItemUI> inItemList = new List<CopyInItemUI>();
	
	UIScrollView view;
    /// <summary>
    /// 判断副本是否开启
    /// </summary>
	bool IsCopyOpen(CopyGroupRef refData)
    {
        FunctionType functionType = FunctionType.None;
        if (refData.sort == 1)//只有单人副本由功能开启表开启
        {
            switch (refData.id)
            {
                case 1: functionType = FunctionType.COPY;
                    break;//这种硬编码,有时间就改掉  by邓成
                case 2: functionType = FunctionType.DOUSHUAIASGARD;
                    break;
                case 3: functionType = FunctionType.FIVEMANOR;
                    break;
                case 5: functionType = FunctionType.TRUEORFALSEMONKEY;
                    break;
                case 6: functionType = FunctionType.WHITEPURGATORY;
                    break;
                default: break;
            }
            bool isOpen = refData.lv <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel && GameCenter.mainPlayerMng.FunctionIsOpen(functionType);
            return isOpen;
        }
        else
        {
            return refData.lv <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel;
        }
	}
    /// <summary>
    /// 排序从大到小排序
    /// </summary>
    /// <param name="_data1"></param>
    /// <param name="_data2"></param>
    /// <returns></returns>
    int CopySortType(CopyGroupRef _data1,CopyGroupRef _data2)
    {
        if (_data1.id > _data2.id)
            return -1;
        if (_data1.id < _data2.id)
            return 1;
        return 0;
    }
	void ShowCopyItem(){
        for (int j = 0; j < inItemList.Count; j++)
        {
            inItemList[j].gameObject.SetActive(false);
        }
		GameObject go = null;
		CopyInItemUI endLess = null;
		List<CopyGroupRef> dataList = ConfigMng.Instance.GetCopyGroupRefTable((int)GameCenter.duplicateMng.CopyType);
		int i =0;
        UIToggle uitog = null;
        dataList.Sort(CopySortType);
		foreach(CopyGroupRef eData in dataList){
            if (IsCopyOpen(eData))
            {
                if (inItemList.Count <= i)
                {
                    go = (GameObject)GameObject.Instantiate(copyItemety);
                    go.transform.parent = copyItemety.transform.parent;
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    go.name = "copyItemety" + (eData.id);
                    endLess = go.GetComponent<CopyInItemUI>();
                    if (endLess != null)
                    {
                        endLess.ShowCopyData = eData;
                        endLess.target.SetActive(false);
                        inItemList.Add(endLess);
                    }
                    uitog = go.GetComponent<UIToggle>();
                    if (uitog != null)
                    {
                        uitog.startsActive = false;
                        uitog.value = false;
                        EventDelegate.Add(uitog.onChange, CopyInItemUITog);
                    }
                    go.SetActive(true);
                }
                else
                {
                    inItemList[i].ShowCopyData = eData;
                    uitog = inItemList[i].GetComponent<UIToggle>();
                    if (uitog != null)
                    {
                        uitog.startsActive = false;
                        uitog.value = false;
                    }
                    inItemList[i].target.SetActive(false);
                    inItemList[i].gameObject.SetActive(true);
                }
                i++;
                //跳转到选中的某个副本
                if (GameCenter.duplicateMng.CurSelectOneCopyType != OneCopySType.NONE && GameCenter.duplicateMng.CurSelectOneCopyType == (OneCopySType)eData.id)
                {
                    uitog.value = true;
                } 
            }
		}
		for(;i<inItemList.Count;i++){
			uitog = inItemList[i].GetComponent<UIToggle>();
			if(uitog != null){
				uitog.startsActive = false;
				uitog.value = false;
			}
			inItemList[i].gameObject.SetActive(false);
		}
		if(table != null)table.repositionNow = true;
//		UIScrollView view = table.transform.parent.GetComponent<UIScrollView>();
		if(view != null){
			view.ResetPosition();
			view.SetDragAmount(0,0,false);
		}
	}
	
	protected override void OnOpen ()
	{
        if (initSubGUIType == SubGUIType.NONE) initSubGUIType = SubGUIType.BCopyTypeOne;
		base.OnOpen ();
		for(int i=0,len=tog.Length;i<len;i++){
			EventDelegate.Add(tog[i].onChange,TogOnChange);
		}
		
		GameCenter.duplicateMng.OnCopyTypeChange += ShowCopyItem;
//		GameCenter.endLessTrialsMng.OnCopyItemTeamData += OnCopyItemTeamData;
        GameCenter.duplicateMng.OnCopyItemChange += ShowCopyRed;
		
		if(view != null){
			view.ResetPosition();
			view.SetDragAmount(0,0,false);
		}
        ShowCopyRed();
		ShowCopyItem();
		//SpringPanel.Begin(view.gameObject,Vector3.zero,10);
        CopyInItemUITog();
	}
    protected override void InitSubWndState()
    {
        base.InitSubWndState();
        switch (initSubGUIType)
        { 
            case SubGUIType.BCopyType:
                GameCenter.duplicateMng.CopyType = DuplicateMng.CopysType.TWOSCOPY;
                break;
            case SubGUIType.BCopyTypeOne:
                GameCenter.duplicateMng.CopyType = DuplicateMng.CopysType.ONESCOPY;
                break;
        }
        if (tog.Length >= (int)GameCenter.duplicateMng.CopyType) tog[(int)GameCenter.duplicateMng.CopyType - 1].value = true;
        if ((int)GameCenter.duplicateMng.CopyType - 1 >= 0 && (int)GameCenter.duplicateMng.CopyType - 1 < tog.Length) tog[(int)GameCenter.duplicateMng.CopyType - 1].value = true;
    }
    /// <summary>
    /// 副本Tog红点
    /// </summary>
    void ShowCopyRed()
    {
        //int i = 1;
        //foreach (GameObject game in togRed)
        //{
        //    game.SetActive(GameCenter.endLessTrialsMng.IsTipCopyTypeRedShow(i));
        //    i++;
        //}
        for (int i = 1; i <= togRed.Length; i++)
        {
            togRed[i-1].SetActive(GameCenter.duplicateMng.IsTipCopyTypeRedShow(i));
        }
    }
	protected override void OnClose ()
	{
		base.OnClose ();
		for(int i=0,len=tog.Length;i<len;i++){
			EventDelegate.Remove(tog[i].onChange,TogOnChange);
		}
//        foreach(CopyInItemUI data in inItemList){
//            if(data != null){
//                if(data.textIcon != null)data.textIcon.mainTexture = null;
////				if(data.ShowCopyData != null)ConfigMng.Instance.BigIconRemove(data.ShowCopyData.icon);
//            }
//        }
        for (int i = 0; i < inItemList.Count; i++)
        {
            CopyInItemUI data = inItemList[i];
            if(data != null)
				if(data.textIcon != null)data.textIcon.mainTexture = null;
        }
        GameCenter.duplicateMng.OnCopyItemChange -= ShowCopyRed;
		GameCenter.duplicateMng.OnCopyTypeChange -= ShowCopyItem;
//		GameCenter.endLessTrialsMng.OnCopyItemTeamData -= OnCopyItemTeamData;
        GameCenter.duplicateMng.CurSelectOneCopyType = OneCopySType.NONE;
	}
	
//	void OnCopyItemTeamData(int copyGroupID){
//		if(GameCenter.teamMng.isInTeam && !GameCenter.teamMng.isLeader){
//			MessageST mst = new MessageST();
//			mst.messID = 166;
//			mst.delYes = delegate {
//				multiple.gameObject.SetActive(true);
//			};
//			mst.delNo = delegate {
//				GameCenter.teamMng.C2S_TeamOut();
//			};
//		}else{
//			multiple.CopyGroupID = copyGroupID;
//			multiple.gameObject.SetActive(true);
//		}
//	}
	
	void Awake(){
		mutualExclusion = true;
		Layer = GUIZLayer.TOPWINDOW;
		copyItemety.SetActive(false);
        //if(InitSubGUIType == SubGUIType.BCopyType){
        //    GameCenter.endLessTrialsMng.CopyType = EndLessTrialsMng.CopysType.TWOSCOPY;
        //}else if(InitSubGUIType == SubGUIType.BCopyTypeOne){
        //    GameCenter.endLessTrialsMng.CopyType = EndLessTrialsMng.CopysType.ONESCOPY;
        //}else{
        //    GameCenter.endLessTrialsMng.CopyType = EndLessTrialsMng.CopysType.ONESCOPY;
        //}
		//if(tog.Length >= (int)GameCenter.endLessTrialsMng.CopyType)tog[(int)GameCenter.endLessTrialsMng.CopyType - 1].startsActive = true;
		//if((int)GameCenter.endLessTrialsMng.CopyType - 1 >= 0 && (int)GameCenter.endLessTrialsMng.CopyType - 1 < tog.Length )tog[(int)GameCenter.endLessTrialsMng.CopyType - 1].startsActive = true;
		
		if(btnClose != null)UIEventListener.Get(btnClose).onClick =delegate(GameObject go) {
			GameCenter.uIMng.SwitchToUI(GUIType.NONE);
		};
		if(copyItemety != null){
			//inItemList.Add(copyItemety.GetComponent<CopyInItemUI>());
			UIToggle uitog = copyItemety.GetComponent<UIToggle>();
			EventDelegate.Add(uitog.onChange,CopyInItemUITog);
			copyItemety.SetActive(false);
		}
		view = table.transform.parent.GetComponent<UIScrollView>();
		isTogStart = false;
	}
	bool isTogStart = false;
	void Update(){
		if(!isTogStart){
			isTogStart = true;
			TogOnChange();
		}
	}
	
	void TogOnChange(){
		for(int i=0,len=tog.Length;i<len;i++){
			if(tog[i].value){
				GameCenter.duplicateMng.CopyType = (DuplicateMng.CopysType)i+1;
				return ;
			}
		}
	}
	
	void CopyInItemUITog(){
		int index = -1;
		for(int i=0,len=inItemList.Count;i<len;i++){
			if(!inItemList[i].gameObject.activeSelf)continue;
			UIToggle tog = inItemList[i].GetComponent<UIToggle>();
			if(tog != null){
				inItemList[i].target.SetActive(tog.value);
				if(tog.value){
					index = i;
					inItemList[i].SeleteCopyScene();
				}
			}
		}
		if(table != null){
			table.repositionNow = true;
			if(index > 0){
//				UIScrollView view = table.transform.parent.GetComponent<UIScrollView>();
                if (view != null && inItemList.Count > 2)
                {
					view.ResetPosition();
                    float setVal = (float)(index+1) / inItemList.Count;  
                    //Debug.Log("index : " + index + "    , count : " + inItemList.Count + "   , value : " + (float)index / inItemList.Count);
                    view.SetDragAmount(1, setVal > 1 ? 1 : setVal, false); 
				}
//				SpringPanel sp = table.transform.parent.GetComponent<SpringPanel>();
//				if(sp != null)SpringPanel.Begin(view.gameObject,new Vector3(sp.target.x,sp.target.y + 158f,0),10);
			}
		}
	}
	
	void OnDestroy(){
        //foreach(CopyInItemUI data in inItemList){
        //    if(data != null){
        //        EventDelegate.Remove(data.GetComponent<UIToggle>().onChange,CopyInItemUITog);
        //    }
        //}
        for (int i = 0; i < inItemList.Count; i++)
        {
            CopyInItemUI data = inItemList[i];
            if (data != null)
                EventDelegate.Remove(data.GetComponent<UIToggle>().onChange, CopyInItemUITog);
        }
//		ConfigMng.Instance.UnloadBigUIIcon();
	}
}
