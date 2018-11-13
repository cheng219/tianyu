//======================================================
//作者:何明军
//日期:2016/7/6
//用途:功能开启数据
//======================================================
using UnityEngine;
using System.Collections;
using System;

public class FunctionDataInfo {
	/// <summary>
	/// 没有与新功能挂钩的功能，如默认开启的
	/// </summary>
	FunctionType type = FunctionType.None;
	/// <summary>
	/// 功能枚举
	/// </summary>
	public FunctionType Type{
		get{
			if(refData == null)return type;
			return (FunctionType)refData.func_type;
		}
	}
    /// <summary>
    /// ID
    /// </summary>
    public int ID 
    {
        get
        {
            if (refData == null) return 0;
            return refData.id;
        }
    }
	/// <summary>
	/// 职业
	/// </summary>
	public int Prof{
		get{
			if(refData == null)return 0;
			return refData.prof;
		}
	}
	
	/// <summary>
	/// vip
	/// </summary>
	public int VipLev{
		get{
			if(refData == null)return 0;
			return refData.vip;
		}
	}
	
	/// <summary>
	/// 下级指引
	/// </summary>
	public FunctionType NextFunc{
		get{
			if(refData == null)return FunctionType.None;
            if (refData.next > 0)
            {
                OpenNewFunctionRef data = ConfigMng.Instance.GetOpenNewFunctionRef(refData.next);
                return data != null ? (FunctionType)data.func_type : FunctionType.None;
            }
            else
                return FunctionType.None;
		}
	}
	/// <summary>
	/// 是否开启
	/// </summary>
	public bool IsOpon{
		get{
			if(refData == null){
				isOpen = true;
				return isOpen;
			}
			if(!GameCenter.instance.NewFunctionLock){
				isOpen = true;
				return isOpen;
			}
            //if(!isOpen)Debug.Log("refData.seqencing:"+refData.seqencing+",FunctionSequence:"+GameCenter.mainPlayerMng.FunctionSequence);
			return isOpen;
		}
	}	
	/// <summary>
	/// 一级界面
	/// </summary>
	public GUIType FunGUIType{
		get{
			if(refData == null)return GUIType.NONE;
			if("0".Equals(refData.open_UI)){
				return GUIType.NONE;
			}
			if(refData.UI_Num == 1){
				return (GUIType)Enum.Parse(typeof(GUIType),refData.open_UI);
			}
			return GUIType.NONE;
		}
	}
	
	/// <summary>
	/// 二级界面
	/// </summary>
	public SubGUIType FunSubGUIType{
		get{
			if(refData == null)return SubGUIType.NONE;
			if("0".Equals(refData.open_UI)){
				return SubGUIType.NONE;
			}
			if(refData.UI_Num == 2){
				return (SubGUIType)Enum.Parse(typeof(SubGUIType),refData.open_UI);
			}
			return SubGUIType.NONE;
		}
	}
	/// <summary>
	/// 名字
	/// </summary>
	public string Name{
		get{
			if(refData == null)return string.Empty;
			if("0".Equals(refData.open_flash_iconTwo)){
				return string.Empty;
			}
			return refData.open_flash_iconTwo;
		}
	}
	/// <summary>
	/// 图片
	/// </summary>
	public string Icon{
		get{
			if(refData == null)return string.Empty;
			if("0".Equals(refData.open_flash_iconOne)){
				return string.Empty;
			}
			return refData.open_flash_iconOne;
		}
	}
	/// <summary>
	/// 副本id
	/// </summary>
	public int SceneID{
		get{
			return refData != null ? refData.where : 0;
		}
	}
	/// <summary>
	/// 指引数据
	/// </summary>
	public OpenNewFunctionGuideRef GuideData{
		get{
			if(refData == null ) return null;
			if(refData != null && refData.appoint_type <= 0)return null;
			return ConfigMng.Instance.GetOpenNewFunctionGuideRef(refData.appoint_type,1);
		}
	}
	/// <summary>
	/// 飞行坐标
	/// </summary>
	public Vector2 FligthPos{
		get{
			return refData != null ? refData.orbit : Vector2.zero;
		}
	}
	bool red = false;
	/// <summary>
	/// 功能红点是否显示
	/// </summary>
	public bool FuncBtnRed{
		get{
			if(!IsOpon)return false;
			return red;
		}
		set{
			if(red != value){
				red = value;
                if (GameCenter.mainPlayerMng.UpdateFunctionRed != null)
                {
                    //Debug.Log("FuncBtnRed");
                    GameCenter.mainPlayerMng.UpdateFunctionRed(this);
                 
                }
			}
		}
	}
	/// <summary>
	/// 功能进度
	/// </summary>
	public int Seqencing{
		get{
			return refData != null ? refData.seqencing : 0;
		}
	}
	/// <summary>
	/// 功能开启提示
	/// </summary>
	public string TipDes{
		get{
			return refData != null ? refData.tips : string.Empty;
		}
	}
	
	
	/// <summary>
	/// 更新开启状态
	/// </summary>
	public bool Update(){
		if(IsOpon){
			return true;
		}
		if(refData.seqencing <= GameCenter.mainPlayerMng.FunctionSequence){
			isOpen = true;
			return isOpen;
		}
//		if(task == null){
//			task = CurTask;
//		}
//		if(task != null){
//			if(refData.open_conditions_data.Count < 2){
//				isOpen = true;
//				return true;
//			}
//			int eid = refData.open_conditions_data[0];
//			int step = refData.open_conditions_data[1];
//			if(task.Task == eid && task.Step >= step){
//				isOpen = true;
//			}
//		}
		return isOpen;
	}
	/// <summary>
	/// 该功能是由程序代码控制开启
	/// </summary>
	public void Update(bool _isopen){
		isOpen = _isopen;
	}
	
//	TaskInfo CurTask{
//		get{
//			return GameCenter.taskMng.GetMainTaskInfo();
//		}
//	}
	
	public FunctionControlType functionControlType = FunctionControlType.NONE;
	
	public enum FunctionControlType{
		NONE = 0,
		/// <summary>
		/// 配表控制
		/// </summary>
		TABLECONTROL,
		/// <summary>
		/// 程序代码控制
		/// </summary>
		PROGRAMCONTROL,
	}
	
	bool isOpen = false;
	OpenNewFunctionRef refData;
	
	public FunctionDataInfo(){}
	/// <summary>
	/// 没有配表数据的功能构造并且该功能是由程序代码控制开启的
	/// </summary>
	public FunctionDataInfo(FunctionType _func,bool _isopen,FunctionControlType _controlType){
		type = _func;
		functionControlType = _controlType;
		if(functionControlType == FunctionControlType.PROGRAMCONTROL)isOpen = _isopen;
	}
	/// <summary>
	/// 没有配表数据的功能构造
	/// </summary>
	public FunctionDataInfo(FunctionType _func){
		type = _func;
		isOpen = true;
	}
	/// <summary>
	/// 有配表数据的功能构造
	/// </summary>
	public FunctionDataInfo(OpenNewFunctionRef _refData){
		refData = _refData;
		Update();
	}
}
