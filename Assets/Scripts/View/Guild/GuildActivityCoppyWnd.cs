//===============================
//作者：邓成
//日期：2016/6/27
//用途：仙盟活动副本显示类(仙域守护、帮派战)
//===============================

using UnityEngine;
using System.Collections;

public class GuildActivityCoppyWnd : GUIBase {
	public GuildProtectCoppyWnd guildProtectCoppyWnd;
	public GuildFightCoppyWnd guildFightCoppyWnd;
    public BattleFieldWnd battleFieldWnd;
	protected override void OnOpen ()
	{
		base.OnOpen ();
		ShowWnd();
	}

	protected override void OnClose ()
	{
		base.OnClose ();
	}

	protected override void HandEvent (bool _bind)
	{
		base.HandEvent (_bind);
		if(_bind)
		{
			
		}else
		{
			
		}
	}
	void ShowWnd()
	{
		switch(GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType)
		{
		case SceneUiType.GUILDPROTECT:
			if(guildProtectCoppyWnd != null)guildProtectCoppyWnd.OpenUI();
			if(guildFightCoppyWnd != null)guildFightCoppyWnd.CloseUI();
            if (battleFieldWnd != null) battleFieldWnd.CloseUI();
			break;
		case SceneUiType.GUILDWAR:
			if(guildProtectCoppyWnd != null)guildProtectCoppyWnd.CloseUI();
			if(guildFightCoppyWnd != null)guildFightCoppyWnd.OpenUI();
            if (battleFieldWnd != null) battleFieldWnd.CloseUI();
			break;
        case SceneUiType.BATTLEFIGHT:
            if(guildProtectCoppyWnd != null)guildProtectCoppyWnd.CloseUI();
		    if(guildFightCoppyWnd != null)guildFightCoppyWnd.CloseUI();
            if (battleFieldWnd != null) battleFieldWnd.OpenUI();
        break;
		}
	}
}
