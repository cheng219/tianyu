//====================================================
//作者: 贺丰
//日期：2015/11/24
//用途：符文单位的数据层对象
//======================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuneData
{
    public int id;
    public bool havegot;
}

public class RuneInfo
{
    #region 服务端数据 by贺丰
    RuneData runeData;
    #endregion
    #region 静态配置数据 by贺丰
    SkillRuneRef runeRef = null;
    public SkillRuneRef RuneRef
    {
        get
        {
            if (runeRef != null) return runeRef;
            runeRef = ConfigMng.Instance.GetSkillRuneRef(runeData.id);
            return runeRef;
        }
    }

    #endregion
    #region 构造 by 贺丰
    public RuneInfo(int _id, bool _got)
    {
        runeData = new RuneData();
        runeData.id = _id;
        runeData.havegot = _got;
    }
  
    #endregion

    #region 访问器
    /// <summary>
    /// 符文ID
    /// </summary>
    public int ID
    {
        get { return runeData.id; }
    }
    /// <summary>
    /// 符文名字
    /// </summary>
    public string Name
    {
        get { return RuneRef.name; }
    }
    /// <summary>
    /// 是否已获得
    /// </summary>
    public bool HaveGot
    {
        get { return runeData.havegot; }
    }
    /// <summary>
    /// 符文描述
    /// </summary>
    public string Des
    {
        get { return RuneRef.des; }
    }
    /// <summary>
    /// 解锁需求玩家等级
    /// </summary>
    public int UnlockPlayerLv
    {
        get { return RuneRef.unlockPlayerLvl; }
    }
    /// <summary>
    /// 解锁需求技能等级
    /// </summary>
    public int UnlockSkillLv
    {
        get { return RuneRef.unlockSkillLvl; }
    }
    /// <summary>
    /// 解锁资源数目
    /// </summary>
    public int UnlockPrice
    {
        get { return RuneRef.unlockPrice; }
    }
    /// <summary>
    /// 解锁需要的资源
    /// </summary>
    public int UnlockItem
    {
        get { return RuneRef.unlockItem; }
    }
    /// <summary>
    /// 图片名字
    /// </summary>
    public string RuneIcon
    {
        get { return RuneRef.runeIcon; }
    }
    /// <summary>
    /// 对应的技能id
    /// </summary>
    public int SkillMainId
    {
        get { return RuneRef.skillMainId; }
    }
    #endregion

}
