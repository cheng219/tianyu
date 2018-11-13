//======================================================
//作者:何明军
//日期:2016/7/6
//用途:走马灯数据结构
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MerryGoRoundDataInfo {

	//int id;


    string trumpetContent=string.Empty;

	string content;
	public string Content{
		get{
            if (trumpetContent != string.Empty)
                return trumpetContent;
			if(content == null)return string.Empty;
            return content;
		}
	}

//    MerryGoRoundRef refData;
//    protected MerryGoRoundRef RefData
//    {
//		get{
//			if(refData == null && id != 0){
//				refData = ConfigMng.Instance.GetMerryGoRoundRef(id);
//			}
//			return refData;
//		}
//	}
	
	public MerryGoRoundDataInfo(){}
	
//	public MerryGoRoundDataInfo(int _id,string _content){
//		id = _id;
//		content = _content;
//	}
	
	public MerryGoRoundDataInfo(int _id,string _contents){
		//id = _id;
        content = _contents;
	}


    public MerryGoRoundDataInfo(string _trumpetContent)
    {
        trumpetContent = _trumpetContent;
    }



}
