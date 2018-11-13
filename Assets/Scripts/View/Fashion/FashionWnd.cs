//==================================
//作者：黄洪兴
//日期：2016/3/15
//用途：时装外加称号窗口类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FashionWnd : SubWnd
{

	#region 数据
    public UIToggle[] toggles = new UIToggle[3];
    public UILabel allAttTitle;
    public UILabel allAttNum;
	public UISprite titleName;
	public UITexture playerPreview;
	public UIButton backBtn;
	public UILabel attributeLabel;
	/// <summary>
	/// 称号增加属性
	/// </summary>
	public UILabel titleAttribute;
	/// <summary>
	/// 称号来源说明
	/// </summary>
	public UILabel titleSource;
	/// <summary>
	/// 所有时装属性加成界面
	/// </summary>
	public GameObject infoObj;
	public TitleUIContainer titleContainer;
	public FashionUIContainer fashionContainer;
	//public List<UIToggle> toggle=new List<UIToggle>();
	public UIButton showTitleInfoBt;
    public UIButton showFashionInfoBtn;
	/// <summary>s
	/// 战斗力
	/// </summary>
	//public UILabel powNum;

    public GameObject fashionObj;
    public GameObject titleObj;
    public GameObject clothesFashionTitle;
    public GameObject weaponFashionTitle;
    public GameObject foreverClothesFashion;
    public GameObject foreverWeaponFashion;
    public GameObject limitClothesFashion;
    public GameObject limitWeaponFashion;
    public UILabel fashionDes;
    public GameObject clothesFashionGet;
    public GameObject weaponFashionGet;
    public UILabel getFashion;
    public GameObject goGetFashionBtn;

    public UISlider fashionPro;
    public UILabel fashionLev;
    public UILabel fashionExp;
    public UIButton chackFashionLevAttr;
    public GameObject levAttrObj;
    public UIButton closeLevAttrObjBtn;
    public FashionAttrUi leftAttrUi;
    public FashionAttrUi rightAttrUi;


	private List<FashionInfo> fashionItemList = new List<FashionInfo>();
	private ItemUI[] uiList;
	//private PlayerBaseInfo curTargetInfo;
    private Dictionary<string, int> allAttDic = new Dictionary<string, int>();
    private Dictionary<EquipSlot, EquipmentInfo> showDic = new Dictionary<EquipSlot, EquipmentInfo>();

	#endregion

	#region 构造

	void Awake()
	{
        type = SubGUIType.SUBFASHION;
        for (int i = 0; i < toggles.Length; i++)
        {
            //EventDelegate.Add(toggles[i].onChange, OpenWndByType);
            if (toggles[i] != null) UIEventListener.Get(toggles[i].gameObject).onClick = ClickToggleEvent;
        }
        if (chackFashionLevAttr != null) UIEventListener.Get(chackFashionLevAttr.gameObject).onClick = OnClickFashionLevAttr;
        if (closeLevAttrObjBtn != null) UIEventListener.Get(closeLevAttrObjBtn.gameObject).onClick = CloseLevAttrObjBtn;
		//curTargetInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
        //if(GameCenter.fashionMng.fashionDic!=null&&GameCenter.fashionMng.clothesFashionDic!=null&&GameCenter.fashionMng.weaponFashionDic!=null)
        //{
        //    RefreshFashionContainer(GameCenter.fashionMng.CurFashionWndType);			
        //}
	}
    protected UIToggle lastChangeToggle = null;
    protected void ClickToggleEvent(GameObject go)
    {
        UIToggle toggle = go.GetComponent<UIToggle>();
        if (toggle != lastChangeToggle)
        {
            OpenWndByType();
        }
        if (toggle != null && toggle.value) lastChangeToggle = toggle;
    }
	protected override void OnOpen()
	{
		base.OnOpen();
        toggles[0].value = false;
        toggles[(int)GameCenter.fashionMng.CurFashionWndType].value = true;
        //Debug.Log("当前打开的是" + GameCenter.fashionMng.CurFashionWndType);
		ToolTipMng.ShowEquipmentModel = false;
        if (infoObj != null)
            infoObj.SetActive(false);
        if (backBtn != null)
            backBtn.gameObject.SetActive(false);
        if (showTitleInfoBt != null) UIEventListener.Get(showTitleInfoBt.gameObject).onClick += ShowInfo;
        if (showFashionInfoBtn != null) UIEventListener.Get(showFashionInfoBtn.gameObject).onClick += ShowInfo;
        if (backBtn != null) UIEventListener.Get(backBtn.gameObject).onClick += CloseInfo;
        GameCenter.fashionMng.GetAllFashion();
		GameCenter.fashionMng.C2S_AskFashionDic ();
        showDic = new Dictionary<EquipSlot, EquipmentInfo>(GameCenter.mainPlayerMng.MainPlayerInfo.CurShowDictionary);
		RefreshTexture ();
        RefreshTitleName();
        OpenWndByType();

        //RefreshFashion();
	}
	protected override void OnClose()
	{
		base.OnClose();
        //ToolTipMng.ShowEquipmentModel = true;
        //GameCenter.titleMng.ChooseTitle = null;
        //if (showInfoBt != null) UIEventListener.Get(showInfoBt.gameObject).onClick -= ShowInfo;
        //if (backBtn != null) UIEventListener.Get(backBtn.gameObject).onClick -= CloseInfo;
        //GameCenter.fashionMng.CurFashionWndType = FashionWndType.CLOTHES;
        //GameCenter.previewManager.ClearModel ();
        //lastChangeToggle = null;
	}
    void OnDestroy()
    {
        GameCenter.fashionMng.OnUpdateFashion -= RefreshFashion;
        GameCenter.fashionMng.OnUpdateFashionList -= RefreshFashion;
        GameCenter.titleMng.UpdateTitle -= RefreshTitle;
        GameCenter.fashionMng.OnChangeTargetFashion -= RefrehFashionDes;
        GameCenter.titleMng.UpDateTargetTitle -= RefreshTitleName;
        GameCenter.fashionMng.OnUpdateFashionLev -= RefreshFashionLev;
        ToolTipMng.ShowEquipmentModel = true;
        GameCenter.titleMng.ChooseTitle = null;
        if (showTitleInfoBt != null) UIEventListener.Get(showTitleInfoBt.gameObject).onClick -= ShowInfo;
        if (showFashionInfoBtn != null) UIEventListener.Get(showFashionInfoBtn.gameObject).onClick -= ShowInfo;
        if (backBtn != null) UIEventListener.Get(backBtn.gameObject).onClick -= CloseInfo;
        GameCenter.fashionMng.CurFashionWndType = FashionWndType.CLOTHES;
        GameCenter.previewManager.ClearModel();
        GameCenter.fashionMng.SetCurFashionInfo();
        lastChangeToggle = null;

    }

	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
            GameCenter.fashionMng.OnUpdateFashion += RefreshFashion;
            GameCenter.fashionMng.OnUpdateFashionList += RefreshFashion;
            GameCenter.titleMng.UpdateTitle += RefreshTitle;
            GameCenter.fashionMng.OnChangeTargetFashion += RefrehFashionDes;
            GameCenter.titleMng.UpDateTargetTitle += RefreshTitleName;
            GameCenter.fashionMng.OnUpdateFashionLev += RefreshFashionLev;
			//GameCenter.curMainPlayer.actorInfo.OnBaseUpdate += RefreshTexture;
		}
		else
		{
            OnDestroy();
            //GameCenter.fashionMng.OnUpdateFashion -= RefreshFashion;
            //GameCenter.fashionMng.OnUpdateFashionList -= RefreshFashion;
            //GameCenter.titleMng.UpdateTitle -= RefreshTitle;
            //GameCenter.fashionMng.OnChangeTargetFashion -= RefrehFashionDes;
            //GameCenter.titleMng.UpDateTargetTitle -= RefreshTitleName;
			//GameCenter.curMainPlayer.actorInfo.OnBaseUpdate -= RefreshTexture;
		}
	}
	#endregion
	void RefreshFashionContainer(FashionWndType _type)
	{
        if (_type == FashionWndType.CLOTHES)
        {
            if (fashionContainer != null)
            {
            fashionContainer.transform.DestroyChildren();
            fashionContainer.RefreshItems(GameCenter.fashionMng.clothesFashionList);
            }
        }
        else if (_type == FashionWndType.WEAPON)
        {
            if (fashionContainer != null)
            {
                fashionContainer.transform.DestroyChildren();
                fashionContainer.RefreshItems(GameCenter.fashionMng.weaponFashionList);
            }
        }

       GameCenter.fashionMng.SetCurFashionInfo();
	}


	void OnSelect(bool selected)
	{
		if(selected&&infoObj.activeSelf)
		{
			infoObj.SetActive (false);
			
		}

	}

    void OpenWndByType()
    {
        FashionWndType _type = GameCenter.fashionMng.CurFashionWndType;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].value)
            {
                _type = (FashionWndType)(i);
                break;
            }
        }
        GameCenter.fashionMng.CurFashionWndType = _type;
        GameCenter.fashionMng.SetCurFashionInfo();
       // RefreshItems(_type);
        switch (_type)
        {
            case FashionWndType.CLOTHES: 
                if (fashionObj != null) fashionObj.SetActive(true); 
                if(titleObj!=null)titleObj.SetActive(false);
                RefreshFashion();
                break;
            case FashionWndType.WEAPON:
                if (fashionObj != null) fashionObj.SetActive(true); 
                if(titleObj!=null)titleObj.SetActive(false);
                RefreshFashion();                
                break;
            case FashionWndType.TITLE: 
                if (fashionObj != null) fashionObj.SetActive(false); 
                if(titleObj!=null)titleObj.SetActive(true);
                RefreshTitle();                
                break;

        }

    }




	void CloseInfo(GameObject go)
	{
		infoObj.SetActive (false);
		go.SetActive (false);

	}
	/// <summary>
	/// 刷新时装预览
	/// </summary>
	public  void RefreshTexture()
	{ 
        GameCenter.previewManager.ClearModel();
        if (GameCenter.mainPlayerMng.MainPlayerInfo != null && playerPreview != null)
		{
            //Debug.Log("进入刷新时装！" + GameCenter.fashionMng.CurTargetFashion.FashionID);
            //showDic[GameCenter.fashionMng.CurTargetFashion.ItemInfo.Slot] = GameCenter.fashionMng.CurTargetFashion.ItemInfo;
            //Dictionary<EquipSlot, EquipmentInfo> showDic = new Dictionary<EquipSlot, EquipmentInfo>(GameCenter.mainPlayerMng.MainPlayerInfo.CurShowDictionary);
            showDic[GameCenter.fashionMng.CurTargetFashion.ItemInfo.Slot] = GameCenter.fashionMng.CurTargetFashion.ItemInfo;
			GameCenter.previewManager.TryPreviewSinglePlayer(GameCenter.mainPlayerMng.MainPlayerInfo, playerPreview, showDic,ActorAnimFSM.EventType.Movie);
            if (playerPreview != null)
                playerPreview.gameObject.SetActive(true);
		}
	}
    /// <summary>
    /// 刷新时装等级
    /// </summary>
    void RefreshFashionLev()
    { 
        int lev = GameCenter.fashionMng.fashionLev;
        int count = ConfigMng.Instance.GetFashionLevelRefTable().Count - 1;
        if (lev >= count)
        { 
            FashionLevelRef fashionLevelRef = ConfigMng.Instance.GetFashionLevelRef(count);
            fashionPro.value = 1;
            fashionLev.text = "LV." + lev.ToString();
            fashionExp.text = GameCenter.fashionMng.fashionExp + " / " + GameCenter.fashionMng.fashionExp;
            if (levAttrObj != null) levAttrObj.gameObject.SetActive(false);
            leftAttrUi.SetContent(fashionLevelRef);
            rightAttrUi.SetContent(fashionLevelRef, fashionLevelRef);
            return;
        }
        else
        {
            FashionLevelRef fashionLevelRef = ConfigMng.Instance.GetFashionLevelRef(lev);
            FashionLevelRef nextFashionLevelRef = ConfigMng.Instance.GetFashionLevelRef(lev + 1);
            if (nextFashionLevelRef == null)
            {
                nextFashionLevelRef = fashionLevelRef;
            }
            if (fashionLevelRef != null)
            {
                fashionPro.value = (float)GameCenter.fashionMng.fashionExp / fashionLevelRef.exp;
                fashionLev.text = "LV." + lev.ToString();
                fashionExp.text = GameCenter.fashionMng.fashionExp + " / " + fashionLevelRef.exp;
                if (levAttrObj != null) levAttrObj.gameObject.SetActive(false);
                leftAttrUi.SetContent(fashionLevelRef);
                rightAttrUi.SetContent(fashionLevelRef, nextFashionLevelRef);
            }
        }
    }
    /// <summary>
    /// 关闭等级属性ui
    /// </summary>
    void CloseLevAttrObjBtn(GameObject go)
    {
        if (levAttrObj != null)
            levAttrObj.SetActive(false);
    }
    /// <summary>
    /// 显示时装等级属性
    /// </summary>
    /// <param name="go"></param>
    void OnClickFashionLevAttr(GameObject go)
    {
        if (levAttrObj != null)
        {
            levAttrObj.SetActive(true);
        }
    }
	/// <summary>
	/// 显示时装属性加成
	/// </summary>
	/// <param name="go">Go.</param>
	void ShowInfo(GameObject go)
    {
        int a = 0;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].value)
            {
                a = i;
                i = toggles.Length;
            }
        }
        if (a == 0||a==1)
        {
            if (allAttTitle != null)
                allAttTitle.text = ConfigMng.Instance.GetUItext(276);
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            allAttDic.Clear();
            for (int j = 0; j < GameCenter.fashionMng.AllAttrNum.Count; j++)
            {
                string n = ConfigMng.Instance.GetAttributeTypeName(GameCenter.fashionMng.AllAttrNum[j].tag);
                if (!allAttDic.ContainsKey(n))
                {
                    allAttDic[n] = GameCenter.fashionMng.AllAttrNum[j].value;
                }
                else
                {
                    allAttDic[n] = allAttDic[n] + GameCenter.fashionMng.AllAttrNum[j].value;
                }
            }
            using (var e = allAttDic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    builder.Append(e.Current.Key + ":" + e.Current.Value + "\n");
                }
            }
            if (allAttNum != null)
                allAttNum.text = builder.ToString();
        }
        if (a == 2)
        {
            if (allAttTitle != null)
                allAttTitle.text = ConfigMng.Instance.GetUItext(276);
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            allAttDic.Clear();


            for (int j = 0; j < GameCenter.titleMng.AllAttrNum.Count; j++)
            {
                string n = ConfigMng.Instance.GetAttributeTypeName(GameCenter.titleMng.AllAttrNum[j].tag);
                if (!allAttDic.ContainsKey(n))
                {
                    allAttDic[n] = GameCenter.titleMng.AllAttrNum[j].value;
                }
                else
                {
                    allAttDic[n] = allAttDic[n] + GameCenter.titleMng.AllAttrNum[j].value;
                }
            }
            using (var e = allAttDic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    builder.Append(e.Current.Key + ":" + e.Current.Value + "\n");
                }
            }
            if (allAttNum != null)
                allAttNum.text = builder.ToString();
        }

		infoObj.SetActive (true);
		backBtn.gameObject.SetActive (true);
	}

    void GoMallWnd(GameObject go)
    {
        if(GameCenter.fashionMng.CurTargetFashion.ShopType==2)
			GameCenter.newMallMng.OpenWndByType(MallItemType.FASHION);
        if (GameCenter.fashionMng.CurTargetFashion.ShopType == 1)
			GameCenter.newMallMng.OpenWndByType(MallItemType.RESTRICTION);
    }
    void ToForever(GameObject go)
    {
        GameCenter.fashionMng.ToFover();
    }


	void RefeshAll()
	{ 
		RefreshFashion ();
		RefreshTexture ();
	}


    void RefrehFashionDes()
    {
       // Debug.Log("刷新说明！！！！");
        if (GameCenter.fashionMng.CurTargetFashion == null)
            return;
        if (clothesFashionTitle != null)
            clothesFashionTitle.SetActive(GameCenter.fashionMng.CurTargetFashion.FashionType!=1);
        if (weaponFashionTitle != null)
            weaponFashionTitle.SetActive(GameCenter.fashionMng.CurTargetFashion.FashionType == 1);
        if (clothesFashionGet != null)
            clothesFashionGet.SetActive(GameCenter.fashionMng.CurTargetFashion.FashionType != 1);
        if (weaponFashionGet != null)
            weaponFashionGet.SetActive(GameCenter.fashionMng.CurTargetFashion.FashionType == 1);
        if (foreverClothesFashion != null)
            foreverClothesFashion.SetActive(GameCenter.fashionMng.CurTargetFashion.FashionType != 1 && GameCenter.fashionMng.CurTargetFashion.Time==0);
        if (foreverWeaponFashion != null)
            foreverWeaponFashion.SetActive(GameCenter.fashionMng.CurTargetFashion.FashionType == 1 && GameCenter.fashionMng.CurTargetFashion.Time == 0);
        if (limitClothesFashion != null)
            limitClothesFashion.SetActive(GameCenter.fashionMng.CurTargetFashion.FashionType != 1 && GameCenter.fashionMng.CurTargetFashion.Time != 0);
        if (limitWeaponFashion != null)
            limitWeaponFashion.SetActive(GameCenter.fashionMng.CurTargetFashion.FashionType == 1 && GameCenter.fashionMng.CurTargetFashion.Time != 0);
        if (fashionDes != null)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            //for (int i = 0; i < GameCenter.fashionMng.CurTargetFashion.Attribute.Count; i++)
            //{
            //    string n = ConfigMng.Instance.GetAttributeTypeName((ActorPropertyTag)GameCenter.fashionMng.CurTargetFashion.Attribute[i].eid);
            //    builder.Append(n + ":" + GameCenter.fashionMng.CurTargetFashion.Attribute[i].count + "\n");                
            //}
            EquipmentInfo info = GameCenter.fashionMng.CurTargetFashion.ItemInfo;

            builder.Append(info.Description.Replace("\\n", "\n")).Append("\n");  

            fashionDes.text = builder.ToString();
        }
        if (GameCenter.fashionMng.CurTargetFashion.IsOwn)
        {
                if (getFashion != null)
                    getFashion.text = ConfigMng.Instance.GetUItext(102);
                if (goGetFashionBtn != null)
                {
                    BoxCollider box = goGetFashionBtn.transform.GetComponent<BoxCollider>();
                    if (box != null)
                        box.enabled = false;
                }
        }
        else
        {
            if (GameCenter.fashionMng.CurTargetFashion.Type == 1)
            {
                if (getFashion != null)
                    getFashion.text = ConfigMng.Instance.GetUItext(100);
                if (goGetFashionBtn != null)
                {
                    UIEventListener.Get(goGetFashionBtn.gameObject).onClick = GoMallWnd;
                    BoxCollider box = goGetFashionBtn.transform.GetComponent<BoxCollider>();
                    if (box != null)
                        box.enabled = true;
                }
            }
            else
            {
                if (getFashion != null)
                    getFashion.text = ConfigMng.Instance.GetUItext(103);
                if (goGetFashionBtn != null)
                {
                    BoxCollider box = goGetFashionBtn.transform.GetComponent<BoxCollider>();
                    if (box != null)
                        box.enabled = false;
                }
            }
        }
        if (GameCenter.fashionMng.CurTargetFashion.RemainTime != null)
        {
            if (getFashion != null)
                getFashion.text = ConfigMng.Instance.GetUItext(101);
            if (goGetFashionBtn != null)
            {
                UIEventListener.Get(goGetFashionBtn.gameObject).onClick = ToForever;
                BoxCollider box = goGetFashionBtn.transform.GetComponent<BoxCollider>();
                if (box != null)
                    box.enabled = true;
            }
        }

        RefreshTexture(); 




    }

	/// <summary>
	/// 刷新时装界面信息
	/// </summary>
	void RefreshFashion()
	{
        RefreshFashionLev();
		//RefreshPowNum ();
        RefrehFashionDes();
		if (GameCenter.fashionMng.fashionDic != null) {
			fashionItemList.Clear ();
            using (var e = GameCenter.fashionMng.fashionDic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    fashionItemList.Add(e.Current.Value);
                }
            }
		}
        //Debug.Log("当前需要打开的时装类型为" + GameCenter.fashionMng.CurFashionWndType);
        RefreshFashionContainer(GameCenter.fashionMng.CurFashionWndType);
		//Debug.Log ("此时的长度为"+fashionItemList.Count);

	}

	//void RefreshPowNum()
	//{
	//	if (powNum != null)
	//		powNum.text = GameCenter.mainPlayerMng.MainPlayerInfo.CurFightVal.ToString ();
	//}

	/// <summary>
	/// 刷新称号界面信息
	/// </summary>
	void RefreshTitle()
	{
		//RefreshPowNum ();
		if (GameCenter.titleMng.titleList != null&&GameCenter.titleMng.titleList.Count>0) {

			titleContainer.RefreshItems (GameCenter.titleMng.titleList);;
		}
		for (int i = 0; i <titleContainer.TitleContainers.Count; i++) {
			titleContainer.TitleContainers [i].RefreshTitle ();
			if (i == 0) {
				if (GameCenter.titleMng.ChooseTitle != null) {
					titleAttribute.text = GameCenter.titleMng.ChooseTitle.Des;
					titleSource.text = GameCenter.titleMng.ChooseTitle.WayDes;

				} else {
					titleAttribute.text =titleContainer.TitleContainers [i].titleinfo.Des;
					titleSource.text = titleContainer.TitleContainers [i].titleinfo.WayDes;
				}
			}
		}
        //if(titleName != null){
        //    if (GameCenter.titleMng.TargetTitle == null)
        //        GameCenter.titleMng.TargetTitle = GameCenter.titleMng.CurUseTitle;
        //    titleName.gameObject.SetActive(GameCenter.titleMng.TargetTitle != null);
        //    if (titleName.gameObject.activeSelf) {
        //        titleName.spriteName = GameCenter.titleMng.TargetTitle.IconName;
        //    }
        //}
	}

    void RefreshTitleName()
    {
        if (titleName != null)
        {
            if (GameCenter.titleMng.ChooseTitle == null)
                GameCenter.titleMng.ChooseTitle = GameCenter.titleMng.CurUseTitle;
            titleName.gameObject.SetActive(GameCenter.titleMng.ChooseTitle != null);
            if (titleName.gameObject.activeSelf)
            {
                titleName.spriteName = GameCenter.titleMng.ChooseTitle.IconName;
                titleName.MakePixelPerfect();
            }
        }
    }








		
}
