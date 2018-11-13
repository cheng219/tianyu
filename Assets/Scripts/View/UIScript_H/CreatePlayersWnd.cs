//======================================================
//作者:何明军
//日期:2016/6/24
//用途:创建角色界面
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatePlayersWnd : GUIBase {

	public GameObject[] players;
	public UISprite[] playersIcon;
	public GameObject btnReturn;
	public GameObject btnCreate;
	
	public UIInput input;
	public GameObject btnName;
	public UITexture texture;
    private System.Random R = new System.Random();
	
	int curProf = 1;
	
	void Awake(){
		mutualExclusion = false;
		Layer = GUIZLayer.NORMALWINDOW;
		
		if(btnReturn != null)UIEventListener.Get(btnReturn).onClick = delegate {GameCenter.instance.GoPassWord();};
		
		if(btnName != null)UIEventListener.Get(btnName).onClick = delegate {UpdateNameValue();};
	}
//	protected List<string> nameList = new List<string>();
//	int index = 0;
	void UpdateNameValue(){
		int profCount = ConfigMng.Instance.ProfNameCount(curProf);
		int nameCount = ConfigMng.Instance.GetNameRefTable().Count;
		if(profCount <= 0 || nameCount <= 0){
			Debug.LogError("NameConfig配表数据不对，职业"+curProf+"的名字或者性配表长度为0，去找左文祥");
			return ;
		}
        //System.Random profRan = new System.Random();
        int profId = R.Next(1, profCount);
        //System.Random nameRan = new System.Random();
        int nameId = R.Next(1, nameCount);
		NameRef nameRef = ConfigMng.Instance.GetNameRef(nameId);
		string name = nameRef != null ? nameRef.Firstname : string.Empty;
		nameRef = ConfigMng.Instance.GetNameRef(profId);
		name += (nameRef != null && nameRef.names.Count >= curProf) ? nameRef.names[curProf - 1] : string.Empty;
//		index++;
//		if(nameList.Contains(name))
//		{
//			Debug.Log("随机到重复名字:"+name+",index:"+(index)+",nameId:"+nameId+",profId:"+profId);
//		}else
//		{
//			nameList.Add(name);
//		}
		input.value = name;
	}
	
	void ShowPlayerDes(GameObject games){
		curProf= (int)UIEventListener.Get(games).parameter;
		ShowPreviewSingle(curProf);
		UpdateNameValue();
	}
	
	void ShowPreviewSingle(int _prof){
		PlayerBaseInfo info =  new PlayerBaseInfo(_prof,_prof,true);
		if(info == null){
			return ;
		}
		CharacterCreateStage state = GameCenter.curGameStage as CharacterCreateStage;
		if(state != null)state.CurSelectRole = info;
        if (btnCreate != null)
        {
            UIEventListener.Get(btnCreate).parameter = info;
            UIEventListener.Get(btnCreate).onClick = Create;
        }
	}

    void Create(GameObject _go)
    {
        if (_go == null) return;
        PlayerBaseInfo info = UIEventListener.Get(_go).parameter as PlayerBaseInfo;
        if (!GameCenter.loginMng.FontHasAllCharacter(input.label.bitmapFont, input.value)||GameCenter.loginMng.CheckName(input.value))
        {
            GameCenter.messageMng.AddClientMsg(490);
            return;
        }
        GameCenter.loginMng.C2S_ReqCreateChar(info.Prof, input.value);
    }

	void ShowPlayers(){
		for(int i=0;i<players.Length;i++){
			if(players[i] != null){
				UIEventListener.Get(players[i]).onClick = ShowPlayerDes;
				UIEventListener.Get(players[i]).parameter = i+1;
			}
		}
	}
	
	protected override void OnOpen ()
	{
		base.OnOpen ();
		ShowPlayers();
		ShowPreviewSingle(curProf);
		UpdateNameValue();
		if(input!= null)EventDelegate.Add(input.onChange,OnChange);
	}
	
	protected override void OnClose ()
	{
		base.OnClose ();
//		GameCenter.previewManager.ClearModel();
		if(input!= null)EventDelegate.Remove(input.onChange,OnChange);
	}
	
	void OnChange(){
		string contents = GameCenter.loginMng.FontHasCharacter(input.label.bitmapFont,input.value);
		if(!string.IsNullOrEmpty(contents)){
			input.value = contents;
			GameCenter.messageMng.AddClientMsg(300);
		}
	}
}
