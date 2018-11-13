//======================================================
//作者:何明军
//日期:2016/7/6
//用途:提示
//======================================================
using UnityEngine;
using System.Collections;

public class FloatTipObject : MonoBehaviour {
	
	public GameObject ordinary;
    public UILabel labelText;
    public UISprite spriteFrame;
	
	public GameObject upgrade;
	public UILabel upText;

    public GameObject item;
    public ItemUI equInfo;
    public UISprite itemBg;
    public UILabel itemCount;

    public GameObject taskFinish;

    public GameObject fightUp;
    public GameObject fightDown;
    public UILabel fightUpLab;
    public UILabel fightDownLab;
    public UILabel curUpFightLab;
    public UILabel curDownFightLab;
    public GameObject effectUp;	  //特效加入 -by ms
    public GameObject effectDown;

	public float addStartValue = 1f;
	public float subtractStepValue = 10f;

	public EquipmentInfo equInfoItem;

	MsgRefData data = null;
	public MsgRefData Data{
		get{
			return data;
		}
		set{
			if(value != null){
//				if(data != null){
//					Debug.Log(data.messID == value.messID);
//				}
				data = value;
				if(data.sort == 12){
                    fightUp.SetActive(false);
                    fightDown.SetActive(false);
					upgrade.SetActive(false);
					ordinary.SetActive(false);
					taskFinish.SetActive(false);
                    if (equInfoItem != null)
                    {
                        itemCount.text = equInfoItem.ItemStrColor + equInfoItem.ItemName + "x" + equInfoItem.StackCurCount;
						itemBg.width = itemCount.width;
                        equInfo.FillInfo(equInfoItem);
                        equInfo.ShowTooltip = false;
                        item.SetActive(true);
                    }
				}else if(data.sort == 11){
                    fightUp.SetActive(false);
                    fightDown.SetActive(false);
                    taskFinish.SetActive(false);
					upgrade.SetActive(true);
                    EventDelegate.Add(upgrade.GetComponent<TweenScale>().onFinished, OnFinishEnd);
					//upgrade.GetComponent<TweenScale>().onFinished = OnFinishEnd;
					ordinary.SetActive(false);
					item.SetActive(false);
					upText.text = data.messStr;
				}
                else if (data.sort == 13)
                {
					if(curUpFightLab != null)curUpFightLab.text = (GameCenter.mainPlayerMng.MainPlayerInfo.CurFightVal - int.Parse(data.messStr)).ToString();
					if(fightUpLab != null)fightUpLab.text = data.messStr;
                    fightUp.SetActive(true);
                    fightDown.SetActive(false);
                    upgrade.SetActive(false);
                    ordinary.SetActive(false);
                    item.SetActive(false);
                    taskFinish.SetActive(false);
                    if (effectUp!=null) PlayEff(effectUp);
					if(curUpFightLab != null)TweenerUILabel.Begin(curUpFightLab.gameObject, 1f, GameCenter.mainPlayerMng.MainPlayerInfo.CurFightVal, GameCenter.mainPlayerMng.MainPlayerInfo.CurFightVal - int.Parse(data.messStr)).delay = data.stopTime;
					if(fightUpLab != null)TweenerUILabel.Begin(fightUpLab.gameObject, 1f, 0, int.Parse(data.messStr)).delay = data.stopTime;
                   // EventDelegate.Add(fightUp.GetComponent<TweenScale>().onFinished, OnFinishEnd);

                }
                else if (data.sort == 14)
                {
                    curDownFightLab.text = (GameCenter.mainPlayerMng.MainPlayerInfo.CurFightVal + int.Parse(data.messStr)).ToString();
                    fightDownLab.text = data.messStr;
                    fightUp.SetActive(false);
                    fightDown.SetActive(true);
                    upgrade.SetActive(false);
                    ordinary.SetActive(false);
                    PlayEff(effectDown);
                    item.SetActive(false);
                    taskFinish.SetActive(false);
                    TweenerUILabel.Begin(curDownFightLab.gameObject, 1f, GameCenter.mainPlayerMng.MainPlayerInfo.CurFightVal, GameCenter.mainPlayerMng.MainPlayerInfo.CurFightVal + int.Parse(data.messStr)).delay = data.stopTime;
                    TweenerUILabel.Begin(fightDownLab.gameObject, 1f, 0, int.Parse(data.messStr)).delay = data.stopTime;
                    //EventDelegate.Add(fightDown.GetComponent<TweenScale>().onFinished, OnFinishEnd);
                }
                else if (data.sort == 15)
                {
                    fightUp.SetActive(false);
                    fightDown.SetActive(false);
                    upgrade.SetActive(false);
                    taskFinish.SetActive(true);
                    EventDelegate.Add(taskFinish.GetComponent<TweenScale>().onFinished, OnFinishEnd);
                    ordinary.SetActive(false);
                    item.SetActive(false);
                }
                else
                {
                    fightUp.SetActive(false);
                    fightDown.SetActive(false);
                    upgrade.SetActive(false);
                    ordinary.SetActive(true);
                    item.SetActive(false);
                    taskFinish.SetActive(false);
                    if (data.font != "0")
                    {
						Object obj = exResources.GetResource(ResourceType.FONT,data.font);
						if (obj != null)
                        {
							UIFont font = obj as UIFont;
                            labelText.bitmapFont = font;
                        }
                        obj = null;
                    }
                    labelText.text = data.messStr;
                    labelText.transform.localScale = new Vector3(1, 1, 1);
                    labelText.fontSize = data.size;
                    //另外一种方式判定文本的长度
                    if (labelText.localSize.x > 450)
                        spriteFrame.transform.localScale = new Vector3(2, 1, 0);
                    else
                        spriteFrame.transform.localScale = new Vector3(1, 1, 0);
                    spriteFrame.gameObject.SetActive(data.showBg);
                }
			}
		}
	}
	
	public System.Action<FloatTipObject> onFinish;
	
	//服务端提示区分，浮动开始无结束事件与浮动结束事件
	bool isOnFinish = false;
	
	/// <summary>
	/// 客服端错误提示的浮动提示UI显示
	/// </summary>
    public IEnumerator StartShow()
    {
		if(data != null){//客服端配表
			float time = 0;
			if(data.showType.Contains(1)){
				time = data.stopTime;
				isOnFinish = (data.showType.Count > 1);
				AddTween(0);
			}
	        yield return new WaitForSeconds(time);
	        if (!isDestroyed)
	        {
	            if(data.acceleration != 0){
//					values = 30;
					isAcceleration = true;
				}else{
                    //解决物品提示位置 不一致的BUG by ljq
                    if (transform.localPosition.y != data.flowStartV3.y && data.sort == 12)
                        transform.localPosition = new Vector3(transform.localPosition.x,data.flowStartV3.y,transform.localPosition.z);
					Vector3 p = transform.localPosition;				
					TweenPosition tp = TweenPosition.Begin(gameObject,data.moveTime,p + data.flowEndV3);
					time = data.moveTime;

					if(data.showType.Contains(2)){
						isOnFinish = true;
						AddTween(time);
					}
					
					if(data.showType.Contains(3)){
						yield return new WaitForSeconds(time);
						isOnFinish = false;
						time = data.stopTime;
						AddTween(0);
					}else{
						isOnFinish = false;
                        EventDelegate.Add(tp.onFinished, OnFinishEnd);//事件被注册到List<EventDelegate>中，所以要掉用这个方法 by易睿
					}
				}
	        }
		}
    }
	//服务端与客服端文字提示
	public IEnumerator StartShow(string text,bool showFrame=true)
    {
		labelText.text = text;
		if(!labelText.enabled || !labelText.gameObject.activeSelf){
			labelText.enabled = true;
			labelText.gameObject.SetActive(true);
		}
		ordinary.SetActive(true);
        spriteFrame.gameObject.SetActive(showFrame);
        yield return new WaitForSeconds(3);
		isOnFinish = false;
        if (!isDestroyed)
        {
            Vector3 p=transform.localPosition;
            TweenPosition.Begin(gameObject,0.4f,new Vector3(p.x,p.y+20f,p.z));
            TweenAlphaAllChild ta = TweenAlphaAllChild.Begin(gameObject, 0.4f, 0);
            EventDelegate.Add(ta.onFinished, OnFinishEnd);
            //ta.onFinished = OnFinishEnd;
        }
	}
	//是否有加速度
	bool isAcceleration = false;
	int acceleNum = 0;
//	float values = 0;
	void Update(){
		if(isAcceleration){
			acceleNum ++;
			gameObject.transform.localPosition += new Vector3(0, (data.acceleration > 0 ? addStartValue : subtractStepValue) + data.acceleration * acceleNum,0);
			if(gameObject.transform.localPosition.y >= data.flowEndV3.y){
				isAcceleration = false;
				isOnFinish = false;
				if(data.showType.Contains(2)){
					AddTween(data.stopTime);
				}else{
					End();
				}
				acceleNum = 0;
			}
		}
	}
	
	void AddTween(float time){
		if(time == 0){
			End();
			return ;
		}
		if(data.showType.Contains(4)){
			TweenAlphaAllChild ta = TweenAlphaAllChild.Begin(gameObject,time, 0);
            EventDelegate.Add(ta.onFinished, OnFinishEnd);
	        //ta.onFinished = OnFinishEnd;
		}else if(data.showType.Contains(5)){
			TweenAlphaAllChild ta = TweenAlphaAllChild.Begin(gameObject,time/2, 1);
			ta.alpha = 0;
			ta.from = 0;
			ta.style = UITweener.Style.PingPong;
			CancelInvoke("End");
			Invoke("End",time);
		}else if(data.showType.Contains(6)){
			TweenScale ts = TweenScale.Begin(gameObject,time, new Vector3(1.1f,1.1f,1.1f));
			ts.method = UITweener.Method.BounceIn;
			CancelInvoke("End");
			Invoke("End",time);
		}else if(data.showType.Contains(7)){
			Vector3 p = transform.localPosition;
			TweenPosition.Begin(gameObject,time,p + data.flowLocaV3);
			TweenScale ts = TweenScale.Begin(gameObject,time, Vector3.zero);
            EventDelegate.Add(ts.onFinished, OnFinishEnd);
		}
	}
	
	void End(){
		if(onFinish != null && !isOnFinish){
			isOnFinish = true;
			onFinish(this);
		}
	}
	
	public void   OnFinishEnd(){
		Destroy(TweenScale.current);
		End();
	}

     private bool isDestroyed;
     void OnDestroy()
     {
         isDestroyed=true;
     }
     //播放特效 -by ms
     void PlayEff(GameObject eff)
     {
         var effect = Instantiate(eff);
         effect.transform.parent = eff.transform.parent;
         effect.transform.localPosition = eff.transform.localPosition;
         effect.transform.localScale = eff.transform.localScale;
         effect.SetActive(true);
     }
}
