//=========================
//作者：黄洪兴
//日期：2016/03/05
//用途：宠物技能槽配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPetSkillNumRefTable : AssetTable
{
	public List<NewPetSkillNumRef> infoList = new List<NewPetSkillNumRef>();
}


[System.Serializable]
public class NewPetSkillNumRef
{
	/// <summary>
	/// 编号
	/// </summary>
	public int id;
	/// <summary>
	/// 技能槽编号
	/// </summary>
	public int num;
	/// <summary>
	/// 所需成长值
	/// </summary>
	public int chengZhang;
	/// <summary>
	/// 所需资质值
	/// </summary>
	public int zizhi;

}