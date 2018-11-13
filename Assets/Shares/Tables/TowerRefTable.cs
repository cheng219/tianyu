//=========================
//作者：黄洪兴
//日期：2016/05/03
//用途：镇魔塔静态配置
//=========================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerRefTable : AssetTable
{
	public List<TowerRef> infoList = new List<TowerRef>();
}


[System.Serializable]
public class TowerRef
{
	public	int id;
    public int lel;
    public int Scene;
    public int Reward1;
    public int Reward2;
    public int Reward3;
    public int ReLife;
    public int Time2;
    public int Time3;














    //public List<ItemValue> difficulty1=new List<ItemValue>();
    //public List<ItemValue> difficulty2=new List<ItemValue>();
    //public List<ItemValue> difficulty3=new List<ItemValue>();
    //public List<ItemValue> difficulty4=new List<ItemValue>();


}


