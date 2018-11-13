//=========================
//作者：黄洪兴
//日期：2016/04/29
//用途：成就分类静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchieveTypeRefTable : AssetTable
{
	public List<AchieveTypeRef> infoList = new List<AchieveTypeRef>();
}


[System.Serializable]
public class AchieveTypeRef
{
	public	int type;
	public string typeName;
	public List<int> numId=new List<int>();


}


