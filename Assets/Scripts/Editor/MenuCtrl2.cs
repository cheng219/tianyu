using UnityEngine;
using UnityEditor;
using System.Collections;

public class MenuCtrl2 : MonoBehaviour {

	[@MenuItem("SLTools/界面预制文字导出")]
    static void CreateWindow()
    {

        UItextTool window = (UItextTool)EditorWindow.GetWindowWithRect(typeof(UItextTool), UItextTool.wr, true, "界面预制文字导出");
        window.autoRepaintOnSceneChange = true;
        window.minSize = new Vector2(350, 400);
        window.position = UItextTool.wr;
        window.titleContent.text = "场景资源信息导出工具窗口";
        window.wantsMouseMove = true;
        window.Show();
        window.ShowUtility();
        window.ShowTab();
    }
}
