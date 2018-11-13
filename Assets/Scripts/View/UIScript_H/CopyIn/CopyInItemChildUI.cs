/// <summary>
/// 何明军
/// 2016/4/7
/// 副本入口界面
/// </summary>
using UnityEngine;
using System.Collections;
public class CopyInItemChildUI : MonoBehaviour {

	public UILabel name;
	public UILabel lev;
	public UISprite[] star;
	public UISpriteEx bgSp;

    public UIToggle myToggle;
	public Color color;
	
	int copyRefID = 0;
	public int CopyRefID{
		get{
			return copyRefID;
		}
		set{
			copyRefID = value;
		}
	}

    public bool TogValue
    {
        get
        {
            return myToggle == null ? false : myToggle.value;
        }
    }
	
	public void SetStar(int copyGID){
		CopyInItemDataInfo serData=null;
		if(GameCenter.duplicateMng.CopyDic.TryGetValue(copyGID,out serData)){
			
			int curStar = serData.copyScenes.ContainsKey(CopyRefID) ? serData.copyScenes[CopyRefID].star : 0;
			for(int i=0,len=star.Length;i<len;i++){
				if(star[i] != null){
					if(i < curStar){
						star[i].gameObject.GetComponent<UISpriteEx>().IsGray = UISpriteEx.ColorGray.normal;
					}else{
						star[i].gameObject.GetComponent<UISpriteEx>().IsGray = UISpriteEx.ColorGray.Gray;
					}
				}
			}
			return ;
		}
		else{
			Debug.LogError("升级后没有服务端数据过来，找小唔知！");
		}
		
		for(int i=0,len=star.Length;i<len;i++){
			if(star[i] != null){
				star[i].gameObject.SetActive(false);
			}
		}
	}
	
	void OnEnable(){
        myToggle = GetComponent<UIToggle>();
		CopyRef refd = ConfigMng.Instance.GetCopyRef(CopyRefID);
		if(refd == null)return ;
		bool isShow = refd.lvId <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel;
		if(name != null){
			name.text = refd.difficulty;
			name.color = isShow ? color : Color.gray;
		}
		if(bgSp != null)bgSp.IsGray = isShow ? UISpriteEx.ColorGray.normal : UISpriteEx.ColorGray.Gray;
		gameObject.GetComponent<BoxCollider>().enabled = isShow;
		AttributeRef attributeRef = ConfigMng.Instance.GetAttributeRef(refd.lvId);
		if(attributeRef.reborn > 0){
			if(lev != null)lev.text = ConfigMng.Instance.GetUItext(10,new string[2]{attributeRef.reborn.ToString(),attributeRef.display_level.ToString()});
		}else{
			if(lev != null)lev.text = ConfigMng.Instance.GetUItext(11,new string[1]{attributeRef.display_level.ToString()});
		}
		if(lev != null)lev.color = isShow ? color : Color.gray;
	}
}
