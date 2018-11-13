//======================================================
//作者:何明军
//日期:2016/7/6
//用途:功能按钮开启控制中心,只支持到功能分页级红点，挂在GuiBase同一对象上
//======================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FunctionButtonCenter : MonoBehaviour {
 
    /// <summary>
    /// 功能按钮的模块组
    /// </summary>
	public FunctionButtonUI[] funcs;
	
	public FunctionChildsButtonUI[] parentFuncs;
	/// <summary>
	/// 附加的总红点,人物头像处的红点
	/// </summary>
	public FunctionType additionalTotalRedPoint = FunctionType.None;
	bool additionalTotalRed;
	public bool AdditionalTotalRed{
		set{
			if(value != additionalTotalRed){
				additionalTotalRed = value;
				if(GameCenter.mainPlayerMng != null)GameCenter.mainPlayerMng.SetFunctionRed(additionalTotalRedPoint,additionalTotalRed);
			}
		}
	}
	/// <summary>
	/// 多个功能红点控制同一个父红点,用组的形式表达
	/// </summary>
	Dictionary<int,ButtonChilds> buttonChilds = new Dictionary<int,ButtonChilds>();
	/// <summary>
	/// 组的结构
	/// </summary>
	private class ButtonChilds{
		public int group;
		public List<FunctionType> childs = new List<FunctionType>();
		
		public ButtonChilds(){}
		
		public ButtonChilds(int _group){
			group = _group;
		}
		
		public void AddChild(FunctionType type){
			if(!childs.Contains(type)){
				childs.Add(type);
			}
		}
		
		public void RemoveChild(FunctionType type){
			if(childs.Contains(type)){
				childs.Remove(type);
			}
		}
	}
	
	
	void Awake(){
        //Debug.Log("名字为------:"+this.gameObject.name+"的物体上挂有--------:"+ funcs.Length+"个FunctionButtonUI[]");
		if(GameCenter.instance == null)return ;
		bool funcLock = GameCenter.instance.NewFunctionLock;
        /// <summary>
        /// 如果功能按钮的模块组为空(该脚本所依附的物体上)通过GetComponentsInChildren返回子物体上所有的FunctionButtonUI组件数组
        /// 一般情况不会出现
        /// </summary>
        if (funcs == null || funcs.Length <= 0){
            //Debug.Log("名字为------:"+this.gameObject.name+"的物体上添加了该脚本但是FunctionButtonUI[]数组为空");
			funcs = gameObject.GetComponentsInChildren<FunctionButtonUI>();
		}
        /// <summary>
        /// 遍历数组判定其父物体显示红点
        /// </summary>
        for (int i=0;i<funcs.Length;i++){
			if(funcs[i] != null && funcs[i].func != FunctionType.None){
                //Debug.Log("名字为"+ funcs[i].gameObject.name+ "的物体的group为"+ funcs[i].group);
				if(funcs[i].parent != null && funcs[i].group > 0)funcs[i].parent.SetActive(!funcLock);
			}
		}
		
		FunctionDataInfo info = null;
		UIGrid grid = null;
        //遍历FunctionButtonUI类型的数组由类型去获取功能开启数据
        for (int i=0;i<funcs.Length;i++){
			if(funcs[i] != null && funcs[i].func != FunctionType.None){
                //Debug.Log("FunctionType   =  " + funcs[i].func + "   int    " + (int)funcs[i].func);
                info = GameCenter.mainPlayerMng.GetFunctionData(funcs[i].func);
				if(info == null){
					funcs[i].gameObject.SetActive(true);
					if(funcs[i].parent != null ){
						funcs[i].parent.SetActive(true);
						grid = funcs[i].parent.GetComponent<UIGrid>();
					}
					if(funcs[i].funcRed != null)funcs[i].funcRed.SetActive(false);
					if(funcs[i].parentRed != null)funcs[i].parentRed.SetActive(false);
				}else{
					funcs[i].TipDes = info.TipDes;
					//按钮显示
					if(funcLock){
						funcs[i].SetShow = info.IsOpon;
						if(funcs[i].parent != null ){
							if(!funcs[i].parent.activeSelf && info.IsOpon){
								funcs[i].parent.SetActive(true);
							}
							grid = funcs[i].parent.GetComponent<UIGrid>();
						}
					}else{
						funcs[i].SetShow = true;
						if(funcs[i].parent != null ){
							funcs[i].parent.SetActive(true);
							grid = funcs[i].parent.GetComponent<UIGrid>();
						}
					}
					//红点
					if(funcs[i].funcRed != null)funcs[i].funcRed.SetActive(info.FuncBtnRed);
					if(funcs[i].group > 0){
						if(!buttonChilds.ContainsKey(funcs[i].group)){
							buttonChilds[funcs[i].group] = new ButtonChilds(funcs[i].group);
						}
                        //如果子物体上有红点显示则以子物体上的FunctionButtonUI组件上的group字段为key 存储子物体上的功能类型
                        //用字典去存放功能列表 然后用功能列表的长度来判断父红点的显示
                        if (info.FuncBtnRed)buttonChilds[funcs[i].group].AddChild(funcs[i].func);
						if(funcs[i].parentRed != null){
							funcs[i].parentRed.SetActive(buttonChilds[funcs[i].group].childs.Count > 0);
						}else{
							Debug.LogError("预制"+funcs[i].name+"的父红点引用为空！");
						}
					}
				}
				if(grid != null)grid.repositionNow = true;
			}
		}
		int totalRedNum = 0;
		UIGrid parentGrid = null;
		for(int i=0;i<parentFuncs.Length;i++){
			if(parentFuncs[i] == null )continue ;
			parentGrid = parentFuncs[i].transform.parent.GetComponent<UIGrid>();
			int childNum = parentFuncs[i].childFuncs.Count;
            //设置初始状态(隐藏和RedNum置为0隐藏自身红点)
            parentFuncs[i].gameObject.SetActive(childNum == 0 ? true : false);
			parentFuncs[i].RedNum = 0;
            //在物体绑定的功能列表不为空的情况下判断自身红点的显隐以及统计红点计数
			if(childNum > 0){
				for(int j=0;j<childNum;j++){
                    //由功能类型获取功能数据
					info = GameCenter.mainPlayerMng.GetFunctionData(parentFuncs[i].childFuncs[j]);
					if(info == null){
						parentFuncs[i].gameObject.SetActive(true);
					}else{
						if(info.IsOpon){
							parentFuncs[i].gameObject.SetActive(true);
						}
						if(info.FuncBtnRed){
							parentFuncs[i].RedNum ++;
						}
					}
                    //统计每个物体上的功能红点计数
                    if (additionalTotalRedPoint != FunctionType.None)totalRedNum += parentFuncs[i].RedNum;
				}
			}
            //在人物红点功能类型不为None的情况下用功能红点统计来判定人物红点功能是否显示
			if(additionalTotalRedPoint != FunctionType.None)AdditionalTotalRed = totalRedNum > 0;
			if(!funcLock)parentFuncs[i].gameObject.SetActive(true);
			if(parentGrid != null)parentGrid.repositionNow = true;
		}
		
	}
	
	void OnEnable(){
		GameCenter.noviceGuideMng.UpdateFunctionData += UpdateFunctionData;
		GameCenter.mainPlayerMng.UpdateFunctionRed += UpdateFunctionRed;
        GameCenter.mainPlayerMng.UpdateServerOpen += UpdateServerOpen;
	}

	void OnDisable(){
		GameCenter.noviceGuideMng.UpdateFunctionData -= UpdateFunctionData;
		GameCenter.mainPlayerMng.UpdateFunctionRed -= UpdateFunctionRed;
        GameCenter.mainPlayerMng.UpdateServerOpen -= UpdateServerOpen;
	}


	void UpdateServerOpen(FunctionDataInfo _info,bool _b)
    {
        //后台活动开启时候红点显示以及功能图标排列
        UIGrid parentGrid = null;
        UIGrid funcsGrid = null;
        //循环遍历FunctionButtonUI[] 如果功能数据的类型与功能模块的类型相等显示此FunctionButtonUI上绑定的parent;
        for (int i = 0; i < funcs.Length; i++)
        {
			if (funcs[i] != null && funcs[i].func == _info.Type )
            {
                if (funcs[i].parent != null && !funcs[i].parent.activeSelf)
                {
                    funcs[i].parent.SetActive(true);
                    //获取父物体上的UIGrid 重新排列
					if (funcsGrid == null) funcsGrid = funcs[i].parent.transform.parent.GetComponent<UIGrid>();
					if (funcsGrid != null) funcsGrid.repositionNow = true;
                }
                //红点的显示
				funcs[i].SetShow = _b;
				if(funcs[i].funcRed != null)funcs[i].funcRed.SetActive(_info.FuncBtnRed);
				if(funcs[i].parentRed != null && _b)funcs[i].parentRed.SetActive(true);
				if (funcsGrid == null) funcsGrid = funcs[i].transform.parent.GetComponent<UIGrid>();
				if (funcsGrid != null) funcsGrid.repositionNow = true;
				break;
            }
        }
        //对主界面下方UI的逻辑处理
        //循环遍历FunctionChildsButtonUI[] 统计红点计数判定红点的显示 UIGrid重新排列
        for (int i = 0; i < parentFuncs.Length; i++)
        {
			if (parentFuncs[i] != null && parentFuncs[i].childFuncs.Contains(_info.Type))
            {
                parentFuncs[i].gameObject.SetActive(_b);
				if(_b){
					parentFuncs[i].RedNum ++;
				}
				if (parentGrid == null) parentGrid = parentFuncs[i].transform.parent.GetComponent<UIGrid>();
				if (parentGrid != null) parentGrid.repositionNow = true;
				break;
            }
        }

    }

	void UpdateFunctionData(FunctionDataInfo _info){
        //Debug.Log("更新功能"+ _info.Type);
		if(_info.Type == FunctionType.None)return ;
		UIGrid parentGrid = null;
		UIGrid funcsGrid = null;
        //功能开启的时候循环遍历FunctionButtonUI[] 如果功能数据的类型与功能模块的类型相等并且功能已经开启则显示此FunctionButtonUI上绑定的parent;
        for (int i=0;i<funcs.Length;i++){
			if(funcs[i] != null && funcs[i].func == _info.Type && _info.IsOpon){
				if(funcs[i].parent != null && !funcs[i].parent.activeSelf){
					funcs[i].parent.SetActive(true);
                    //获取此FunctionButtonUI的父物体上的UIGrid组件重新排列一下位置
                    if (funcsGrid == null) funcsGrid = funcs[i].parent.transform.parent.GetComponent<UIGrid>();
					if (funcsGrid != null) funcsGrid.repositionNow = true;
				}
                //设置绑定FunctionButtonUI组件的物体的显隐以及红点的显示
                funcs[i].SetShow = _info.IsOpon;
				if(funcs[i].funcRed != null)funcs[i].funcRed.SetActive( _info.FuncBtnRed);
                //设置FunctionButtonUI上父物体的红点获取父物体上的UiGrid组件重新排列位置
                if (funcs[i].parentRed != null && _info.FuncBtnRed)funcs[i].parentRed.SetActive(true);
				if(funcsGrid == null)funcsGrid = funcs[i].transform.parent.GetComponent<UIGrid>();
				if(funcsGrid != null)funcsGrid.repositionNow = true;
				break;
			}
		}
        //更新功能时对于主界面下方UI的逻辑处理 
        //循环遍历FunctionChildsButtonUI[]统计红点计数判定红点的显示 UIGrid重新排列
        for (int i=0;i<parentFuncs.Length;i++){
			if(parentFuncs[i] != null && parentFuncs[i].childFuncs.Contains(_info.Type) && _info.IsOpon){
				parentFuncs[i].gameObject.SetActive(true);
				if(_info.FuncBtnRed){
					parentFuncs[i].RedNum ++;
				}
				if(parentGrid == null)parentGrid = parentFuncs[i].transform.parent.GetComponent<UIGrid>();
				if(parentGrid != null)parentGrid.repositionNow = true;
				break;
			}
		}
	}
	
	void UpdateFunctionRed(FunctionDataInfo _info){
        //更新红点 
        for (int i=0;i<funcs.Length;i++){
            //三层if嵌套 最外层判定功能数据开启的类型
			if(funcs[i] != null && funcs[i].func == _info.Type && _info.IsOpon){
                //次外层以group为条件判断字典中是否包含funcs[i].group的key
                if (funcs[i].group > 0 && buttonChilds.ContainsKey(funcs[i].group)){
                    //内层  如果子物体上有红点显示则以子物体上的FunctionButtonUI组件上的group字段为key 存储子物体上的功能类型 否则删除该子物体上的功能类型
                    if (_info.FuncBtnRed){
						buttonChilds[funcs[i].group].AddChild(funcs[i].func);
					}else{
						buttonChilds[funcs[i].group].RemoveChild(funcs[i].func);
					}
                    //子物体所绑定的父物体红点的显示判断
					if(funcs[i].parentRed != null){
						funcs[i].parentRed.SetActive(buttonChilds[funcs[i].group].childs.Count > 0);
					}
				}
				if(funcs[i].funcRed != null)funcs[i].funcRed.SetActive(_info.FuncBtnRed);
				break;
			}
		}
        //更新红点时对于主界面下方UI的逻辑处理 
        //循环遍历FunctionChildsButtonUI[]统计红点计数
        int totalRedNum = 0;
		for(int i=0;i<parentFuncs.Length;i++){
			if(parentFuncs[i] == null )continue ;
			if(parentFuncs[i].childFuncs.Contains(_info.Type)){
				if(_info.FuncBtnRed){
					parentFuncs[i].RedNum ++;
				}else{
					if(parentFuncs[i].RedNum > 0)parentFuncs[i].RedNum --;
				}
			}
			if(additionalTotalRedPoint != FunctionType.None){
				totalRedNum += parentFuncs[i].RedNum;
			}
		}
		AdditionalTotalRed = totalRedNum > 0;
	}
}
