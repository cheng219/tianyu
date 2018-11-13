//=====================================
//作者:邓成
//日期:2016/7/25
//用途:单个任务界面
//=====================================

using UnityEngine;
using System.Collections;

public class TaskListSingle : MonoBehaviour {

    public UILabel title;
    public GameObject complete;
    public UILabel taskGoal;
	public GameObject guideFx;

	protected float lastChangeTime = 0f;
	protected bool isShow = false;

	protected TaskInfo myTaskInfo;

    public TaskInfo MyTaskInfo
    {
        get
        {
            return myTaskInfo;
        }
        set
        {
            if (myTaskInfo != null) myTaskInfo.OnPropertyUpdate -= Refresh;
            myTaskInfo = value;
            Refresh();
            myTaskInfo.OnPropertyUpdate += Refresh;
        }
    }

    void OnEnable()
    {
		
    }

    void OnDisable()
    {
        if (myTaskInfo != null) myTaskInfo.OnPropertyUpdate -= Refresh;
    }

    void OnDestroy()
    {
        if (myTaskInfo != null) myTaskInfo.OnPropertyUpdate -= Refresh;
    }
    

    protected void Refresh()
    {
		ShowFx(false);
        if (myTaskInfo.TaskLineIsEnd) 
        {
            return;
        }
        if (myTaskInfo == null)
        {
			if(complete != null)complete.SetActive(false);
            return;
        }
		if(title != null)title.text = myTaskInfo.TaskName;
		if(taskGoal != null) taskGoal.text = myTaskInfo.TaskGoalText;
        TaskStateType state = myTaskInfo.TaskState;
		if(complete != null)complete.gameObject.SetActive(state == TaskStateType.Finished);
    }


    public TaskListSingle CreateNew(Transform _parent, int _index)
    {
        GameObject obj = Instantiate(this.gameObject) as GameObject;
        obj.transform.parent = _parent;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        return obj.GetComponent<TaskListSingle>();
    }

	void OnClick()
	{
		ShowFx(false);
	}
	public void ShowFx(bool _show)
	{
		isShow = false;
		if(MyTaskInfo == null || MyTaskInfo.TaskType != TaskType.Main)
		{
			isShow = true;//不是主线不进Update
			if(guideFx != null && guideFx.activeSelf)guideFx.SetActive(false);
			return;
		}
		if(_show)
		{
			MainPlayerInfo mainInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
			bool isInTaskPath = GameCenter.curMainPlayer != null && GameCenter.curMainPlayer.CurFSMState == MainPlayer.EventType.TASK_PATH_FIND;
			if(guideFx != null)
			{
				if(mainInfo.CurSceneUiType == SceneUiType.NONE && mainInfo.CurLevel <= 30 && !isInTaskPath)
				{
					guideFx.SetActive(_show);
					isShow = true;
					lastChangeTime = Time.time;
				}else
				{
					if(guideFx.activeSelf)guideFx.SetActive(false);
					isShow = false;
					lastChangeTime = Time.time;
				}
			}
		}else
		{
			if(guideFx != null && guideFx.activeSelf)guideFx.SetActive(_show);
			isShow = false;
			lastChangeTime = Time.time;
		}
	}

	void Update()
	{
		if(!isShow && Time.frameCount % 100 == 0)
		{
			if(!GameCenter.noviceGuideMng.StartGuide)//引导中不显示
			{
				if(Time.time - lastChangeTime > 5.0f)
				{
					ShowFx(true);
				}
			}
		}
	}
}
