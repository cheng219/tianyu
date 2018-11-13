//=========================
//作者：黄洪兴
//日期：2016/04/26
//用途：通用说明静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DescriptionRefTable : AssetTable
{
	public List<DescriptionRef> infoList = new List<DescriptionRef>();
}


[System.Serializable]
public class DescriptionRef
{
	public	int id;
	public string title;
	public string content;


}


