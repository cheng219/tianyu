///////////////////////////////////////////////////////////////////////////////////////////
//作者：沙新佳 
//最后修改时间：#Date#
//脚本描述： 
///////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(UISpriteEx), true)]
public class UISpriteExInspector : UISpriteInspector
{
	protected override bool ShouldDrawProperties ()
	{
		base.ShouldDrawProperties();
		SerializedProperty cg = NGUIEditorTools.DrawProperty("ColorGray", serializedObject, "isGray", GUILayout.MinWidth(20f));
		if ( cg!= null )
		{
			UISpriteEx.ColorGray colorMode = (UISpriteEx.ColorGray)cg.intValue;
		}
		GUILayout.Space(6f);

		SerializedProperty wm = NGUIEditorTools.DrawProperty("GrayMaterial", serializedObject, "grayMaterial", GUILayout.MinWidth(20f));
		if ( wm!= null )
		{
			Material ffgrayMaterial = wm.objectReferenceValue as Material;
		}
		GUILayout.Space(6f);
		return true;
	}
	
	
	/// <summary>
	/// Draw the sprite preview.
	/// </summary>
	
	public override void OnPreviewGUI (Rect rect, GUIStyle background)
	{
		UISpriteEx sprite = target as UISpriteEx;
		if (sprite == null || !sprite.isValid) return;
		
		Texture2D tex = sprite.mainTexture as Texture2D;
		if (tex == null) return;
		
		UISpriteData sd = sprite.atlas.GetSprite(sprite.spriteName);
		NGUIEditorTools.DrawSprite(tex, rect, sd, sprite.color);
	}
}
