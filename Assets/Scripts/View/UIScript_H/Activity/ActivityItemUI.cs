/// <summary>
/// 何明军
/// 2016/6/22
/// 活动
/// </summary>

using UnityEngine;
using System.Collections;

public class ActivityItemUI : MonoBehaviour {

	public UILabel name;
	public UILabel num;
	public UILabel numDes;
	public UILabel des;
	public UILabel lev;
	
	public UISprite icon;
	public GameObject stateE;
	public GameObject stateIn;
	public GameObject stateNo;
	public GameObject stateTwoNo;
	
	public bool isInMainMapWnd = false;
	public UITimer time;
	/// <summary>
	/// 活动进行中，才可监听该活动结束事件
	/// </summary>
	public System.Action<GameObject> OnOverTime;
	
	ActivityListRef data;
	public ActivityListRef SetData{
		get{
			return data;
		}
		set{
			if(value != null){
				data = value;
				Show();
				ShowState();
			}
		}
	}
	
	ActivityDataInfo SerVerData{
		get{
			return GameCenter.activityMng.GetActivityDataInfo(data.id);
		}
	}
    void Show(){
		if(name != null)name.text = data.name;
		if(des != null)des.text = data.typeres.Replace("\\n","\n");
		if(icon != null){
			icon.spriteName = data.icon;
            icon.MakePixelPerfect();
			UIButton btn = gameObject.GetComponent<UIButton>();
            if(btn != null) btn.normalSprite = data.icon;
		}
		if(SerVerData == null)return ;
		if(lev != null){
			AttributeRef attributeRef = ConfigMng.Instance.GetAttributeRef(data.level == 0 ? 1 : data.level);
			if(attributeRef.reborn > 0){
				lev.text = ConfigMng.Instance.GetUItext(12,new string[2]{attributeRef.reborn.ToString(),attributeRef.display_level.ToString()});
			}else{
				lev.text = ConfigMng.Instance.GetUItext(13,new string[1]{attributeRef.display_level.ToString()});
			}
			lev.color = SerVerData.ActivityLev ? Color.white : Color.red;
		}
		int times  = SerVerData.Times;
		if( num != null)num.text = SerVerData.Num + "/" + times;
		if(numDes != null)numDes.enabled = times == 0;
		if(num != null)num.enabled = times != 0;
	}
	void ShowState(){
		ActivityState state = ActivityState.NOTATTHE;
        //Debug.Log("SerVerData.ID" + SerVerData.ID+"...."+ SerVerData.State);
		if(SerVerData != null){
			state = SerVerData.State;
			UILabel lab = num;
			if(stateE != null)lab = stateE.GetComponent<UILabel>();
			if(lab != null)lab.text = data.nameend;
			if(stateIn != null)lab = stateIn.GetComponent<UILabel>();
			if(lab != null)lab.text = data.namestart;
			if(stateNo != null)lab = stateNo.GetComponent<UILabel>();
			if(lab != null)lab.text = data.nameready;
			if(stateE != null)stateE.SetActive(state == ActivityState.HASENDED); 
			if(stateIn != null)stateIn.SetActive(state == ActivityState.ONGOING); 
			if(stateNo != null)stateNo.SetActive(state == ActivityState.NOTATTHE && !isInMainMapWnd); 
			if(stateTwoNo != null)stateTwoNo.SetActive(false); 
			
			if(time != null){
				time.gameObject.SetActive(false);
			}
            //时间改为延迟服务器一分钟 
            int aTime = SerVerData.UpDateTime;
//			Debug.Log(data.name + "_活动_"+aTime +"_状态_"+state);
			if(isInMainMapWnd){
                if (state == ActivityState.NOTATTHE && aTime == ActivityMng.ShowTime)//活动前五分钟，进行全服公告提示
                {
                    //Debug.Log("进行开服活动前五分钟全服公告提示！！！");
                    ChatInfo info = new ChatInfo(70, data.name,1);
                    GameCenter.chatMng.UpdateChatContent(info);
                    if (GameCenter.chatMng.ChatInfoOutEvent != null) GameCenter.chatMng.ChatInfoOutEvent(info);
                    if ((int)GameCenter.chatMng.CurChatType == info.chatTypeID || GameCenter.chatMng.CurChatType == (int)ChatInfo.Type.All && info.chatTypeID != (int)ChatInfo.Type.Private)
                    {
                        if (GameCenter.chatMng.SelectChatTypeEvent != null)
                            GameCenter.chatMng.SelectChatTypeEvent((int)GameCenter.chatMng.CurChatType);
                    }
                }
				if(state == ActivityState.ONGOING){
					if(aTime > 0){
                        ActivityWnd wnd = GameCenter.uIMng.GetGui<ActivityWnd>();
                        if (wnd == null && !GameCenter.activityMng.haveTipDic.ContainsKey(data.id)&& !GameCenter.activityMng.ActivityOnGoingList.ContainsKey(data.id))
                        {
                            GameCenter.activityMng.ActivityOnGoingList.Add(data.id,data);
                            if (GameCenter.activityMng.OnActivityOnGoing != null)
                            {
                                GameCenter.activityMng.OnActivityOnGoing();
                            }
                        }
						CancelInvoke("ShowState");
						Invoke("ShowState",(float)aTime);
					}
				}else if(state == ActivityState.NOTATTHE && aTime <= ActivityMng.ShowTime){
					if(time != null){
						time.gameObject.SetActive(true);
						time.StartIntervalTimer(aTime);
						if(aTime > 0){
							CancelInvoke("ShowState");
							Invoke("ShowState",(float)aTime);
						}
					}
				}else if(state == ActivityState.HASENDED || (state == ActivityState.NOTATTHE && aTime > ActivityMng.ShowTime)){
					if(OnOverTime != null){
						OnOverTime(gameObject);
                    }
                else if(state == ActivityState.HASENDED)
                    {
                        //Debug.Log("抛出活动结束事件");
                        GameCenter.activityMng.ActivityOnGoingList.Remove(data.id);
                        GameCenter.activityMng.haveTipDic[data.id] = data;
                        GameCenter.activityMng.OnActivityOver();
                    }
				}
			}else{
				if(aTime > 0){
					CancelInvoke("ShowState");
					Invoke("ShowState",(float)aTime);
				}
			}
		}
	}
	
	void OnDisable(){
		CancelInvoke("ShowState");
    }
	#region 主界面MainMapActiveUI显示补助逻辑
	
	public void SetMainWndItemTime(int times){
		if(time != null && isInMainMapWnd){
			time.StartIntervalTimer(times);
		}
	}
	#endregion;
}
