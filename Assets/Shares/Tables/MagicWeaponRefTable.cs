//=========================
//作者：黄洪兴
//日期：2016/3/8
//用途：法宝静态配置表
//=========================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicWeaponRefTable : AssetTable
{
	public List<MagicWeaponRef> infoList = new List<MagicWeaponRef>();
}

[System.Serializable]
public class MagicWeaponRef
{
	public int id;
	public int itemId;
	public string icon;
    public string skillIcon;

}
