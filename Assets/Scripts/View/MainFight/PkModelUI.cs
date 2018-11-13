//======================================================
//作者:何明军
//日期:2016/8/15
//用途:pk模式切换UI
//======================================================
using UnityEngine;
using System.Collections;

public class PkModelUI : MonoBehaviour {

	public UIToggle tog;
	public UIButton btn;
	public UIButton close;
	public UILabel des;
	
	PkMode pkModel;
	public PkMode PkModel{
		get{
			return pkModel;
		}
		set{
			pkModel = value;
		}
	}
	
	void OnEnable(){
		if(btn != null)UIEventListener.Get(btn.gameObject).onClick = delegate {
			GameCenter.mainPlayerMng.pkModelTipShow = tog.value;
			GameCenter.mainPlayerMng.C2S_SetCampMode(PkModel);
			gameObject.SetActive(false);
		};
		if(close != null)UIEventListener.Get(close.gameObject).onClick = delegate {
			gameObject.SetActive(false);
		};
		int id = 0;
		if(PkModel != PkMode.PKMODE_PEASE)
		{
			switch(PkModel)
			{
			case PkMode.PKMODE_ALL:
				id = 96;
				break;
			case PkMode.PKMODE_GUILD:
				id = 98;
				break;
			case PkMode.PKMODE_TEAM:
				id = 97;
				break;
			case PkMode.PKMODE_JUSTICE:
				id = 99;
				break;
			}
		}
		if(des != null)des.text = ConfigMng.Instance.GetUItext(id);
	}
}
