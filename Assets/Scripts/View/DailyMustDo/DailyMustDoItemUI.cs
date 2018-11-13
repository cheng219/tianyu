//===============================
//作者：邓成
//日期：2016/7/7
//用途：每日必做Item界面类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DailyMustDoItemUI : MonoBehaviour {
	public UILabel labName;
	public UILabel labDes;
	public UILabel labFinishTimes;
	public UISprite icon;
	public UIButton btnGo;
	//public UIButton btnReward;
	public GameObject blackMask;
	//public UILabel labBtnReward;
	//public ItemUI itemReward;
    public UILabel addLivelyNum;

	protected DailyMustDoInfo CurMustDoInfo = null;
    protected GuildLivelyInfo CurLivelyInfo = null;
	void Awake()
	{
		//if(btnReward != null)UIEventListener.Get(btnReward.gameObject).onClick = GetReward;
		if(btnGo != null)UIEventListener.Get(btnGo.gameObject).onClick = GoMustDoItem;
	}
	public void SetData(DailyMustDoInfo _data)
	{
		CurMustDoInfo = _data;
		if(labName != null)labName.text = "[u]"+_data.Name;
		if(labDes != null)labDes.text = _data.Des;
		//if(labBtnReward != null)labBtnReward.text = _data.ButtonName;
        if (icon != null)
        {
            icon.spriteName = _data.IconName;
            icon.MakePixelPerfect();
        }
		//if(itemReward != null)itemReward.FillInfo(_data.RewardItem);
		if(labFinishTimes != null)
		{
			labFinishTimes.enabled = (_data.TotalTimes != 0);
			labFinishTimes.text = _data.FinishTimes.ToString()+"/"+_data.TotalTimes.ToString();
		}
        string str = ConfigMng.Instance.GetUItext(137);
        if (addLivelyNum != null)
        {
            addLivelyNum.text = str + "+" + _data.StaticLivelyCount.ToString();
            addLivelyNum.enabled = !(_data.MustDoState == DailyMustDoInfo.RewardState.CANGET);
        }
		//if(btnReward != null)btnReward.isEnabled = (_data.MustDoState == DailyMustDoInfo.RewardState.CANGET);
        if (blackMask != null) blackMask.SetActive(_data.MustDoState == DailyMustDoInfo.RewardState.CANGET);
		//if(btnGo != null)btnGo.gameObject.SetActive(_data.UISort != 0);
	}
	protected void GoMustDoItem(GameObject go)
	{
		if(CurMustDoInfo != null)
		{
			if(CurMustDoInfo.UISort == 1)
			{
				GUIType type = (GUIType)System.Enum.Parse(typeof(GUIType),CurMustDoInfo.UIType);
				if(type == GUIType.GUILDMAIN && string.IsNullOrEmpty(GameCenter.mainPlayerMng.MainPlayerInfo.GuildName))
				{
					GameCenter.messageMng.AddClientMsg(408);
					return;
				}
				GameCenter.uIMng.SwitchToUI(type);
			}else if(CurMustDoInfo.UISort == 2)
			{
				SubGUIType type = (SubGUIType)System.Enum.Parse(typeof(SubGUIType),CurMustDoInfo.UIType);
				GameCenter.uIMng.SwitchToSubUI(type);
			}
            //LivelyRef的where字段判断是否做自动寻路
            if(CurMustDoInfo.RefData.where != null&&CurMustDoInfo.RefData.where.Count>=3)//大于3,否则下标越界
            {
                CheckAutoPath(CurMustDoInfo.RefData.where);
            }
        }
        //仙盟活跃
        if (CurLivelyInfo != null)
        {
            if (CurLivelyInfo.UISort == 1)
            {
                GUIType type = (GUIType)System.Enum.Parse(typeof(GUIType), CurLivelyInfo.UIType);
                if (type == GUIType.GUILDMAIN && string.IsNullOrEmpty(GameCenter.mainPlayerMng.MainPlayerInfo.GuildName))
                {
                    GameCenter.messageMng.AddClientMsg(408);
                    return;
                }
                GameCenter.uIMng.SwitchToUI(type);
            }
            else if (CurLivelyInfo.UISort == 2)
            {
                SubGUIType type = (SubGUIType)System.Enum.Parse(typeof(SubGUIType), CurLivelyInfo.UIType);
                GameCenter.uIMng.SwitchToSubUI(type);
            }
            //LivelyRef的where字段判断是否做自动寻路
            if (CurLivelyInfo.RefData.where != null && CurLivelyInfo.RefData.where.Count >= 3)//大于3,否则下标越界
            {
                CheckAutoPath(CurLivelyInfo.RefData.where);
            }
        }
    }
	protected void GetReward(GameObject go)
	{
		if(CurMustDoInfo != null && CurMustDoInfo.MustDoState == DailyMustDoInfo.RewardState.CANGET)
		{
			GameCenter.dailyMustDoMng.C2S_ReqGetMustDoReward(CurMustDoInfo.ID);
		}
	}

	public DailyMustDoItemUI CreateNew(Transform _parent)
	{
		GameObject obj = Instantiate(this.gameObject) as GameObject;
		obj.transform.parent = _parent;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = Vector3.zero;
		return obj.GetComponent<DailyMustDoItemUI>();
	}
    /// <summary>
    /// LivelyRef的where字段判断是否自动寻路
    /// </summary>
    void CheckAutoPath(List<int> _list)
    {
            int id = _list[0];
            int pos_x = _list[1];
            int pos_y = _list[2];
            if (GameCenter.curMainPlayer.GoTraceTarget(id, pos_x, pos_y))
            {
                //Debug.Log("老司机开车");
            }
            else
            {
                Debug.LogError("无法寻路到指定位置");
            }
    }

    public void SetData(GuildLivelyInfo _data)
    {
        CurLivelyInfo = _data;
        if (labName != null) labName.text = "[u]" + _data.Name;
        string finishNum = _data.FinishTimes.ToString() + "/" + _data.TotalTimes.ToString();
        if (labDes != null) labDes.text = _data.Des.Replace("#1", finishNum);
        if (labFinishTimes != null)
        {
            labFinishTimes.enabled = false;  
        }
        string str = ConfigMng.Instance.GetUItext(137);
        if (addLivelyNum != null)
        {
            addLivelyNum.text = str + "+" + _data.StaticLivelyCount.ToString();
            addLivelyNum.enabled = true;
        }
        if (blackMask != null) blackMask.SetActive(_data.IsFinished);
        if (btnGo != null) btnGo.gameObject.SetActive(_data.UISort != 0 && !_data.IsFinished);
    }
}
