//======================================================
//作者:何明军
//日期:2016/7/6
//用途:功能父按钮与子按钮关系,只支持到功能分页级红点,子>>父.挂在功能按钮上
//======================================================
using UnityEngine;
using System.Collections;

public class FunctionButtonUI : MonoBehaviour {

	/// <summary>
	/// 多个不同的功能红点控制同一个父红点,用组的形式表达
	/// 组ID,主界面用到
	/// </summary>
	public int group = 0;
	public GameObject parent;
	public GameObject parentRed;
	/// <summary>
	/// 具体功能界面
	/// </summary>
	public FunctionType func;
	public GameObject funcRed;
	
	public ShowType showType = ShowType.IsGray;
	
	public enum ShowType{
		None = 0,
		/// <summary>
		/// 灰色显示
		/// </summary>
		IsGray,
		/// <summary>
		/// 隐藏
		/// </summary>
		IsHide,
	}
	string tipDes;
	public string TipDes{
		set{
			tipDes = value;
		}
	}
	
	public UISprite sp;
//	[System.NonSerialized]UISpriteEx cover;
	BoxCollider box;
	
	bool show = true;
	public bool SetShow{
		set{
			if(show != value){
				show = value;
			}
			if(showType == ShowType.IsGray){
				CreateCover();
			}else if(showType == ShowType.IsHide){
				gameObject.SetActive(show);
			}else{
				gameObject.SetActive(true);
			}
		}
	}
	
	void OnEnable(){
		if(sp != null){
			sp.gameObject.SetActive(!show);
		}
	}
	
	void CreateCover(){
//		if(sp == null)sp = gameObject.transform.GetChild(0).GetComponent<UISpriteEx>();
		if(sp != null){
			sp.gameObject.SetActive(!show);
			UIEventListener.Get(sp.gameObject).onClick = OnShowTip;
//			return ;
		}
//		if(cover == null && sp != null)cover = NGUITools.AddChild(gameObject,sp.gameObject).GetComponentInChildren<UISpriteEx>();
//		
//		if(cover != null){
//			cover.IsGray = show ? UISpriteEx.ColorGray.normal:UISpriteEx.ColorGray.Gray;
//			cover.transform.localPosition = sp.transform.localPosition;
//			cover.transform.localRotation = sp.transform.localRotation;
//			cover.transform.localScale = sp.transform.localScale;
//			cover.depth = sp.depth + 1;
//			BoxCollider coverBox = cover.gameObject.AddComponent<BoxCollider>();
//			coverBox.center = Vector3.zero;
//			coverBox.size = box.size;
//			cover.gameObject.SetActive(!show);
//			
//		}
	}
	
	void OnShowTip(GameObject games){
		MessageST mst = new MessageST();
		mst.messID = 401;
		mst.words = new string[1]{tipDes};
		GameCenter.messageMng.AddClientMsg(mst);
	}
}
