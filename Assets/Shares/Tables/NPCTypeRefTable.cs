//=====================================
//作者：易睿
//日期：2015/7/07
//用途：NPCType静态配置
//==========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCTypeRefTable : AssetTable
{
    public List<NPCTypeRef> infoList = new List<NPCTypeRef>();
}


[System.Serializable]
public class NPCTypeRef
{
    /// <summary>
    /// NPC编号，必须500000+的ID字段
    /// </summary>
    public int id;
    /// <summary>
    /// 资源版本号
    /// </summary>
    public int res_version;
    /// <summary>
    /// 资源位置
    /// </summary>
    public int path_type;
    /// <summary>
    /// 功能标示1
    /// </summary>
    public int function1;
    /// <summary>
    /// 功能标示2
    /// </summary>
    public int function2;
    /// <summary>
    /// NPC名字
    /// </summary>
    public string name;
    /// <summary>
    /// 称号
    /// </summary>
    public string title;
    /// <summary>
    /// NPC资源
    /// </summary>
    public string res_name;
    /// <summary>
    /// 骨骼资源
    /// </summary>
    public string boneName;
	/// <summary>
	/// 所属场景
	/// </summary>
	public int scene;
	/// <summary>
	/// X坐标
	/// </summary>
	public int scene_x;
	/// <summary>
	/// Z坐标
	/// </summary>
	public int scene_y;
	/// <summary>
	/// 朝向  正上背对玩家是0度
	/// </summary>
	public int scene_point;
    /// <summary>
    /// NPC说话
    /// </summary>
    public string wav_name;
    /// <summary>
    /// NPC描述
    /// </summary>
    public string dec;
    /// <summary>
    /// NPC说话的内容
    /// </summary>
    public string talk;
    /// <summary>
    /// 头像
    /// </summary>
    public string res;
    /// <summary>
    /// 模型的名称高度
    /// </summary>
    public float height;
    /// <summary>
    /// npc模型缩放
    /// </summary>
    public float npcSize;
    /// <summary>
    /// 预览的时候，模型缩放  by吴江
    /// </summary>
    public float preview_scale;
	/// <summary>
    /// 预览的时候，模型位置  by 
    /// </summary>
	public Vector3 previewPscale = Vector3.zero;
	/// <summary>
    /// 预览的时候，模型旋转角度  by 
    /// </summary>
	public Vector3 previewRscale = Vector3.zero;
    /// <summary>
    /// 任务UI中模型的大小
    /// </summary>
    public float taskPreviewScale;
    /// <summary>
    /// 任务UI中模型的大小
    /// </summary>
    public Vector3 taskpreviewPscale = Vector3.zero;
    /// <summary>
    /// 任务UI中模型的大小
    /// </summary>
    public Vector3 taskpreviewRscale = Vector3.zero;
    /// <summary>
    /// 模型资源列表（装备列表）
    /// </summary>
    public List<int> equipList;
	public float preview_px;
	public float preview_py;
	public float preview_pz;
	public float preview_rx;
	public float preview_ry;
	public float preview_rz;

}
