//=====================================
//作者:黄洪兴
//日期:2016/4/21
//用途:市场等级分页UI
//========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 市场等级分页UI
/// </summary>
public class MarketLevUI : MonoBehaviour
{
	#region 控件数据
	public int nums;

	public GameObject levObj;
	public GameObject backObj;
	#endregion 
	// Use this for initialization
	void Start () {

		if(levObj!=null)
		{
			UIEventListener.Get (levObj).onClick -= ChooseMarketLev;
			UIEventListener.Get(levObj).onClick += ChooseMarketLev;
		}

	}

	void  ChooseMarketLev(GameObject obj)
	{
		GameCenter.marketMng.InitLimit (3, nums);
		if (backObj != null)
			backObj.SetActive (false);
//		Debug.Log ("当前选择的是等级分页的第"+nums+"级");
		gameObject.transform.parent.gameObject.SetActive (false);
		GameCenter.marketMng.marketPage = 1;
        GameCenter.marketMng.C2S_AskMarketItem(GameCenter.marketMng.marketPage);

	}




}
