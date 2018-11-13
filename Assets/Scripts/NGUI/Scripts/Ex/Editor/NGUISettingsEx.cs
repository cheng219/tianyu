///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 
//最后修改时间：#Date#
//脚本描述： 
///////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NGUISettingsEx : NGUISettings {
	static public UISpriteEx AddSpriteEx (GameObject go)
	{
		UISpriteEx w = NGUITools.AddWidget<UISpriteEx>(go);
		w.name = "Sprite";
		w.atlas = atlas;
		w.spriteName = selectedSprite;
		
		if (w.atlas != null && !string.IsNullOrEmpty(w.spriteName))
		{
			UISpriteData sp = w.atlas.GetSprite(w.spriteName);
			if (sp != null && sp.hasBorder)
				w.type = UISpriteEx.Type.Sliced;
		}
		
		w.pivot = pivot;
		w.width = 100;
		w.height = 100;
		w.MakePixelPerfect();
		return w;
	}

	/// <summary>
	/// Convenience method -- add a texture.
	/// </summary>
	
	static public UITexture AddTexture (GameObject go)
	{
		UITexture w = NGUITools.AddWidget<UITexture>(go);
		w.name = "Texture";
		w.pivot = pivot;
		w.mainTexture = texture;
		w.width = 100;
		w.height = 100;
		w.shader = Shader.Find("Unlit/Transparent Colored Gray");
		return w;
	}


	/// <summary>
	/// Convenience method -- add a new panel.
	/// </summary>
	
	static public UIPanelEx AddPanel (GameObject go)
	{
		if (go == null) return null;
		int depth = UIPanelEx.nextUnusedDepth;
		UIPanelEx panel = NGUITools.AddChild<UIPanelEx>(go);
		panel.depth = depth;
		return panel;
	}
}
