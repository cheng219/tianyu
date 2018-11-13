//=========================
//作者：黄洪兴
//日期：2016/04/29
//用途：成就静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementRefTable : AssetTable
{
	public List<AchievementRef> infoList = new List<AchievementRef>();
}


[System.Serializable]
public class AchievementRef
{
	public	int id;
	public int level;
	public string levelName;
	public string des;
	public int judge1;
	public int judgeNum1;
	public int judge2;
	public int judgeNum2;
	public AttributePair attribute;
    public string titleName;


}


