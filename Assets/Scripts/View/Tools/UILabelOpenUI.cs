//===============================
//作者：邓成
//日期：2016/5/18
//用途：前往打开某个界面的组件
//===============================

using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class UILabelOpenUI : MonoBehaviour {
	public string eg = ConfigMng.Instance.GetUItext(350);
	public string uiDes = ConfigMng.Instance.GetUItext(351);
	public string uiType = "MALL";
	public UILabel labText;

	void OnEnable()
	{
        GameHelper.SetUrlToUIByWndId(labText, uiDes, uiType);
	}
}
//===============================
//	/// <summary>
//	/// 二级界面枚举
//	/// </summary>
//	public enum SubGUIType
//	{
//		NONE,
//
//		#region 何明军
//		/// <summary>
//		/// 合成	
//		/// </summary>
//		BSynthesis,
//		/// <summary>
//		/// 人物基础属性	
//		/// </summary>
//		BPlayerInfo,
//		/// <summary>
//		/// 转生
//		/// </summary>
//		BRein,
//		/// <summary>
//		/// 邮件
//		/// </summary>
//		BMail,
//		/// <summary>
//		/// 单人副本
//		/// </summary>
//		BCopyTypeOne,
//		/// <summary>
//		/// 多人副本
//		/// </summary>
//		BCopyType,
//		#endregion
//
//		#region add dc
//		/// <summary>
//		/// 预览他人装备
//		/// </summary>
//		PREVIEWEQUIP,
//		/// <summary>
//		/// 预览他人信息
//		/// </summary>
//		PREVIEWINFORMATION,
//		/// <summary>
//		/// 预览他人宠物
//		/// </summary>
//		PREVIEWPET,
//		/// <summary>
//		/// 每日运镖
//		/// </summary>
//		DAILYDART,
//		/// <summary>
//		/// 仙盟运镖
//		/// </summary>
//		GUILDDART,
//		/// <summary>
//		/// 公会主界面的运镖
//		/// </summary>
//		GUILDMAINDART,
//		/// <summary>
//		/// 攻城战城内商店
//		/// </summary>
//		GUILDCITYSTORE,
//		/// <summary>
//		/// 强化
//		/// </summary>
//		STRENGTHING,
//		/// <summary>
//		/// 升阶
//		/// </summary>
//		EQUIPMENTUPGRADE,
//		/// <summary>
//		/// 橙炼
//		/// </summary>
//		ORANGEREFINE,
//		/// <summary>
//		/// 洗练
//		/// </summary>
//		EQUIPMENTWASH,
//		/// <summary>
//		/// 镶嵌,
//		/// </summary>
//		EQUIPMENTINLAY,
//		/// <summary>
//		/// 继承
//		/// </summary>
//		EQUIPMENTEXTEND,
//		#endregion
//		CHOOSE_CHAR,
//		CREATE_CHAR,
//		/// <summary>
//		/// 佣兵界面
//		/// </summary>
//		ENTOURAGE_BRIEF,
//		ENTOURAGE_EQ,
//		ENTOURAGE_PROP,
//		/// <summary>
//		/// 坐骑子界面
//		/// </summary>
//		MOUNTFEED,
//		MOUNTEQ,
//		/// <summary>
//		/// 系统设置,渲染子界面
//		/// </summary>
//		SYSTEM_SETTING_RENDER,
//		/// <summary>
//		/// 系统设置,帐号相关子界面
//		/// </summary>
//		SYSTEM_SETTING_ID,
//		/// <summary>
//		/// 商城子界面
//		/// </summary>
//		NORMALMALL,
//		/// <summary>
//		/// 签到子界面
//		/// </summary>
//		DAILYCHECK,
//		/// <summary>
//		/// 日常子界面
//		/// </summary>
//		DAILYACTIVE,
//		/// <summary>
//		/// 公会信息子界面
//		/// </summary>
//		GUILDINFO,
//		/// <summary>
//		/// 队伍设置子界面 by 贺丰
//		/// </summary>
//		TEAMSETTING,
//		/// <summary>
//		/// 信息子界面
//		/// </summary>
//		INFORMATION,
//		/// <summary>
//		/// 称号界面
//		/// </summary>
//		TITLE,
//
//		/// <summary>
//		/// 预览他人佣兵
//		/// </summary>
//		PREVIEWENTOURAGE,
//		/// <summary>
//		/// 预览他人遗物
//		/// </summary>
//		PREVIEWRELIC,
//		GUILDBUILDING,
//		/// <summary>
//		/// 公会副本
//		/// </summary>
//		GUILDDUNGEON,
//		/// <summary>
//		/// 排行榜
//		/// </summary>
//		UNION_RANK,
//		CLAN_RANK,
//		LEV_RANK,
//
//		/// <summary>
//		/// 大地图
//		/// </summary>
//		WORLDMAP,
//		CHANGELINE,
//
//		/// <summary>
//		/// 普通奖励子界面
//		/// </summary>
//		SEVENDAYSREWARDSUBWND,
//		PLAYERLEVELSUBWND,
//		CHARGEREWARDSUBWND,
//
//
//		/// <summary>
//		/// 试炼场难度窗口
//		/// </summary>
//		TRIALSDIFFICULT,
//
//		/// <summary>
//		/// 阵营子窗口
//		/// </summary>
//		CAMPBASE,
//		CAMPCONGRESS,
//		DRAGON,
//
//		/// <summary>
//		/// 排行榜子窗口
//		/// </summary>
//		ADMIRED_RANK,
//
//		/// <summary>
//		/// 强化子窗口
//		/// </summary>
//		STRENG,
//		STARUP,
//		RECAST,
//		INLAY,
//
//		/// <summary>
//		/// 宠物子窗口
//		/// </summary>
//		PETINFORMATION,//信息
//		GROWUP,//成长
//		LINGXIU,//灵修
//		FUSE,//融合
//		GUARD,//守护
//		PETSKILL,//技能
//		MOUNT,//坐骑
//		ILLUSION,//幻化
//
//		#region add ljq
//		PREVIEWREWAD,// 藏宝阁预览窗口
//		WAREHOUSE,//临时仓库窗口
//		REWARD,//奖励窗口
//		NEWRANKINGSUBWND,//排行榜
//		ACHIEVEMENT,//成就
//		#endregion
//		/// <summary>
//		/// 仙友子界面
//		/// </summary>
//		FRIEND,
//		/// <summary>
//		/// 仙侣子界面
//		/// </summary>
//		COUPLE,
//		/// <summary>
//		/// 结义子界面
//		/// </summary>
//		SWORN,
//		/// <summary>
//		/// 飞升子界面
//		/// </summary>
//		SOARING,
//		/// <summary>
//		///二级背包界面 
//		/// </summary>
//		SUBBACKPACK,
//	}
//
//	/// <summary>
//	/// 一级界面枚举
//	/// </summary>
//	public enum GUIType
//	{
//		/// <summary>
//		/// 空,非任何窗口(关闭一切)
//		/// </summary>
//		NONE,
//		/// <summary>
//		/// 登陆界面
//		/// </summary>
//		LOGIN,
//		/// <summary>
//		/// 选角界面
//		/// </summary>
//		CHOOSE_CHAR,
//		/// <summary>
//		/// 创角界面
//		/// </summary>
//		CREATE_CHAR,
//		/// <summary>
//		/// 加载等待界面(圆圈)
//		/// </summary>
//		WAIT,
//		/// <summary>
//		/// 请求加载等待滚动界面(圆圈)
//		/// </summary>
//		PANELLOADING,
//		/// <summary>
//		/// 提示
//		/// </summary>
//		MESSAGE,
//		/// <summary>
//		/// 二次确认框
//		/// </summary>
//		SECONDCONFIRM,
//		/// <summary>
//		/// 加载界面(黑屏读条)
//		/// </summary>
//		LOADING,
//		#region NPC相关
//		/// <summary>
//		/// NPC对话UI
//		/// </summary>
//		NPCDIALOGUE,
//		/// <summary>
//		/// NPC每日运镖界面
//		/// </summary>
//		NPCDAILYDART,
//		/// <summary>
//		/// NPC仙盟运镖界面
//		/// </summary>
//		NPCGUILDDART,
//		/// <summary>
//		/// NPC膜拜
//		/// </summary>
//		NPCMORSHIP,
//		/// <summary>
//		/// NPC仙侣
//		/// </summary>
//		NPCXIANLV,
//		/// <summary>
//		/// NPC结义
//		/// </summary>
//		NPCSWORN,  
//		#endregion
//		/// <summary>
//		/// 取名字
//		/// </summary>
//		ENTERNAME,
//		/// <summary>
//		/// 登陆奖励
//		/// </summary>
//		//LOGREWARDS,
//
//		/// <summary>
//		/// 队伍 
//		/// </summary>
//		TEAMMATEWND,
//		/// <summary>
//		/// 活动消耗确认
//		/// </summary>
//		ATIVITYCONSUMPTION,
//		/// <summary>
//		/// 转职
//		/// </summary>
//		CHARINFJOB,
//		/// <summary>
//		/// 礼包开启
//		/// </summary>
//		TREASURECHEST,
//		/// <summary>
//		/// 战斗失败
//		/// </summary>
//		FORCE,
//		/// <summary>
//		/// 战斗失败提示
//		/// </summary>
//		FORCETIP,
//		/// <summary>
//		/// 探宝
//		/// </summary>
//		TREASURE,
//		/// <summary>
//		/// 副本结算
//		/// </summary>
//		COPYWIN,
//		/// <summary>
//		/// 副本结束或者复活
//		/// </summary>
//		DEADWND,
//		/// <summary>
//		/// 任务寻路中提示窗
//		/// </summary>
//		TASK_FINDING,
//		/// <summary>
//		/// 在线奖励
//		/// </summary>
//		ONLINEREWARDS,
//		/// <summary>
//		/// 小助手
//		/// </summary>
//		FIGHTVALUE,
//		/// <summary>
//		/// 任务主界面 by吴江
//		/// </summary>
//		TASK_MAIN,
//		/// <summary>
//		/// 时装界面
//		/// </summary>
//		COSMETIC,
//		/// <summary>
//		/// 黑屏窗口
//		/// </summary>
//		BLACK_SCREEN,
//		/// <summary>
//		/// 连击窗口
//		/// </summary>
//		CONTINU_HIT,
//		/// <summary>
//		/// 礼包奖励窗口
//		/// </summary>
//		REWARD_BOX,
//		/// <summary>
//		/// 场景动画
//		/// </summary>
//		SCENE_ANIMATION,
//		/// <summary>
//		///  主城主界面
//		/// </summary>
//		MAINCITY,
//		/// <summary>
//		/// 地下城主界面
//		/// </summary>
//		MAINDUNGEON,
//		/// <summary>
//		/// 技能升级界面
//		/// </summary>
//		ABILITY,
//		/// <summary>
//		/// 系统设置界面
//		/// </summary>
//		SYSTEMSETTING,
//		/// <summary>
//		/// 消息确认窗口
//		/// </summary>
//		MESSAGEBOX,
//		/// <summary>
//		/// 邮件窗口
//		/// </summary>
//		MAIL,
//		/// <summary>
//		/// 聊天窗口
//		/// </summary>
//		CHAT,
//		/// <summary>
//		/// 矿工战斗倒计时窗口
//		/// </summary>
//		COMBATCOUNTDOWN,
//		/// <summary>
//		/// 矿工最终结算窗口
//		/// </summary>
//		FINALSETTLEMEN,
//		/// <summary>
//		/// 矿工单次结算窗口
//		/// </summary>
//		INTERIMAWARD,
//		/// <summary>
//		/// 矿工提示矿口
//		/// </summary>
//		PROTECTMINERSTIP,
//		/// <summary>
//		/// 查看其他玩家信息
//		/// </summary>
//		PREVIEW_OPC,
//		/// <summary>
//		/// 怪物选中头像界面
//		/// </summary>
//		MONSTER_HEAD,
//		/// <summary>
//		/// 查看主玩家信息(背包,属性,主玩家预览)
//		/// </summary>
//		PREVIEW_MAIN,
//		/// <summary>
//		/// 修行界面
//		/// </summary>
//		PRACTICE,
//		/// <summary>
//		/// VIP详细界面
//		/// </summary>
//		VIP,
//		/// <summary>
//		/// 迷你背包
//		/// </summary>
//		MINIBACKPACK,
//		/// <summary>
//		/// 技能学习系统
//		/// </summary>
//		SKILL,
//		/// <summary>
//		/// 社交界面 包括好友和消息中心
//		/// </summary>
//		SOCIAL,
//		/// <summary>
//		/// 商城界面
//		/// </summary>
//		MALL,
//		/// <summary>
//		/// 商店界面
//		/// </summary>
//		STORE,
//		/// <summary>
//		/// 荣誉商店界面
//		/// </summary>
//		HORNORMALL,
//		/// <summary>
//		/// 英雄试炼商店界面
//		/// </summary>
//		HEROMALL,
//		/// <summary>
//		/// 获得需预览模型的新物品提示窗口
//		/// </summary>
//		NEWTHINGSGET,
//		/// <summary>
//		/// 使用条件显示界面 
//		/// </summary>
//		USECONDITION,
//		/// <summary>
//		/// 公会创建界面
//		/// </summary>
//		GUILDCREATE,
//		/// <summary>
//		/// 公会主界面
//		/// </summary>
//		GUILDMAIN,
//		/// <summary>
//		/// 坐骑
//		/// </summary>
//		MOUNT,
//		/// <summary>
//		/// 断线重新连接
//		/// </summary>
//		RECONNECT,
//		/// <summary>
//		/// 章节
//		/// </summary>
//		CHAPTER,
//		/// <summary>
//		/// 阵营
//		/// </summary>
//		CAMPJOIN,
//		CAMPMAIN,
//		/// <summary>
//		/// 膜拜
//		/// </summary>
//		WORSHIP,
//		/// <summary>
//		/// 拍卖行
//		/// </summary>
//		AUCTION,
//		/// <summary>
//		/// 战斗力改变提示
//		/// </summary>
//		FIGHTVALUETIP,
//		/// <summary>
//		/// 快捷购买
//		/// </summary>
//		QUICKBUY,
//		/// <summary>
//		/// 宝石升级
//		/// </summary>
//		GEMUPWND,
//		/// <summary>
//		/// 排行榜
//		/// </summary>
//		RANK,
//		/// <summary>
//		/// 竞技场主界面
//		/// </summary>
//		ARENE,
//		/// <summary>
//		/// 竞技场回合界面
//		/// </summary>
//		COMPOUND,
//		/// <summary>
//		/// 等待其他玩家加载UI
//		/// </summary>
//		WAITOTHERPLAYERS,
//		/// <summary>
//		/// 切磋界面
//		/// </summary>
//		PKUI,
//
//		/// <summary>
//		/// 快捷回复聊天
//		/// </summary>
//		CHATFASTREPLY,
//		/// <summary>
//		/// 副本选择
//		/// </summary>
//		DUNGEON_SELECT,
//		/// <summary>
//		/// 成长（专长）
//		/// </summary>
//		GROWUP,
//		/// <summary>
//		/// 强化
//		/// </summary>
//		STRENGTHEN,
//		/// <summary>
//		/// 活动匹配
//		/// </summary>
//		ATIVITYMATCHING,
//		/// <summary>
//		/// 强化mini
//		/// </summary>
//		STRENGTHENMINI,
//		/// <summary>
//		/// 雇佣军
//		/// </summary>
//		MERCENSRYARMY,
//		/// <summary>
//		/// 雇佣军询问
//		/// </summary>
//		MERCENSRYARMYPop,
//		/// <summary>
//		/// 合作与对抗试炼界面
//		/// </summary>
//		MANYTRIALSCOMBAT,
//		/// <summary>
//		/// 涌金泉
//		/// </summary>
//		GOLDPOOL,
//		/// <summary>
//		/// 福利系统
//		/// </summary>
//		REWARD,
//		/// <summary>
//		/// 暗月旅团
//		/// </summary>
//		SMALLGAME,
//		/// <summary>
//		/// 重磅奖励
//		/// </summary>
//		SUPERREWARD,
//		/// <summary>
//		/// 资源更新进度
//		/// </summary>
//		UPDATEASSET,
//		/// <summary>
//		/// 精英挑战二级介绍界面
//		/// </summary>
//		COPYABYSS,
//		/// <summary>
//		/// 系统公告
//		/// </summary>
//		FULLSCALE,
//		/// <summary>
//		/// 创建角色提示窗口
//		/// </summary>
//		CREATE_PLAYER_TIP,
//		/// <summary>
//		/// 说明系统界面
//		/// </summary>
//		DESCRIPTIONWND,
//		/// <summary>
//		/// 3V3竞技场结算界面
//		/// </summary>
//		CAPTUREFLAG,
//		/// <summary>
//		///  获取装备展示界面 
//		/// </summary>
//		EQUIPMENTSHOW,
//		/// <summary>
//		/// 服务器选择
//		/// </summary>
//		SERVERCHOICE,
//		/// <summary>
//		///副本扫荡
//		/// </summary>
//		SWEEPCARBON,
//		/// <summary>
//		///军衔等级界面
//		/// </summary>
//		MILITARYLV,
//		/// <summary>
//		///军衔等级升级界面
//		/// </summary>
//		MILITARYLVUP,
//		/// <summary>
//		/// 公会祭坛快捷窗口
//		/// </summary>
//		GUILDALTARPOP,
//		/// <summary>
//		/// 雇佣军结算窗口
//		/// </summary>
//		ARMYWINWND,
//		/// <summary>
//		/// 充值福利
//		/// </summary>
//		CHARGEWELFARE,
//		/// <summary>
//		/// 竞技联赛窗口
//		/// </summary>
//		SPORTSLEAGUE,
//		/// <summary>
//		/// 时装到期窗口
//		/// <summary>
//		SHOWEXPIRE,
//		/// <summary>
//		/// 双击翻滚窗口
//		/// </summary>
//		ROLLINGGUIDE,
//		/// <summary>
//		/// 双击翻滚窗口CD
//		/// </summary>
//		ROLLINGGUIDECD,
//		/// <summary>
//		/// 竞技，决斗，切磋同意结算窗口
//		/// </summary>
//		SETTLEMENT,
//		/// <summary>
//		/// 副本5S奖励提示
//		/// </summary>
//		COPPYREWARDWND,
//		/// <summary>
//		/// 战斗UI界面  以下新项目开始
//		/// </summary>
//		MAINFIGHT,
//		/// <summary>
//		/// 小地图
//		/// </summary>
//		LITTLEMAP,
//		/// <summary>
//		/// 大地图
//		/// </summary>
//		LARGEMAP,
//		/// <summary>
//		/// 任务引导
//		/// </summary>
//		TASK,
//		/// <summary>
//		/// 菜单
//		/// </summary>
//		MENU,
//		/// <summary>
//		/// 技能升级
//		/// </summary>
//		SKILLUPGRADE,
//		/// <summary>
//		/// 人物属性
//		/// </summary>
//		PROPERTY,
//		/// <summary>
//		/// 雇佣军
//		/// </summary>
//		SOLDIERS,
//		/// <summary>
//		/// 怪物头像
//		/// </summary>
//		MONSTERHEAD,
//		/// <summary>
//		/// 强化界面
//		/// </summary>
//		STRENG,
//		/// <summary>
//		/// 信息展示界面
//		/// </summary>
//		INFORMATION,
//		/// <summary>
//		/// 快速组队界面 by 贺丰
//		/// </summary>
//		QUICKTEAM,
//		/// <summary>
//		/// 遗物界面 by龙英杰
//		/// </summary>
//		ANCIENTRELIC,
//		/// <summary>
//		/// 遗物升级界面 by龙英杰
//		/// </summary>
//		RELICUP,
//		/// <summary>
//		/// 排行榜 by龙英杰
//		/// </summary>
//		RANKING,
//		/// <summary>
//		/// 好友界面 by龙英杰
//		/// </summary>
//		FRIEND,
//		/// <summary>
//		/// 好友推荐界面 by龙英杰
//		/// </summary>
//		RECOMMENDFRIENDS,
//		/// <summary>
//		/// 历史信息界面 by龙英杰
//		/// </summary>
//		HISTORYITEMS,
//		/// <summary>
//		/// 倒计时器 by龙英杰
//		/// </summary>
//		TIMER,
//		/// <summary>
//		/// 试炼场
//		/// </summary>
//		TRIALS,
//		/// <summary>
//		/// 副本 by龙英杰
//		/// </summary>
//		DUNGEON,
//		/// <summary>
//		/// 副本匹配窗口 by龙英杰
//		/// </summary>
//		DUNGEONREADY,
//		/// <summary>
//		/// 副本结算 by龙英杰
//		/// </summary>
//		DUNGEONSETTLEMENT,
//		/// <summary>
//		/// 试炼场结算
//		/// </summary>
//		TRIALSETTLEMENT,
//		/// <summary>
//		/// 佣兵详细信息界面
//		/// </summary>
//		ENTOURAGE,
//		/// <summary>
//		/// 佣兵主界面
//		/// </summary>
//		ENTOURAGE_MAIN,
//		/// <summary>
//		/// 宝箱获得物品界面
//		/// </summary>
//		BOXGOTITEM,
//		/// <summary>
//		/// 打开每日界面
//		/// </summary>
//		DailyActivity,
//		/// <summary>
//		/// 普通奖励界面
//		/// </summary>
//		CommondReward,
//		/// <summary>
//		/// <summary>
//		/// 回收界面
//		/// </summary>
//		RECYCLE,
//		/// <summary>
//		/// 星级提升的统一界面
//		/// </summary>
//		STARUP,
//		/// <summary>
//		/// 邮箱
//		/// </summary>
//		MAILBOX,
//		/// <summary>
//		/// 随机宝箱抽取
//		/// </summary>
//		RANDCHEST,
//		/// <summary>
//		/// 随机宝箱获得物品界面
//		/// </summary>
//		RANDOMCHESTBOX,
//		/// <summary>
//		/// 探宝界面
//		/// </summary>
//		SEARCHTREASURE,
//		/// <summary>
//		/// 冒险界面
//		/// </summary>
//		ADVENTURE,
//		/// <summary>
//		/// 挖宝状态界面
//		/// </summary>
//		DURINGDIGTREATURE,
//		/// <summary>
//		/// 悬赏任务界面
//		/// </summary>
//		POSTREWARD,
//		/// <summary>
//		/// 阵营任务界面
//		/// </summary>
//		CAMPREWARD,
//		/// <summary>
//		/// 英雄挑战界面
//		/// </summary>
//		HEROKILLER,
//		/// <summary>
//		/// 英雄挑战继续界面
//		/// </summary>
//		HEROKILLERCONTINUE,
//		/// <summary>
//		/// 宝箱获得物品界面
//		/// </summary>
//		BOXREWARD,
//		/// <summary>
//		/// 预览他人信息
//		/// </summary>
//		PREVIEWOTHERS,
//		/// <summary>
//		/// 新手引导
//		/// </summary>
//		NEWBIEGUIDE,
//		/// <summary>
//		/// 法宝界面
//		/// </summary>
//		MAGICWEAPON,
//		/// <summary>
//		/// 宠物界面
//		/// </summary>
//		SPRITEANIMAL,
//		/// <summary>
//		/// 送花界面
//		/// </summary>
//		SENDFLOWER,
//		/// <summary>
//		/// 选择婚礼类型界面
//		/// </summary>
//		MARRIAGE,
//		/// <summary>
//		/// 等级奖励界面
//		/// </summary>
//		RANKREWARD,
//		/// <summary>
//		/// 每日奖励界面
//		/// </summary>
//		EVERYDAYREWARD,
//
//		#region 仙侠新添枚举
//		/// <summary>
//		/// 镇魔塔
//		/// </summary>
//		MagicTowerWnd,
//		/// <summary>
//		/// 活动大厅
//		/// </summary>
//		ATIVITY,
//		/// <summary>
//		/// 竞技场结算界面
//		/// </summary>
//		ARENERESULT,
//		/// <summary>
//		/// 背包界面(包括分解、合成)
//		/// </summary>
//		BACKPACK,
//		/// <summary>
//		/// 背包界面(只有单独的背包)
//		/// </summary>
//		BACKPACKWND,
//		/// <summary>
//		/// 副本结算 翻牌
//		/// </summary>
//		COPYWINFLOP,
//		/// <summary>
//		/// <summary>
//		/// 多人准备
//		/// </summary>
//		COPYMULTIPLEWND,
//		/// <summary>
//		/// 副本入口
//		/// </summary>
//		COPYINWND,
//		/// <summary>
//		/// 无尽挑战
//		/// </summary>
//		ENDLESSWND,
//		/// <summary>
//		/// 仓库
//		/// </summary>
//		STORAGE,
//		/// <summary>
//		/// 环式任务
//		/// </summary>
//		RINGTASK,
//		/// <summary>
//		/// 试炼任务
//		/// </summary>
//		TRIALTASK,
//		/// <summary>
//		/// 试用翅膀弹窗界面
//		/// </summary>
//		TRIALWING,
//		/// <summary>
//		/// 装备培养
//		/// </summary>
//		EQUIPMENTTRAINING,
//		/// <summary>
//		/// 藏宝阁
//		/// </summary>
//		TREASUREHOUSE,
//		/// <summary>
//		/// 排行榜
//		/// </summary>
//		NEWRANKING,
//		/// <summary>
//		/// 七天奖励
//		/// </summary>
//		SEVENDAYREWARD,
//		/// <summary>
//		/// 首冲大礼
//		/// </summary>
//		FIRSTCHARGEBONUS,
//		/// <summary>
//		/// 通用说明弹窗
//		/// </summary>
//		DESCRIPTION,
//		/// <summary>
//		/// 物品购买
//		/// </summary>
//		BUYWND,
//		/// <summary>
//		/// 商店
//		/// </summary>
//		SHOPWND,
//		/// <summary>
//		/// 铸魂
//		/// </summary>
//		CASTSOUL,
//		/// <summary>
//		/// 商城
//		/// </summary>
//		NEWMALL,
//		/// <summary>
//		/// 仙盟商店
//		/// </summary>
//		GUILDSHOP,
//		/// <summary>
//		/// 仙盟仓库
//		/// </summary>
//		GUILDSTORAGE,
//		/// <summary>
//		/// 仙盟技能
//		/// </summary>
//		GUILDSILL,
//		/// <summary>
//		/// 其他仙盟
//		/// </summary>
//		GUILDLIST,
//		/// <summary>
//		/// 下载奖励
//		/// </summary>
//		DOWNLOADBONUS,
//		/// <summary>
//		/// 市场
//		/// </summary>
//		MARKET,
//		/// <summary>
//		/// 市场上架物品
//		/// </summary>
//		PUTAWAY,
//		/// <summary>
//		/// 挑战BOSS
//		/// </summary>
//		BOSSCHALLENGE,
//		/// <summary>
//		/// 副本UI
//		/// </summary>
//		MAINCOPPY,
//		/// <summary>
//		/// 在线奖励
//		/// </summary>
//		ONLINEREWARD,
//		/// <summary>
//		/// 运镖界面
//		/// </summary>
//		DARTWND,
//		/// <summary>
//		/// 武道会
//		/// </summary>
//		BUDOKAI,
//		/// <summary>
//		/// 武道会匹配
//		/// </summary>
//		BUDOKAIMATCHING,
//		/// <summary>
//		/// 仙域守护
//		/// </summary>
//		GUILDPROTECT,
//		/// <summary>
//		/// 攻城战
//		/// </summary>
//		GUILDSIEGE,
//		/// <summary>
//		/// 复活
//		/// </summary>
//		RESURRECTION,
//		/// <summary>
//		/// 公会战
//		/// </summary>
//		GUILDFIGHT,
//		/// <summary>
//		/// 仙盟篝火
//		/// </summary>
//		GUILDBONFIRE,
//		#endregion
//
//	}
//===============================
