/// <summary>
/// 何明军
/// 2016/8/19
/// UIToggle 补充组件。
/// 重一级界面预制中剥离出二级界面，并在打开二级界面时加载，解决打开界面卡顿
/// </summary>


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIToggleCheckStateSelect : MonoBehaviour {

	public UIToggle[] togs;
	public SubGUIType[] subWndType;
	public GUIBase guiBase;
	public string[] subWndPath;
	[System.NonSerialized]bool isStart = false;
	[System.NonSerialized]SubGUIType curSubGUIType = SubGUIType.NONE;
	[System.NonSerialized]SubGUIType initSubGUIType = SubGUIType.NONE;
	void Awake(){
		if(guiBase != null && guiBase.subWndArray.Length <= 0)guiBase.subWndArray = new SubWnd[subWndType.Length];
		for(int i=0,len=togs.Length;i<len;i++){
			if(togs[i] != null)EventDelegate.Add(togs[i].onChange, onChange);
		}
		isStart = false;
		
	}
	
	void Start(){
		if(togs == null || togs.Length == 0 || guiBase == null){
			return ;
		}
		if(guiBase.InitSubGUIType != SubGUIType.NONE){
			for(int i=0,len=togs.Length;i<len;i++){
				if(togs[i] != null && togs[i].gameObject.activeSelf && subWndType.Length > i){
					if(subWndType[i] == guiBase.InitSubGUIType){
						togs[i].startsActive = true;
					}else{
						togs[i].startsActive = false;
					}
				}
			}
		}
	}

	void Update(){
		if(!isStart){
			isStart = true;
			onChange();
			initSubGUIType = guiBase.InitSubGUIType;
			curSubGUIType = guiBase.CurSubGUIType;
		}else{
			OpenSubWnd();
		}
	}
	
	void OpenSubWnd(){
		if(initSubGUIType != guiBase.InitSubGUIType){
			initSubGUIType = guiBase.InitSubGUIType;
			for(int i=0;i<subWndType.Length;i++){
				if(initSubGUIType == subWndType[i]){
					togs[i].value = true;
					return ;
				}
			}
		}
		if(curSubGUIType != guiBase.CurSubGUIType){
			curSubGUIType = guiBase.CurSubGUIType;
			for(int i=0;i<subWndType.Length;i++){
				if(guiBase.CurSubGUIType == subWndType[i]){
					togs[i].value = true;
					return ;
				}
			}
		}
	}
	
	void OnDisable(){
		if(guiBase != null)
		for(int i=0,len=guiBase.subWndArray.Length;i<len;i++){
			if(guiBase.subWndArray[i] != null && guiBase.subWndArray[i].gameObject.activeSelf){
				guiBase.subWndArray[i].CloseUI();
			}
		}
	}

	void OnDestroy(){
		if(togs == null || togs.Length == 0 || guiBase == null){
			return ;
		}
		for(int i=0,len=togs.Length;i<len;i++){
			if(togs[i] != null)EventDelegate.Remove(togs[i].onChange, onChange);
		}
	}

	void onChange(){
		if(!isStart || guiBase== null)return;
		for(int i=0,len=togs.Length;i<len;i++){
			if(togs[i] != null && togs[i].gameObject.activeSelf){
				if(guiBase.subWndArray.Length > i){
					if(togs[i].value){
						ShowSubWnd(i,true);
					}else{
						if(guiBase.subWndArray[i] != null)ShowSubWnd(i,false);
					}
				}
			}
		}
	}
	[System.NonSerialized]Object obj;
	[System.NonSerialized]GameObject wnd;
	protected void ShowSubWnd(int _index,bool _show){
		if(guiBase== null)return ;
		if(guiBase.subWndArray.Length > _index && guiBase.subWndArray[_index] == null){
			if(subWndPath.Length > _index){
				obj = exResources.GetResource(ResourceType.GUI,subWndPath[_index]);
				if(obj == null)return ;
				wnd = (GameObject)Instantiate(obj);
				wnd.transform.parent = guiBase.transform;
				wnd.transform.transform.localPosition = Vector3.zero;
				wnd.transform.transform.localScale = Vector3.one;
				guiBase.subWndArray[_index] = wnd.GetComponent<SubWnd>();
				wnd.SetActive(false);
				obj = null;
				wnd = null;
			}
		}
		if(guiBase.subWndArray[_index] == null)return ;
		if(!guiBase.subWndArray[_index].gameObject.activeSelf && _show)
			guiBase.subWndArray[_index].OpenUI();
		if(guiBase.subWndArray[_index].gameObject.activeSelf && !_show)
			guiBase.subWndArray[_index].CloseUI();
	}
}