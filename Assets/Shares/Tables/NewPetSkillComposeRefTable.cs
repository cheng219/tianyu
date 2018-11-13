//=========================
//作者：黄洪兴
//日期：2016/03/05
//用途：宠物技能合成配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPetSkillComposeRefTable : AssetTable
{
	public List<NewPetSkillComposeRef> infoList = new List<NewPetSkillComposeRef>();
}


[System.Serializable]
public class NewPetSkillComposeRef
{
	public int id;
	public int item;
	public int needLevel;
	public int needNum;
}