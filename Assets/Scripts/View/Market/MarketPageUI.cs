//=====================================
//作者:黄洪兴
//日期:2016/4/21
//用途:市场小分页UI
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 市场小分页UI组件
/// </summary>
public class MarketPageUI : MonoBehaviour
{
	#region 控件数据
	public int nums;
    public GameObject chooseSprite;
	public GameObject pageObj;
    public string pageID;
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
	//	protected SkillInfo oldSkillinfo;  //for upgrade effect -by ms
	#endregion
	// Use this for initialization
	void Start () {

        //if (GameCenter.marketMng.marketPageUI != null)
        //{
        //    Debug.Log("@@@@@@@" + GameCenter.marketMng.marketPageUI+":"+pageID);
        //    if (chooseSprite != null)
        //        chooseSprite.SetActive(GameCenter.marketMng.marketPageUI == pageID);
        //}
        //else
        //{
        //    chooseSprite.SetActive(false);
        //}


	}

    void OnEnable()
    {
        if (pageObj != null)
        {
            UIEventListener.Get(pageObj).onClick += ChooseMarketPage;
        }
    }

    void OnDisable()
    {
        if (pageObj != null)
        {
            UIEventListener.Get(pageObj).onClick -= ChooseMarketPage;
        }

    }





    void ChooseMarketPage(GameObject obj)
    {
        if (marketTypeinfo != null)
            GameCenter.marketMng.InitLimit(1, marketTypeinfo.ID);
        GameCenter.marketMng.InitLimit(2, nums);
        GameCenter.marketMng.marketPage = 1;
        GameCenter.marketMng.marketPageUI = pageID;
        GameCenter.marketMng.C2S_AskMarketItem(GameCenter.marketMng.marketPage);

    }


}
