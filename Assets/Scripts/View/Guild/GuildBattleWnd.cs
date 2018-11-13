//==================================
//作者：邓成
//日期：2016/4/13
//用途：仙盟战界面类
//=================================

using UnityEngine;
using System.Collections;

public class GuildBattleWnd : SubWnd {
	public UIButton btnBattle;
	void Awake()
	{
		if(btnBattle != null)UIEventListener.Get(btnBattle.gameObject).onClick = GuildBattle;
	}
	protected override void OnOpen()
	{
		base.OnOpen();
	}
	protected override void OnClose()
	{
		base.OnClose();
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{

		}
		else
		{ 

		}
	}
	void GuildBattle(GameObject go)
	{
        //GameCenter.messageMng.AddClientMsg(382);
		//GameCenter.uIMng.SwitchToUI(GUIType.GUILDFIGHT);
        GameCenter.uIMng.GenGUI(GUIType.GUILDFIGHT,true);
	}
}
