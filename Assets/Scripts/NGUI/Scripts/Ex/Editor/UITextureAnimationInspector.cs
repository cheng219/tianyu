//----------------------------------------------
//作者：沙新佳 qq：287490904
//最后修改时间：2016/7/19
//----------------------------------------------

using UnityEngine;
using UnityEditor;

/// <summary>
/// Inspector class used to edit UISpriteAnimations.
/// </summary>

[CanEditMultipleObjects]
[CustomEditor(typeof(UITextureAnimation))]
public class UITextureAnimationInspector : Editor
{
	/// <summary>
	/// Draw the inspector widget.
	/// </summary>

	public override void OnInspectorGUI ()
	{
		GUILayout.Space(3f);
		NGUIEditorTools.SetLabelWidth(80f);
		serializedObject.Update();

		NGUIEditorTools.DrawProperty("Framerate", serializedObject, "mFPS");
		NGUIEditorTools.DrawProperty("Rows", serializedObject, "Rows");
		NGUIEditorTools.DrawProperty("Columns", serializedObject, "Columns");
		NGUIEditorTools.DrawProperty("tileNum", serializedObject, "tileNum");
		NGUIEditorTools.DrawProperty("Loop", serializedObject, "mLoop");
		NGUIEditorTools.DrawProperty("Pixel Snap", serializedObject, "mSnap");

		serializedObject.ApplyModifiedProperties();
	}
}
