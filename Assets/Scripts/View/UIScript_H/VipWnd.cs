/// <summary>
/// 何明军
/// 2016/4/19
/// VIP界面
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VipWnd : GUIBase {

	#region 数据
	#region UI控件
    public UILabel vLev;
	public UILabel vExp;
	public UISlider vExpSlider;
	public UILabel vDes;
	public UIButton btnToOther;
	public UIButton btnClose;
	
	public GameObject etyVipItem;
	
	public List<ItemUI> items;
	
	public UILabel vipLevDes;
	public UIScrollView uiView;
	public UILabel vipLev;
	public UIButton btnToRewerd;
	public UIButton btnHaveToRewerd;
	
	protected List<VipItemUI> vipList = new List<VipItemUI>();
	protected UIGrid uiGrid;
    public UISprite takeRewradRed;
    #endregion
    #region 字段
    protected FDictionary dataList;
    bool isStart = false;
    #endregion
	#endregion
    #region 访问器
    VIPDataInfo Data
    {
        get
        {
            return GameCenter.vipMng.VipData;
        }
    }

    VIPRef NeeData
    {
        get
        {
            int lev = Data.vLev + 1 > dataList.Count ?
                dataList.Count : Data.vLev + 1;
            return ConfigMng.Instance.GetVIPRef(lev);
        }
    }
    #endregion
    #region Unity函数
    void Awake(){
		
		mutualExclusion = true;
		Layer = GUIZLayer.NORMALWINDOW;
        dataList = ConfigMng.Instance.GetVIPRefTable();
        if(etyVipItem!=null)
        {
            uiGrid = etyVipItem.transform.parent.GetComponent<UIGrid>();
            etyVipItem.SetActive(false);
        }
		CreateVipLev();
	}
	void Start(){
		isStart = false;
		StartCoroutine(StartOver());
	}
    #endregion

	#region 协程刷新显示
	IEnumerator StartOver(){
		yield return new WaitForEndOfFrame();
		isStart = true;
		OnVipChange();
	}
	#endregion
	
	

	
	#region UI的填充与显示
    void CreateVipLev(){
		//UIToggle uiTog = null;
		VipItemUI vipItemUI = null; 
		dataList.Remove(0);
		vipList.Clear();
		uiGrid.maxPerLine = dataList.Count;
		foreach(VIPRef data in dataList.Values){
			GameObject go = (GameObject)GameObject.Instantiate(etyVipItem);
			go.transform.parent = etyVipItem.transform.parent;
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			
			vipItemUI = go.GetComponent<VipItemUI>();
			EventDelegate.Add(go.GetComponent<UIToggle>().onChange,OnVipChange);
			
			if(vipItemUI != null){
				vipItemUI.RefData = data;
				vipList.Add(vipItemUI);
			}
			go.SetActive(true);
		}
		int dataLev = GameCenter.vipMng.VipData.vLev - 1;
		if(vipList.Count > dataLev){
			vipList[dataLev < 0 ? 0:dataLev].GetComponent<UIToggle>().startsActive = true;//默认选择当前VIP等级 
		}else{
			if(vipList.Count > 0)vipList[0].GetComponent<UIToggle>().startsActive = true;
		}
		if(uiGrid != null)uiGrid.repositionNow = true;
		
	}
	
	void OnVipChange(){
		if(!isStart)return;
		VipItemUI refaui = null;
		for(int i=0;i< vipList.Count;i++){
			UIToggle tog = vipList[i].GetComponent<UIToggle>();
			if(tog.value){
				refaui = vipList[i];
				break;
			}
		}
		if(refaui == null)return ;
		string[] strList = new string[1]{refaui.RefData.id.ToString()};
		vipLevDes.text = refaui.RefData.show;
		if(uiView !=null)uiView.ResetPosition();
		vipLev.text = ConfigMng.Instance.GetUItext(2,strList);
		
		UIEventListener.Get(btnToRewerd.gameObject).parameter = refaui.RefData.id;
//		btnToRewerd.isEnabled = refaui.RefData.id <= Data.vLev;
		btnToRewerd.gameObject.SetActive(!Data.vipReward.Contains(refaui.RefData.id));
        if (NeeData != null) takeRewradRed.gameObject.SetActive(GameCenter.mainPlayerMng.MainPlayerInfo.VIPExp >= refaui.RefData.exp && !Data.vipReward.Contains(refaui.RefData.id));
		btnHaveToRewerd.gameObject.SetActive(Data.vipReward.Contains(refaui.RefData.id));
		
		for(int i=0,len=items.Count;i<len;i++){
			if(i < refaui.RefData.items.Count && refaui.RefData.items[i].eid > 0){
				items[i].FillInfo(new EquipmentInfo(refaui.RefData.items[i].eid,refaui.RefData.items[i].count,EquipmentBelongTo.PREVIEW));
				items[i].gameObject.SetActive(true);
			}else{
				items[i].gameObject.SetActive(false);
			}
		}

        for (int i = 0, max = vipList.Count; i < max; i++)
        {
            if (vipList[i].redMind != null) 
                vipList[i].redMind.gameObject.SetActive(GameCenter.mainPlayerMng.MainPlayerInfo.VIPExp >= vipList[i].RefData.exp && !Data.vipReward.Contains(vipList[i].RefData.id));
        }
	}
	
	void ShowMyVip(){
        if(vLev!=null)
		    vLev.text = Data.vLev.ToString();
		if(Data.IsFullLevel){
			vDes.text = ConfigMng.Instance.GetUItext(88);
			vExp.enabled = false;
			vExpSlider.value = 1;
		}else{
            if(vExp!=null)
			vExp.text = GameCenter.mainPlayerMng.MainPlayerInfo.VIPExp +"/"+NeeData.exp;
            if(vExpSlider!=null)
			vExpSlider.value = (float)Data.vExp / (float)NeeData.exp;
			string[] strList = new string[2]{(NeeData.exp - Data.vExp)/10+"",NeeData.id.ToString()};
            if(vDes!=null)
			vDes.text = ConfigMng.Instance.GetUItext(1,strList);
		}
		OnVipChange();
	}
    #endregion
    #region 事件句柄
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if(btnClose!=null)
            UIEventListener.Get(btnClose.gameObject).onClick += CloseWnd;
            if(btnToOther!=null)
            UIEventListener.Get(btnToOther.gameObject).onClick += OpenCharge;
            if(btnToRewerd!=null)
            UIEventListener.Get(btnToRewerd.gameObject).onClick += GetReward;
        }
        else
        {
            if (btnClose != null)
                UIEventListener.Get(btnClose.gameObject).onClick -= CloseWnd;
            if (btnToOther != null)
                UIEventListener.Get(btnToOther.gameObject).onClick -= OpenCharge;
            if (btnToRewerd != null)
                UIEventListener.Get(btnToRewerd.gameObject).onClick -= GetReward;
        }
    }
    #endregion
    #region 控件事件的响应
    /// <summary>
    /// 关闭界面
    /// </summary>
    void CloseWnd(GameObject _obj)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }
    /// <summary>
    /// 跳转到充值界面
    /// </summary>
    void OpenCharge(GameObject _obj)
    {
        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
    }
    /// <summary>
    /// 领取奖励
    /// </summary>
    void GetReward(GameObject _obj)
    {
        if (GameCenter.vipMng.VipData.vLev <= 0)
        {
            GameCenter.messageMng.AddClientMsg(289);
            return;
        }
        int id = (int)UIEventListener.Get(btnToRewerd.gameObject).parameter;
        if (GameCenter.vipMng.VipData.vLev < id)
        {
            GameCenter.messageMng.AddClientMsg(154);
            return;
        }
        GameCenter.vipMng.C2S_VIPRewarde(id);
    }
    #endregion

    #region OnOpen与OnClose
    protected override void OnOpen ()
	{
		base.OnOpen ();
		
		ShowMyVip();
		GameCenter.vipMng.OnVIPDataUpdate += ShowMyVip;
		
		if(uiGrid == null)return ;
		if(GameCenter.vipMng.VipData.vLev >= 7){
			uiGrid.Reposition();
			UIScrollView view = uiGrid.transform.parent.GetComponent<UIScrollView>();
			if(view != null)view.SetDragAmount(0f,1f,false);
		}else{
			uiGrid.Reposition();
			UIScrollView view = uiGrid.transform.parent.GetComponent<UIScrollView>();
			if(view != null)view.SetDragAmount(0f,0f,false);
		}
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
		GameCenter.vipMng.OnVIPDataUpdate -= ShowMyVip;
	}
	#endregion
	
	#region 注销与销毁
    void OnDisable(){
		GameCenter.vipMng.OnVIPDataUpdate -= ShowMyVip;
	}
	void OnDestroy(){
		if(vipList.Count > 0){
			for(int i=0;i< vipList.Count;i++){
				UIToggle tog = vipList[i].GetComponent<UIToggle>();
				if(tog!= null)EventDelegate.Remove(tog.onChange,OnVipChange);
			}
		}
	}
#endregion
}
