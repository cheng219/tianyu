/// <summary>
/// 何明军
/// 2016/4/19
/// 副本结算界面
/// </summary>

using UnityEngine;
using System.Collections;

public class CopySettlementFlopWnd : GUIBase {

	public CopySettlementBrandUI[] flops;
	
 	public GameObject btnNext;
	public GameObject btnOk;
    public GameObject btnAgain;
    public UITimer timer;
    public UITimer leveTimer;
    public UITimer againTimer;
	public float effctTime = 1f;
	
	bool isUpdate = true;
	void Awake(){
		mutualExclusion = false;
		Layer = GUIZLayer.TOPWINDOW;
		isUpdate = true;

        if (btnOk != null) btnOk.SetActive(false);
        if (btnNext != null)btnNext.SetActive(false);
        if (btnAgain != null) btnAgain.SetActive(false);
    }
	bool isClickFlop = false;
	void OnClickFlop(GameObject games){
		int flopID = (int)UIEventListener.Get(games).parameter;
		GameCenter.duplicateMng.C2S_ReqSettlementFlop(flopID);
		isUpdate = false;
		isClickFlop = true;
	}
	int curFlop = 0;
	float time = 0;
	void Update(){
		if(isUpdate){
			if((Time.time - time) > effctTime){
				time = Time.time;
				flops[curFlop%flops.Length].SetTween();
				curFlop++;
			}
		}
		if(isClickFlop){
			isClickFlop = false;
		}
	}

	CopySettlementDataInfo data{
		get{
			return GameCenter.duplicateMng.CopySettlementDataInfo;
		}
	}
	
	void ShowSettlementFlop(){
		SceneRef scene = ConfigMng.Instance.GetSceneRef(GameCenter.mainPlayerMng.MainPlayerInfo.SceneID);
		btnNext.SetActive(scene.sort == SceneType.ENDLESS);
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
		curFlop = 0;
		time = Time.time;
		if(btnOk != null)UIEventListener.Get(btnOk).onClick = delegate {
			ToFlopWmd();
		};
        leveTimer.StartIntervalTimer(8);
		CancelInvoke("ToFlopWmd");
		Invoke("ToFlopWmd",8);
		GameCenter.duplicateMng.OnOpenCopySettlementFlop += OnOpenCopySettlementFlop;
		for(int i=0,len=flops.Length;i<len;i++){
			UIEventListener.Get(flops[i].gameObject).onClick += OnClickFlop;
			UIEventListener.Get(flops[i].gameObject).parameter = i + 1;
		}
		isSelectFolp = false;
	}

	protected override void OnClose ()
	{
		base.OnClose ();
		CancelInvoke("ToFlopWmd");
		GameCenter.duplicateMng.OnOpenCopySettlementFlop -= OnOpenCopySettlementFlop;
		for(int i=0,len=flops.Length;i<len;i++){
			UIEventListener.Get(flops[i].gameObject).onClick -= OnClickFlop;
		}
	}
	//只进入一次
	bool isSelectFolp = false;
    void OnOpenCopySettlementFlop()
    {
        if (isSelectFolp) { return; }
        isSelectFolp = true;
        EquipmentInfo item = null;
        for (int i = 0, len = flops.Length; i < len; i++)
        {
            item = data.flopItems[i + 1] as EquipmentInfo;
            if (i == data.clickFlop - 1)
            {
                flops[i].SetSelete(true);
                flops[i].SetItem(item);
            }
            else
            {
                flops[i].SetSelete(false);
                flops[i].SetItem(item);
            }
        }
        if (btnOk != null)
        {
            btnOk.SetActive(true);
            UIEventListener.Get(btnOk).onClick = delegate
            {
                GameCenter.uIMng.ReleaseGUI(GUIType.COPYWINFLOP);
                GameCenter.duplicateMng.C2S_OutCopy();
            };
        }
        if (btnNext != null)
        {
            btnNext.SetActive(!GameCenter.endLessTrialsMng.IsFastClearance && !GameCenter.endLessTrialsMng.isLastChapter && GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.btnShowType == SceneBtnShowType.Next);
            if (btnNext != null) UIEventListener.Get(btnNext).onClick = delegate
            {
                GameCenter.uIMng.ReleaseGUI(GUIType.COPYWINFLOP);
                GameCenter.endLessTrialsMng.NextEnd();
            };
        }
        if (btnAgain != null)
        {
            int curSceneId = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.id;
            btnAgain.SetActive(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneRef.btnShowType == SceneBtnShowType.Again && curSceneId != 110311 && curSceneId != 110321);
            if (btnAgain != null) UIEventListener.Get(btnAgain).onClick = delegate
            {
                GameCenter.uIMng.ReleaseGUI(GUIType.COPYWINFLOP);
                GameCenter.duplicateMng.AgainToCopyItem();
            };
         }
        if (btnNext != null)
                {
                    if (btnNext.activeSelf)
                    {
                        leveTimer.gameObject.SetActive(false);
                        timer.StartIntervalTimer(8);
                        timer.onTimeOut = (x) =>
                          {
                              GameCenter.uIMng.ReleaseGUI(GUIType.COPYWINFLOP);
                              GameCenter.endLessTrialsMng.NextEnd();
                          };
                    }
                    //else
                    //{
                    //    leveTimer.gameObject.SetActive(true);
                    //    leveTimer.StartIntervalTimer(8);
                    //    leveTimer.onTimeOut = (x) =>
                    //    {
                    //        timer.onTimeOut = (x) =>
                    //          {
                    //              GameCenter.endLessTrialsMng.NextEnd();
                    //          };
                    //    }
                    else
                    {
                        leveTimer.gameObject.SetActive(true);
                        leveTimer.StartIntervalTimer(8);
                        leveTimer.onTimeOut = (x) =>
                        {
                            //Debug.Log("离开副本");
                            GameCenter.uIMng.ReleaseGUI(GUIType.COPYWINFLOP);
                            GameCenter.duplicateMng.C2S_OutCopy();
                        };
                    }
                }
                //    if(btnAgain != null)
                //    {
                //       if (btnAgain.activeSelf)
                //      {
                //        leveTimer.gameObject.SetActive(false);
                //        againTimer.StartIntervalTimer(8);
                //        againTimer.onTimeOut = (x) =>
                //        {
                //            GameCenter.duplicateMng.AgainToCopyItem();
                //        };
                //      }
            //}
    }
	void ToFlopWmd(){
		GameCenter.duplicateMng.C2S_ReqSettlementFlop(1);
		isUpdate = false;
	}
}
