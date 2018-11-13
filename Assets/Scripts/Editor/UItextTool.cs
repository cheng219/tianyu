////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//时间：2014.3.8
//作者：吴江
//文件描述：用于对一个场景资源进行资源依赖配置表导出，并且剥离该场景的资源，生成分散的资源包。 用于手机游戏加载场景的负载分散。
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using System.Text;

public class UItextTool : EditorWindow
{

    protected enum BuildingType
    {
        单个文件 = 0,
        整个文件夹的所有文件 = 1,
    }




    #region 数据

    public static Rect wr = new Rect(200, 140, 410, 460);


    List<string> waitBuildList = new List<string>();


    protected BuildingType buildingType = BuildingType.整个文件夹的所有文件;



    /// <summary>
    /// 编辑器界面拖入的记录文件
    /// </summary>
    protected Object assetBuildRecordObject = null;

    /// <summary>
    /// 本次打包的版本号
    /// </summary>
    int curVersionID = 0;

    /// <summary>
    /// 被比对的版本号
    /// </summary>
    int beCompareVersionID = 0;

    int[] versionIDArray = new int[0];
    string[] versionDesArray = new string[0];

    string path = Application.dataPath + "/Table/";
    int pos;
    string path_res;
    #endregion

    public void Init()
    {
        content.text = "目标文件:";
        content2.text = "保存路径";
    }


    GUIContent content = new GUIContent();
    GUIContent content2 = new GUIContent();
    Object excelObj = null;

    bool isBuilding = false;
    void OnGUI()
    {
        if (!isBuilding)
        {
            EditorGUILayout.LabelField("【注意】：在工程目录中选择要导出的预制或包含预制的文件夹");
            if (Selection.activeObject != null) EditorGUILayout.LabelField("当前选择的对象是：" + Selection.activeObject.name);



            buildingType = (BuildingType)EditorGUILayout.EnumPopup("打包模式", buildingType);

            GUILayout.BeginHorizontal();
            path = EditorGUILayout.TextField(content2, path);
            if (GUILayout.Button("...", GUILayout.Width(30)))
            {
                string newPath = EditorUtility.SaveFolderPanel("AssetBundle Exports Path", path, "");
                if (newPath.Length != 0)
                {
                    path = newPath;
                }
            }
            GUILayout.EndHorizontal();
           // excelObj = EditorGUILayout.ObjectField(excelObj,typeof(Object), false);

            //按钮操作
            if (GUI.Button(new Rect(3, wr.height - 60, wr.width - 6, 20), "导出"))
            {
                StartBuild();
            }
            if (GUI.Button(new Rect(wr.width - 110, wr.height - 30, 100, 20), "关闭"))
                this.Close();
        }
        else
        {
            EditorGUILayout.LabelField("路径：" + path);
            EditorGUILayout.LabelField("生成中，请等待。。。。。");
            EditorGUILayout.LabelField("生成进度：" + curCount.ToString() + "/" + totalCount.ToString());
        }
    }


    protected int lastCount = 0;

    void Update()
    {
        if (waitBuildList.Count > 0 && buildPendings == 0)
        {
            lastCount = waitBuildList.Count;
            BuildAssetsData();
        }
        else
        {
            if (lastCount > 0)
            {
                SaveCSV(PrefabTextDataDic, path);
                curCount = totalCount;
                lastCount = 0;
            }
        }
    }

    int totalCount = 0;
    int curCount = 0;
    int buildPendings = 0;
    public void StartBuild()
    {
        Caching.CleanCache();

        waitBuildList.Clear();


        switch (buildingType)
        {
            case BuildingType.单个文件:
                if (Selection.activeObject == null)
                    return;
                waitBuildList.Add(AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID()));
                break;
            case BuildingType.整个文件夹的所有文件:
                Object[] list = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
                if (list.Length == 0)
                    return;
                foreach (var item in list)
                {
                    string Url = AssetDatabase.GetAssetPath(item);
                    waitBuildList.Add(Url);
                }
                break;
            default:
                return;
        }
        totalCount = waitBuildList.Count;
        curCount = 0;
        isBuilding = true;
    }



    /// <summary>
     /// 将DataTable中数据写入到CSV文件中
     /// </summary>
     /// <param name="dt">提供保存数据的DataTable</param>
     /// <param name="fileName">CSV的文件路径</param>
     public static void SaveCSV(Dictionary<string, PrefabTextData> dt, string fullPath)
     {
         fullPath = fullPath + "UIText";
         FileInfo fi = new FileInfo(fullPath);
         if (!fi.Directory.Exists)
         {
             fi.Directory.Create();
         }
         FileStream fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
         //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
         StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
         string data = "";
         //写出列名称
         //foreach (string item in dt.Keys)
         //{
         //    data += item;
         //    data += ",";
         //}
         //sw.WriteLine(data);
         //foreach (string item in dt.Keys)
         //{
         //    if (dt[item].prefabTextList.Count == 0) continue;
         //    data = item + ",";
         //    int index = 0;
         //    foreach (PrefabTextData.PrefabText prefabText in dt[item].prefabTextList)
         //    {
         //        string str = prefabText.text;
         //        str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
         //        if (str.Contains(',') || str.Contains('"') 
         //            || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
         //        {
         //           str = string.Format("\"{0}\"", str);
         //      }
         //        data += str;
         //        if (index < dt[item].prefabTextList.Count - 1)
         //       {
         //            data += ",";
         //       }
         //        index++;
         //    }
         //    sw.WriteLine(data);
         //}


         foreach (string item in dt.Keys)
         {
             if (dt[item].prefabTextList.Count == 0) continue;
             int index = 0;
             foreach (PrefabTextData.PrefabText prefabText in dt[item].prefabTextList)
             {
                 if (index == 0)
                 {
                     data = item + ",";
                 }
                 else
                 {
                     data = "   ,";
                 }
                 string str = prefabText.text;
                 if (str.Length == 0) continue;
                 str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                 if (str.Contains(',') || str.Contains('"')
                     || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                 {
                     str = string.Format("\"{0}\"", str);
                 }
                 if (str.Contains('\r'))
                 {
                     str = str.Replace("\r", @"\r");
                 }
                 if (str.Contains('\n'))
                 {
                     str = str.Replace("\n", @"\n");
                 }
                 data += prefabText.prefabName + ",";
                 data += str;
                 sw.WriteLine(data);
                 index++;
             }
         }
         //Archaeology

         sw.Close();
         fs.Close();
     }

    int dependencies = 0;
    void BuildAssetsData()
    {
        if (waitBuildList.Count == 0 || !isBuilding) return;
        buildPendings++;
        GetData();
    }



    Dictionary<string, PrefabTextData> PrefabTextDataDic = new Dictionary<string, PrefabTextData>();

    protected void GetData()
    {
        Caching.CleanCache();
        PrefabTextDataDic.Clear();

        for (int i = 0; i < waitBuildList.Count; i++)
        {
            GameObject obj = AssetDatabase.LoadAssetAtPath(waitBuildList[i], typeof(GameObject)) as GameObject;
            if (obj == null)
            {
                continue;
            }
			if(PrefabTextDataDic.ContainsKey(obj.name))
			{
				Debug.LogWarning("相同的预制ming:"+obj.name);
			}else
			{
           		PrefabTextDataDic.Add(obj.name, new PrefabTextData(obj));
			}
        }

        //生成基本缓存数据
        foreach (var item in PrefabTextDataDic.Values)
        {
            item.BuildData();
        }

    }
}



public class PrefabTextData
{


    public class PrefabText 
    {
        public string prefabName = string.Empty;
        public string text = string.Empty;

        public PrefabText(UILabel _label)
        {
            prefabName = _label.gameObject.name;
            text = _label.text;

        }
    }


    /// <summary>
    /// 资源路径
    /// </summary>
    public string assetPath = string.Empty;


    /// <summary>
    /// 文件对象
    /// </summary>
    public GameObject activeObject = null;


    public string parentName = string.Empty;
    /// <summary>
    /// 预制文字队列
    /// </summary>
    public List<PrefabText> prefabTextList = new List<PrefabText>();

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="_assetPathies"></param>
    /// <param name="_activeObject"></param>
    /// <param name="_assetsDepent"></param>
    public PrefabTextData(GameObject _activeObject)
    {
        if (_activeObject == null) return;
        activeObject = _activeObject;

    }

    /// <summary>
    /// 创建内部资源引用数据
    /// </summary>
    public void BuildData()
    {
        if (activeObject == null ) return;
        parentName = activeObject.name;
        prefabTextList.Clear();
        UILabel[] labels = activeObject.GetComponentsInChildren<UILabel>(true);
        for (int i = 0; i < labels.Length; i++)
        {
            if(labels[i].text.Contains("#$*")) continue; //排除符 #$*
            prefabTextList.Add(new PrefabText(labels[i]));
        }
    }


}