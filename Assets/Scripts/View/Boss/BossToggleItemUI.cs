//===============================
//作者：邓成
//日期：2016/4/27
//用途：挑战BOSS标签显示类
//===============================

using UnityEngine;
using System.Collections;

public class BossToggleItemUI : MonoBehaviour {
	public UILabel labName;
	public UISprite tipPic;
	public UIToggle toggle;
	public UISprite background;
    public UISprite otherTipPic;//新增已通关的提示(今日已通关) By唐源
	protected System.Action<BossChallengeData> curCallback;
	protected BossChallengeData bossData;

	public void SetData(BossChallengeData data,System.Action<BossChallengeData> callback,bool isChecked)
	{
		bossData = data;
		curCallback = callback;
		if(labName != null)labName.text = data.bossName;
		if(tipPic != null)
		{
		    tipPic.enabled = data.CanKill;
		}
        //新增提示今日已通关 By唐源
        if (otherTipPic!=null)
        {
            //Debug.Log("今日已通关data.AlreadyCarnet:" + data.AlreadyCarnet);
            //otherTipPic.enabled = data.AlreadyCarnet;
            otherTipPic.gameObject.SetActive(data.AlreadyCarnet);
        }
		if(labName != null && data.CurBossRef != null)
			SetBackgroundColor((BossChallengeWnd.ToggleType)data.CurBossRef.type);
		if(toggle != null)
		{
			EventDelegate.Remove(toggle.onChange,OnChange);
			EventDelegate.Add(toggle.onChange,OnChange);
		}
	}

	public void SetData(int bossID,System.Action<BossChallengeData> callback,bool isChecked)
	{
		BossChallengeData data = GameCenter.bossChallengeMng.BossChallengeDic.ContainsKey(bossID)?GameCenter.bossChallengeMng.BossChallengeDic[bossID]:null;
		bossData = data;
		curCallback = callback;
		if(data != null)
		{
			if(labName != null)labName.text = data.bossName;
			if(tipPic != null)tipPic.enabled = data.CanKill;
            //if (otherTipPic != null) otherTipPic.enabled = data.AlreadyCarnet;
            if (data.CurBossRef.type == (int)BossChallengeWnd.ToggleType.SealBoss)
			{
				int startUp = (int)Time.realtimeSinceStartup;
				tipPic.enabled = data.CanKill && data.CurBossRef.needLevel <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel && data.AppearTime <= startUp;
			}else
			{
				tipPic.enabled = data.CanKill;
			}
            if (otherTipPic!=null)
            {
                //Debug.Log("进入到是否已通关");
                //Debug.Log("boss："+ data.bossName+"是否已通关"+ data.AlreadyCarnet);
                otherTipPic.gameObject.SetActive(data.AlreadyCarnet);
            }
            if (labName != null && data.CurBossRef != null)
				SetBackgroundColor((BossChallengeWnd.ToggleType)data.CurBossRef.type);
			if(toggle != null)
			{
				EventDelegate.Remove(toggle.onChange,OnChange);
				EventDelegate.Add(toggle.onChange,OnChange);
			}
		}
	}

    public void SetData(BossRef data)
    {
        string name = string.Empty;
        if(data != null)
        {
            MonsterRef mob = ConfigMng.Instance.GetMonsterRef(data.monsterId);
            if(mob != null)name = mob.name;
        }
        if (labName != null) labName.text = name;
        if (tipPic != null)
        {
            tipPic.enabled = false;
        }
        //新增提示今日已通关 By唐源
        if (otherTipPic != null)
        {
            otherTipPic.gameObject.SetActive(false);
        }
        if (labName != null)
            SetBackgroundColor(BossChallengeWnd.ToggleType.RongEBoss);
        if (toggle != null)
        {
            EventDelegate.Remove(toggle.onChange, OnChange);
            EventDelegate.Add(toggle.onChange, OnChange);
        }
    }

	void OnChange()
	{
		if(curCallback != null && bossData != null && toggle != null && toggle.value)
			curCallback(bossData);
	}

	public void SetChecked()
	{
		if(toggle != null)
		{
			toggle.value = true;
			EventDelegate.Execute(toggle.onChange);
		}
	}

	void SetBackgroundColor(BossChallengeWnd.ToggleType type)
	{
		Color color = Color.white;
		switch(type)
		{
		case BossChallengeWnd.ToggleType.SceneBoss:
			color = new Color(0f,1f,60f/255f);//00ff3c
			break;
		case BossChallengeWnd.ToggleType.UnderBoss:
			color = new Color(0f,252f/255f,1f);//00fcff
			break;
		case BossChallengeWnd.ToggleType.RongEBoss:
			color = new Color(1f,156f/255f,0f);//ff9c00
			break;
		case BossChallengeWnd.ToggleType.LiRongEBoss:
			color = new Color(1f,84f/255f,0f);//ff5400
			break;
		case BossChallengeWnd.ToggleType.SealBoss:
			color = new Color(1f,252f/255f,0f);//fffc00
			break;
		default:
			break;
		}
		if(labName != null)
			labName.color = color;
	}

	public void ClearData()
	{
		curCallback = null;
		bossData = null;
		if(toggle != null)
			toggle.value = false;
		gameObject.SetActive(false);
	}
}
