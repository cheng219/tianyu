/// <summary>
///  何明军
/// 2016/4/19
/// 转生界面
/// </summary>

using UnityEngine;
using System.Collections;

public class ReincarnationItemUI : MonoBehaviour {

	public UILabel[] attributes;
	
	public ReinSkillUI skills;
	
	public ReinSkillUI skillsPop;
	
	public void SetContent(SuperLifeRef cur,SuperLifeRef next = null){
		if(cur != null){
			for(int i = 0,len = attributes.Length;i<len;i++){
				if(attributes[i] != null){
					if(next == null){
						SetLabelText(attributes[i],cur.attr[i],i,cur);
					}else{
						SetLabelText(attributes[i],i,cur,next);
					}
				}
			}
		}else{
			for(int i = 0,len = attributes.Length;i<len;i++){
				if(attributes[i] != null){
					if(next == null){
						SetLabelText(attributes[i],0,i,cur);
					}else{
						SetLabelText(attributes[i],i,cur,next);
					}
				}
			}
		}
		if(skills !=null ){
			if(next != null && next.unlock.Count > 0){
				for(int i=0;i<next.unlock.Count;i++){
					if(GameCenter.mainPlayerMng.MainPlayerInfo.Prof == next.unlock[i].prof){
						skills.CurSkillInfo = new SkillInfo(next.unlock[i].skillID,1);
						skillsPop.gameObject.SetActive(false);
						skillsPop.CurSkillInfo = skills.CurSkillInfo;
	//					UIEventListener.Get(skills.gameObject).onClick = delegate {
	//						if(skillsPop != null)skillsPop.gameObject.SetActive(true);
	//					};
					}
				}
			}else{
				skills.CurSkillInfo = null;
			}
		}
	}
	
	void SetLabelText(UILabel label,int val,int index,SuperLifeRef cur){
		if(index == 0){
			label.text = val > 0 ? cur.attr[index] +"-"+cur.atk2 : val.ToString();
		}else if(index == 1){
			label.text = val > 0 ? cur.attr[index]+"-"+cur.def2 : val.ToString();
		}else{
			label.text = val.ToString();
		}
	}
	
	void SetLabelText(UILabel label,int index,SuperLifeRef cur,SuperLifeRef next = null){
		if(index == 0){
			label.text = "("+(next.attr[index]-cur.attr[index]) +"-"+(next.atk2-cur.atk2)+")";
		}else if(index == 1){
			label.text = "("+(next.attr[index]-cur.attr[index]) +"-"+(next.def2-cur.def2)+")";
		}else{
            label.text = next.attr[index].ToString();
		}
	}
}
