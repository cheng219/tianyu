//=================================================
//作者：吴江
//日期：2015/5/6
//用途：用来专门存放各种定义。（注意禁止引用外部的定义）,确保在不依赖任何外部组件的情况下，这个脚本能编译通过
//==================================================




using UnityEngine;
using System.Collections;
using System.ComponentModel;


public enum ObjectType
{
    Unknown = 0,
    /// <summary>
    /// 玩家
    /// </summary>
    Player = 1,
    /// <summary>
    /// 玩家
    /// </summary>
    PreviewPlayer = 2,
    /// <summary>
    /// 坐骑
    /// </summary>
    Mount = 3,
    /// <summary>
    /// 怪物
    /// </summary>
    MOB = 4,
    /// <summary>
    /// NPC
    /// </summary>
    NPC = 5,
    /// <summary>
    /// 出发器
    /// </summary>
    SceneItem = 6,
    /// <summary>
    /// 随从
    /// </summary>
    Entourage = 7,
    /// <summary>
    /// 掉落物品
    /// </summary>
    DropItem = 8,
    /// <summary>
    /// 传送点
    /// </summary>
    FlyPoint = 10,
    /// <summary>
    /// 动画对象
    /// </summary>
    CGObject = 11,
    /// <summary>
    /// 陷阱
    /// </summary>
    Trap = 12,
    /// <summary>
    /// 模型,雕像
    /// </summary>
    Model = 13,
}


public enum NPCType
{
    NORMOL,
}

/// <summary>
/// 运镖类型
/// </summary>
public enum DartType
{
	/// <summary>
	/// 每日运镖
	/// </summary>
	DailyDart = 1,
	/// <summary>
	/// 公会运镖
	/// </summary>
	GuildDart = 2,
}

/// <summary>
/// 资源地址类型
/// </summary>
public enum AssetPathType
{
    StreamingAssetsPath,
    PersistentDataPath,
    DataPath,
    TemporaryCachePath,
    Http,
}


public enum PreviewConfigType
{
    Dialog,
    Task,
}

/// <summary>
/// 攻击后果类型
/// </summary>
public enum AttackResultType
{
    /// <summary>
    /// 普通
    /// </summary>
    ATT_SORT_NARMAL = 0,
    /// <summary>
    /// 闪避
    /// </summary>
    ATT_SORT_DODGE = 1,
    /// <summary>
    /// 爆机
    /// </summary>
    ATT_SORT_CRIT = 2,
    /// <summary>
    /// 抵御
    /// </summary>
    ATT_SORT_HIGHDEF = 3,
	/// <summary>
	/// 幸运一击
	/// </summary>
	ATT_SORT_LUCKY_HIT = 4,
}
/// <summary>
/// 技能效果造成原因类型
/// </summary>
public enum AbilityResultCAUSEType
{
	NORMAL = 0,
	BUFF = 1,
	USESKILL = 2,
}

/// <summary>
/// 防御后果类型
/// </summary>
public enum DefResultType
{
    /// <summary>
    /// 普通
    /// </summary>
    DEF_SORT_NO = 0,
    /// <summary>
    /// 治疗
    /// </summary>
    DEF_SORT_TREAT = 1,
    /// <summary>
    /// 死亡
    /// </summary>
    //DEF_SORT_DIE = 2,
    /// <summary>
    /// 后仰
    /// </summary>
    DEF_SORT_STIFLE = 3,
    /// <summary>
    /// 击倒
    /// </summary>
    DEF_SORT_KICK2 = 4,
    /// <summary>
    /// 浮空
    /// </summary>
    DEF_SORT_KICK = 5,
    /// <summary>
    /// 免疫治疗
    /// </summary>
    DEF_SORT_UNTREAT = 6,
    /// <summary>
    /// 免疫击倒
    /// </summary>
    DEF_SORT_NOKICKDOWN = 7,
    /// <summary>
    /// 免疫后仰击退
    /// </summary>
    DEF_SORT_NOSTIFLE = 8,
    /// <summary>
    /// 免疫浮空击退
    /// </summary>
    DEF_SORT_NOKICK = 9,
    /// <summary>
    /// 回蓝
    /// </summary>
    DEF_SORT_ADDMP = 10,
    /// <summary>
    /// 扣蓝
    /// </summary>
    DEF_SORT_DELMP = 11,
}

public enum MountPracticeType
{
    Normal = 1,
    Better = 2,
    Best = 3,
}

/// <summary>
/// 技能等级数据的key结构 by吴江
/// </summary>
public class SkillLvDataKey
{
    public int skillID;
    public int skillLevel;
    public SkillLvDataKey(int _id, int _level)
    {
        skillID = _id;
        skillLevel = _level;
    }
}

/// <summary>
/// 二级界面枚举
/// </summary>
public enum SubGUIType
{
    NONE,
	
	#region 何明军
	/// <summary>
	/// 合成	
	/// </summary>
	BSynthesis,
	/// <summary>
	/// 人物基础属性	
	/// </summary>
	BPlayerInfo,
	/// <summary>
	/// 转生
	/// </summary>
	BRein,
	/// <summary>
	/// 邮件
	/// </summary>
	BMail,
	/// <summary>
	/// 单人副本
	/// </summary>
	BCopyTypeOne,
	/// <summary>
	/// 多人副本
	/// </summary>
	BCopyType,
	#endregion

	#region add dc
	/// <summary>
	/// 预览他人装备
	/// </summary>
	PREVIEWEQUIP,
	/// <summary>
	/// 预览他人信息
	/// </summary>
	PREVIEWINFORMATION,
	/// <summary>
	/// 预览他人宠物
	/// </summary>
	PREVIEWPET, 
	/// <summary>
	/// 每日运镖
	/// </summary>
	DAILYDART,
	/// <summary>
	/// 仙盟运镖
	/// </summary>
	GUILDDART,
	/// <summary>
	/// 公会主界面的运镖
	/// </summary>
	GUILDMAINDART,
	/// <summary>
	/// 攻城战城内商店
	/// </summary>
	GUILDCITYSTORE,
	/// <summary>
	/// 强化
	/// </summary>
	STRENGTHING,
	/// <summary>
	/// 升阶
	/// </summary>
	EQUIPMENTUPGRADE,
	/// <summary>
	/// 橙炼
	/// </summary>
	ORANGEREFINE,
	/// <summary>
	/// 洗练
	/// </summary>
	EQUIPMENTWASH,
	/// <summary>
	/// 镶嵌,
	/// </summary>
	EQUIPMENTINLAY,
	/// <summary>
	/// 继承
	/// </summary>
	EQUIPMENTEXTEND,
	/// <summary>
	/// 分解
	/// </summary>
	DECOMPOSITION,
	/// <summary>
	/// 背包背景界面
	/// </summary>
	BACKPACKBACKWND,
	/// <summary>
	/// 地宫BOSS
	/// </summary>
	UNDERBOSS,
	/// <summary>
	/// 封印BOSS
	/// </summary>
	SEALBOSS,
	/// <summary>
	/// 场景BOSS
	/// </summary>
	SCENEBOSS,
	/// <summary>
	/// 熔恶BOSS
	/// </summary>
	RONGEBOSS,
	/// <summary>
	/// 里熔恶BOSS
	/// </summary>
	LIRONGEBOSS,
	#endregion
	/// <summary>
	/// 普通登录
	/// </summary>
    NORMALLOGIN,
	/// <summary>
	/// 平台登陆
	/// </summary>
	PLATFORMLOGIN,
	/// <summary>
	/// 选服界面
	/// </summary>
	SELECTSERVER,
    /// <summary>
    /// 坐骑子界面
    /// </summary>
    MOUNTFEED,
    MOUNTEQ,
    /// <summary>
    /// 系统设置,渲染子界面
    /// </summary>
    SYSTEM_SETTING_RENDER,
    #region hhx

    /// <summary>
    /// 技能学习界面
    /// </summary>
    SKILLUPWND,
    /// <summary>
    /// 小助手
    /// </summary>
    LITTLEHELPER,
    /// <summary>
    /// 游戏公告
    /// </summary>
    NOTICESUBWND,
    /// <summary>
    /// 挂机设置
    /// </summary>
    HANGUP,
    #endregion


    /// <summary>
    /// 系统设置,帐号相关子界面
    /// </summary>
    SYSTEM_SETTING_ID,
    /// <summary>
    /// 商城子界面
    /// </summary>
    NORMALMALL,
    /// <summary>
    /// 签到子界面
    /// </summary>
    DAILYCHECK,
    /// <summary>
    /// 日常子界面
    /// </summary>
    DAILYACTIVE,
    /// <summary>
    /// 公会信息子界面
    /// </summary>
    GUILDINFO,
    /// <summary>
    /// 队伍设置子界面 by 贺丰
    /// </summary>
    TEAMSETTING,
    /// <summary>
    /// 信息子界面
    /// </summary>
    INFORMATION,
    /// <summary>
    /// 称号界面
    /// </summary>
    TITLE,

    /// <summary>
    /// 预览他人佣兵
    /// </summary>
    PREVIEWENTOURAGE,
    /// <summary>
    /// 预览他人遗物
    /// </summary>
    PREVIEWRELIC,
    GUILDBUILDING,
    /// <summary>
    /// 公会副本
    /// </summary>
    GUILDDUNGEON,
    /// <summary>
    /// 排行榜
    /// </summary>
    UNION_RANK,
    CLAN_RANK,
    LEV_RANK,

    /// <summary>
    /// 大地图
    /// </summary>
    WORLDMAP,
    CHANGELINE,

    /// <summary>
    /// 普通奖励子界面
    /// </summary>
    SEVENDAYSREWARDSUBWND,
    PLAYERLEVELSUBWND,
    CHARGEREWARDSUBWND,
    /// <summary>
    /// 排行榜子窗口
    /// </summary>
    ADMIRED_RANK,

    /// <summary>
    /// 强化子窗口
    /// </summary>
    STRENG,
    STARUP,
    RECAST,
    INLAY,

    /// <summary>
    /// 宠物子窗口
    /// </summary>
    PETINFORMATION,//信息
    GROWUP,//成长
    LINGXIU,//灵修
    FUSE,//融合
    GUARD,//守护
    PETSKILL,//技能
	MOUNT,//坐骑
    ILLUSION,//幻化
    MOUNTEQUIP,//骑装

    #region add ljq
    PREVIEWREWAD,// 藏宝阁预览窗口
    WAREHOUSE,//临时仓库窗口
    REWARD,//奖励窗口
    NEWRANKINGSUBWND,//排行榜
    ACHIEVEMENT,//成就
    #endregion
    /// <summary>
    /// 仙友子界面
    /// </summary>
    FRIEND,
    /// <summary>
    /// 仙侣子界面
    /// </summary>
    COUPLE,
    /// <summary>
    /// 结义子界面
    /// </summary>
    SWORN,
    /// <summary>
    /// 飞升子界面
    /// </summary>
    SOARING,
	/// <summary>
	///二级背包界面 
	/// </summary>
	SUBBACKPACK,
    /// <summary>
    /// 仙羽界面
    /// </summary>
    SUBWING,
	/// <summary>
	/// 客服
	/// </summary>
	QUESTIONS,
    /// <summary>
    /// 时装
    /// </summary>
    SUBFASHION,
	/// <summary>
	/// 新功能开启
	/// </summary>
	FUNCTIONOPEN,
	/// <summary>
	/// 指引
	/// </summary>
	GUIDEOPEN,
	/// <summary>
	/// 任务提示界面
	/// </summary>
	TASKTIP,
	/// <summary>
	/// PK规则介绍界面
	/// </summary>
	PKTIP,
    /// <summary>
    /// 法宝淬炼
    /// </summary>
    MAGICREFINE,
    /// <summary>
    /// 法宝注灵
    /// </summary>
    MAGICADDSOUL,
    /// <summary>
    /// 预览他人坐骑
    /// </summary>
    PREVIEWMOUNT,
    /// <summary>
    /// 深渊封印
    /// </summary>
    SEALEQU,
    /// <summary>
    /// 语音设置
    /// </summary>
    VOICEPLAY,
	/// <summary>
	/// 公会成员扩充
	/// </summary>
	EXPANDMEMBER,



    /// <summary>
    /// 宝藏活动预览
    /// </summary>
    TREASURETROVEPREVIEW,
    /// <summary>
    /// 宝藏活动排行
    /// </summary>
    TREASURETROVERANK,
    /// <summary>
    /// 宝藏活动奖励
    /// </summary>
    TREASURETROVEREWARD, 

    /// <summary>
    /// 开服贺礼
    /// </summary>
    OPENSERVERREWARD,
    /// <summary>
    /// 抽奖
    /// </summary>
    WDFLOTTERY,
    /// <summary>
    /// 聚宝盆
    /// </summary>
    CORNUCOPIA,

    /// <summary>
    /// 开服活动的四个枚举
    /// </summary>
    WDFACTIVITY1,
    WDFACTIVITY2,
    WDFACTIVITY3,
    WDFACTIVITY4,

    /// <summary>
    /// boss副本
    /// </summary>
    BOSSCOPPY,
}

/// <summary>
/// 一级界面枚举
/// </summary>
public enum GUIType
{
    /// <summary>
    /// 空,非任何窗口(关闭一切)
    /// </summary>
    NONE,
    /// <summary>
    /// 登陆界面
    /// </summary>
    LOGIN,
    /// <summary>
    /// 创角界面
    /// </summary>
    CREATE_CHAR,
    /// <summary>
    /// 加载等待界面(圆圈)
    /// </summary>
    WAIT,
    /// <summary>
    /// 请求加载等待滚动界面(圆圈)
    /// </summary>
    PANELLOADING,
    /// <summary>
    /// 提示
    /// </summary>
    MESSAGE,
    /// <summary>
    /// 二次确认框
    /// </summary>
    SECONDCONFIRM,
    /// <summary>
    /// 加载界面(黑屏读条)
    /// </summary>
    LOADING,
	#region NPC相关
    /// <summary>
    /// NPC对话UI
    /// </summary>
    NPCDIALOGUE,
	/// <summary>
	/// NPC每日运镖界面
	/// </summary>
	NPCDAILYDART,
	/// <summary>
	/// NPC仙盟运镖界面
	/// </summary>
	NPCGUILDDART,
	/// <summary>
	/// NPC膜拜
	/// </summary>
	NPCMORSHIP,
	/// <summary>
	/// NPC仙侣
	/// </summary>
	NPCXIANLV,
    /// <summary>
    /// NPC结义
    /// </summary>
    NPCSWORN,  
	/// <summary>
	/// NPC充值返利
	/// </summary>
	NPCRECHARGE,
    /// <summary>
    /// NPC夺宝奇兵
    /// </summary>
    NPCRAIDERARK,
	#endregion
	#region 太长了
    /// <summary>
    /// 战斗失败
    /// </summary>
    FORCE,
	/// <summary>
	/// 战斗失败提示
	/// </summary>
	FORCETIP,
    /// <summary>
    /// 副本结算
    /// </summary>
    COPYWIN,
    /// <summary>
    /// 任务寻路中提示窗
    /// </summary>
    TASK_FINDING,
    /// <summary>
    /// 系统设置界面
    /// </summary>
    SYSTEMSETTING,
    /// <summary>
    /// 邮件窗口
    /// </summary>
    MAIL,
    /// <summary>
    /// 怪物选中头像界面
    /// </summary>
    MONSTER_HEAD,
    /// <summary>
    /// 查看主玩家信息(背包,属性,主玩家预览)
    /// </summary>
    PREVIEW_MAIN,
    /// <summary>
    /// 修行界面
    /// </summary>
    PRACTICE,
    /// <summary>
    /// VIP详细界面
    /// </summary>
    VIP,
    /// <summary>
    /// 商店界面
    /// </summary>
    STORE,
    /// <summary>
    /// 公会主界面
    /// </summary>
    GUILDMAIN,
    /// <summary>
    /// 坐骑
    /// </summary>
    MOUNT,
    /// <summary>
    /// 断线重新连接
    /// </summary>
    RECONNECT,
    /// <summary>
    /// 重新登陆
    /// </summary>
    RETURN_LOGIN,
    /// <summary>
    /// 拍卖行
    /// </summary>
    AUCTION,
    /// <summary>
    /// 竞技场主界面
    /// </summary>
    ARENE,
    /// <summary>
    /// 福利系统
    /// </summary>
    REWARD,
    /// <summary>
    /// 暗月旅团
    /// </summary>
    SMALLGAME,
    /// <summary>
    /// 资源更新进度
    /// </summary>
    UPDATEASSET,
    /// <summary>
    /// 说明系统界面
    /// </summary>
    DESCRIPTIONWND,
    /// <summary>
    /// 3V3竞技场结算界面
    /// </summary>
    CAPTUREFLAG,
    /// <summary>
    ///副本扫荡
    /// </summary>
    SWEEPCARBON,
    /// <summary>
    /// 竞技，决斗，切磋同意结算窗口
    /// </summary>
    SETTLEMENT,
    /// <summary>
    /// 战斗UI界面  以下新项目开始
    /// </summary>
    MAINFIGHT,
    /// <summary>
    /// 小地图
    /// </summary>
    LITTLEMAP,
    /// <summary>
    /// 大地图
    /// </summary>
    LARGEMAP,
    /// <summary>
    /// 任务引导
    /// </summary>
    TASK,
    /// <summary>
    /// 信息展示界面
    /// </summary>
    INFORMATION,
    /// <summary>
    /// 宝箱获得物品界面
    /// </summary>
    BOXGOTITEM,
    /// <summary>
    /// 宝箱获得物品界面
    /// </summary>
    BOXREWARD,
    /// <summary>
    /// 预览他人信息
    /// </summary>
    PREVIEWOTHERS,
    /// <summary>
    /// 法宝界面
    /// </summary>
    MAGICWEAPON,
    /// <summary>
    /// 宠物界面
    /// </summary>
    SPRITEANIMAL,
    /// <summary>
    /// 送花界面
    /// </summary>
    SENDFLOWER,
    /// <summary>
    /// 选择婚礼类型界面
    /// </summary>
    MARRIAGE,
    /// <summary>
    /// 等级奖励界面
    /// </summary>
    RANKREWARD,
    /// <summary>
    /// 每日奖励界面
    /// </summary>
    EVERYDAYREWARD,
	#endregion

    #region 仙侠新添枚举
	/// <summary>
	/// 走马灯
	/// </summary>
	MERRYGOROUND,
	/// <summary>
	/// 功能开启,引导
	/// </summary>
	FUNCTION,
	/// <summary>
	/// 镇魔塔
	/// </summary>
	MagicTowerWnd,
	/// <summary>
	/// 活动大厅
	/// </summary>
	ATIVITY,
	/// <summary>
	/// 竞技场结算界面
	/// </summary>
	ARENERESULT,
	/// <summary>
	/// 背包界面(包括分解、合成)
	/// </summary>
	BACKPACK,
	/// <summary>
	/// 背包界面(只有单独的背包)
	/// </summary>
	BACKPACKWND,
	/// <summary>
	/// 副本结算 翻牌
	/// </summary>
	COPYWINFLOP,
	/// <summary>
	/// <summary>
	/// 多人准备
	/// </summary>
	COPYMULTIPLEWND,
	/// <summary>
	/// 副本入口
	/// </summary>
	COPYINWND,
	/// <summary>
	/// 无尽挑战
	/// </summary>
	ENDLESSWND,
    /// <summary>
    /// 仓库
    /// </summary>
	STORAGE,
	/// <summary>
	/// 仓库背景界面
	/// </summary>
	STORAGEBASE,
	/// <summary>
	/// 环式任务
	/// </summary>
	RINGTASK, 
	/// <summary>
	/// 试炼任务
	/// </summary>
	TRIALTASK,
    /// <summary>
    /// 试用翅膀弹窗界面
    /// </summary>
    TRIALWING,
	/// <summary>
	/// 装备培养
	/// </summary>
	EQUIPMENTTRAINING,
    /// <summary>
    /// 藏宝阁
    /// </summary>
    TREASUREHOUSE,
    /// <summary>
    /// 排行榜
    /// </summary>
    NEWRANKING,
    /// <summary>
    /// 七天奖励
    /// </summary>
    SEVENDAYREWARD,
    /// <summary>
    /// 首冲大礼
    /// </summary>
    FIRSTCHARGEBONUS,
    /// <summary>
    /// 通用说明弹窗
    /// </summary>
    DESCRIPTION,
	/// <summary>
	/// 物品购买
	/// </summary>
	BUYWND,
	/// <summary>
	/// 商店
	/// </summary>
	SHOPWND,
	/// <summary>
	/// 铸魂
	/// </summary>
	CASTSOUL,
	/// <summary>
	/// 商城
	/// </summary>
	NEWMALL,
	/// <summary>
	/// 仙盟商店
	/// </summary>
	GUILDSHOP,
	/// <summary>
	/// 仙盟仓库
	/// </summary>
	GUILDSTORAGE,
	/// <summary>
	/// 仙盟技能
	/// </summary>
	GUILDSILL,
	/// <summary>
	/// 其他仙盟
	/// </summary>
	GUILDLIST,
	/// <summary>
	/// 下载奖励
	/// </summary>
	DOWNLOADBONUS,
	/// <summary>
	/// 市场
	/// </summary>
	MARKET,
	/// <summary>
	/// 市场上架物品
	/// </summary>
	PUTAWAY,
	/// <summary>
	/// 挑战BOSS
	/// </summary>
	BOSSCHALLENGE,
	/// <summary>
	/// 副本UI
	/// </summary>
	MAINCOPPY,
    /// <summary>
    /// 在线奖励
    /// </summary>
    ONLINEREWARD,
	/// <summary>
	/// 运镖界面
	/// </summary>
	DARTWND,
    /// <summary>
    /// 武道会
    /// </summary>
    BUDOKAI,
    /// <summary>
    /// 武道会匹配
    /// </summary>
    BUDOKAIMATCHING,
	/// <summary>
	/// 仙域守护
	/// </summary>
	GUILDPROTECT,
	/// <summary>
	/// 攻城战
	/// </summary>
	GUILDSIEGE,
    /// <summary>
    /// 复活
    /// </summary>
    RESURRECTION,
    /// <summary>
    /// 公会战
    /// </summary>
    GUILDFIGHT,
    /// <summary>
    /// 仙盟篝火
    /// </summary>
    GUILDBONFIRE,
    /// <summary>
    /// 充值界面
    /// </summary>
    RECHARGE,
    /// <summary>
    /// 新技能提示界面
    /// </summary>
    NEWSKILL,
    /// <summary>
    /// 交易界面
    /// </summary>
    TRADE,
    /// <summary>
    /// 数量选择界面
    /// </summary>
    BATCHNUM,
	/// <summary>
	/// 仙盟活动副本UI(仙盟守护、仙盟战、火焰山战场)
	/// </summary>
	GUILDACTIVITYCOPPY,
	/// <summary>
	/// 每日必做
	/// </summary>
	DAILYMUSTDO,
    /// <summary>
    /// 新称号获得提示
    /// </summary>
    NEWTITLEMSG,
	/// <summary>
	/// 聊天
	/// </summary>
	CHAT,
	/// <summary>
	/// 任务提示(去做环任务or试炼任务)
	/// </summary>
	NOVICETIP,
    /// <summary>
    /// 精彩活动
    /// </summary>
    WDFACTIVE,  
    /// <summary>
    /// 充值优惠（优惠充值、爱心礼包、周卡充值）
    /// </summary>
    PRIVILEGE,  
    /// <summary>
    /// 武道会结算
    /// </summary>
    BUDOKAIEND, 
    /// <summary>
    /// 开服贺礼
    /// </summary>
    OPENSERVER,
	/// <summary>
	/// 退出解压界面
	/// </summary>
	RETURN_EXIT,
    /// <summary>
    /// 活动结算界面
    /// </summary>
    ACTIVITYBALANCE,
    /// <summary>
    /// 欢迎界面
    /// </summary>
    WELCOME,
	/// <summary>
	/// 场景动画
	/// </summary>
	SCENE_ANIMATION,
	/// <summary>
	/// 场景动画黑屏
	/// </summary>
	BLACK_SCREEN,
    /// <summary>
    /// 药品不足提示界面
    /// </summary>
    DRUGLACKWND,
    /// <summary>
    /// 祝福界面
    /// </summary>
    BLESSWND,
    /// <summary>
    /// 模型展示界面
    /// </summary>
    SHOWMODELUI,
    /// <summary>
    /// 收到999朵玫瑰炫耀界面
    /// </summary>
    SHOWFLOWER,
    /// <summary>
    /// 新手引导跳转副本弹出的即将进入界面
    /// </summary>
    NEWFUNCTIONTIPUI,
    /// <summary>
    /// 升级到20级弹出的UI
    /// </summary>
    UPTIPUI,
    /// <summary>
    /// 传送花费提示
    /// </summary>
    FLYREMIND,
    /// <summary>
    /// 皇室宝箱界面
    /// </summary>
     ROYALBOXWND,
    /// <summary>
    /// 批量使用界面
    /// </summary>
    BATCHUSE,

    #endregion
    /// <summary>
    /// 新奖励预览界面
    /// </summary>
    NEWREWARDPREVIEW,


    /// <summary>
    /// 立即使用的弹窗
    /// </summary>
    IMMEDIATEUSE,
    

    /// <summary>
    /// 省电模式弹窗
    /// </summary>
    POWERSAVING,
    /// <summary>
    /// 新手引导弹窗
    /// </summary>
    NEWGUID,

    /// <summary>
    /// 火焰山战场结算界面
    /// </summary>
    BATTLEFIELDSETTMENT,
    /// <summary>
    /// 火焰山评分说明界面
    /// </summary>
    BATTLECOMENTDES,
    /// <summary>
    /// 离线经验
    /// </summary>
    OFFLINEREWARD,
        /// <summary>
    /// 奇缘系统
    /// </summary>
    MIRACLE,
    /// <summary>
    /// 对话弹框
    /// </summary>
    DIALOGBOX,
    /// <summary>
    /// 奇缘系统入口
    /// </summary>
    ENTERMIRACLE,
    /// <summary>
    /// 改名卡
    /// </summary>
    RENAMECARD,



    /// <summary>
    /// 宝藏活动
    /// </summary>
    TREASURETROVE,
    /// <summary>
    /// 战败记录
    /// </summary>
    DEFEATRECORD,
    /// <summary>
    /// 七日挑战
    /// </summary>
    SEVENCHALLENGE,
    /// <summary>
    ///每日首充
    /// </summary>
    DAILYFIRSTRECHARGE,
    /// <summary>
    /// 环式任务类型
    /// </summary>
    RINGTASKTYPE,
      /// <summary>
    /// 挂机副本
    /// </summary>
    HANGUPCOPPY,
    /// <summary>
    /// 仙盟活跃
    /// </summary>
    GUILDACTIVE,
    /// <summary>
    /// 自动重连
    /// </summary>
    AUTO_RECONNECT,
}

public enum MailType : int
{
    System = 1,//系统
    Vip = 3,//vip
    Normal = 2,//普通

    /// <summary>
    /// 交易
    /// </summary>
    Trade = 3,
}

/// <summary>
/// 福利系统枚举
/// </summary>
public enum RewardGatherType
{
    Online_Reward,
    Login_Reward,
    Power_Reward,
    Newbie_Reward,
    Offline_Reward,
    FirstCharge_Reward,
    VIPLevel_Reward,
    ActiveNum_Reward,
}


/// <summary>
/// GUI的Layer。 
/// </summary>
public enum GUIZLayer
{
    BASE = 0,//基础层：主UI
    NORMALWINDOW = 1000,//标准窗口层
    TOPWINDOW = 2000,//前置窗口层
    TIP = 6000,// 修改3000 - 5000
    COVER = 4000, //覆盖层

}


public enum NPCTaskState
{
    /// <summary>
    /// 无可接/已接/可交任务，不展示头顶任务标记
    /// </summary>
    None,
    /// <summary>
    /// 有可接任务，头顶展示黄色感叹号！
    /// </summary>
    CanTake,
    /// <summary>
    /// 有可交任务。头顶展示黄色问号
    /// </summary>
    CanCommit,
    /// <summary>
    /// 有已接任务，头顶展示灰色问号
    /// </summary>
    HasTake,
}


public enum ResourceType
{
    GUI,
    PLAYER,
    MONSTER,
    NPC,
    EFFECT,
    SCENEITEM,
    TEXT,
    TEXTURE,
    OTHER,
    FONT,
    SOUND,
	PICTURE,//图片
}

/// <summary>
/// 物品的行为类型
/// </summary>
public enum EquipActionType
{
    /// <summary>
    /// /无使用效果
    /// </summary>
    none = 0,
    /// <summary>
    /// 固定奖励
    /// </summary>
    FixedReward,
    /// <summary>
    /// 随机奖励
    /// </summary>
    RandReward,
    /// <summary>
    /// 文字显示效果
    /// </summary>
    ShowText,
    /// <summary>
    /// 配方学习
    /// </summary>
    StudyRecipe,
    /// <summary>
    /// 获得坐骑
    /// </summary>
    add_mount,
    /// <summary>
    /// 获得时装
    /// </summary>
    Add_Cosmetic,

	/// <summary>
	/// 增加buff
	/// </summary>
	add_buff,
    /// <summary>
    /// 改名
    /// </summary>
    rename,
	/// <summary>
	/// 增加强化幸运值
	/// </summary>
	add_lucky,
	/// <summary>
	/// 跳转强化
	/// </summary>
	open_ui,
	/// <summary>
	/// 激活法宝
	/// </summary>
	activate_fb,
	/// <summary>
	/// 激活宠物
	/// </summary>
	activate_animal,
	/// <summary>
	/// 激活翅膀
	/// </summary>
	activate_wings,
	/// <summary>
	/// 开宝箱
	/// </summary>
	get_reward,
	/// <summary>
	/// 激活时装
	/// </summary>
	model_clothes,
    /// <summary>
    /// 减少杀戮值
    /// </summary>
    reduce_kill_value,
    /// <summary>
    /// 回血
    /// </summary>
    add_hp,
    /// <summary>
    /// 回蓝
    /// </summary>
    add_mp,
    /// <summary>
    /// 激活坐骑幻化
    /// </summary>
    activate_illusion,
    /// <summary>
    /// 提升一级
    /// </summary>
    add_lev,
    /// <summary>
    /// 回城
    /// </summary>
    back_city,
    /// <summary>
    /// 随机传送
    /// </summary>
    random_fly,
    /// <summary>
    /// 使用消耗性宝箱
    /// </summary>
    get_reward_ex,
    /// <summary>
    /// 充值卡
    /// </summary>
    recharge_card,
    /// <summary>
    /// 激活称号
    /// </summary>
    add_title,
    /// <summary>
    /// 激活并使用称号
    /// </summary>
    activate_title,
}

/// <summary>
/// 任务数据类型
/// </summary>
public enum TaskDataType
{
    /// <summary>
    /// 已开始的任务（数据来源：服务端）
    /// </summary>
    Started,
    /// <summary>
    /// 还未开始的任务（数据来源：根据玩家级别和客户端配置决定）
    /// </summary>
    UnStart,
    /// <summary>
    /// 已完全结束的任务/任务线（数据来源：服务端）
    /// </summary>
    Ended,
}

/// <summary>
/// 技能后果的表现判断来源
/// </summary>
public enum AbilityHitAnimType
{
    ByClient,
    ByServer,
}


public enum EquipSlot
{
    None = 0,
	sarmor = 1,//护肩
	body = 2,//护衣
	special = 3,//护腿
	bracers = 4,//护腕
	belt = 5,//腰带	
	shoes = 6,//鞋子
	head = 7,//头盔
	necklace = 8,//项链
	badge = 9,//护符
	ring = 10,//戒指
    weapon = 11,//武器
    glove = 12,//手套
    wing=13,//翅膀
    magicweapon=14,//法宝

    cosPart1=101,//身体 
    cosPart2=102,//武器
	count,

	resource=201,//资源
	gem301=301,//生命宝石
	gem302=302,//攻击宝石
	gem303=303,//防御宝石
	gem304=304,//命中宝石
	gem305=305,//闪避宝石
	gem306=306,//暴击宝石
	gem307=307,//韧性宝石

    Headband = 1001,//头带
    Armor = 1002,//护甲
    Saddle = 1003,//鞍部
    Hoofsteel = 1004,//铁蹄
    Toko = 1005,//镫子
    Whip = 1006,//鞭子
    Reins = 1007,//缰绳
    Ornaments = 1008,//挂饰
}

/// <summary>
/// 弹道飞行类型
/// </summary>
public enum ArrowFlyType
{
    NONE = 0,
    /// <summary>
    /// 按高度飞行
    /// </summary>
    SKY = 1,
    /// <summary>
    /// 不按高度，贴地飞行
    /// </summary>
    LAND = 2,
}
/// <summary>
/// 物品是否可回收
/// </summary>
public enum GoodsRecycleType
{
    NO = 1,
    YES = 2,
}
/// <summary>
/// 物品是否需要获得提示
/// </summary>
public enum GoodsAttentionType
{
    NO = 1,
    YES = 2,
}

public enum EquipmentFamily
{
    WEAPON,//武器
    ARMOR,//防具
    JEWELRY,//首饰
    COSMETIC,//时装
    PET,//宠物
    POTION,//药水
    CONSUMABLES,//消耗品
    NORMAL,//普通物品
    MATERIAL,//材料
    TASK,//任务物品
    GEM,//宝石
    MOUNT,//坐骑
    MOUNTFOOD,//坐骑食物
    MOUNTEQUIP,//坐骑装备
    RENAME,//改名
	PETSKILLBOOK,//宠物技能书
	CHANGE,//变强
    TOKEN,//信物
    SPECIALBOX,//特殊宝箱
}

public enum ItemActionType
{
    /// <summary>
    /// 无行为
    /// </summary>
    None,
    /// <summary>
    /// 尝试使用/穿上
    /// </summary>
    TryToUse,
    /// <summary>
    /// 卸下
    /// </summary>
    TryTakeOff,
    /// <summary>
    /// 试穿
    /// </summary>
    TryPreviewEquip,
    /// <summary>
    /// 尝试销毁
    /// </summary>
    TryToDestory,
    /// <summary>
    /// 确认销毁
    /// </summary>
    SureToDestory,
    /// <summary>
    /// 常规的左边按钮 是装备则脱下或者装备，不是则使用
    /// </summary>
    NormalLeft,
    /// <summary>
    /// 常规的中间按钮 是装备则试穿，不是则批量使用
    /// </summary>
    NormalMiddle,
    /// <summary>
    /// 常规的右边按钮 一般是销毁
    /// </summary>
    NormalRight,
    /// <summary>
    /// 显示时装
    /// </summary>
    ChangeCosmeticState,
    /// <summary>
    /// 商城购买
    /// </summary>
    MallBuy,
    HonorMallBuy,
    SpecialBuy,
    /// <summary>
    /// 商店购买
    /// </summary>
    StoreBuy,
	/// <summary>
	/// 城内商店购买
	/// </summary>
	CityShopBuy,
    /// <summary>
    /// 商店出售
    /// </summary>
    StoreSell,
    /// <summary>
    /// 选择添加（）
    /// </summary>
    SelectAdd,
    /// <summary>
    /// 替换
    /// </summary>
    ReplaceThis,
    /// <summary>
    /// 拍卖行购买by 	
    /// </summary>
    AuctionBuy,
    /// <summary>
    /// 拍卖行取回
    /// </summary>
    AuctionRetrieve,
    /// <summary>
    /// 拍卖行发布广告
    /// </summary>
    AuctionHore,
    /// <summary>
    /// 新装备引导使用或者装备 by 
    /// </summary>
    UseBetter,
    /// <summary>
    /// 新装备引导寄售
    /// </summary>
    AuctionQuickSell,
    /// <summary>
    /// 新装备引导取消
    /// </summary>
    QuitEquip,
    /// <summary>
    /// 锻造材料.
    /// </summary>
    StrengEquipment,
    /// <summary>
    /// 镶嵌--宝石  by 
    /// </summary>
    Inlay,
	/// <summary>
	/// 卸下宝石
	/// </summary>
	UnInlay,
	/// <summary>
	/// 合成(宝石热显上,跳转合成界面)
	/// </summary>
	Synthetic,
    /// <summary>
    /// 升级--宝石  by 
    /// </summary>
    UpGrade,
    /// <summary>
    /// 分解  by 
    /// </summary>	
    TryToDecompose,
    /// <summary>
    /// 收获 
    /// </summary>
    Harvest,
    /// <summary>
    /// 探宝分解
    /// </summary>
    TreasureDecompose,
    ShowDetails,
    /// <summary>
    /// 回收
    /// </summary>
    Recircle,
	#region 仙侠新添
	/// <summary>
	/// 放入到仓库
	/// </summary>
	PutInStorage,
	/// <summary>
	/// 从仓库取出
	/// </summary>
	TakeOutStorage,
	/// <summary>
	/// 拍卖行上架
	/// </summary>
	Putaway,
    /// <summary>
    /// 商店购回
    /// </summary>
    Redeem,
    /// <summary>
    /// 炫耀
    /// </summary>
    Flaunt,
    /// <summary>
    /// 交易
    /// </summary>
    Trade,
    /// <summary>
    /// 变为永久
    /// </summary>
    ToForever,
    /// <summary>
    /// 祝福
    /// </summary>
    BLESSING,
    /// <summary>
    /// 取出
    /// </summary>
    TAKEOUT,
    /// <summary>
    /// 融合
    /// </summary>
    MIX,
	#endregion
}
/// <summary>
/// ItemUI展示类型(对应不同的操作按钮)
/// </summary>
public enum ItemShowUIType
{
	NONE,
	PREVIEW,
	/// <summary>
	/// 普通背包
	/// </summary>
	NORMALBAG,
	/// <summary>
	/// 商店背包
	/// </summary>
	SHOPBAG,
	/// <summary>
	/// 市场背包
	/// </summary>
	MARKETBAG,
    /// <summary>
    /// 公会商店背包
    /// </summary>
    GUILDSHOPBAG,
    /// <summary>
    /// 交易背包
    /// </summary>
    TRADEBAG,
}


public enum AbilityType
{
    MoveBack = -1,
    Normal = 0,
    MoveON = 1,
    STOP = 2,
}


public enum OPCUpdateTag
{
    NONE = 0,
    Level_Change = 1,//等级变化
    Class_Change = 12,//职业变化
    Equip_Change = 13,//装备变化
    Cosmetic_Head_State = 101,//头盔时装状态
    Cosmetic_Sarmor_State = 102,//肩膀时装状态
    Cosmetic_Body_State = 103,//身体时装状态
    Cosmetic_Special_State = 104,//特殊时装状态
    Cosmetic_Weapon_State = 105,//武器时装状态
}

/// <summary>
/// 引导方向
/// </summary>
public enum GuildDirection
{
    UP = 1,
    DOWN = 2,
    LEFT = 3,
    RIGHT = 4,
}
/// <summary>
/// 引导展示类型
/// </summary>
public enum GuildTipType
{
    ARROW = 1,
    TEXT = 2,
    ARROWANDTEXT = 3,
    SLIDE = 4,
}


/// <summary>
/// 标识类型 
/// </summary>
public enum LogoType
{
    /// <summary>
    /// 第一次组队
    /// </summary>
    FristTeam = 1,

    XXX = 2,
    YYY = 4,
}

/// <summary>
/// 对象基础属性
/// </summary>
public enum ActorBaseTag
{
	/// <summary>
	/// 杀戮值
	/// </summary>
	SLAVALUE = 14,
	/// <summary>
	/// 杀戮等级
	/// </summary>
	SLALEVEL = 18,
	/// <summary>
	/// PK模式
	/// </summary>
	PKMODE = 19,
    /// <summary>
    /// 当前等级
    /// </summary>
    Level = 101,
    /// <summary>
    /// 当前血量
    /// </summary>
    CurHP = 102,
    /// <summary>
    /// 当前蓝量
    /// </summary>
    CurMP = 103,
    /// <summary>
    /// 当前经验
    /// </summary>
    Exp = 104,
    /// <summary>
    /// 等级上限
    /// </summary>
    LevelLimit = 105,
    /// <summary>
    /// 经验上限
    /// </summary>
    ExpLimit = 106,
    /// <summary>
    /// 金币上限
    /// </summary>
    CoinLimit = 1305,
    /// <summary>
    /// 元宝上限
    /// </summary>
    DiamondLimit = 1306,
    /// <summary>
    /// 战力
    /// </summary>
    FightValue = 109,
	/// <summary>
	/// 阵营
	/// </summary>
	Camp = 110,
	/// <summary>
	/// 反击状态
	/// </summary>
	CounterAttack = 115,
    /// <summary>
    /// 军衔等级
    /// </summary>
    MilitaryLv = 10001,
	/// <summary>
	/// 当前绑定钱币
	/// </summary>
	BindCoin = 300,
	/// <summary>
	/// 当前非绑定钱币
	/// </summary>
	UnBindCoin = 301,
    /// <summary>
    /// 真元
    /// </summary>
    REALYUAN = 302,
	/// <summary>
	/// 当前钻石/元宝 
	/// </summary>
	Diamond = 303,
	/// <summary>
	/// 当前修为,转生资源
	/// </summary>
	Fix = 304,
	/// <summary>
	/// 悟性,技能资源
	/// </summary>
	SkillRes = 305,
	/// <summary>
	/// 灵气,飞升低等资源
	/// </summary>
	LowFlyUpRes = 306,
	/// <summary>
	/// 仙气,飞升高等资源
	/// </summary>
	HighFlyUpRes = 307,
	/// <summary>
	/// 功勋,功勋商店货币
	/// </summary>
	Exploit = 308,
	/// <summary>
	/// 声望,声望商店货币
	/// </summary>
	Repuutation = 309,
	/// <summary>
	/// 积分
	/// </summary>
	Integral = 310,
	/// <summary>
	/// VIP经验
	/// </summary>
	VIPExp = 311,
	/// <summary>
	/// 仙盟贡献
	/// </summary>
	GuildContribution = 312,

    /// <summary>
    /// 绑定钻石
    /// </summary>
    BindDiamond = 313,


    /// <summary>
    /// 整个数据都变化了，服务端的协议。 by吴江
    /// </summary>
    TOTAL,

}

public enum ActorPropertyTag
{
	NONE,
    /// <summary>
    /// 攻击上限
    /// </summary>
    ATKUP = 1,
    /// <summary
    /// 攻击下限
    /// </summary>
    ATKDOWN = 2,
    /// <summary>
    /// 防御上限
    /// </summary>
    DEFUP = 3,
    /// <summary>
    /// 防御下限
    /// </summary>
    DEFDOWN = 4,
    /// <summary>
    /// 生命上限
    /// </summary>
    HPLIMIT = 5,
    /// <summary>
    /// 法力上限
    /// </summary>
    MPLIMIT = 6,
    /// <summary>
    /// 命中
    /// </summary>
    HIT = 7,
    /// <summary>
    /// 闪避
    /// </summary>
    DOD = 8,
    /// <summary>
    /// 暴击
    /// </summary>
    CRI = 9,
    /// <summary>
    /// 韧性
    /// </summary>
    TOUGH = 10,
    /// <summary>
    /// 增伤
    /// </summary>
    ADDHURT = 11,
    /// <summary>
    /// 减伤
    /// </summary>
    REDUCEHURT = 12,
    /// <summary>
    /// 幸运
    /// </summary>
    LUCKY = 13,
    /// <summary>
    /// 杀戮
    /// </summary>
    SLA = 14,
    /// <summary>
    /// 攻击
    /// </summary>
    ATK=15,
    /// <summary>
    /// 防御
    /// </summary>
    DEF = 16,
	/// <summary>
	/// 正义值
	/// </summary>
	JUSTICE = 17,
    /// <summary>
    /// 当前移动速度
    /// </summary>
    MOVESPD = 100,

	HP = 102,

	MP = 103,

    TOTAL = 10000,//整个数据都变化了，服务端的协议。 BY吴江
}

public enum MailUpdateTag
{
    NONE = 0,
    AllCHANGE = 1,
    NEWMAIL = 2,//新
    DELETEMAIL = 3,//删邮件
    DELETEITEM = 4,//清空物品
    MARKREAD = 5,//标记已读
    HASITEM = 6,//有物品不能删除
    NOMONEY = 7,//没钱不能提取
    FLUSHMAIL = 8,//刷新邮件

    INPUTRECEIVERNAME = 9,
    INPUTTITLE = 10,
    INPUTCONTENT = 11,
}
public enum MailTab : int
{
    Inbox = 0,
    WriteMail = 1,
}
public enum ChatUpdateTag
{
    NONE = 0,
    NEWMSG = 1,
    INPUTRECEIVERNAME = 2,
    INPUTCONTENT = 3,
    TABCTRL = 4,
    NAMEPOP = 5,
    VOICEPLAY = 6,
}
/// <summary>
/// 系统设置类型
/// </summary>
public enum SystemSettingType : int
{
    Music,
    SoundEffect,
    FastMode,
    Vibrate,
}
public enum PlayerPlayMode : int
{
    NormalMode = 0,//普通正常模式
    MercenariesMode = 1,//雇佣兵模式
}


/// <summary>
///音效枚举
/// </summary>
public enum SystemSound
{
    /// <summary>
    /// 按钮按下的声效
    /// </summary>
    [Description("system/1.wav")]
    ButtonSound,
    /// <summary>
    /// 背包满的提示声
    /// </summary>
    [Description("system/2.wav")]
    PackageFull,
    /// <summary>
    /// 单独文字信息（私聊）
    /// </summary>
    [Description("system/3.wav")]
    PrivateChatNewMsg,
    /// <summary>
    /// 单独语音信息（私聊）
    /// </summary>
    [Description("system/4.wav")]
    PrivateChatNewVoiceMsg,
    /// <summary>
    /// 等级提升
    /// </summary>
    [Description("system/5.wav")]
    LevelUp,
    /// <summary>
    /// 点击进入游戏的唰声
    /// </summary>
    [Description("system/6.wav")]
    EnterGame,

    /// <summary>
    /// 合成成功
    /// </summary>
    [Description("system/7.wav")]
    CompositeOK,

    /// <summary>
    /// 副本胜利，竞技场胜利
    /// </summary>
    [Description("system/8.wav")]
    WinBattle,

    /// <summary>
    /// 获得金钱声
    /// </summary>
    [Description("system/9.wav")]
    GotMuchMoney,

    /// <summary>
    /// 获得物品声
    /// </summary>
    [Description("system/10.wav")]
    GotManyItem,

    /// <summary>
    /// 加入公会,加入组队
    /// </summary>
    [Description("system/11.wav")]
    JoinTeam,

    /// <summary>
    /// 离开公会,离开队伍
    /// </summary>
    [Description("system/12.wav")]
    LeaveTeam,
    /// <summary>
    /// 拍卖行物品上下架
    /// </summary>
    [Description("system/13.wav")]
    AuctionsUpdate,
    /// <summary>
    /// 强化成功
    /// </summary>
    [Description("system/14.wav")]
    StrengthenSucceed,
    /// <summary>
    /// 强化失败
    /// </summary>
    [Description("system/15.wav")]
    StrengthenFailure,
    /// <summary>
    /// 收到组队信息
    /// </summary>
    [Description("system/16.wav")]
    InviteJoinTeam,
    /// <summary>
    /// 邮件来了时的声音
    /// </summary>
    [Description("system/17.wav")]
    MailNewMsg,
    /// <summary>
    /// 治疗声
    /// </summary>
    [Description("system/18.wav")]
    Cure,
    /// <summary>
    /// 副本选择
    /// </summary>
    [Description("system/Ui_button.wav")]
    CoppyItem,
    /// <summary>
    /// 点击物品
    /// </summary>
    [Description("system/dianjijixu.wav")]
    Item,
    /// <summary>
    /// 天地神兵
    /// </summary>
    [Description("system/baoxiang.wav")]
    Lottery,
    /// <summary>
    /// 天地神兵十连抽
    /// </summary>
    [Description("system/baoxiangshilian.wav")]
    TenLottery,
    /// <summary>
    /// 涌金泉金币
    /// </summary>
    [Description("system/yongjinquan.wav")]
    GoldPool,
    /// <summary>
    /// 涌金泉金币十连抽
    /// </summary>
    [Description("system/yongjinquanshilian.wav")]
    GoldPoolTen,
    /// <summary>
    /// 涌金泉金币暴击
    /// </summary>
    [Description("system/yongjinquanbaoji.wav")]
    GoldPoolDouble,
    /// <summary>
    /// 任务点击
    /// </summary>
    [Description("system/dianjijixu.wav")]
    TaskItem,
    /// <summary>
    ///专长点击
    /// </summary>
    [Description("system/dianjijixu.wav")]
    zhuangchang,
    /// <summary>
    ///点击继续
    /// </summary>
    [Description("system/dianjijixu.wav")]
    ClickContinue,
    /// <summary>
    /// 强化中
    /// </summary>
    [Description("system/Qianghuazhong.wav")]
    Qianghuazhong,
    /// <summary>
    /// 探宝出碎片
    /// </summary>
    [Description("system/tanbaobaoshi.wav")]
    tanbaobaoshi,
    /// <summary>
    /// 探宝出宝石
    /// </summary>
    [Description("system/tanbaosuipian.wav")]
    tanbaosuipian,
    /// <summary>
    /// 胜利宝箱
    /// </summary>
    [Description("system/shenglibaoxiang.wav")]
    shenglibaoxiang,
    /// <summary>
    /// 胜利宝箱打开
    /// </summary>
    [Description("system/shenglibaoxiangdakai.wav")]
    shenglibaoxiangdakai,
    /// <summary>
    /// 胜利星星
    /// </summary>
    [Description("system/shenglixingxing.wav")]
    shenglixingxing,
    /// <summary>
    /// 新功能开启
    /// </summary>
    [Description("system/xingongnengkaiqi.wav")]
    xingongnengkaiqi,
    /// <summary>
    /// 章节加声效
    /// </summary>
    [Description("system/zhangjiewancheng.wav")]
    zhangjiewancheng,
}
/// <summary>
/// 玩家动作枚举
/// </summary>
public enum PlayerActionType
{
    NONE = 0,
    NORMAL,
    DIG,
    FISH,
}
/// <summary>
/// 新功能类型
/// </summary>
public enum FunctionType
{
    None = 0,//无
	/// <summary>
	/// 人物
	/// </summary>
	MAINCHARACTER= 1,
	/// <summary>
	/// 技能
	/// </summary>
	SKILLS= 2,
	/// <summary>
	/// 外形
	/// </summary>
	SHAPE = 3,
	/// <summary>
	/// 仙羽
	/// </summary>
	FAIRYFEATHER= 4,
	/// <summary>
	/// 飞升
	/// </summary>
	SOARING= 5,
	/// <summary>
	/// 转生
	/// </summary>
	REIN = 6,
	
	
	/// <summary>
	/// 背包
	/// </summary>
	MAINBACK = 7,
	/// <summary>
	/// 合成
	/// </summary>
	SYNTHETIC= 8,
	/// <summary>
	/// 分解
	/// </summary>
	DECOMPOSITION= 9,
	/// <summary>
	/// 仓库
	/// </summary>
	WAREHOUSE = 10,
	
	/// <summary>
	/// 强化
	/// </summary>
	STRENGTHENING=11,
	/// <summary>
	/// 升阶
	/// </summary>
	DEGREE=12,
	/// <summary>
	/// 橙炼
	/// </summary>
	ORANGEREFINING=13,
	/// <summary>
	/// 洗练
	/// </summary>
	WASHSPRACTICE=14,
	/// <summary>
	/// 镶嵌
	/// </summary>
	MOSAIC=15,
	/// <summary>
	/// 继承
	/// </summary>
	INHERITANCE=16,
	
	
	/// <summary>
	/// 宠物的信息分页
	/// </summary>
	PET=17,
	/// <summary>
	/// 坐骑
	/// </summary>
	MOUNT=18,
	/// <summary>
	/// 幻化
	/// </summary>
	UNREAL=19,
	
	
	/// <summary>
	/// 仙盟
	/// </summary>
	FAIRYAU=20,
	
	
	/// <summary>
	///邮件
	/// </summary>
	MAIL=21,
	/// <summary>
	///好友
	/// </summary>
	FRIENDS=22,
	/// <summary>
	///仙侣
	/// </summary>
	FAIRYCOUPLE=23,
	/// <summary>
	///结义
	/// </summary>
	SWORN=24,
	/// <summary>
	///客服
	/// </summary>
	CUSTOMERSERVICE=25,
	/// <summary>
	/// 排行
	/// </summary>
	RANKING=26,
	/// <summary>
	/// 成就
	/// </summary>
	ACHIEVEMENT=27,
	
	
	/// <summary>
	/// 市场
	/// </summary>
	MARKET=28,
	/// <summary>
	/// 商城
	/// </summary>
	MALL=29,
	
	
	/// <summary>
	/// 理财周卡
	/// </summary>
	FINANCIAL=30,
	
	/// <summary>
	/// 无尽试炼
	/// </summary>
	ENDLESSTRIAL=31,
	/// <summary>
	/// 奈何桥副本
	/// </summary>
	COPY=32,
	/// <summary>
	/// 活动
	/// </summary>
	ACTIVITY=33,
	/// <summary>
	/// 竞技场
	/// </summary>
	ARENA=34,
	/// <summary>
	/// 修行
	/// </summary>
	PRACTICE=35,
	/// <summary>
	/// 法宝
	/// </summary>
	MAGIC=36,
	/// <summary>
	/// 镇魔塔
	/// </summary>
	TOWNMAGIC=37,
	/// <summary>
	/// 商店
	/// </summary>
	STORE=38,
	/// <summary>
	/// 铸魂
	/// </summary>
	CASTINGSOUL=39,
	/// <summary>
	/// 领取奖励（在线奖励）
	/// </summary>
	ONLINEREWARDS=40,
	/// <summary>
	/// 福利
	/// </summary>
	WELFARE=41,
	/// <summary>
	/// 等级奖励
	/// </summary>
	LEVELREWARD=42,
	
	/// <summary>
	/// 挑战BOSS
	/// </summary>
	CHALLENGEBOSS=43,
	/// <summary>
	/// 每日必做
	/// </summary>
	ADAYWILLDO=44,
	
	/// <summary>
	/// 首充大礼
	/// </summary>
	FIRSTCHARGE=45,
	/// <summary>
	/// 节日活动
	/// </summary>
	FESTIVAL=46,
	/// <summary>
	/// 精彩活动
	/// </summary>
	WONDERFUL=47,
	/// <summary>
	/// 七天奖励
	/// </summary>
	SEVENDAYS=48,
	/// <summary>
	/// 藏宝库
	/// </summary>
	HIDDENTREASURE=49,
	/// <summary>
	/// 环任务
	/// </summary>
	RINGTASK=50,
	/// <summary>
	/// 试炼任务
	/// </summary>
	TESTTASK=51,
	/// <summary>
	/// 地宫BOSS
	/// </summary>
	UNDERBOSS=52,
	/// <summary>
	/// 场景BOSS
	/// </summary>
	SCENEBOSS=53,
	/// <summary>
	/// 封印BOSS
	/// </summary>
	SEALBOSS=54,
	/// <summary>
	/// 熔恶BOSS
	/// </summary>
	RONGEBOSS=55,
	/// <summary>
	/// 里熔恶BOSS
	/// </summary>
	LIRONGEBOSS=56,
    /// <summary>
    /// 开服贺礼
    /// </summary>
    OPENSERVER=57,
	/// <summary>
	/// 宠物成长
	/// </summary>
	PETGROWUP = 58,
	/// <summary>
	/// 宠物灵修
	/// </summary>
	PETTHEKING = 59,
	/// <summary>
	/// 宠物融合
	/// </summary>
	PETFUSE = 60,
	/// <summary>
	/// 宠物守护
	/// </summary>
	PETGUARD = 61,
	/// <summary>
	/// 宠物技能
	/// </summary>
	PETSKILL=62,
	/// <summary>
	/// 每日必做
	/// </summary>
	DAILYMUSTDO = 63,
    /// <summary>
	MUSTDOUPGRADE = 63,
	/// <summary>
	/// 每日必做副本分页
	/// </summary>
	MUSTDOCOPPY = 64,
	/// <summary>
	/// 每日必做活动分页
	/// </summary>
	MUSTDOACTIVITY = 65,
	/// <summary>
	/// 每日必做其他分页
	/// </summary>
	MUSTDOOTHER = 66,
    /// <summary>
    /// 爱心大礼
    /// </summary>
    LOVEREWARD = 67,
    /// <summary>
    /// 开服活动
    /// </summary>
    OPENACTIVE = 68,
    /// <summary>
    /// 合服活动
    /// </summary>
   COMBINESERVER = 69,
	/// <summary>
	/// 主界面人物头像红点
	/// </summary>
	FIGUREHEADRED = 70,
    /// <summary>
    /// 兜率仙宫
    /// </summary>
    DOUSHUAIASGARD = 71,
    /// <summary>
    /// 五庄观
    /// </summary>
    FIVEMANOR = 72,
    /// <summary>
    /// 真假美猴王
    /// </summary>
    TRUEORFALSEMONKEY = 73,
    /// <summary>
    /// 白骨寒狱
    /// </summary>
    WHITEPURGATORY = 74,
    /// <summary>
    /// 骑装
    /// </summary>
    RIDINGSUIT = 75,
    /// <summary>
    /// 皇室宝箱
    /// </summary>
    ROYALBOX = 76,
    /// <summary>
    /// 仙盟捐献
    /// </summary>
    GUILDDONATE=77,
    /// <summary>
    /// 仙盟技能
    /// </summary>
    GUILDSKILL=78,
    /// <summary>
    /// 奇缘
    /// </summary>
    MIRACLE=79,
    /// <summary>
    /// 二冲
    /// </summary>
    TWOCHARGE=80,
    /// <summary>
    /// 登陆红利
    /// </summary>
    LOGINBONUS = 81,
    /// <summary>
    /// 挂机副本
    /// </summary>
    HANGUPCOPPY = 82,
    /// <summary>
    /// boss副本
    /// </summary>
    BOSSCOPPY = 83,
    /// <summary>
    /// 仙盟活跃
    /// </summary>
    GUILDACTIVE = 84,
}
/// <summary>
/// 功能提示目标
/// </summary>
public enum FunctionTipTag
{
    NONE,
    MAIN,//主菜单
    SOCIAL,//社交
    PACKAGE,//背包
    SKILL,//技能
    GROWUP,//成长
    STRENGTH,//强化
    MAIL,//邮件
    MOUNT,//坐骑
    GEM,//宝石
    GEMSLOT,//宝石槽位开启
    RARETREASURE,//稀世珍宝
    LOTTERY,//抽取
    SPECIALSTORE,//神秘商店
    HELPER,//小助手
    GUILD,//公会恶龙
    GUILDMEM,//公会成员
    CHARGE,//充值福利
    FIRSTCHARGE,//首次充值
    WEEKORMONTH,//周卡月卡
    DAILYCHARGE,//每日充值
    GROWTHPLAN,//成长计划
    CONSUMEREWARD,//消费返利
    TASK,//任务提示
}




public class FDictionary
{
    Hashtable mHashTable = new Hashtable();
    public object this[object key]
    {
        get
        {
            if (mHashTable.ContainsKey(key)) return mHashTable[key] as object;
            return null;
        }
        set
        {
            mHashTable[key] = value;
        }
    }

    public void Clear()
    {
        mHashTable.Clear();
    }

    public bool ContainsKey(object key)
    {
        return mHashTable.ContainsKey(key);
    }

    public void Remove(object key)
    {
        if (mHashTable.ContainsKey(key))
        {
            mHashTable.Remove(key);
        }
    }

    public ICollection Values
    {
        get
        {
            return mHashTable.Values;
        }
    }

    public ICollection Keys
    {
        get
        {
            return mHashTable.Keys;
        }
    }

    public int Count
    {
        get
        {
            return mHashTable.Count;
        }
    }

    public bool TryGetValue(object _key, out object _value)
    {
        if (mHashTable.ContainsKey(_key))
        {
            _value = mHashTable[_key];
            return true;
        }
        _value = false;
        return false;
    }

    public void Add(object _key, object _value)
    {
        if (_key == null) return;
        mHashTable[_key] = _value;
    }
}


/// <summary>
/// 阵营类型
/// </summary>
public enum CampType
{
    /// <summary>
    /// 无阵营1
    /// </summary>
    None = 1,
    /// <summary>
    /// 联盟2
    /// </summary>
    Alliance = 2,
    /// <summary>
    /// 部落3
    /// </summary>
    Tribe = 3,
}


public enum BuffType
{
    NONE = 0,
    /// <summary>
    /// 属性增加
    /// </summary>
    ATTRIBUTE = 1,
    /// <summary>
    /// 百分比属性增加
    /// </summary>
    ATTRIBUTEPER = 2,
    /// <summary>
    /// 回复
    /// </summary>
    RECOVER = 3,
    /// <summary>
    /// 百分比回复
    /// </summary>
    RECOVERPER = 4,
    /// <summary>
    /// 获得经验的万分比
    /// </summary>
    EXPINCREPER = 5,
    /// <summary>
    ///伤害减免
    /// </summary>
    DMGREDUCE = 6,
    /// <summary>
    /// 百分比伤害减免
    /// </summary>
    DMGREDUCEPER = 7,
    /// <summary>
    /// 伤害百分比吸收
    /// </summary>
    DMGABSORB = 8,
    /// <summary>
    /// 攻击者受到伤害的万分比
    /// </summary>
    THORN = 9,
    /// <summary>
    /// 对应的技能id
    /// </summary>
    COUNTER = 10,
    /// <summary>
    /// 伤害转化成护盾值的万分比
    /// </summary>
    SHIELD = 11,
    /// <summary>
    /// 伤害转化成法力值的万分比
    /// </summary>
    MANASHIELD = 12,
    /// <summary>
    /// 恢复生命的绝对值
    /// </summary>
    LIFESTEAL = 13,
    /// <summary>
    /// 无敌
    /// </summary>
    INVIN = 14,
    /// <summary>
    /// 冰箱
    /// </summary>
    FORZEN = 15,
    /// <summary>
    /// BUFF持续期间
    /// </summary>
    UNDIE = 16,
    /// <summary>
    /// 嘲讽
    /// </summary>
    TAUNT = 17,
    /// <summary>
    /// 控制免疫
    /// </summary>
    CONTROLIMMUNE = 18,
    /// <summary>
    /// 生效脚本
    /// </summary>
    SCRIPT = 19,
    /// <summary>
    /// 附带伤害的绝对值
    /// </summary>
    ADDDMG = 20,
    /// <summary>
    /// 风怒，每次攻击一定几率附带一次普通攻击伤害
    /// </summary>
    WINDFURY = 21,
    /// <summary>
    /// 隐身
    /// </summary>
    INVISIBLE = 22,
    /// <summary>
    /// 变更为指定的模型
    /// </summary>
    MODELCHANGE = 23,
    /// <summary>
    /// 范围伤害
    /// </summary>
    AOEDMG = 24,
    /// <summary>
    /// 纯控制BUFF,无其他效果
    /// </summary>
    CONTROL = 25,
    /// <summary>
    /// 霸体
    /// </summary>
    UNBREAK = 26,
    /// <summary>
    /// 增加玩家的基础属性百分比
    /// </summary>
    STATICATTRIBUTEPER=27,
    /// <summary>
    /// 减伤到一点血
    /// </summary>
    DMGREDUCEFIX,
}

/// <summary>
///  影响范围
/// </summary>
public enum BuffImpactAreaType
{
    NONE = 0,
    /// <summary>
    /// 圆形
    /// </summary>
    CYCLE = 1,
    /// <summary>
    /// 矩形
    /// </summary>
    RECT = 2,
    /// <summary>
    /// 扇形
    /// </summary>
    SECTOR = 3,
    /// <summary>
    /// 环形
    /// </summary>
    RING = 4,
    /// <summary>
    /// 点
    /// </summary>
    POINT = 5,
}
/// <summary>
/// 切换地图是否清除该BUFF
/// </summary>
public enum BuffMapCleanType
{
    NONE = 0,
    /// <summary>
    /// 过图不清除
    /// </summary>
    KEEP = 1,
    /// <summary>
    /// 过图清除
    /// </summary>
    CLEAN = 2,
    /// <summary>
    /// 过图不清，只回主城清除
    /// </summary>
    HOMECLEAN = 3,
}
/// <summary>
/// 在该BUFF下是否可以上下坐骑
/// </summary>
public enum BuffRiderEnableType
{
    NONE = 0,
    /// <summary>
    /// 有此BUFF时，可上下坐骑
    /// </summary>
    YES = 1,
    /// <summary>
    /// 有此BUFF时，不可上下坐骑
    /// </summary>
    NO = 2,
}
/// <summary>
/// 对BUFF目标的控制类型
/// </summary>
public enum BuffControlSortType
{
    /// <summary>
    /// 没有控制效果
    /// </summary>
    NONE = 0,
    /// <summary>
    /// 定身, 无法移动
    /// </summary>
    FREEZE = 1,
    /// <summary>
    /// 恐惧,无法进行任何操作，会自动随机移动
    /// </summary>
    FEAR = 2,
    /// <summary>
    /// 睡眠（闷棍、魅惑等）, 无法进行任何操作，受到伤害会解除
    /// </summary>
    SLEEP = 3,
    /// <summary>
    /// 放逐,无法进行任何操作，也不承受任何伤害
    /// </summary>
    BANISH = 4,
    /// <summary>
    /// 沉默,无法释放技能
    /// </summary>
    SILENT = 5,
    /// <summary>
    /// 引导技能中, 无法释放技能，可移动
    /// </summary>
    PERSISTENT = 6,
    /// <summary>
    /// 眩晕, 无法进行任何操作
    /// </summary>
    STUN = 7,
    /// <summary>
    /// 持续性技能, 无法释放技能，可移动，无法被沉默打断
    /// </summary>
    DURATIVE = 8,
}

/// <summary>
/// 过场动画单一动画行为的跳出类型
/// </summary>
public enum SceneAnimBreakType
{
	/// <summary>
	/// 结束动画
	/// </summary>
	End = 0,
	/// <summary>
	/// 可跳过
	/// </summary>
	CanBreak =1,
	/// <summary>
	/// 顺序播放，不可被中断
	/// </summary>
	Sequence = 2,
	/// <summary>
	/// 挂起，无视时间
	/// </summary>
	HoldOn = 3,
}


public enum SceneAnimTargetType
{
	/// <summary>
	/// 无
	/// </summary>
	None,
	/// <summary>
	/// 主玩家
	/// </summary>
	MainPlayer,
	/// <summary>
	/// 其他玩家
	/// </summary>
	OtherPlayer,
	/// <summary>
	/// NPC
	/// </summary>
	NPC,
	/// <summary>
	/// 怪物
	/// </summary>
	Monster,
	/// <summary>
	/// 主相机
	/// </summary>
	MainCamera,
}
/// <summary>
/// 过场动画类型
/// </summary>
public enum SceneAnimationType
{
	/// <summary>
	/// 未知
	/// </summary>
	None,
	/// <summary>
	/// 增加角色以及场景物品对象
	/// </summary>
	AddInterActiveObj,
	/// <summary>
	/// 删除目标角色以及场景物品对象
	/// </summary>
	DelInterActiveObj,
	/// <summary>
	/// 控制角色的位移（同时播放移动动作）
	/// </summary>
	ActorMove,
	/// <summary>
	/// 播放角色的动作与动作对应的特效
	/// </summary>
	ActorAction,
	/// <summary>
	/// 传送角色至目标坐标（同时目标坐标播放传送特效）
	/// </summary>
	ActorTransfer,
	/// <summary>
	/// 角色头顶播放表情（聊天中的表情图片）
	/// </summary>
	ActorExpression,
	/// <summary>
	/// 角色NPC对话
	/// </summary>
	NPCDialog,
	/// <summary>
	/// 角色NPC泡泡说话
	/// </summary>
	BubbleText,
	/// <summary>
	/// 于目标坐标点播放特效
	/// </summary>
	ActionEffect,
	/// <summary>
	/// 播放flash动画
	/// </summary>
	Flash,
	/// <summary>
	/// 播放音乐、音效
	/// </summary>
	AudioAction,
	/// <summary>
	/// 原画演示
	/// </summary>
	ArtistShow,
	/// <summary>
	/// 初始化场景（将主城恢复成游戏默认的正常状态）
	/// </summary>
	BackToScene,
	/// <summary>
	/// UI遮罩及渐变（例：全屏一黑、全屏慢慢变黑）
	/// </summary>
	ScreenChange,
	/// <summary>
	/// 移动镜头
	/// </summary>
	CameraMove,
	/// <summary>
	/// 晃动镜头
	/// </summary>
	CameraShake,
	/// <summary>
	/// 弹出命名UI
	/// </summary>
	OpenUI,
	/// <summary>
	/// 等待
	/// </summary>
	HoldOn,
	/// <summary>
	/// 电影边框
	/// </summary>
	FilmFrame,
	/// <summary>
	/// 镜头绑定（为了适应旧CG表，实际行为可以适配成CameraMove）
	/// </summary>
	LensBinding,
	/// <summary>
	/// 隐藏怪物或者NPC
	/// </summary>
	HiddenMonster,
	/// <summary>
	/// 平移，一般用于被击退
	/// </summary>
	Translation,
	/// <summary>
	/// 设置主角站立姿态
	/// </summary>
	StandbyState,
	/// <summary>
	/// 设置漂浮文字的隐藏显示
	/// </summary>
	DigitalDisplay,
	/// <summary>
	/// 对象死亡
	/// </summary>
	ActorDead,
	/// <summary>
	/// 技能循环
	/// </summary>
	ActionCycle,
	/// <summary>
	/// 黑白彩色切换
	/// </summary>
	BlackWhite,
	/// <summary>
	/// 相机自由移动(包含角度)
	/// </summary>
	FreeCamera,
	/// <summary>
	/// 显示某个指定UI
	/// </summary>
	DisplayUI,
	/// <summary>
	/// 点击屏幕后执行下一步否则等待设定时间后才执行下一步
	/// </summary>
	SingleClick,
	/// <summary>
	/// 双击屏幕后执行下一步否则等待设定时间后才执行下一步
	/// </summary>
	DoubleClick,
}