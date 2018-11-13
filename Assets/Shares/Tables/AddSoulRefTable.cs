//=========================
//作者：黄洪兴
//日期：2016/3/5
//用途：注灵静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddSoulRefTable : AssetTable
{
	public List<AddSoulRef> infoList = new List<AddSoulRef>();
}

[System.Serializable]
public class AddSoulRef
{
	public int id;
	public int relationID;
	public int quality;
	public int star;
	public string labelName;
	public List<int> attributeId=new List<int>();
	public List<int> attributeNum=new List<int>();
	public SkillLKey skill;
	public List<int> consume=new List<int>();
	public List<int> consumeNum=new List<int>();
	public int fighting;
	public List<int> randomExp=new List<int>();
    public string skillDescribe;

//	public 	List<AttributePair> attrs = new List<AttributePair> ();


}
