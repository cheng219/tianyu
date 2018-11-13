//======================================================
//作者:何明军
//日期:2016/7/6
//用途:功能父按钮与子按钮关系,只支持到功能分页级红点,父>>子。主界面专用，最下面的功能按钮
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FunctionChildsButtonUI : MonoBehaviour {
	
	public List<FunctionType> childFuncs;
	public GameObject funcRed;
	
	int redNum = 0;
	public int RedNum{
		get{
			return redNum;
		}
		set{
			if(value > 0){
				redNum = value;
				if(funcRed != null)funcRed.SetActive(true);
			}else{
				redNum = 0;
				if(funcRed != null)funcRed.SetActive(false);
			}
		}
	}
}
