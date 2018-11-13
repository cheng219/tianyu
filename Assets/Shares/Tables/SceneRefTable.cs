//=====================================
//作者：易睿
//日期：2015/5/6
//用途：JL场景系统数据
//==========================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneRefTable : AssetTable
{
    public List<SceneRef> infoList = new List<SceneRef>();
}


[System.Serializable]
public class SceneRef
{
    public int id;
    public string name;
    public SceneType sort;
    public int max_agent;
    public int full_create;
    public int life_time;
    public int in_x;
    public int in_z;
    public int out_scene;
    public int out_x;
    public int out_z;
    public int on_time_off;
    public string on_time;
    public string res;
    public string des;
    public int cam_y;
    public int cam_distance;
    public int cam_x_min;
    public int cam_x_max;
    public int cam_z_min;
    public int cam_z_max;
    public string res_prefab;
    //public int sp;
    //public int coin;
    //public int exp;
    //public int box;
    //public int map;
    //public int lev;
    //public int scene_x;
    //public int scene_y;
    //public int team_coin;
    //public int team_exp;
    //public int perfect_box;
    //public List<int> task_itemID = new List<int>();
    //public int map_id;
    public int map_word;
    //public string explain;
    //public string prompt;
	
	public int big_map;
	public int need_lv;	
	//public int need_scene;	
	public string icon;	
    //public int map_x;	
    //public int map_y;	
	public List<int> scene_items = new List<int>();	
	//public int type2;	
	public int addasset;
	
	public string music;
    /// <summary>
    /// 场景loading时的图片
    /// </summary>
    public string loadingPic;

    //public int medalsS;
    //public int medalsA;
    //public int medalsB;
    //public int medalsC;
    //public int medalsD;
    //public int scoreS;
    //public int scoreA;
    //public int scoreB;
    //public int scoreC;
    //public int scoreD;

    /// <summary>
    /// 可否主动使用技能
    /// </summary>
    public int battleenable;

    ///// <summary>
    ///// 伤害影响目标范围
    ///// </summary>
    //public DamageTargetRange damageType;
    /// <summary>
    /// 是否可以仇杀
    /// </summary>
    public int killenable;
    /// <summary>
    /// 是否可以切换阵营开关
    /// </summary>
    public int campenable;
    /// <summary>
    /// 是否可以打开大地图
    /// </summary>
    public int bigmapenable;
    /// <summary>
    /// 是否可原地复活
    /// </summary>
    public int reviveenable;
    /// <summary>
    /// 进入时是否有提示
    /// </summary>
    public int enteralert;
    /// <summary>
    /// 提示文字
    /// </summary>
    public string alerttext;
    /// <summary>
    /// 可否组队进入
    /// </summary>
    public int teamenable;
    /// <summary>
    /// 最大分线数量
    /// </summary>
    public int maxline;
    /// <summary>
    /// 场景宽
    /// </summary>
    public float sceneWidth;
    /// <summary>
    /// 场景长
    /// </summary>
    public float sceneLength;
	/// <summary>
	/// 翻拍奖励
	/// </summary>
	public List<ItemValue> flopReward=new List<ItemValue>();
	/// <summary>
	/// 奖励
	/// </summary>
	public List<ItemValue> reward=new List<ItemValue>();

	public int uiType;
	/// <summary>
	/// 两星奖励时间
	/// </summary>
	public int starTime2;
	/// <summary>
	/// 三星奖励时间
	/// </summary>
	public int starTime3;
	/// <summary>
	/// 寻路资源
	/// </summary>
	public string replaceNavmesh;

    public Vector2 safeAreaPoint;
    public int safeAreaRadius;
    public int reviveNum;
    /// <summary>
    /// PK模式
    /// </summary>
    public int pk_mode;
    /// <summary>
    /// 暂停
    /// </summary>
    public int suspend;
	/// <summary>
	/// 结算按钮显示 
	/// </summary>
	public SceneBtnShowType btnShowType;

    public int Scenegroup;
    public int allow_fly_by_pos_src;
    public int allow_fly_by_pos_dest;
    public int autofight_distance;
    public int reborn_type;
    public int dropRemainTimes;
    /// <summary>
    /// 该场景是否允许使用药品 1可以使用 0不可以使用
    /// </summary>
    public int useMedicItem;

}

public enum SceneBtnShowType{
	None = 0,
	/// <summary>
	/// 下一关
	/// </summary>
	Next,
	/// <summary>
	/// 继续
	/// </summary>
	Again,
}


	public enum SceneType
{
    /// <summary>
    /// 未指定
    /// </summary>
    NONE = 100,
    /// <summary>
    /// 和平野图
    /// </summary>
	PEACEFIELD = 0,
    /// <summary>
    /// 阵营野图
    /// </summary>
    CAMPFIELD = 1,
    /// <summary>
    /// 乱斗野图
    /// </summary>
    SCUFFLEFIELD = 2,
    /// <summary>
    /// 城镇图
    /// </summary>
    CITY = 3,
    /// <summary>
    /// 副本
    /// </summary>
    DUNGEONS = 4,
	/// <summary>
	/// 多人副本
	/// </summary>
	MULTIPLE = 5,
	/// <summary>
	/// 无尽副本
	/// </summary>
	ENDLESS = 6,
    /// <summary>
    /// 竞技场
    /// </summary>
    ARENA = 7,
    /// <summary>
    /// 战场 
    /// </summary>
    BATTLEFIELD = 8,
}

public enum SceneUiType
{
	NONE = 0,
	/// <summary>
	/// 断魂桥
	/// </summary>
	BRIDGE = 1,
	/// <summary>
	/// 无量圣地
	/// </summary>
	HOLYLAND = 2,
	/// <summary>
	/// 灵兽岛
	/// </summary>
	PETLAND = 3,
	/// <summary>
	/// 死亡荒漠
	/// </summary>
	DESERT = 4,
	/// <summary>
	/// 寒冰炼狱
	/// </summary>
	ICE = 5,
	/// <summary>
	/// 无尽试炼
	/// </summary>
	ENDLESS = 6,
	/// <summary>
	/// 竞技场
	/// </summary>
	ARENA = 7,
	/// <summary>
	/// 镇魔塔
	/// </summary>
	TOWER = 8,
	/// <summary>
	/// 仙侣
	/// </summary>
	XIANLV = 9,
	/// <summary>
	/// 仙域守护
	/// </summary>
	GUILDPROTECT = 10,
	/// <summary>
	/// 仙盟战
	/// </summary>
	GUILDWAR = 11,
	/// <summary>
	/// 武道会
	/// </summary>
	BUDOKAI = 12,
	/// <summary>
	/// 封神战
	/// </summary>
	GODSWAR = 13,
	/// <summary>
	/// 封印BOSS
	/// </summary>
	SEALBOSS = 14,
	/// <summary>
	/// 仙盟篝火
	/// </summary>
	GUILDFIRE = 15,
	/// <summary>
	/// 熔恶之地
	/// </summary>
	LIRONGELAND = 16,
	/// <summary>
	/// 攻城战
	/// </summary>
	GUILDSIEGE = 17,
	/// <summary>
	/// 地宫BOSS
	/// </summary>
	UNDERBOSS = 18,
	/// <summary>
	/// 熔恶之地
	/// </summary>
	RONGELAND = 19,
	/// <summary>
	/// 新手地图
	/// </summary>
	NEWBIEMAP = 20,
    /// <summary>
    /// 夺宝奇兵
    /// </summary>
    RAIDERARK = 21,
    /// <summary>
    /// 火焰山战场
    /// </summary>
    BATTLEFIGHT = 22,
    /// <summary>
    /// 挂机副本一层
    /// </summary>
    HANGUPCOPPYFIRSTFLOOR = 23,
    /// <summary>
    /// 挂机副本二层
    /// </summary>
    HANGUPCOPPYSECONDFLOOR = 24,
    /// <summary>
    /// BOSS副本
    /// </summary>
    BOSSCOPPY = 25,
}

///// <summary>
///// 伤害影响目标范围
///// </summary>
//public enum DamageTargetRange
//{
//    NO,
//    MONSTER,
//    ENEMYCAMP,
//    ALL,
//}