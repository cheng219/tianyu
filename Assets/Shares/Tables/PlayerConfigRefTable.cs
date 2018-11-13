//=====================================
//作者：易睿
//日期：2015/5/4
//用途：JL人物系统数据
//==========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerConfigRefTable : AssetTable {
    public List<PlayerConfig> infoList = new List<PlayerConfig>();
}


[System.Serializable]
public class PlayerConfig
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;
    /// <summary>
    /// 资源版本
    /// </summary>
    public int res_version;
    /// <summary>
    /// 资源路径
    /// </summary>
    public int path_type;
    public int physique_num;
    public int physique_inc;
    public int perception_num;
    public int perception_inc;
    public int trick_num;
    public int trick_inc;
    /// <summary>
    /// 高度
    /// </summary>
    public int height;
    /// <summary>
    /// 性别
    /// </summary>
    public int sex; 
    /// <summary>
    /// 出生场景
    /// </summary>
    public int born_scene;
    /// <summary>
    /// 出生坐标x
    /// </summary>
    public int born_x;
    /// <summary>
    /// 出生坐标y
    /// </summary>
    public int born_y;

    public int proFlag;
    /// <summary>
    /// 初始任务
    /// </summary>
    public int scene_task;
    /// <summary>
    /// 初始任务步骤
    /// </summary>
    public int scene_step;
    /// <summary>
    /// 防御评分
    /// </summary>
    public int start_fuyu;
    /// <summary>
    /// 控制评分
    /// </summary>
    public int start_kz;
    /// <summary>
    /// 辅助评分
    /// </summary>
    public int start_fz;
    /// <summary>
    /// 攻击评分
    /// </summary>
    public int start_gj;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public int atk_dis;
    /// <summary>
    /// 声音资源路径类型
    /// </summary>
    public int sound_pathtype;
    /// <summary>
    /// 移动速度，150为基础速率
    /// </summary>
    public int baseMoveSpd;
    /// <summary>
    /// 名字高度
    /// </summary>
    public float name_height;
    /// <summary>
    /// 滚动技能CD
    /// </summary>
    public int roundSkill;
    /// <summary>
    /// 转职时候播放CG动画
    /// </summary>
    public int transferForCG;
    /// <summary>
    /// 攻击空放时的硬直技能ID
    /// </summary>
    public int hitLoseSkill;
    /// <summary>
    /// 名称
    /// </summary>
    public string name;
    /// <summary>
    /// 资源名
    /// </summary>
    public string res_name;
    /// <summary>
    /// 描述文字
    /// </summary>
    public string des;
    /// <summary>
    /// 头像图标
    /// </summary>
    public string res_head_Icon;
    /// <summary>
    /// 全身像
    /// </summary>
    public string icon_datu;
    /// <summary>
    /// 受伤音效
    /// </summary>
    public string sound_hurt_res;
    /// <summary>
    /// 死亡音效
    /// </summary>
    public string sound_death_res;
    /// <summary>
    /// 半身像图片
    /// </summary>
    public string icon_half;
    /// <summary>
    /// 名字美术图片
    /// </summary>
    public string namePic;
    /// <summary>
    /// 职业图标
    /// </summary>
    public string sign;
    /// <summary>
    /// 骨骼资源名
    /// </summary>
    public string bone;
    /// <summary>
    /// 描述2
    /// </summary>
    public string desTwo;
    /// <summary>
    /// 预览的时候，模型缩放  by吴江
    /// </summary>
    public float preview_scale;
    /// <summary>
    /// 模型高度
    /// </summary>
    public float module_height;
    /// <summary>
    /// 模型根节点与质心的高度差
    /// </summary>
    public float centerDiff;
    /// <summary>
    /// 人物步伐动画速度
    /// </summary>
    public float paceSpeed;

    public Vector3 previewPscale;
    public Vector3 previewRscale;
    /// <summary>
    /// 默认显示装备ID
    /// </summary>
    public List<int> defaultEquipList = new List<int>();
    /// <summary>
    /// 前置职业ID
    /// </summary>
    public List<int> up_ID = new List<int>();
    /// <summary>
    /// 后置职业ID
    /// </summary>
    public List<int> form_ID = new List<int>();
    /// <summary>
    /// 普攻技能组
    /// </summary>
    public List<int> mormalSkillList = new List<int>();
    /// <summary>
    /// 出生主动技能
    /// </summary>
    public List<int> basic_skill = new List<int>();

    /// <summary>
    /// 创建角色时展示用的模型组
    /// </summary>
    public List<int> create_player_res = new List<int>();


    /// <summary>
    /// 创建角色时的展示动作
    /// </summary>
    public int display_action;

    /// <summary>
    /// 创建角色时的模型
    /// </summary>
    public string display_res;
}



[System.Serializable]
public class ItemValue
{
    public int eid;
    public int count;
    public ItemValue()
    {

    }

    public ItemValue(int eid, int count)
    {
        this.eid = eid;
        this.count = count;
    }
}



