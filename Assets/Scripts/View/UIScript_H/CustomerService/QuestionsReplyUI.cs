//======================================================
//作者:何明军
//日期:2016/7/6
//用途:问题描述UI
//======================================================
using UnityEngine;
using System.Collections;

public class QuestionsReplyUI : MonoBehaviour {
	
	public UIInput questionTitil;
	public UIInput questionDes;
	public UIToggle[] questiontypes;
	public UILabel[] typeLabs;
	public UIButton questionSubmit;
	
	public UILabel time;
	public UILabel titil;
	public UILabel des;
	public UILabel typeDes;
	public UILabel replyDes;
	public UIToggle[] evaluations;
	public UIButton replySubmit;
	
	public GameObject isReply;
	public GameObject dontReply;
	
	public GameObject games;
	
	QuestionsReplyInfo info;
	
	public QuestionsReplyInfo DataInfo{
		get{
			return info;
		}
		set{
			info = value;
			Show();
		}
	}
	
	void OnEnable(){
		if(questionTitil != null)questionTitil.value = string.Empty;
		if(questionDes != null)questionDes.value = string.Empty;
		
		if(questionSubmit != null){
			UIEventListener.Get(questionSubmit.gameObject).onClick = delegate {
				for(int i=0;i<questiontypes.Length;i++){
					if(questiontypes[i] != null){
						UIToggle tog = questiontypes[i].GetComponent<UIToggle>();
						if(tog != null && tog.value){
							QuestionsReplyInfo questionInfo = new QuestionsReplyInfo(questionTitil.value,questionDes.value,i+1);
							if(GameCenter.mainPlayerMng.OnQuestionsReplyRequest != null)GameCenter.mainPlayerMng.OnQuestionsReplyRequest(questionInfo);
							return ;
						}
					}
				}
			};
		}
		Show();
		if(games != null)games.SetActive(false);
	}
	
	void Show(){
		if(info == null){
			return ;
		}
		if(games != null)games.SetActive(true);
		if(time != null)time.text = info.Time;
		if(titil != null)titil.text = info.Titil;
		if(des != null)des.text = info.Des;
		if(replyDes != null)replyDes.text = info.ReplyDes;
		if(typeDes != null)typeDes.text = info.Type >= typeLabs.Length ? string.Empty : typeLabs[info.Type].text;
		
		if(isReply != null)isReply.SetActive(info.IsReply);
		if(dontReply != null)dontReply.SetActive(!info.IsReply);
		if(replySubmit != null)replySubmit.gameObject.SetActive(info.Evaluation <= 0);
		for(int i=0;i<evaluations.Length;i++){
			if(evaluations[i] != null){
				if(info.Evaluation <= 0){
					evaluations[0].startsActive = true;
				}else{
					if(i+1 == info.Evaluation){
						evaluations[i].startsActive = true;
					}else{
						evaluations[i].startsActive = false;
					}
				}
				BoxCollider box = evaluations[i].GetComponent<BoxCollider>();
				box.enabled = info.Evaluation <= 0 && info.IsReply;
			}
		}
		
		if(replySubmit != null){
			UIEventListener.Get(replySubmit.gameObject).onClick = delegate {
				for(int i=0;i<evaluations.Length;i++){
					if(evaluations[i] != null){
						UIToggle tog = evaluations[i].GetComponent<UIToggle>();
						if(tog != null && tog.value){
							if(!info.IsReply && info.Evaluation <= 0){
								GameCenter.messageMng.AddClientMsg(381);
								return ;
							}
							QuestionsReplyInfo questionInfo = new QuestionsReplyInfo(DataInfo.ID,i+1);
							if(GameCenter.mainPlayerMng.OnQuestionsReplyRequest != null)GameCenter.mainPlayerMng.OnQuestionsReplyRequest(questionInfo);
							return ;
						}
					}
				}
			};
		}
	}
}
