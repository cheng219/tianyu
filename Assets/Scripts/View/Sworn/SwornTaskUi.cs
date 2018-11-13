//==================================
//作者：朱素云
//日期：2016/5/7
//用途：结义任务UI
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwornTaskUi : MonoBehaviour 
{
    public UILabel taskDes;//任务描述
    public UIButton pathFindBtn;//寻路
    public UILabel []friendName;//玩家与义友名字 
    private SwornData data = null;
    private Dictionary<int, Dictionary<int, int>> taskProgress
    {
        get
        {
            return GameCenter.swornMng.taskProgress;
        }
    }
    //任务表
    private TaskInfo task = null;
    public TaskInfo Task
    {
        get
        {
            return task;
        }
        set
        {
            if (value != null) task = value; 
            Show();
        }
    }

    void Show()
    { 
        data = GameCenter.swornMng.data;
        if (Task != null)
        {
            if (taskDes != null) taskDes.text = Task.SimpleDesUntilComplete;
            if (pathFindBtn != null) UIEventListener.Get(pathFindBtn.gameObject).onClick = delegate
            {
                GameCenter.taskMng.CurfocusTask = Task;
                GameCenter.taskMng.GoTraceTask(Task);
            };

            if (friendName.Length > 0)
            {
                for (int i = 0; i < friendName.Length; i++)
                {
                    if (friendName[i] != null)
                    {
                        if (i < data.brothers.Count)
                        {
                            int id = data.brothers[i].uid;
                            friendName[i].text = "[e7ffe8]" + data.brothers[i].name;
                            if (taskProgress.ContainsKey(id))
                            {
                                if (taskProgress[id].ContainsKey(Task.ID))
                                    friendName[i].text = "[6ef574]" + data.brothers[i].name;//任务完成了名字变绿
                            }
                        }
                        else
                            friendName[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
