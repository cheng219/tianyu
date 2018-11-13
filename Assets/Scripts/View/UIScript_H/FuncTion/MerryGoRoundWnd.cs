//======================================================
//作者:何明军
//日期:2016/7/6
//用途:走马灯界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MerryGoRoundWnd : GUIBase {

	public float speedVal = 3f;
	public UILabel lab;
	public Vector3 startV3;
	public Vector3 distanceV3;
	public GameObject tatget;
	
	List<MerryGoRoundDataInfo> merryList = new List<MerryGoRoundDataInfo>();
	
	void Awake(){
		mutualExclusion = false;
		Layer = GUIZLayer.TIP + 560;
		if(tatget != null)tatget.SetActive(false);
		merryList.Clear();
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
		MainPlayerMng.OnCreateNew += OnMerryEventNew;
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
		MainPlayerMng.OnCreateNew -= OnMerryEventNew;
		GameCenter.chatMng.OnAddMerryGoRoundData -=  AddMerryGoRound;
		CancelInvoke("ResetRound");
	}
	
	void OnMerryEventNew(){
		CancelInvoke("ResetRound");
        GameCenter.chatMng.OnAddMerryGoRoundData -= AddMerryGoRound;
        GameCenter.chatMng.OnAddMerryGoRoundData += AddMerryGoRound;
	}
	
	void AddMerryGoRound(MerryGoRoundDataInfo _data){
		merryList.Add(_data);
		if(!isStart)isStart = true;
		if(tatget != null)tatget.SetActive(true);
	}
	bool onRound = false;
	void SpeedRound(){
		if(lab == null){
			isStart = false;
			if(tatget != null)tatget.SetActive(false);
			return ;
		}
		onRound = true;
		lab.enabled = false;
		lab.text = merryList[0].Content;
		lab.transform.localPosition = startV3;
//		Bounds bds = NGUIMath.CalculateAbsoluteWidgetBounds(lab.transform);
		float speedDis = distanceV3.x + lab.width - startV3.x;
		float val = speedDis / speedVal;
		lab.enabled = true;
		TweenPosition.Begin(lab.gameObject,val,new Vector3(-speedDis,distanceV3.y,distanceV3.z));
		
		CancelInvoke("ResetRound");
		Invoke("ResetRound",val);
		merryList.RemoveAt(0);
	}
	
	void ResetRound(){
		onRound = false;
	}
	
	bool isStart = false;
	// Update is called once per frame
	void Update () {
		if(isStart && !onRound){
			if(merryList.Count > 0){
				SpeedRound();
			}else{
				isStart = false;
				if(tatget != null)tatget.SetActive(false);
			}
		}
	}
}
