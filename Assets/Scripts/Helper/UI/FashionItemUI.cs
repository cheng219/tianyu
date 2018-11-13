
//=====================================
//作者:黄洪兴
//日期:2016/03/19
//用途:时装组件
//========================================


using UnityEngine;
using System.Collections;
/// <summary>
/// 时装UI组件
/// </summary>
public class FashionItemUI : MonoBehaviour
{
	#region 控件数据
    /// <summary>
    /// 是否穿上的图片
    /// </summary>
    public UISprite isGetedSprite;
	/// <summary>
	/// 穿时装
	/// </summary>
	public GameObject getFashionBtn;
	public UILabel fashionName;
   // public UILabel fashionState;
	public UITimer timer;
	public UISpriteEx itemIcon;
    public ItemUI item;

    public GameObject choosedSprite;
    public GameObject chooseBtn;
    //public GameObject showFashionBtn;
	//public bool showTooltip=false;
	/// <summary>
	/// 当前时装对应的物品数据
	/// </summary>
	EquipmentInfo eq;
	int remaiTime;
	//bool  isSelectThis=false;
    bool isShowRemaiTime=false;
	#endregion 
	#region 数据
	/// <summary>
	/// 选择事件
	/// </summary>
	public System.Action<FashionItemUI> OnSelectEvent = null;
	/// <summary>
	/// 当前填充的数据
	/// </summary>
	public FashionInfo fashionInfo;
	//	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
	public FashionInfo CurFashionInfo
	{
		get { return fashionInfo; }
	}
	public static FashionInfo GetEqInfo(GameObject _obj)
	{
		FashionItemUI fashionItemUI = _obj.GetComponent<FashionItemUI>();
        if (fashionItemUI == null)
		{
			Debug.LogError("请在控件对象上加上<FashionItemUI>组件!!");
			return null;
		}
		return fashionItemUI.CurFashionInfo;
	}
	#endregion
	// Use this for initialization
	void Start () {


	}


    void OnEnable()
    {
        if (getFashionBtn != null)
            UIEventListener.Get(getFashionBtn).onClick += PutFashion;
        if (chooseBtn != null)
            UIEventListener.Get(chooseBtn).onClick += OnChoose;
        //if (showFashionBtn != null)
        //UIEventListener.Get(showFashionBtn).onClick += ShowToolTip;
    }

    void OnDisable()
    {
        if (getFashionBtn != null)
            UIEventListener.Get(getFashionBtn).onClick -= PutFashion;
        if (chooseBtn != null)
            UIEventListener.Get(chooseBtn).onClick -= OnChoose;
        //if (showFashionBtn!=null)
        //UIEventListener.Get(showFashionBtn).onClick -= ShowToolTip;
    }


	void OnChoose(GameObject obj)
	{

        GameCenter.fashionMng.CurTargetFashion = fashionInfo;
        if (GameCenter.fashionMng.OnChangeTargetFashion != null)
            GameCenter.fashionMng.OnChangeTargetFashion();
		//		SkillUpgradesWnd.curSkillUIinfo=this.skillinfo;
		//        if (OnSelectEvent != null)
		//        {
		//            OnSelectEvent(this);
		//        }
	}
	void PutFashion(GameObject obj)
	{
        if (fashionInfo != null && fashionInfo.IsOwn || isShowRemaiTime)
        {

            if (fashionInfo.FashionType == 2)
                {
                    if (GameCenter.fashionMng.InUseClothesFashion != null)
                    {
                        if (GameCenter.fashionMng.InUseClothesFashion.FashionID != fashionInfo.FashionID)
                        {
                            GameCenter.fashionMng.C2S_AskPutFashion(fashionInfo.FashionID, 1);
                        }
                        else
                        {
                            GameCenter.fashionMng.C2S_AskPutFashion(fashionInfo.FashionID, 0);
                        }

                    }
                    else
                    {
                        GameCenter.fashionMng.C2S_AskPutFashion(fashionInfo.FashionID, 1);
                    }
                }
              else  if (fashionInfo.FashionType == 1)
                {
                    if (GameCenter.fashionMng.InUseWeaponFashion != null)
                    {
                       if(GameCenter.fashionMng.InUseWeaponFashion.FashionID != fashionInfo.FashionID)
                       {
                           GameCenter.fashionMng.C2S_AskPutFashion (fashionInfo.FashionID, 1);
                       }
                        else{
                           GameCenter.fashionMng.C2S_AskPutFashion (fashionInfo.FashionID, 0);
                       }

                    }
                    else
                    {
                        GameCenter.fashionMng.C2S_AskPutFashion(fashionInfo.FashionID, 1);
                    }
                }

            GameCenter.fashionMng.CurTargetFashion = fashionInfo;
            //if (GameCenter.fashionMng.OnChangeTargetFashion != null)
            //    GameCenter.fashionMng.OnChangeTargetFashion();
		}
		//		SkillUpgradesWnd.curSkillUIinfo=this.skillinfo;
		//        if (OnSelectEvent != null)
		//        {
		//            OnSelectEvent(this);
		//        }
	}




	/// <summary>
	/// 填充数据
	/// </summary>
	/// <param name="_info"></param>
	public void FillInfo(FashionInfo _info)
	{
		if (_info == null) {
			fashionInfo = null;
		} 
		else {
			fashionInfo = _info;
			eq = new EquipmentInfo (fashionInfo.Item, EquipmentBelongTo.PREVIEW);
		}
			
		RefreshFashion ();
	}

    //void ShowToolTip()
    //{
    //    if (fashionInfo == null)
    //        return;
    //    if (fashionInfo.IsOwn)
    //    {

    //        if (eq != null)
    //        {
    //            if(fashionInfo.Time==0)
    //                ToolTipMng.ShowEquipmentTooltip(eq, ItemActionType.Flaunt, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);
    //        }

    //    }
    //    else
    //    {
			
    //        if (fashionInfo.Type == 1)
    //        {
    //            if (eq != null)
    //            {

    //                //GameCenter.buyMng.buyType = 1;
    //                //GameCenter.buyMng.id = fashionInfo.ShopID;
    //                //if(ConfigMng.Instance.GetMallRef (fashionInfo.ShopID)!=null)
    //                //GameCenter.buyMng.CurPrice = ConfigMng.Instance.GetMallRef (fashionInfo.ShopID).price;
    //                if (fashionInfo.RemainTime != null && fashionInfo.Time > 0)
    //                {
    //                    GameCenter.fashionMng.CurTargetFashion = fashionInfo;
    //                    ToolTipMng.ShowEquipmentTooltip(eq, ItemActionType.ToForever, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);
    //                } 
    //                else
    //                {

    //                    if (ConfigMng.Instance.MallItemByEquipmentID.ContainsKey(fashionInfo.Item))
    //                        GameCenter.newMallMng.CurMallType = (MallItemType)ConfigMng.Instance.MallItemByEquipmentID[fashionInfo.Item].type;
    //                    ToolTipMng.ShowEquipmentTooltip(eq, ItemActionType.MallBuy, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);
    //                }

    //            }

    //        }
    //        else
    //        {
    //            if (eq != null)
    //            {

    //                ToolTipMng.ShowEquipmentTooltip(eq, ItemActionType.None, ItemActionType.None, ItemActionType.None, ItemActionType.None, this.gameObject);

    //            }

    //        }

    //    }

		
    //}


	/// <summary>
	/// 刷新表现
	/// </summary>
	public void RefreshFashion()
	{
        isShowRemaiTime = false;
        if (fashionInfo != null)
        {
            if (fashionInfo.RemainTime != null)
            {
                int time = fashionInfo.RemainTime.remainTime - (int)Time.time + (int)fashionInfo.RemainTime.getTime;
                if (time > 0)
                {
                    if (timer != null)
                    {
                        timer.enabled = true;
                        timer.StartIntervalTimer(time);
                        isShowRemaiTime = true;
                    }
                }
                else
                {
                   if (timer != null) timer.enabled = false;
                    isShowRemaiTime = false;
                }

            }
            else
            {
                if (timer != null) timer.enabled = false;
                isShowRemaiTime = false;
            }

            if (item != null)
            {
                item.FillInfo(fashionInfo.ItemInfo); 
                //if (fashionInfo.IsOwn)
                //{ 
                //    if (eq != null)
                //    {
                //        if (fashionInfo.Time == 0)
                //            item.SetActionBtn(ItemActionType.None, ItemActionType.None, ItemActionType.Flaunt); 
                //    } 
                //}
                //else
                //{ 
                //    if (fashionInfo.Type == 1)
                //    {
                //        if (eq != null)
                //        { 
                //            if (fashionInfo.RemainTime != null && fashionInfo.Time > 0)
                //            {
                //                item.SetActionBtn(ItemActionType.None, ItemActionType.None, ItemActionType.ToForever);  
                //            } 
                //            else
                //            {
                //                item.SetActionBtn(ItemActionType.None, ItemActionType.None, ItemActionType.MallBuy);   
                //            } 
                //        } 
                //    } 
                //}
            }
            if (itemIcon != null)
            {
                //itemIcon.spriteName = (new EquipmentInfo(fashionInfo.Item, EquipmentBelongTo.PREVIEW)).IconName;
                itemIcon.IsGray = fashionInfo.IsOwn || isShowRemaiTime ? UISpriteEx.ColorGray.normal : UISpriteEx.ColorGray.Gray;
            }
            if (isGetedSprite != null)
            {
                isGetedSprite.gameObject.SetActive(false);
                if (fashionInfo.FashionType == 2)
                {
                    if (GameCenter.fashionMng.InUseClothesFashion != null)
                        isGetedSprite.gameObject.SetActive(GameCenter.fashionMng.InUseClothesFashion.FashionID == fashionInfo.FashionID);
                }
                if (fashionInfo.FashionType == 1)
                {
                    if (GameCenter.fashionMng.InUseWeaponFashion != null)
                        isGetedSprite.gameObject.SetActive(GameCenter.fashionMng.InUseWeaponFashion.FashionID == fashionInfo.FashionID);
                }
            }
            if (fashionName != null)
            {
                fashionName.text = fashionInfo.FashionName;
                if (fashionInfo.IsOwn || isShowRemaiTime)
                    fashionName.text = "[00FF00]"+fashionName.text;              
            }
            if (choosedSprite != null)
                choosedSprite.SetActive(fashionInfo.FashionID == GameCenter.fashionMng.CurTargetFashion.FashionID);
            //if (!isShowRemaiTime)
            //{
            //    if (fashionInfo.IsOwn)
            //    {
            //        if (fashionInfo.Time == 0)
            //        {
            //            fashionState.text = "可永久使用";
            //        }
            //    }
            //    else
            //    {
            //        if (fashionInfo.Type == 1)
            //        {
            //            fashionState.text = "点击图标可购买";
            //        }
            //        else
            //        {
            //            fashionState.text = "";
            //        }
            //    }
            //}
        }
        else
        {
            Debug.Log("时装数据为空！！！");
        }
	}
}
