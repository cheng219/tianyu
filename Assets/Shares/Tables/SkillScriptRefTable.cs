//=============================
//作者：黄洪兴
//日期：2015/3/9
//用途：技能脚本表
//=============================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillScriptRefTable : AssetTable
{
	public List<SkillScriptRef> infoList = new List<SkillScriptRef>();
}


[System.Serializable]
public class SkillScriptRef
{
	/// <summary>
	/// 技能ID
	/// </summary>
	public int skillId;
	/// <summary>
	/// 脚本ID
	/// </summary>
	public int scriptID;

}
