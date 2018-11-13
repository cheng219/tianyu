//======================================================
//作者:何明军
//日期:16/6/17
//用途:竞技场战斗界面
//======================================================
using UnityEngine;
using System.Collections;

public class MainCopyArenaUI : MonoBehaviour {

	public UITimer time;
	public GameObject titil;
	public GameObject go;
	public GameObject ready;
	public GameObject ko;
	
	void Awake(){
		time.StartIntervalTimer(99);
		titil.SetActive(false);
		go.SetActive(false);
		ready.SetActive(false);
		ko.SetActive(false);
		
		GameCenter.duplicateMng.OnOpenArenaSettlement = delegate {
			if(!GameCenter.duplicateMng.CopySettlementDataInfo.showKo){
				GameCenter.uIMng.SwitchToUI(GUIType.ARENERESULT);
				return ;
			}
			titil.SetActive(true);
			ko.SetActive(true);
			CancelInvoke("CloseKo");
			Invoke("CloseKo",1f);
		};
	}
	
	void OnEnable()
	{
		titil.SetActive(true);
		ready.SetActive(true);
		CancelInvoke("CloseReady");
		Invoke("CloseReady",1f);
	}
	
	void CloseReady(){
		ready.SetActive(false);
		go.SetActive(true);
		CancelInvoke("CloseGo");
		Invoke("CloseGo",1f);
	}
	
	void CloseGo(){
		titil.SetActive(false);
		go.SetActive(false);
        if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneType == SceneType.ARENA)
        {
            GameCenter.curMainPlayer.GoAutoFight();//竞技场开启自动战斗
        }
	}
	
	void CloseTitil(){
		titil.SetActive(false);
	}
	
	void CloseKo(){
		titil.SetActive(false);
		ko.SetActive(false);
		GameCenter.uIMng.SwitchToUI(GUIType.ARENERESULT);
	}
}
