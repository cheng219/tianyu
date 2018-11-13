//=====================================
//作者:黄洪兴
//日期:2016/4/21
//用途:市场价格分页UI
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 市场价格分页UI
/// </summary>
public class MarketPriceUI : MonoBehaviour
{
	#region 控件数据
	public int nums;

	public GameObject priceObj;
	public GameObject backObj;
	#endregion 
	// Use this for initialization
	void Start () {

		if(priceObj!=null)
		{
			UIEventListener.Get (priceObj).onClick -= ChooseMarketLev;
			UIEventListener.Get(priceObj).onClick += ChooseMarketLev;
		}

	}

	void  ChooseMarketLev(GameObject obj)
	{
		GameCenter.marketMng.InitLimit (5, nums);
		if (backObj != null)
			backObj.SetActive (false);
//		Debug.Log ("当前选择的是价格分页的第"+nums+"级");
		gameObject.transform.parent.gameObject.SetActive (false);
		GameCenter.marketMng.marketPage = 1;
        GameCenter.marketMng.C2S_AskMarketItem(GameCenter.marketMng.marketPage);
	}




}
