//======================================================
//作者:何明军
//日期:2016/7/6
//用途:功能开启界面
//======================================================
using UnityEngine;
using System.Collections;

public class FunctionOpenWnd : SubWnd {
	
	public UISprite icon;
	public UILabel name;
	
	public GameObject fligthDontShow;
	public GameObject fligthEffect;
	
	FunctionDataInfo funcInfo = null;
	public FunctionDataInfo FuncInfo{
		set{
			funcInfo = value;
		}
	}
	bool moving = false;
	void Show(){
		if(funcInfo == null)return ;
		if(fligthEffect != null)fligthEffect.SetActive(false);
		if(fligthDontShow != null)fligthDontShow.SetActive(true);
		if(icon != null){
			icon.transform.localPosition = Vector3.zero;
			icon.spriteName = funcInfo.Icon;
			icon.MakePixelPerfect();
		}
		if(name != null)name.text = funcInfo.Name;
	}

	protected override void OnOpen ()
	{
		base.OnOpen ();
		Show();
		GameCenter.uIMng.ReleaseGUI(GUIType.NPCDIALOGUE);
		UIEventListener.Get(gameObject).onClick = OnClickGame;
	}

	protected override void OnClose ()
	{
		base.OnClose ();
		if(icon != null){
			TweenPosition tw = icon.GetComponent<TweenPosition>();
			if(tw != null)Destroy(tw);
			icon.transform.localPosition = Vector3.zero;
		}
		moving = false;
	}
	
	void OnClickGame(GameObject games){
		if(funcInfo == null || moving)return ;
		if(funcInfo.FligthPos != Vector2.zero){
			moving = true;
			if(fligthDontShow != null)fligthDontShow.SetActive(false);
			if(fligthEffect != null)fligthEffect.SetActive(true);
			GameCenter.uIMng.OpenFuncShowMenu(true);
			GameCenter.uIMng.ShowMapMenu(true);
			if(icon != null){
				icon.transform.localPosition = Vector3.zero;
				TweenPosition.Begin(icon.gameObject,1,funcInfo.FligthPos);
			}
			CancelInvoke("Next");
			Invoke("Next",1);
		}else{
			if(funcInfo.SceneID > 0){
				GameCenter.mainPlayerMng.C2STaskFly(funcInfo.SceneID);
			}
			if(funcInfo.GuideData != null){
				GameCenter.noviceGuideMng.OpenGuide(funcInfo.GuideData);
			}
			CloseUI();
		}
	}
	
	void Next(){
		if(funcInfo.GuideData != null){
			GameCenter.noviceGuideMng.OpenGuide(funcInfo.GuideData);
		}
        else if (funcInfo.SceneID > 0)
        {
            GameCenter.mainPlayerMng.C2STaskFly(funcInfo.SceneID);
        }
		CloseUI();
	}
}
