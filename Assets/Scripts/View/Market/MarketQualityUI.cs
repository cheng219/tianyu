//=====================================
//作者:黄洪兴
//日期:2016/4/21
//用途:市场品质分页UI
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 市场品质分页UI
/// </summary>
public class MarketQualityUI : MonoBehaviour
{
	#region 控件数据
	public int nums;

	public GameObject qualityObj;
	public GameObject backObj;
	#endregion 
	// Use this for initialization
	void Start () {

		if(qualityObj!=null)
		{
			UIEventListener.Get (qualityObj).onClick -= ChooseMarketLev;
			UIEventListener.Get(qualityObj).onClick += ChooseMarketLev;
		}

	}

	void  ChooseMarketLev(GameObject obj)
	{
		GameCenter.marketMng.InitLimit (4, nums);
		if (backObj != null)
			backObj.SetActive (false);
//		Debug.Log ("当前选择的是品质分页的第"+nums+"级");
		gameObject.transform.parent.gameObject.SetActive (false);
		GameCenter.marketMng.marketPage = 1;
        GameCenter.marketMng.C2S_AskMarketItem(GameCenter.marketMng.marketPage);
	}




}
