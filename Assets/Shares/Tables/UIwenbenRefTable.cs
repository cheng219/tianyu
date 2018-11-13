//=====================================
//作者：黄洪兴
//日期：2016/3/11
//用途：UI文本静态配置
//==========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIwenbenRefTable : AssetTable
{
    public List<UIwenbenRef> infoList = new List<UIwenbenRef>();
}


[System.Serializable]
public class UIwenbenRef
{

	public int id;

	public string text;

	public string des;


}
	
