using UnityEngine;
using UnityEditor;
using System.Collections;
/// <summary>
///  NewBehaviourScript script.
/// </summary>
/// 
[CustomEditor(typeof(VolumeFogController))]
[CanEditMultipleObjects]
public class VolumeFogEditor : Editor  {
	public SerializedProperty radiusProp;

	public VolumeFogController Target
	{
		get
		{
			return (VolumeFogController)target;
		}
	}
	void OnEnable () {
		radiusProp = serializedObject.FindProperty ("radius");
		Target.radius = radiusProp.floatValue;
	}
	public override void OnInspectorGUI()
	{
		base.DrawDefaultInspector();
		serializedObject.Update ();

//		EditorGUILayout.FloatField(radiusProp.floatValue);
//		if (!radiusProp.hasMultipleDifferentValues)
		serializedObject.ApplyModifiedProperties();
	}

	// Custom GUILayout progress bar.
	void ProgressBar(float value,string label) {
		// Get a rect for the progress bar using the same margins as a textfield:
		Rect rect = GUILayoutUtility.GetRect (18f, 18f, "TextField");
		EditorGUI.ProgressBar (rect, value, label);
		EditorGUILayout.Space ();
	}

	void OnSceneGUI()
	{
		Handles.BeginGUI();
		Handles.color = Color.red;
		Handles.Label(Selection.activeGameObject.transform.position + Vector3.up*Target.radius,"体积雾："+
		              Selection.activeGameObject.transform.position.ToString()+Target.radius.ToString());

		Handles.DrawWireDisc(Selection.activeGameObject.transform.position, Vector3.up, Target.radius);
		Target.radius = Handles.ScaleValueHandle(Target.radius,Selection.activeGameObject.transform.position +new Vector3(Target.radius,0,0),
			                         Quaternion.identity,
			                         20,
			                         Handles.CylinderCap,
			                         60);
		//规定GUI显示区域 
		GUILayout.BeginArea(new Rect(0,0,200f,Screen.height));
		GUILayout.BeginVertical();  

		//GUI绘制文本框
		string content = string.Format("当前选中【ID】{0}【name】{1}【radius】{2}的体积雾对象",Selection.activeGameObject.GetInstanceID().ToString(),Selection.activeGameObject.name.ToString(),radiusProp.floatValue.ToString());
		GUILayout.TextArea(content);	
		if (GUI.changed)EditorUtility.SetDirty (target);
		GUILayout.EndVertical();
		GUILayout.EndArea(); 
		Handles.EndGUI();
	}

}
