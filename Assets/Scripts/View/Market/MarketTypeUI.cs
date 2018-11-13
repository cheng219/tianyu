//=====================================
//作者:黄洪兴
//日期:2016/4/19
//用途:市场大分页UI
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 市场大分页UI组件
/// </summary>
public class MarketTypeUI : MonoBehaviour
{
	#region 控件数据
	public UISprite pageSprite;
	/// <summary>
	/// 大分页名字
	/// </summary>
	public UILabel typeName;
	public GameObject typeObj;

    public GameObject pagesParent;
    /// <summary>
    /// 小分页预置
    /// </summary>
    public GameObject pagesInstantiate;
	#endregion 
	#region 数据
	/// <summary>
	/// 选择事件
	/// </summary>
	public System.Action<MarketTypeUI> OnSelectEvent = null;
	/// <summary>
	/// 当前填充的数据
	/// </summary>
	public MarketTypeInfo marketTypeinfo;



    public int typeID;
	//	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
	#endregion
	// Use this for initialization
	void Start () {

		if(typeObj!=null)
		{
			UIEventListener.Get (typeObj).onClick -= ChooseMarketType;
			UIEventListener.Get(typeObj).onClick += ChooseMarketType;
		}

	}

	void  ChooseMarketPage(GameObject obj)
	{
	}

	void ChooseMarketType(GameObject obj)
	{
        GameCenter.marketMng.marketPageUI = "";
		GameCenter.marketMng.InitLimit (1,marketTypeinfo.ID);
//		Debug.Log ("当前选择的是第"+marketTypeinfo.ID+"个大分页");
		GameCenter.marketMng.MarketTypes [marketTypeinfo.ID-1].ShowPage = !GameCenter.marketMng.MarketTypes [marketTypeinfo.ID-1].ShowPage;
		if (GameCenter.marketMng.OnChooseType != null)
            GameCenter.marketMng.OnChooseType(typeID, GameCenter.marketMng.MarketTypes[marketTypeinfo.ID - 1].ShowPage);
		if (GameCenter.marketMng.MarketTypes [marketTypeinfo.ID - 1].ShowPage) {
			GameCenter.marketMng.marketPage = 1;
			GameCenter.marketMng.C2S_AskMarketItem (GameCenter.marketMng.marketPage);
		}
			

	}


	/// <summary>
	/// 填充数据
	/// </summary>
	/// <param name="_info"></param>
	public void FillInfo(MarketTypeInfo _info)
	{
		if (_info == null) {
			marketTypeinfo = null;
			return;
		} 
		else {
			marketTypeinfo = _info;
		}
		RefreshMarketType ();
	}
	/// <summary>
	/// 刷新表现
	/// </summary>
	public void RefreshMarketType()
	{
		if (typeName != null)typeName.text = marketTypeinfo.Tabname;
		if (pageSprite != null)pageSprite.gameObject.SetActive (!marketTypeinfo.ShowPage);
        if (marketTypeinfo.ShowPage)
        {
            DestroyItem();
            Vector3 V = Vector3.zero;
            for (int i = 0; i < marketTypeinfo.Pagename.Count; i++)
            {
                GameObject obj = Instantiate(pagesInstantiate);
                if (obj != null)
                {
                    if (pagesParent != null)
                    obj.transform.parent = pagesParent.transform;
                    obj.transform.localPosition = V;
                    obj.transform.localScale = Vector3.one;
                    MarketPageUI ui = obj.GetComponent<MarketPageUI>();
                    if (ui != null)
                    {
                        ui.nums = i + 1;
                        obj.GetComponentInChildren<UILabel>().text = marketTypeinfo.Pagename[i];
                        ui.marketTypeinfo = marketTypeinfo;
                        ui.pageID = typeID.ToString() + (i + 1).ToString();
                    }
                    obj.SetActive(true);
                    V = new Vector3(V.x,V.y-53,V.z);
                }
            }
        }
        else
        {
            if (pagesParent != null)
                pagesParent.transform.DestroyChildren();

        }

	}


    void DestroyItem()
    {
        if (pagesParent != null)
        {
            pagesParent.transform.DestroyChildren();
        }
    }



	void RefreshMarketPage()
	{
	}




}
