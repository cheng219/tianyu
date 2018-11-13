//======================================================
//作者:何明军
//日期:2016/7/6
//用途:客服界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerServiceWnd : SubWnd {
	
	
	public QuestionsReplyUI sample;
	
	public QuestionsReplyUI curQuestions;
	
	UIGrid grid;
	
	List<QuestionsReplyUI> gameList = new List<QuestionsReplyUI>();
	
	void Awake(){
		gameList.Add(sample);
	}
	
	void ShowList(){
		if(sample == null){
			Debug.LogError("预制上sample引用为空!");
			return;
		}
		List<QuestionsReplyInfo> dataList = GameCenter.mainPlayerMng.QuestionList;
		QuestionsReplyUI ety = null;
		if(grid != null)grid.maxPerLine = dataList.Count;
		int i=0;
		for(;i<dataList.Count;i++){
			if(gameList.Count > i){
				ety = gameList[i];
			}else{
				GameObject go = Instantiate(sample.gameObject);
				go.transform.parent = sample.transform.parent;
				go.transform.localScale = Vector3.one;
				go.name = "QuestionsReplyUI_" + i;
				ety = go.GetComponent<QuestionsReplyUI>();
				if(ety != null)gameList.Add(ety);
			}
			ety.DataInfo = dataList[i];
			UIToggle tog = ety.GetComponent<UIToggle>();
			if(tog != null){
				tog.startsActive = i==0;
				tog.value = i==0;
				EventDelegate.Remove(tog.onChange,OnQuestionsChange);
				EventDelegate.Add(tog.onChange,OnQuestionsChange);
			}
			ety.gameObject.SetActive(true);
		}
		for(;i<gameList.Count;i++){
			gameList[i].gameObject.SetActive(false);
		}
		if(grid != null)grid.repositionNow = true;
		
		GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.CUSTOMERSERVICE,false);
	}
	
	void OnQuestionsChange(){
		int i=0;
		for(;i<gameList.Count;i++){
			UIToggle tog = gameList[i].GetComponent<UIToggle>();
			if(tog != null && tog.value){
				curQuestions.DataInfo = gameList[i].DataInfo;
				if(gameList[i].DataInfo.GetPathType == QuestionsReplyInfo.PathType.REPLY)SetQuestionsInfo(gameList[i].DataInfo);
				return ;
			}
		}
	}

	protected override void OnOpen ()
	{
		base.OnOpen ();
		if(sample == null){
			Debug.LogError("预制上sample引用为空!");
			return;
		}
		if(grid == null){
			grid = sample.transform.parent.GetComponent<UIGrid>();
		}
		
		if(GameCenter.mainPlayerMng.IsUpdateQuestionList){
			SetQuestionsInfo(null);
			GameCenter.mainPlayerMng.IsUpdateQuestionList = false;
		}

		GameCenter.mainPlayerMng.OnQuestionsReplyUpdate += ShowList;
		GameCenter.mainPlayerMng.OnQuestionsReplyRequest += SetQuestionsInfo;
		ShowList();
		
//		SetQuestionsInfo(null);
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
		GameCenter.mainPlayerMng.OnQuestionsReplyUpdate -= ShowList;
		GameCenter.mainPlayerMng.OnQuestionsReplyRequest -= SetQuestionsInfo;
	}
		
	
	#region 下载客服数据
	
	void SetQuestionsInfo(QuestionsReplyInfo info){
		QuestionsReplyInfo.PathType pathType = QuestionsReplyInfo.PathType.NONE;
		string path = string.Empty;
		string text = string.Empty;
		if(info != null){
			pathType = info.GetPathType;
			path = info.GetPath(pathType);
		}else{
			info = new QuestionsReplyInfo();
			pathType = QuestionsReplyInfo.PathType.QUESTIONSALL;
			path = info.GetPath(pathType);
		}
		if(path.Equals(string.Empty)){
			return ;
		}
		GetQuestionsInfo(path,(x)=>{
			text = x;
			if(string.IsNullOrEmpty(text)){
				Debug.LogError("下载客服数据失败，路径= ："+path);
				return ;
			}
			int reportid = 0 ;
			switch(pathType){
			case QuestionsReplyInfo.PathType.QUESTIONSALL:
//				Debug.Log("text   = " + text);
				
				LitJson.JsonData jsonData = LitJson.JsonMapper.ToObject(text);
				if (jsonData != null && text.Contains("result"))
				{
					if ((int)jsonData["result"] != 0)
					{
						Debug.LogError("下载客服数据失败 ："+text);
						return ;
					}
				}
				if(text.Contains("count")){
					int count = (int)jsonData["count"];
					if(text.Contains("reports")){
						LitJson.JsonData reports = jsonData["reports"];
						for(int i=0;i<count;i++){
							LitJson.JsonData reportsEty = reports[i];
							int id = (int)reportsEty["reportId"];
							string infodes = (string)reportsEty["info"];
							string time = (string)reportsEty["reportTime"];
							string reply = (string)reportsEty["reply"];
							int type = (int)reportsEty["type"];
							int assess = (int)reportsEty["assess"];
							QuestionsReplyInfo question = new QuestionsReplyInfo(id,assess,type,time,reply,infodes);
							GameCenter.mainPlayerMng.AddQuestion(question);
						}
					}
				}
				ShowList();
				break;
			case QuestionsReplyInfo.PathType.EVALUATE:
				GameCenter.mainPlayerMng.UpdateQuestion(info.ID,info.Evaluation);
				ShowList();
				break;
			case QuestionsReplyInfo.PathType.REPLY:
				reportid = 0;
				string replydes = string.Empty;
				if(reportid > 0)GameCenter.mainPlayerMng.UpdateQuestion(reportid,replydes);
				List<QuestionsReplyInfo> dataList = GameCenter.mainPlayerMng.QuestionList;
				QuestionsReplyUI ety = null;
				if(grid != null)grid.maxPerLine = dataList.Count;
				int j=0;
				for(;j<dataList.Count;j++){
					if(gameList.Count > j){
						gameList[j].DataInfo = dataList[j];
					}
				}
				break;
			case QuestionsReplyInfo.PathType.REPROT:
				string[] strReport = text.Split('|');
				if(strReport.Length >=3 ){
					if(!strReport[0].Equals("0"))return ;
					info.Update(strReport[1],strReport[2]);
					GameCenter.mainPlayerMng.AddQuestion(info);
					ShowList();
				}
				break;
			default:
				break;
			}
		});
	}
	
	void GetQuestionsInfo(string path,System.Action<string> OnComplete){
		if(!string.IsNullOrEmpty(path)){
			StartCoroutine(Request(path,OnComplete));
		}else{
			OnComplete(string.Empty);
		}
	}
	
	IEnumerator Request(string path,System.Action<string> OnComplete){
//		Debug.Log("KeFu :  path ==  "+path);
		WWW www = new WWW(path);
		int serlizeID = (int)NetMsgMng.CreateNewSerializeID();
		GameCenter.msgLoackingMng.UpdateSerializeList(serlizeID, true);
		yield return www;
		if(www.isDone){
//			Debug.Log("Url   +++  " + www.url);
			if(!string.IsNullOrEmpty(www.error)){
				Debug.LogError("下载客服数据失败www.error ："+www.error);
				OnComplete(string.Empty);
			}else{
				GameCenter.msgLoackingMng.UpdateSerializeList(serlizeID, false);
				
				if(!string.IsNullOrEmpty(www.text)){
					string content = www.text;
					OnComplete(content);
				}else{
					Debug.LogError("下载客服数据的内容不对www.text为 ："+www.text);
					OnComplete(string.Empty);
				}
			}
		}
//		www.Dispose();
	}
	
	void OnDestroy(){
		this.StopAllCoroutines();
		int i=0;
		for(;i<gameList.Count;i++){
			UIToggle tog = gameList[i].GetComponent<UIToggle>();
			if(tog != null){
				EventDelegate.Remove(tog.onChange,OnQuestionsChange);
			}
		}
		
	}
	#endregion
}
