///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 
//最后修改时间：#Date#
//脚本描述： 
///////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

/// <summary>
/// NGUIMenu 的扩展
/// </summary>

static public class NGUIMenuEx
{
#region Selection
	
#endregion
#region Create

	[MenuItem("NGUI/Create/SpriteEx &#e", false, 6)]
	static public void AddSpriteEx ()
	{
		GameObject go = NGUIEditorTools.SelectedRoot(true);
		
		if (go != null)
		{
			Selection.activeGameObject = NGUISettingsEx.AddSpriteEx(go).gameObject;
		}
		else Debug.Log("You must select a game object first.");
	}
	[MenuItem("NGUI/Create/TextureEx &#p", false, 6)]
	static public void AddTexture ()
	{
		GameObject go = NGUIEditorTools.SelectedRoot(true);
		
		if (go != null)
		{
			Selection.activeGameObject = NGUISettingsEx.AddTexture(go).gameObject;
		}
		else Debug.Log("You must select a game object first.");
	}

	[MenuItem("NGUI/Create/PanelEx", false, 6)]
	static void AddPanel ()
	{
		UIPanelEx panel = NGUISettingsEx.AddPanel(NGUIMenu.SelectedRoot());
		Selection.activeGameObject = (panel == null) ? NGUIEditorTools.SelectedRoot(true) : panel.gameObject;
	}
#endregion
#region Attach


#endregion
#region Tweens

#endregion
#region Open

#endregion
#region Options

#endregion


}
