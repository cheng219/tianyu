//=====================================
//作者:黄洪兴
//日期:2016/4/21
//用途:市场货币方式分页UI
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 市场货币方式UI
/// </summary>
public class MarketMoneyTypeUI : MonoBehaviour
{
	#region 控件数据
	public int nums;

	public GameObject moneyTypeObj;
	public GameObject backObj;
	#endregion 
	// Use this for initialization
	void Start () {

		if(moneyTypeObj!=null)
		{
			UIEventListener.Get (moneyTypeObj).onClick -= ChooseMarketLev;
			UIEventListener.Get(moneyTypeObj).onClick += ChooseMarketLev;
		}

	}

	void  ChooseMarketLev(GameObject obj)
	{
		GameCenter.marketMng.InitLimit (6, nums);
		if (backObj != null)
			backObj.SetActive (false);
//		Debug.Log ("当前选择的货币种类分页的第"+nums+"级");
		gameObject.transform.parent.gameObject.SetActive (false);
		GameCenter.marketMng.marketPage = 1;
        GameCenter.marketMng.C2S_AskMarketItem(GameCenter.marketMng.marketPage);
	}




}
