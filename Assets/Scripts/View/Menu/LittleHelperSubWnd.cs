//====================================
//作者：黄洪兴
//日期：2016/6/4
//用途：小助手窗口
//=====================================


using UnityEngine;
using System.Collections;

public class LittleHelperSubWnd : SubWnd
{

    /// <summary>
    /// 小助手选项
    /// </summary>
    public UIToggle[] littleHelperTogs = new UIToggle[5];
    public GameObject grid;
    
   



    #region UNITY
    void Awake()
    {
        type = SubGUIType.LITTLEHELPER;
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
    protected override void OnOpen()
    {
        base.OnOpen();
        littleHelperTogs[0].value = false;
        littleHelperTogs[(int)GameCenter.littleHelperMng.NeedType].value = true;
        for (int i = 0; i < littleHelperTogs.Length; i++)
        {
            UIEventListener.Get(littleHelperTogs[i].gameObject).onClick = ClickToggleEvent;
        }

        ClickToggleEvent(littleHelperTogs[(int)GameCenter.littleHelperMng.NeedType].gameObject);
    }

    protected override void OnClose()
    {
        base.OnClose();
    }

    protected UIToggle lastChangeToggle = null;
    protected void ClickToggleEvent(GameObject go)
    {
        UIToggle toggle = go.GetComponent<UIToggle>();
        if (toggle != lastChangeToggle)
        {
            Refresh();
        }
        if (toggle != null && toggle.value) lastChangeToggle = toggle;
    }


    #endregion

    public void Refresh()
    {
        int num=0;
        for (int i = 0; i < littleHelperTogs.Length; i++)
        {
            if (littleHelperTogs[i].value)
            {
                //GameCenter.littleHelperMng.NeedType = (LittleHelpType)i;
                num = i;
            }
        }
        if (!GameCenter.littleHelperMng.LittleHelperDic.ContainsKey(num + 1))
            return;
        if (GameCenter.littleHelperMng.LittleHelperDic[num + 1].Count < 1)
            return;
        DestroyItem();
        Vector3 V3=Vector3.zero;
        for (int i = 0; i < GameCenter.littleHelperMng.LittleHelperDic[num + 1].Count; i++)
        { 
            LittleHelperRef littleHelper = GameCenter.littleHelperMng.LittleHelperDic[num + 1][i];
            TaskInfo curTask = GameCenter.taskMng.GetMainTaskInfo();
            if ((curTask != null && littleHelper != null && littleHelper.TaskId.Count > 1))
            {
                //Debug.Log(" step :   " + curTask.Step + "  taskId :  " + littleHelper.TaskId[1]);
                if (curTask.Step >= littleHelper.TaskId[1])//该条任务已经完成功能已经开启
                {
                    GameObject obj = Instantiate(exResources.GetResource(ResourceType.GUI, "SetUp/LittleHelperItem")) as GameObject;
                    if (obj != null)
                    {
                        obj.transform.parent = grid.transform;
                        obj.transform.localPosition = V3;
                        obj.transform.localScale = Vector3.one;
                        obj.GetComponent<LittleHelperItem>().FillInfo(littleHelper);
                        V3 = new Vector3(V3.x, V3.y - 105, V3.z);
                    }
                    obj = null;
                }
            }
            else//主线任务做完
            {
                if (littleHelper != null && littleHelper.TaskId.Count > 0)
                {
                    bool isMainTakEnded = GameCenter.taskMng.IsTaskEnded(littleHelper.TaskId[0], TaskType.Main);
                    if (isMainTakEnded)
                    {
                        GameObject obj = Instantiate(exResources.GetResource(ResourceType.GUI, "SetUp/LittleHelperItem")) as GameObject;
                        if (obj != null)
                        {
                            obj.transform.parent = grid.transform;
                            obj.transform.localPosition = V3;
                            obj.transform.localScale = Vector3.one;
                            obj.GetComponent<LittleHelperItem>().FillInfo(littleHelper);
                            V3 = new Vector3(V3.x, V3.y - 105, V3.z);
                        }
                        obj = null;
                    }
                }
            }
        } 
    }

    //void RefreshByType()
    //{
    //    int num =(int) GameCenter.littleHelperMng.NeedType;
    //    littleHelperTogs[num].value = true;
    //    Refresh(this.gameObject);
    //}


    void DestroyItem()
    {
        if (grid != null)
        {
            grid.transform.DestroyChildren();
        }
    }

    void OnDestroy()
    {
        //GameCenter.littleHelperMng.NeedType = LittleHelpType.STRONG;
    }
    #region 控件事件





   
    #endregion
}
