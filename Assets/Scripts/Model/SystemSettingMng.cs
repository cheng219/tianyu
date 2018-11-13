//====================================
//作者：黄洪兴
//日期：2016/7/23
//用途：系统设置管理类
//=====================================




using UnityEngine;
using System.Collections;
using System;
using st.net.NetBase;

public class SystemSettingMng
{

	protected int lastVersionCode = -1;
	public int LastVersionCode
	{
		get
		{
			return lastVersionCode;
		}
		set
		{
			if (lastVersionCode != value)
			{
				lastVersionCode = value;
				PlayerPrefs.SetInt(LAST_VERSION_CODE, value);
			}
		}
	}

    #region 构造

    public static SystemSettingMng CreateNew()
    {
        if (GameCenter.systemSettingMng == null)
        {
            SystemSettingMng systemSettingMng = new SystemSettingMng();
            systemSettingMng.Init();
            return systemSettingMng;
        }
        else
        {
            GameCenter.systemSettingMng.UnRegist();
            GameCenter.systemSettingMng.Init();
            return GameCenter.systemSettingMng;
        }
    }


    protected void Init()
    {
        showStateTexts = PlayerPrefs.HasKey(SHOW_STATE_TEXTS) ? PlayerPrefs.GetInt(SHOW_STATE_TEXTS) > 0 : true;
        cullingShow = true;

        //音乐音量
        bgmVolume = PlayerPrefs.HasKey(SETTING_NAME_BGM_VOLUME) ? PlayerPrefs.GetFloat(SETTING_NAME_BGM_VOLUME) : 1.0f;
        //音效音量
        soundEffectVolume = PlayerPrefs.HasKey(SETTING_NAME_SOUND_EFFECT_VOLUME) ? PlayerPrefs.GetFloat(SETTING_NAME_SOUND_EFFECT_VOLUME) : 1.0f;
        NGUITools.soundVolume = soundEffectVolume;

        //打开音乐
        openMusic = PlayerPrefs.HasKey(OPEN_MUSIC) ? PlayerPrefs.GetInt(OPEN_MUSIC) > 0 : true;
        //打开音效
        openSoundEffect = PlayerPrefs.HasKey(OPEN_SOUND_EFFECT) ? PlayerPrefs.GetInt(OPEN_SOUND_EFFECT) > 0 : true;
        //自动播放世界频道语音
        autoPlayWorldVoice = PlayerPrefs.HasKey(AUTO_PLAY_WORLD_VOICE) ? PlayerPrefs.GetInt(AUTO_PLAY_WORLD_VOICE) > 0 : false;
        //自动播放仙盟频道语音
        autoPlayAllyVoice = PlayerPrefs.HasKey(AUTO_PLAY_ALLY_VOICE) ? PlayerPrefs.GetInt(AUTO_PLAY_ALLY_VOICE) > 0 : false;
        //自动播放队伍频道语音
        autoPlayTeamVoice = PlayerPrefs.HasKey(AUTO_PLAY_TEAM_VOICE) ? PlayerPrefs.GetInt(AUTO_PLAY_TEAM_VOICE) > 0 : true;
        //自动播放私聊频道语音
        autoPlayChatVoice = PlayerPrefs.HasKey(AUTO_PLAY_CHAT_VOICE) ? PlayerPrefs.GetInt(AUTO_PLAY_CHAT_VOICE) > 0 : true;

        //同屏人数,默认为30  
        maxPlayer = PlayerPrefs.HasKey(MAX_PLAYER) ? PlayerPrefs.GetInt(MAX_PLAYER) : 20;
        //是否展示血条
        ShowBloodSlider = PlayerPrefs.HasKey(SHOW_BLOOD_SLIDER) ? PlayerPrefs.GetInt(SHOW_BLOOD_SLIDER) > 0 : true;

        RealTimeShadow = PlayerPrefs.HasKey(REAL_SHADOW) ? PlayerPrefs.GetInt(REAL_SHADOW) > 0 : false;

		lastVersionCode = PlayerPrefs.HasKey(LAST_VERSION_CODE)?PlayerPrefs.GetInt(LAST_VERSION_CODE):-1;

        //CurRendererQuality = PlayerPrefs.HasKey(RENDERER_QUALITY) ? (RendererQuality)PlayerPrefs.GetInt(RENDERER_QUALITY) : RendererQuality.LOW;
        fluencyMode = PlayerPrefs.HasKey(FLUENCY_MODE) ? PlayerPrefs.GetInt(FLUENCY_MODE) > 0 : false;
        intelligentMode = PlayerPrefs.HasKey(INTELLIGENTMODE_MODE) ? PlayerPrefs.GetInt(INTELLIGENTMODE_MODE) > 0 : false;
        otherPlayerWing = PlayerPrefs.HasKey(OTHER_PLAYER_WING) ? PlayerPrefs.GetInt(OTHER_PLAYER_WING) > 0 : true;
        otherPlayerSkill = PlayerPrefs.HasKey(OTHER_PLAYER_SKILL) ? PlayerPrefs.GetInt(OTHER_PLAYER_SKILL) > 0 : true;
        otherPlayerPet = PlayerPrefs.HasKey(OTHER_PLAYER_PET) ? PlayerPrefs.GetInt(OTHER_PLAYER_PET) > 0 : true;
        otherPlayerTitle = PlayerPrefs.HasKey(OTHER_PLAYER_TITLE) ? PlayerPrefs.GetInt(OTHER_PLAYER_TITLE) > 0 : true;
        otherPlayerMagic = PlayerPrefs.HasKey(OTHER_PLAYER_MAGIC) ? PlayerPrefs.GetInt(OTHER_PLAYER_MAGIC) > 0 : true;
        otherPlayerEffect = PlayerPrefs.HasKey(OTHER_PLAYER_EFFECT) ? PlayerPrefs.GetInt(OTHER_PLAYER_EFFECT) > 0 : true;
        onlyHostile = PlayerPrefs.HasKey(ONLY_HOSTILE) ? PlayerPrefs.GetInt(ONLY_HOSTILE) > 0 : false;
        itemName = PlayerPrefs.HasKey(ITEM_NAME) ? PlayerPrefs.GetInt(ITEM_NAME) > 0 : true;
        monsterName = PlayerPrefs.HasKey(MONSTER_NAME) ? PlayerPrefs.GetInt(MONSTER_NAME) > 0 : true;
        selfEffect = PlayerPrefs.HasKey(SELF_EFFECT) ? PlayerPrefs.GetInt(SELF_EFFECT) > 0 : true;
        lowTexture = PlayerPrefs.HasKey("LOWTEXTURE") ? PlayerPrefs.GetInt("LOWTEXTURE") > 0 : false;
        CurRendererQuality = PlayerPrefs.HasKey(RENDERER_QUALITY) ? (RendererQuality)PlayerPrefs.GetInt(RENDERER_QUALITY) : RendererQuality.HIGHT;

        hasAutoSetQuality = PlayerPrefs.HasKey(HAS_SET_QUALITY) ? PlayerPrefs.GetInt(HAS_SET_QUALITY) > 0 : false;
        isSafeOpen = PlayerPrefs.HasKey("ISSAFEOPEN") ? PlayerPrefs.GetInt("ISSAFEOPEN") > 0 : true;
        safeModel = PlayerPrefs.HasKey("SAFEMODEL") ? PlayerPrefs.GetInt("SAFEMODEL") > 0 : true;
        isAutoBuy = PlayerPrefs.HasKey("ISAUTOBUY") ? PlayerPrefs.GetInt("ISAUTOBUY") > 0 : false;
        safeLifeNum = PlayerPrefs.HasKey("SAFELIFENUM") ? PlayerPrefs.GetInt("SAFELIFENUM") : 80;
        safeMagicNum = PlayerPrefs.HasKey("SAFEMAGICNUM") ? PlayerPrefs.GetInt("SAFEMAGICNUM") : 80;
        flyLifeNum = PlayerPrefs.HasKey("FLYLIFENUM") ? PlayerPrefs.GetInt("FLYLIFENUM") : 20;
        randomFlyLifeNum = PlayerPrefs.HasKey("RANDOMFLYLIFENUM") ? PlayerPrefs.GetInt("RANDOMFLYLIFENUM") : 20;
        isFlyOpen = PlayerPrefs.HasKey("ISFLYOPEN") ? PlayerPrefs.GetInt("ISFLYOPEN") > 0 : false;
        isRandomFlyOpen = PlayerPrefs.HasKey("ISRANDOMFLYOPEN") ? PlayerPrefs.GetInt("ISRANDOMFLYOPEN") > 0 : false;
        isAutoResurrection = PlayerPrefs.HasKey("ISAUTORESURRECTION") ? PlayerPrefs.GetInt("ISAUTORESURRECTION") > 0 : false;
        isHideBoss = PlayerPrefs.HasKey("ISHIDEBOSS") ? PlayerPrefs.GetInt("ISHIDEBOSS") > 0 : false;
        pickModel = PlayerPrefs.HasKey("PICKMODEL") ? PlayerPrefs.GetInt("PICKMODEL") : 1;
    }


    protected void UnRegist()
    {
    }
    #endregion

	#region http地址统计
    public static string NOTICE_HTTP_ADDRESS = "https://xy-uc.lynlzqy.com/rpc/handle?Act=GetAnment&GameID=8&SvrID=0&SourceID={0}&Time={1}&Sign={2}";
    public static string NOTICE_HTTP_ADDRESS_PARAMETER = "GameID=8&SourceID={0}&SvrID=0&Time={1}:226C887FBC3EF7155681B5A2AF89B5A8";

    public static string MY_SERVER_HTTP_ADDRESS = "https://xy-uc.lynlzqy.com/rpc/handle?Act=MyServerList&GameID=8&PageNumber=1&PageSize=10&SourceID={0}&SvrID=0&Time={1}&UserID={2}&Version={3}&Sign={4}";
    public static string MY_SERVER_HTTP_ADDRESS_PARAMETER = "GameID=8&PageNumber=1&PageSize=10&SourceID={0}&SvrID=0&Time={1}&UserID={2}&Version={3}:226C887FBC3EF7155681B5A2AF89B5A8";

    public static string PAGE_SERVER_HTTP_ADDRESS = "https://xy-uc.lynlzqy.com/rpc/handle?Act=ChooseServer&GameID=8&PageNumber={0}&PageSize=10&SourceID={1}&SvrID=0&Time={2}&Version={3}&Sign={4}";
    public static string PAGE_SERVER_HTTP_ADDRESS_PARAMETER = "GameID=8&PageNumber={0}&PageSize=10&SourceID={1}&SvrID=0&Time={2}&Version={3}:226C887FBC3EF7155681B5A2AF89B5A8";

    public static string WHITE_LIST_HTTP_ADDRESS = "https://xy-uc.lynlzqy.com/rpc/handle?Act=WhiteList&GameID=8&SourceID={0}&SvrID={1}&Time={2}&Sign={3}";
    public static string WHITE_LIST_HTTP_ADDRESS_PARAMETER = "GameID=8&SourceID={0}&SvrID={1}&Time={2}:226C887FBC3EF7155681B5A2AF89B5A8";

    //"http://xyuc.zhangwangkj.cn:7005/gameManage/sysloginnotice/loginNotice?platForm=1&sourceId={0}";
    //"http://xyuc.zhangwangkj.cn:7001/game_gm/rpc?act=GetUserInfo&sourceId={0}&time={1}&sign={2}&userid={3}&pg=1&versions={4}";
    //"https://xyuc.zhangwangkj.cn:7001/game_gm/rpc?act=GetServerInfo&sourceId={0}&time={1}&pg={2}&sign={3}&svrid={4}&versions={5}";
    //"http://xyuc.zhangwangkj.cn:7001/game_gm/rpc?act=ReqLogin&sourceId={0}&userid={1}&time={2}&svrid={3}&sign={4}";
	#endregion

    #region 静态配置
    public static readonly string SETTING_NAME_SOUND_EFFECT_VOLUME = "SE_VOLUME";

    public static readonly string SETTING_NAME_BGM_VOLUME = "BGM_VOLUME";

    public static readonly string SHOW_STATE_TEXTS = "SHOWSTATETEXTS";

    public static readonly string OPEN_MUSIC = "OPENMUSIC";

    public static readonly string OPEN_SOUND_EFFECT = "OPENSOUNDEFFECT";

    public static readonly string OPEN_VIBRATE = "OPENVIBRATE";

    public static readonly string SHOW_BLOOD_SLIDER = "SHOWBLOODSLIDER";

    public static readonly string EFFECT_LIMIT = "EFFECTLIMIT";

    public static readonly string MAX_PLAYER = "MAX_PLAYER";

    public static readonly string REAL_SHADOW = "REALSHADOW";

    public static readonly string HAS_SET_QUALITY = "HAS_SET_QUALITY";

    public static readonly string RENDERER_QUALITY = "RENDERERQUALITY";

    public static readonly string OTHER_PLAYER_WING = "OTHERPLAYERWING";

    public static readonly string OTHER_PLAYER_SKILL = "OTHERPLAYERSKILL";

    public static readonly string OTHER_PLAYER_PET = "OTHERPLAYERPET";

    public static readonly string OTHER_PLAYER_TITLE = "OTHERPLAYERTITLE";

    public static readonly string OTHER_PLAYER_MAGIC = "OTHERPLAYERMAGIC";

    public static readonly string OTHER_PLAYER_EFFECT = "OTHERPLAYEREFFECT";

    public static readonly string ONLY_HOSTILE = "ONLYHOSTILE";

    public static readonly string ITEM_NAME = "ITEMNAME";

    public static readonly string MONSTER_NAME = "MONSTERNAME";

    public static readonly string SELF_EFFECT = "SELFEFFECT";

    public static readonly string FLUENCY_MODE = "FLUENCYMODE";

    public static readonly string INTELLIGENTMODE_MODE = "INTELLIGENTMODEMODE";

	public static readonly string LAST_VERSION_CODE = "LASTVERSIONCODE";

    public static readonly string AUTO_PLAY_WORLD_VOICE = "AUTOPLAYWORLDVOICE";

    public static readonly string AUTO_PLAY_ALLY_VOICE = "AUTOPLAYALLYVOICE";

    public static readonly string AUTO_PLAY_TEAM_VOICE = "AUTOPLAYTEAMVOICE";

    public static readonly string AUTO_PLAY_CHAT_VOICE = "AUTOPLAYCHATVOICE";

    /// <summary>
    /// 超时时间限制 by吴江
    /// </summary>
    public const float TIME_OUT_LIMIT = 5.0f;
	/// <summary>
	/// 重连超时
	/// </summary>
	public const float RECONNECT_TIME_OUT = 1.5f;
    /// <summary>
    /// 初始化背包容量 by吴江
    /// </summary>
    public const int INIT_BAG_NUM = 60;
    /// <summary>
    /// 每次背包扩容增加的数量 by吴江
    /// </summary>
    public const int INCRE_BAG_NUM = 1;
    /// <summary>
    /// 背包上限
    /// </summary>
    public const int MAX_BAG_NUM = 120;
    /// <summary>
    /// 背包每页显示格子数
    /// </summary>
    public const int PER_PAGE_BAG_NUM = 20;
        /// <summary>
   /// 仓库上限  
     /// </summary>
    public const int MAX_STORAGE_NUM = 80;
    /// <summary>
    /// 公会仓库上限
    /// </summary>
    public const int MAX_GUILD_STORAGE_NUM = 60;
    /// <summary>
    /// 是否展示漂浮文字 by吴江
    /// </summary>
    protected static bool showStateTexts = true;
    /// <summary>
    /// 是否根据距离切割显示对象 by吴江
    /// </summary>
    protected static bool cullingShow = true;
    /// <summary>
    /// 背景音乐音量大小 by吴江
    /// </summary>
    protected static float bgmVolume = 1.0f;
    /// <summary>
    /// 音效音量大小   by吴江
    /// </summary>
    protected static float soundEffectVolume = 1.0f;
    /// <summary>
    /// 是否打开音乐  by吴江
    /// </summary>
    protected static bool openMusic = true;
    /// <summary>
    /// 是否打开音效  by吴江
    /// </summary>
    protected static bool openSoundEffect = true;
    /// 是否开启流畅模式 by黄洪兴
    /// </summary>
    protected static bool fluencyMode = false;
    /// <summary>
    /// 是否开启震动  by吴江
    /// </summary>
    protected static bool openVibrate = true;
    /// <summary>
    /// 是否开启实时阴影 by吴江
    /// </summary>
    protected static bool realTimeShadow = false;
    /// <summary>
    /// 是否开启智能模式 by黄洪兴
    /// </summary>
    protected static bool intelligentMode = false;
    /// <summary>
    /// 是否显示其他玩家翅膀 by黄洪兴
    /// </summary>
    protected static bool otherPlayerWing = true;
    /// <summary>
    /// 是否显示其他玩家技能特效 by黄洪兴
    /// </summary>
    protected static bool otherPlayerSkill = true;
    /// <summary>
    /// 是否显示其他玩家宠物 by黄洪兴
    /// </summary>
    protected static bool otherPlayerPet = true;
    /// <summary>
    /// 是否显示其他玩家称号 by黄洪兴
    /// </summary>
    protected static bool otherPlayerTitle = true;
    /// <summary>
    /// 是否显示其他玩家法宝 by黄洪兴
    /// </summary>
    protected static bool otherPlayerMagic = true;
    /// <summary>
    /// 是否显示其他玩家强化功能特效 by黄洪兴
    /// </summary>
    protected static bool otherPlayerEffect = true;
    /// <summary>
    /// 是否仅显示可攻击对象 by黄洪兴
    /// </summary>
    protected static bool onlyHostile = false;
    /// <summary>
    /// 是否显示掉落道具名称 by黄洪兴
    /// </summary>
    protected static bool itemName = true;
    /// <summary>
    /// 是否显示怪物名字 by黄洪兴
    /// </summary>
    protected static bool monsterName = true;
    /// <summary>
    /// 是否显示自身特效 by黄洪兴
    /// </summary>
    protected static bool selfEffect = true;
    /// <summary>
    /// 特效上限
    /// </summary>
    protected static int effectLimit = 10;
    /// <summary>
    /// 是否使用低纹理地图  by黄洪兴
    /// </summary>
    protected static bool lowTexture = false;
    /// <summary>
    /// 无操作进入省电模式的时间
    /// </summary>
    public const int POWERSAVING_TIME = 120;

    #region 挂机设置

    /// <summary>
    /// 是否开启自动吃药
    /// </summary>
    protected static bool isSafeOpen;
    /// <summary>
    /// 自动吃药的模式  从大到小为TRUE
    /// </summary>
    protected static bool safeModel = true;
    /// <summary>
    /// 是否自动购买药品
    /// </summary>
    protected static bool isAutoBuy;
    /// <summary>
    /// 自动吃红药的血量
    /// </summary>
    protected static int safeLifeNum;
    /// <summary>
    /// 自动吃蓝药的蓝量
    /// </summary>
    protected static int safeMagicNum;
    /// <summary>
    /// 自动使用回城的血量
    /// </summary>
    protected static int flyLifeNum;
    /// <summary>
    /// 自动使用随机的血量
    /// </summary>
    protected static int randomFlyLifeNum;
    /// <summary>
    /// 是否开启自动回城
    /// </summary>
    protected static bool isFlyOpen;
    /// <summary>
    /// 是否开启自动随机传送
    /// </summary>
    protected static bool isRandomFlyOpen;
    /// <summary>
    /// 是否自动复活
    /// </summary>
    protected static bool isAutoResurrection;
    /// <summary>
    /// 是否自动躲避BOSS
    /// </summary>
    protected static bool isHideBoss;
    /// <summary>
    /// 拾取品质
    /// </summary>
    protected static int pickModel;


    #endregion

    #region 语音设置 zsy

    /// <summary>
    /// 是否自动播放世界频道语音
    /// </summary>
    protected static bool autoPlayWorldVoice = false;
    /// <summary>
    /// 是否自动播放仙盟语音
    /// </summary>
    protected static bool autoPlayAllyVoice = false;
    /// <summary>
    /// 是否自动播放队友语音
    /// </summary>
    protected static bool autoPlayTeamVoice = true;
    /// <summary>
    /// 是否自动播放私聊语音
    /// </summary>
    protected static bool autoPlayChatVoice = true;
    /// <summary>
    /// 语音设置更新
    /// </summary>
    //public System.Action OnVoiceSetUpdate;
    #endregion


    /// <summary>
    /// 是否显示传送收费提示是则不显示
    /// </summary>
    public bool ShowFlyTips = false;
    /// <summary>
    /// 是否显示鲜花不足的快捷购买提示
    /// </summary>
    public bool ShowBuyFlower = true;


    /// <summary>
    /// 特效上限
    /// </summary>
    public int EffectLimit
    {
        get
        {
            return effectLimit;
        }
        set
        {
            if (effectLimit != value)
            {
                effectLimit = value;
                PlayerPrefs.SetInt(EFFECT_LIMIT, value);
            }
        }
    }
    /// <summary>
    /// 是否显示血条 by吴江
    /// </summary>
    protected static bool showBloodSlider = true;
    /// <summary>
    /// 是否显示血条 by吴江
    /// </summary>
    public bool ShowBloodSlider
    {
        get
        {
            return showBloodSlider;
        }
        set
        {
            if (showBloodSlider != value)
            {
                showBloodSlider = value;
                PlayerPrefs.SetInt(SHOW_BLOOD_SLIDER, value ? 1 : 0);
                if (OnUpdateShowBloodSlider != null)
                {
                    OnUpdateShowBloodSlider();
                }
            }
        }
    }
    public static Action OnUpdateShowBloodSlider;

    /// <summary>
    /// 是否开启实时阴影 by吴江
    /// </summary>
    public static bool RealTimeShadow
    {
        get
        {
            return realTimeShadow;
        }
        set
        {
            if (realTimeShadow != value)
            {
                realTimeShadow = value;
                PlayerPrefs.SetInt(REAL_SHADOW, value ? 1 : 0);
                if (OnUpdateRealTimeShadow != null)
                {
                    OnUpdateRealTimeShadow();
                }
            }
        }
    }
    public static Action OnUpdateRealTimeShadow;

    /// <summary>
    /// 是否展示漂浮文字（加减血，状态变化等）
    /// </summary>
    public static bool ShowStateTexts
    {
        get
        {
            return showStateTexts;
        }
        set
        {
            if (showStateTexts != value)
            {
                showStateTexts = value;
                PlayerPrefs.SetInt(SHOW_STATE_TEXTS, value ? 1 : 0);
            }
        }
    }

    /// <summary>
    /// 是否根据与主角的距离进行渲染切割
    /// </summary>
    public static bool CullingShow
    {
        get
        {
            return cullingShow;
        }
        set
        {
            cullingShow = value;
        }
    }

    #endregion

    #region 实时交互配置

    public System.Action<SystemSettingType, bool,float> OnUpdate;


    

    /// <summary>
    /// 屏蔽其他玩家法宝
    /// </summary>
    public System.Action OnUpdateOtherPlayerMagicWeapon;
    /// <summary>
    /// 屏蔽其他玩家翅膀事件
    /// </summary>
    public System.Action OnUpdateOtherPlayerWing;
    /// <summary>
    /// 屏蔽其他玩家技能特效事件
    /// </summary>
    public System.Action OnUpdateOtherPlayerSkill;

    /// <summary>
    /// 屏蔽其他玩家宠物事件
    /// </summary>
    public System.Action OnUpdateOtherPlayerPet;

    /// <summary>
    /// 最大人数显示变化事件
    /// </summary>
    public System.Action OnUpdateMaxPlayer;

    /// <summary>
    /// 更新他人称号显示事件
    /// </summary>
    public System.Action OnUpdateOtherPlayerTitle;

    /// <summary>
    /// 更新怪物名字显示事件
    /// </summary>
    public System.Action OnUpdateMonsterName;

    /// <summary>
    /// 更新掉落装备名字显示事件
    /// </summary>
    public System.Action OnUpdateItemName;

    #region 系统设置
    /// <summary>
    /// 是否开启流畅模式
    /// </summary>
    public bool FluencyMode
    {
        set
        {
            if (fluencyMode != value)
            {
                fluencyMode = value;
                PlayerPrefs.SetInt(FLUENCY_MODE, value ? 1 : 0);
            }
        }
        get
        {
            return fluencyMode;
        }
    }

    /// <summary>
    /// 是否开启智能模式
    /// </summary>
    public bool IntelligentMode
    {
        set
        {
            if (intelligentMode != value)
            {
                intelligentMode = value;
                PlayerPrefs.SetInt(INTELLIGENTMODE_MODE, value ? 1 : 0);
            }
        }
        get
        {
            return intelligentMode;
        }
    }
    protected static bool hasAutoSetQuality = false;
    public static bool HasAutoSetQuality
    {
        get
        {
            return hasAutoSetQuality;
        }
        set
        {
            if (hasAutoSetQuality != value)
            {
                hasAutoSetQuality = value;
                PlayerPrefs.SetInt(HAS_SET_QUALITY, value ? 1 : 0);
            }
        }
    }


    /// <summary>
    /// 背景音乐声音大小
    /// </summary>
	public float BGMVolume
    {
        set
        {
            if (bgmVolume != value)
            {
                bgmVolume = value;
                PlayerPrefs.SetFloat(SETTING_NAME_BGM_VOLUME, value);
                if (OnUpdate != null && OpenMusic) OnUpdate(SystemSettingType.Music, false, BGMVolume);
            }
        }
        get
        {
            return bgmVolume;
        }
    }
    /// <summary>
    /// 特效音效声音大小
    /// </summary>
	public float SoundEffectVolume
    {
        set
        {
            if (soundEffectVolume != value)
            {
                soundEffectVolume = value;
                PlayerPrefs.SetFloat(SETTING_NAME_SOUND_EFFECT_VOLUME, value);
                if (OnUpdate != null) OnUpdate(SystemSettingType.SoundEffect, openSoundEffect, SoundEffectVolume);
                NGUITools.soundVolume = value;
            }
        }
        get
        {
            return soundEffectVolume;
        }
    }
    /// <summary>
    /// 是否开放背景音乐
    /// </summary>
    public bool OpenMusic
    {
        get
        {
            return openMusic;
        }
        set
        {
            if (openMusic != value)
            {
                openMusic = value;
                PlayerPrefs.SetInt(OPEN_MUSIC, value ? 1 : 0);
                if (OnUpdate != null)
                {
                    if (openMusic)
                    {
                        OnUpdate(SystemSettingType.Music, openMusic, BGMVolume);
                    }
                    else
                    {
                        OnUpdate(SystemSettingType.Music, openMusic, 0);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 是否开放特效声音
    /// </summary>
    public bool OpenSoundEffect
    {

        get
        {
            return openSoundEffect;
        }
        set
        {
            if (openSoundEffect != value)
            {
                openSoundEffect = value;
                PlayerPrefs.SetInt(OPEN_SOUND_EFFECT, value ? 1 : 0);
                if (OnUpdate != null)
                {
                    if (openSoundEffect)
                    {
                        OnUpdate(SystemSettingType.SoundEffect, openSoundEffect, SoundEffectVolume);
                    }
                    else
                    {
                        OnUpdate(SystemSettingType.SoundEffect, openSoundEffect, 0);
                    }
                }
            }
        }

    }

    /// <summary>
    /// 是否开启震动
    /// </summary>
    public bool OpenVibrate
    {
        get
        {
            return openVibrate;
        }
        set
        {
            if (openVibrate != value)
            {
                openVibrate = value;
                PlayerPrefs.SetInt(OPEN_VIBRATE, value ? 1 : 0);
                if (OnUpdate != null) OnUpdate(SystemSettingType.Vibrate, value,0);
            }
        }
    }
    int maxPlayer = 20;
    public int MaxPlayer
    {
        get
        {
            return maxPlayer;
        }
        set
        {
            if (maxPlayer != value)
            {
                maxPlayer = value;
                PlayerPrefs.SetInt(MAX_PLAYER, maxPlayer);
                if (OnUpdateMaxPlayer != null) OnUpdateMaxPlayer();
                //Debug.Log("设置最大人数为" + maxPlayer);
            }
        }
    }
    /// <summary>
	/// 是否显示其他玩家翅膀
	/// </summary>
	public bool OtherPlayerWing
	{
		get
		{
			return otherPlayerWing;
		}
		set
		{
			if (otherPlayerWing != value)
			{
				otherPlayerWing = value;
				PlayerPrefs.SetInt(OTHER_PLAYER_WING, otherPlayerWing?1:0);
                if (OnUpdateOtherPlayerWing != null)
                    OnUpdateOtherPlayerWing();
			}
		}
	}

	/// <summary>
	/// 是否显示其他玩家技能特效
	/// </summary>
	public bool OtherPlayerSkill
	{
		get
		{
			return otherPlayerSkill;
		}
		set
		{
			if (otherPlayerSkill != value)
			{
				otherPlayerSkill = value;
				PlayerPrefs.SetInt(OTHER_PLAYER_SKILL, otherPlayerSkill?1:0);
                if (OnUpdateOtherPlayerSkill != null)
                    OnUpdateOtherPlayerSkill();
			}
		}
	}

	/// <summary>
	/// 是否显示其他玩家的宠物
	/// </summary>
	public bool OtherPlayerPet
	{
		get
		{
			return otherPlayerPet;
		}
		set
		{
			if (otherPlayerPet != value)
			{
				otherPlayerPet= value;
				PlayerPrefs.SetInt(OTHER_PLAYER_PET, otherPlayerPet?1:0);
                if (OnUpdateOtherPlayerPet != null) OnUpdateOtherPlayerPet();
			}
		}
	}

	/// <summary>
	/// 是否显示其他玩家的称号
	/// </summary>
	public bool OtherPlayerTitle
	{
		get
		{
			return otherPlayerTitle;
		}
		set
		{
			if (otherPlayerTitle != value)
			{
				otherPlayerTitle= value;
				PlayerPrefs.SetInt(OTHER_PLAYER_TITLE, otherPlayerTitle?1:0);
				if (OnUpdateOtherPlayerTitle != null) OnUpdateOtherPlayerTitle();
			}
		}
	}

	/// <summary>
	/// 是否显示其他玩家的法宝
	/// </summary>
	public bool OtherPlayerMagic
	{
		get
		{
			return otherPlayerMagic;
		}
		set
		{
			if (otherPlayerMagic != value)
			{
				otherPlayerMagic= value;
				PlayerPrefs.SetInt(OTHER_PLAYER_MAGIC, otherPlayerMagic?1:0);
                if (OnUpdateOtherPlayerMagicWeapon != null)
                    OnUpdateOtherPlayerMagicWeapon();
			}
		}
	}

	/// <summary>
	/// 是否显示其他玩家的强化功能特效
	/// </summary>
	public bool OtherPlayerEffect
	{
		get
		{
			return otherPlayerEffect;
		}
		set
		{
			if (otherPlayerEffect != value)
			{
				otherPlayerEffect= value;
				PlayerPrefs.SetInt(OTHER_PLAYER_EFFECT, otherPlayerEffect?1:0);
			}
		}
	}

	/// <summary>
	/// 是否仅显示可攻击对象
	/// </summary>
	public bool OnlyHostile
	{
		get
		{
			return onlyHostile;
		}
		set
		{
			if (onlyHostile != value)
			{
				onlyHostile= value;
				PlayerPrefs.SetInt(ONLY_HOSTILE, onlyHostile?1:0);
			}
		}
	}

	/// <summary>
	/// 是否显示掉落物品名字
	/// </summary>
	public bool ItemName
	{
		get
		{
			return itemName;
		}
		set
		{
			if (itemName != value)
			{
				itemName= value;
				PlayerPrefs.SetInt(ITEM_NAME, itemName?1:0);
				if (OnUpdateItemName != null) OnUpdateItemName();
			}
		}
	}

	/// <summary>
	/// 是否显示怪物名字
	/// </summary>
	public bool MonsterName
	{
		get
		{
			return monsterName;
		}
		set
		{
			if (monsterName != value)
			{
				monsterName= value;
				PlayerPrefs.SetInt(MONSTER_NAME, monsterName?1:0);
				if (OnUpdateMonsterName != null) OnUpdateMonsterName();
			}
		}
	}
    
	/// <summary>
	/// 是使用低纹理地图
	/// </summary>
    public bool LowTexture
	{
		get
		{
            return lowTexture;
		}
		set
		{
            if (lowTexture != value)
			{
                lowTexture = value;
                PlayerPrefs.SetInt("LOWTEXTURE", lowTexture ? 1 : 0);
			}
		}
	}


	/// <summary>
	/// 是否显示自身特效
	/// </summary>
	public bool SelfEffect
	{
		get
		{
			return selfEffect;
		}
		set
		{
			if (selfEffect != value)
			{
				selfEffect= value;
				PlayerPrefs.SetInt(SELF_EFFECT, selfEffect?1:0);
			}
		}
	}
    protected bool isPowerSaving = false;
    public bool IsPowerSaving
    {
        get { return isPowerSaving; }
        set { isPowerSaving = value; }
    }
    /// <summary>
    /// 玩家操作事件，用于关闭省电模式
    /// </summary>
    public System.Action OnPowerSavingEvent;
    protected RendererQuality noRecordQuality = RendererQuality.NONE;
    public RendererQuality RendererQualityNoRecord
    {
        get
        {
            return noRecordQuality;
        }
        set
        {
            if (noRecordQuality != value)
            {
                noRecordQuality = value;
                SetQualitySettings(noRecordQuality);
                if (OnUpdateRenderQuality != null)
                {
                    OnUpdateRenderQuality(noRecordQuality);
                }
            }
        }
    }
    #endregion


    #region 挂机设置
    /// <summary>
    /// 是否开启自动吃药
    /// </summary>
    public bool IsSafeOpen
    {
        get
        {
            return isSafeOpen;
        }
        set
        {
            if (isSafeOpen != value)
            {
                isSafeOpen = value;
                PlayerPrefs.SetInt("ISSAFEOPEN", value ? 1 : 0);
            }
        }
    }

    /// <summary>
    /// 自动吃药的模式 从大到小为true
    /// </summary>
    public bool SafeModel
    {
        get
        { 
            return safeModel;
        }
        set
        {
            if (safeModel != value)
            {
                safeModel = value;
                PlayerPrefs.SetInt("SAFEMODEL", value ? 1 : 0);
            }
        }
    }


    /// <summary>
    /// 是否自动购买药品
    /// </summary>
    public bool IsAutoBuy
    {
        get
        {
            return isAutoBuy;
        }
        set
        {
            if (isAutoBuy != value)
            {
                isAutoBuy = value;
                PlayerPrefs.SetInt("ISAUTOBUY", value ? 1 : 0);
                if (value)
                {
                    GameCenter.shopMng.ShowedHPDrugBtn = false;
                } 
            }
        }
    }
    /// <summary>
    /// 自动吃蓝药的蓝量
    /// </summary>
    public int SafeMagicNum
    {
        get
        {
            return safeMagicNum;
        }
        set
        {
            if (safeMagicNum != value)
            {
                safeMagicNum = value;
                PlayerPrefs.SetInt("SAFEMAGICNUM", value);
            }
        }
    }
    /// <summary>
    /// 自动吃红药的血量
    /// </summary>
    public int SafeLifeNum
    {
        get
        {
            return safeLifeNum;
        }
        set
        {
            if (safeLifeNum != value)
            {
                safeLifeNum = value;
                PlayerPrefs.SetInt("SAFELIFENUM", value);
            }
        }
    }
    /// <summary>
    /// 自动使用回城的血量
    /// </summary>
    public int FlyLifeNum
    {
        get
        {
            return flyLifeNum;
        }
        set
        {
            if (flyLifeNum != value)
            {
                flyLifeNum = value;
                PlayerPrefs.SetInt("FLYLIFENUM", value);
            }
        }
    }
    /// <summary>
    /// 自动使用随机的血量
    /// </summary>
    public int RandomFlyLifeNum
    {
        get
        {
            return randomFlyLifeNum;
        }
        set
        {
            if (randomFlyLifeNum != value)
            {
                randomFlyLifeNum = value;
                PlayerPrefs.SetInt("RANDOMFLYLIFENUM", value);
            }
        }
    }

    /// <summary>
    /// 是否开启自动回城
    /// </summary>
    public bool IsFlyOpen
    {
        get
        {
            return isFlyOpen;
        }
        set
        {
            if (isFlyOpen != value)
            {
                isFlyOpen = value;
                PlayerPrefs.SetInt("ISFLYOPEN", value ? 1 : 0);
            }
        }
    }

    /// <summary>
    /// 是否开启自动随机传送
    /// </summary>
    public bool IsRadomFlyOpen
    {
        get
        {
            return isRandomFlyOpen;
        }
        set
        {
            if (isRandomFlyOpen != value)
            {
                isRandomFlyOpen = value;
                PlayerPrefs.SetInt("ISRANDOMFLYOPEN", value ? 1 : 0);
            }
        }
    }

    /// <summary>
    /// 是否开启自动复活
    /// </summary>
    public bool IsAutoResurrection
    {
        get
        {
            return isAutoResurrection;
        }
        set
        {
            if (isAutoResurrection != value)
            {
                isAutoResurrection = value;
                PlayerPrefs.SetInt("ISAUTORESURRECTION", value ? 1 : 0);
            }
        }
    }

    /// <summary>
    /// 是否开启自动避开BOSS
    /// </summary>
    public bool IsHideBoss
    {
        get
        {
            return isHideBoss;
        }
        set
        {
            if (isHideBoss != value)
            {
                isHideBoss = value;
                PlayerPrefs.SetInt("ISHIDEBOSS", value ? 1 : 0);
            }
        }
    }

    /// <summary>
    /// 拾取品质
    /// </summary>
    public int PickModel
    {
        get
        {
            return pickModel;
        }
        set
        {
            if (pickModel != value)
            {
                pickModel = value;
                PlayerPrefs.SetInt("PICKMODEL", value);
            }
        }
    }



    /// <summary>
    /// 是否自动播放世界频道语音
    /// </summary>
    public bool AutoPlayWorldVoice
    {
        get
        {
            return autoPlayWorldVoice;
        }
        set
        {
            if (autoPlayWorldVoice != value)
            {
                autoPlayWorldVoice = value;
                PlayerPrefs.SetInt(AUTO_PLAY_WORLD_VOICE, value ? 1 : 0); 
            }
        }
    }

    /// <summary>
    /// 是否自动播放仙盟频道语音
    /// </summary>
    public bool AutoPlayAllyVoice
    {
        get
        {
            return autoPlayAllyVoice;
        }
        set
        {
            if (autoPlayAllyVoice != value)
            {
                autoPlayAllyVoice = value;
                PlayerPrefs.SetInt(AUTO_PLAY_ALLY_VOICE, value ? 1 : 0); 
            }
        }
    }

    /// <summary>
    /// 是否自动播放队伍频道语音
    /// </summary>
    public bool AutoPlayTeamVoice
    {
        get
        {
            return autoPlayTeamVoice;
        }
        set
        {
            if (autoPlayTeamVoice != value)
            {
                autoPlayTeamVoice = value;
                PlayerPrefs.SetInt(AUTO_PLAY_TEAM_VOICE, value ? 1 : 0); 
            }
        }
    }
    /// <summary>
    /// 是否自动播放私聊频道语音
    /// </summary>
    public bool AutoPlayChatVoice
    {
        get
        {
            return autoPlayChatVoice;
        }
        set
        {
            if (autoPlayChatVoice != value)
            {
                autoPlayChatVoice = value;
                PlayerPrefs.SetInt(AUTO_PLAY_CHAT_VOICE, value ? 1 : 0); 
            }
        }
    }


    #endregion




    #endregion

    /// <summary>
	/// 设置为流畅模式
	/// </summary>
	public void OnFluencyModel()
	{
		FluencyMode = true;
		OtherPlayerWing = false;
		OtherPlayerSkill = false;
		OtherPlayerPet = false;
		OtherPlayerTitle = false;
		OtherPlayerMagic = false;
		OtherPlayerEffect = false;
        SelfEffect = false;
		MaxPlayer = 0;
        IntelligentMode = false;
		//Debug.Log ("通过智能模式自动调整为流畅模式");
		
	}

    #region 画面配置
    protected RendererQuality rendererQuality = RendererQuality.NONE;

    public RendererQuality CurRendererQuality
    {
        get
        {
            return rendererQuality;
        }
        set
        {
            if (rendererQuality != value)
            {
                rendererQuality = value;
                SetQualitySettings(rendererQuality);
                PlayerPrefs.SetInt(RENDERER_QUALITY, (int)rendererQuality);
                if (OnUpdateRenderQuality != null)
                {
                    OnUpdateRenderQuality(rendererQuality);
                }
            }
        }
    }

    public System.Action<RendererQuality> OnUpdateRenderQuality;

    public enum RendererQuality
    {
        NONE = 0,
        LOW = 1,
        MID = 2,
        HIGHT = 3,
    }


    protected int originWidth = -1;
    protected int originHeight = -1;

    public void SetQualitySettings(RendererQuality _quality)
    {
        if (originWidth < 0 || originHeight < 0)
        {
            originWidth = Screen.width;
            originHeight = Screen.height;
        }
        // NGUIDebug.Log("_quality = " + _quality + " , Screen.width = " + Screen.width + " , Screen.height = " + Screen.height);
        rendererQuality = _quality;
        float tarWidth = 720;
        float tarHeight = 1280;
        switch (_quality)
        {
            case RendererQuality.HIGHT:
                Screen.SetResolution(originWidth, originHeight, true);
                tarWidth = Screen.width;
                tarHeight = Screen.height;
                EffectLimit = 100;
                QualitySettings.antiAliasing = 2;
                break;
            case RendererQuality.MID:
                Screen.SetResolution(originWidth, originHeight, true);
                tarWidth = Screen.width;
                tarHeight = Screen.height;
                EffectLimit = 50;
                QualitySettings.antiAliasing = 0;
                break;
            case RendererQuality.NONE:
            case RendererQuality.LOW:
                if (Screen.height > 720 || Screen.width > 1280)
                {
                    Screen.SetResolution(1280, 720, true);
                }
                tarWidth = 1280;
                tarHeight = 720;
                EffectLimit = 20;
                //MaxPlayer = 5;
                QualitySettings.antiAliasing = 0;
                break;
        }
        for (int i = 0; i < UIRoot.list.Count; i++)
        {
            UIRoot.list[i].scalingStyle = UIRoot.Scaling.Constrained;

            if ((tarWidth / (float)tarHeight) > (1136 / (float)640))
            {
                UIRoot.list[i].manualHeight = 640;
            }
            else
            {
                UIRoot.list[i].manualHeight = 1136 * Screen.height / Screen.width;
            }
            // NGUIDebug.Log("tarWidth = " + tarWidth + " ,tarHeight = " + tarHeight +  " , manualHeight  = " + UIRoot.list[i].manualHeight);

            //if (Mathf.Abs((Screen.currentResolution.width / (float)Screen.currentResolution.height) - (UIRoot.list[i].manualWidth / UIRoot.list[i].manualHeight)) > 0.1f)
            //{
            //    UIRoot.list[i].manualHeight = (int)tarHeight;
            //    UIAnchor[] anchors = UIRoot.list[i].GetComponentsInChildren<UIAnchor>(true);
            //    for (int j = 0; j < anchors.Length; j++)
            //    {
            //        anchors[j].runOnlyOnce = false;
            //    }
            //}
        }
    }
    #endregion


}
