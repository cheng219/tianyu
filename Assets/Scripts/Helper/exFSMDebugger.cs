///////////////////////////////////////////////////////////////////////////////
//作者：吴江
//日期：2015/5/22
//用途：编辑器界面中展示某状态机的信息（供研发人员用）
///////////////////////////////////////////////////////////////////////////////

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;



class exFSMDebugger : EditorWindow
{
    public bool lockSelection = false;

    private fsm.Machine curEdit = null;
    private int index = 0;
    private GameObject curGO = null;
    private GUIStyle textStyle = new GUIStyle();
    private FSMBase[] fsmList = null;
    private List<string> options = new List<string>();



    protected virtual fsm.Machine GetStateMachine(GameObject _go, int _idx)
    {
        index = 0;
        options.Clear();
		fsmList = _go.GetComponents<SmartActorAnimFSM>();
        if (_idx < fsmList.Length)
        {
            index = _idx;
            for (int i = 0; i < fsmList.Length; ++i)
                options.Add(fsmList[i].GetType().ToString());
            return fsmList[index].stateMachine;
        }
        return null;
    }



    [MenuItem("ex/Debugger/FSM Debugger")]
    public static exFSMDebugger NewWindow()
    {
        exFSMDebugger newWindow = EditorWindow.GetWindow<exFSMDebugger>();
        return newWindow;
    }



    protected void OnEnable()
    {
        textStyle.alignment = TextAnchor.MiddleLeft;
        name = "FSM Debugger";
        wantsMouseMove = false;
        autoRepaintOnSceneChange = true;
        // position = new Rect ( 50, 50, 800, 600 );
    }

    void Init()
    {
    }


    public void Debug(Object _obj, int _idx = 0)
    {
        GameObject go = _obj as GameObject;
        if (go == null)
        {
            return;
        }

        //
        fsm.Machine machine = GetStateMachine(go, _idx);

        // check if repaint
        if (curEdit != machine)
        {
            curEdit = machine;
            curGO = go;
            Init();

            Repaint();
            return;
        }
    }



    void OnSelectionChange()
    {
        if (curEdit == null || lockSelection == false)
        {
            GameObject go = Selection.activeGameObject;
            if (go)
            {
                Debug(go, 0);
            }
        }
        Repaint();
    }



    void OnGUI()
    {
        EditorGUI.indentLevel = 0;
        if (curEdit == null)
        {
            GUILayout.Space(10);
            GUILayout.Label("Please select a GameObject for editing");
            return;
        }
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.FlexibleSpace();
        index = EditorGUILayout.Popup(index, options.ToArray(), EditorStyles.toolbarDropDown);
        lockSelection = GUILayout.Toggle(lockSelection, "Lock", EditorStyles.toolbarButton);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        if (curEdit != null && curGO != null)
            curEdit.ShowDebugGUI(curGO.name, textStyle);
    }
}
#endif