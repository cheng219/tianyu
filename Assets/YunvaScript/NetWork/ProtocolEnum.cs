using UnityEngine;
using System;
using System.Collections;

namespace YunvaIM
{
    public enum ProtocolEnum:uint
    {
        #region IM_LOGIN
        IM_LOGIN_REQ = 0x11000,                 //云娃登录请求
        IM_LOGIN_RESP = 0x11001,                //云娃登录返回
        IM_THIRD_LOGIN_REQ=0x11002,             //cp账号登录请求
        IM_THIRD_LOGIN_RESP = 0x11003,          //cp账号登录返回
        IM_LOGOUT_REQ = 0x11004,                //注销
        IM_DEVICE_SETINFO=0x11012,              //设置设备信息
        IM_RECONNECTION_NOTIFY = 0x11013,       //重连成功通知
        IM_GET_THIRDBINDINFO_REQ = 0x11014,     //获取第三方账号信息
        IM_GET_THIRDBINDINFO_RESP = 0x11015,    //获取第三方信息返回
        IM_NET_STATE_NOTIFY = 0x11016 ,         //网络状态通知
        #endregion

        #region IM_TOOS
        IM_RECORD_STRART_REQ    =   0x19000,        //开始录音(最长60秒)
        IM_RECORD_STOP_REQ  =   0x19001,            //停止录音请求  回调返回录音文件路径名
        IM_RECORD_STOP_RESP =   0x19002,            //停止录音返回  回调返回录音文件路径名
        IM_RECORD_STARTPLAY_REQ =   0x19003,        //播放录音请求
        IM_RECORD_FINISHPLAY_RESP = 0x19004,        //播放语音完成
        IM_RECORD_STOPPLAY_REQ  =   0x19005,        //停止播放语音
        IM_SPEECH_START_REQ     =0x19006,           //开始语音识别
        IM_SPEECH_STOP_REQ  =0x19007,               //停止语音识别
        IM_SPEECH_STOP_RESP     =   0x19009,        //语音识别完成返回
        IM_SPEECH_SETLANGUAGE_REQ   =   0x19008,    //设置语音识别语言
        IM_UPLOAD_FILE_REQ      =   0x19010 ,       //上传文件
        IM_UPLOAD_FILE_RESP     =    0x19011 ,      //上传文件回应
        IM_DOWNLOAD_FILE_REQ=   0x19012,            //下载文件请求
        IM_DOWNLOAD_FILE_RESP=0x19013,              //下载文件回应
        IM_RECORD_SETINFO_REQ = 0x19014,            //设置录音信息
        IM_RECORD_VOLUME_NOTIFY = 0x19015,          //录音声音大小通知
		IM_RECORD_PLAY_PERCENT_NOTIFY = 0x19016,	//播放URL下载进度
		IM_TOOL_HAS_CACHE_FILE = 0x19017,			//判断URL文件是否存在
        #endregion

		#region IM_LBS
		IM_UPLOAD_LOCATION_REQ = 0x18000,            //上传地理位置请求
		IM_UPLOAD_LOCATION_RESP=0x18001,            //上传地理位置返回
		IM_GET_LOCATION_REQ = 0x18002,              //获取位置信息请求
		IM_GET_LOCATION_RESP = 0x18003,               //获取位置信息返回
		IM_SEARCH_AROUND_REQ = 0x18004,              //搜索（附近）用户
		IM_SEARCH_AROUND_RESP=0x18005,              //搜索（附近）用户返回
		IM_SHARE_LOCATION_REQ=0x18006,              //隐藏地理位置请求
		IM_SHARE_LOCATION_RESP=0x18007,             //隐藏地理位置回应
		IM_LBS_GET_SUPPORT_LANG_REQ=0x18008,//获取支持的（包括搜索、返回信息等）本地化语言列表请求
		IM_LBS_GET_SUPPORT_LANG_RESP=0x18009,//获取支持的（包括搜索、返回信息等）本地化语言列表回应
		IM_LBS_SET_LOCAL_LANG_REQ=0x18010,//设置LBS本地化语言
		IM_LBS_SET_LOCAL_LANG_RESP=0x18011,//设置LBS本地化语言回应
		IM_LBS_SET_LOCATING_TYPE_REQ=0x18012,//设置定位方式
		IM_LBS_SET_LOCATING_TYPE_RESP=0x18013,////设置定位方式回应
		#endregion

        #region IM_FRIEND
        IM_FRIEND_ADD_REQ = 0x12000,            //请求添加好友
        IM_FRIEND_ADD_RESP = 0x12001,           //添加好友回应，对方接受/拒绝
        IM_FRIEND_ADD_NOTIFY = 0x12002,         //好友请求通知
        IM_FRIEND_ADD_ACCEPT = 0x12003,         //添加好友确定
        IM_FRIEND_ACCEPT_RESP = 0x12004,        //添加好友回应
        IM_FRIEND_DEL_REQ = 0x12005,            //删除好友请求
        IM_FRIEND_DEL_RESP = 0x12006,           //删除好友返回
        IM_FRIEND_DEL_NOTIFY = 0x12007,         //删除好友通知
        IM_FRIEND_RECOMMAND_REQ = 0x12008,      //推荐好友
        IM_FRIEND_RECOMMAND_RESP = 0x12009,     //推荐好友回应
        IM_FRIEND_OPER_REQ = 0x12010,           //好友操作请求(黑名单)
        IM_FRIEND_OPER_RESP = 0x12011,          //好友操作回应(黑名单)
        IM_FRIEND_LIST_NOTIFY = 0x12012,        //好友列表推送
        IM_FRIEND_BLACKLIST_NOTIFY = 0x12013,   //黑名单列表推送
        IM_FRIEND_NEARLIST_NOTIFY=0x12014,      //最近联系人推送
        IM_FRIEND_STATUS_NOTIFY=0x12015,        //好友状态推送
        IM_FRIEND_INFOSET_REQ=0x12016,          //设置好友信息
        IM_FRIEND_INFOSET_RESP=0x12017,         //设置好友信息返回
        IM_FRIEND_SEARCH_REQ = 0x12018,         //搜索好友请求
        IM_FRIEND_SEARCH_RESP = 0x12019,        //搜索好友回应
        IM_USER_GETINFO_REQ = 0x12020,          //获取个人信息
        IM_USER_GETINFO_RESP = 0x12021,         //返回个人信息数据
        IM_USER_SETINFO_REQ = 0x12022,          //修改个人信息
        IM_USER_SETINFO_RESP = 0x12023,         //修改个人信息响应
        IM_USER_SETINFO_NOTIFY = 0x12024,       //好友个人信息修改通知
        IM_REMOVE_CONTACTES_REQ = 0x12026,        //删除最近联系人
        IM_REMOVE_CONTACTES_RESP=0x12027,       //删除最近联系人响应
        #endregion

        #region IM_FRIEND_CHAT
        IM_CHAT_FRIEND_TEXT_REQ = 0x14000,//好友聊天-文本
		IM_CHAT_FRIEND_IMAGE_REQ = 0x14001,//好友聊天-图像
		IM_CHAT_FRIEND_AUDIO_REQ = 0x14002,//好友聊天 - 语音
		IM_CHAT_FRIEND_RESP = 0x14004,//发送好友消息回应
		IM_CHAT_FRIEND_NOTIFY = 0x14003,////好友聊天通知
		#endregion

        #region IM_CLOUDMSG
        IM_CLOUDMSG_NOTIFY = 0x15002,           //云消息通知
        IM_CLOUDMSG_LIMIT_REQ = 0x15003,        //请求云消息
        IM_CLOUDMSG_LIMIT_RESP = 0x15004,       //请求云消息响应
        IM_CLOUDMSG_LIMIT_NOTIFY = 0x15005,     //云消息回应通知
        IM_MSG_PUSH = 0x15006,                  //PUSH消息
        IM_CLOUDMSG_READ_STATUS = 0x15007,      //云消息确认
        IM_CLOUDMSG_IGNORE_REQ = 0x15008,       //离线消息忽略
        #endregion

        #region IM_CHANNEL
        IM_CHANNEL_LOGIN_REQ = 0x16007,         //登录 注:登录账号传入了通配符，会直接登录， 不需要再调此登录
        IM_CHANNEL_LOGIN_RESP=0x16008,
        IM_CHANNEL_LOGOUT_REQ = 0x16009,        //退出频道
		IM_CHANNEL_MODIFY_REQ=0x16011,          //修改通配符
		IM_CHANNEL_MODIFY_RESP=0x16012,         //修改通配符回应
        IM_CHANNEL_GETINFO_REQ = 0x16000,       //获取频道信息请求
        IM_CHANNEL_GETINFO_RESP = 0x16001,      //获取频道信息返回
        IM_CHANNEL_TEXTMSG_REQ = 0x16002,       //发送频道文字消息请求
        IM_CHANNEL_VOICEMSG_REQ = 0x16003,      //发送频道语音消息
        IM_CHANNEL_SENDMSG_RESP = 0x16010,//发送消息回应
        IM_CHANNEL_MESSAGE_NOTIFY = 0x16004,//频道收到消息通知
        IM_CHANNEL_HISTORY_MSG_REQ = 0x16005,//频道获取历史消息请求
        IM_CHANNEL_HISTORY_MSG_RESP = 0x16006,//频道获取历史消息返回
		IM_CHANNEL_PUSH_MSG_NOTIFY = 0x16013,//频道PUSH消息通知
        IM_CHANNEL_GET_PARAM_REQ = 0x16014,//获取当前频道相关参数
        IM_CHANNEL_GET_PARAM_RESP=0x16015,//获取当前频道相关参数返回
        #endregion

        #region IM_GROUPS
        IM_GROUP_USERLIST_NOTIFY = 0x13000,//群用户列表
        IM_GROUP_USERMDY_NOTIFY = 0x13001,//修改资料通知
        IM_GROUP_CREATE_REQ = 0x13002,//创建群
        IM_GROUP_CREATE_RESP = 0x13003,//创建群回应
        IM_GROUP_SEARCH_REQ = 0x13004,//搜索群
        IM_GROUP_SEARCH_RESP  =    0x13005,//搜索群回应
        IM_GROUP_JOIN_REQ = 0x13006,//加入群
        IM_GROUP_JOIN_NOTIFY = 0x13007,//加入群通知
        IM_GROUP_JOIN_ACCEPT = 0x13008,//同意拒绝加群
        IM_GROUP_JOIN_RESP = 0x13009,//申请加群返回结果通知
        IM_GROUP_EXIT_REQ = 0x13010,//退群
        IM_GROUP_EXIT_RESP = 0x13011,//退群响应
        IM_GROUP_EXIT_NOTIFY = 0x13012,//退群通知
        IM_GROUP_MODIFY_REQ = 0x13013,//修改群属性
        IM_GROUP_MODIFY_RESP = 0x13014,//修改群属性响应
        IM_GROUP_SHIFTOWNER_REQ = 0x13015,//转移群主请求
        IM_GROUP_SHIFTOWNER_NOTIFY = 0x13016,//转移群主通知
        IM_GROUP_SHIFTOWNER_RESP = 0x13017,//转移群主响应
        IM_GROUP_KICK_REQ = 0x13018,//踢除群成员
        IM_KGROUP_KICK_NOTIFY = 0x13019,//踢除群成员通知
        IM_GROUP_KICK_RESP = 0x13020,//踢除群成员返回
        IM_GROUP_INVITE_REQ = 0x13021,//邀请好友入群
        IM_GROUP_INVITE_NOTIFY = 0x13022,//邀请通知
        IM_GROUP_INVITE_ACCEPT = 0x13023,//被邀请者同意或拒绝群邀请
        IM_GROUP_INVITE_RESP = 0x13024,//邀请好友入群响应
        IM_GROUP_SETROLE_REQ = 0x13025,//设置群成员角色请求
        IM_GROUP_SETROLE_RESP = 0x13026,//设置群成员角色返回
        IM_GROUP_SETROLE_NOTIFY = 0x13027,//设置群成员角色通知
        IM_GROUP_DISSOLVE_REQ = 0x13028,//解散群请求
        IM_GROUP_DISSOLVE_RESP = 0x13029,//解散群响应
        IM_GROUP_SETOTHER_REQ = 0x13030,//管理员修改他人名片
        IM_GROUP_SETOTHER_NOTIFY = 0x13031,//修改他人名片通知
        IM_GROUP_SETOTHER_RESP = 0x13032,//修改他人名片返回
        IM_GROUP_PROPERTY_NOTIFY = 0x13033,//群属性通知(群列表)
        IM_GROUP_MEMBER_ONLINE = 0x13034,//群成员上线
        IM_GROUP_USERJOIN_NOTIFY=  0x13035,//新成员加入群


        #endregion

        #region other
        CreateEnterPointAtFirst,
        RemoveMessageList,//当拉黑或删除好友列表的时候，触发这个事件
        #endregion

    };
  public  enum YvNet : int
    {
        YvNetDisconnect=0,
        YvNetConnect=1,
    };
  public   enum Yvimspeech_language
    {
        im_speech_zn = 1, //中文
        im_speech_ct = 2, //粤语
        im_speech_en = 3, //英语
    };
	public   enum yvimspeech_outlanguage
	{
		im_speechout_simplified       = 0,  //简体中文
		im_speechout_traditional      = 1,  //繁体中文
	};
    public enum e_addfriend_affirm
    {
        af_refuse = 0,//拒绝
        af_agree = 1, //同意加好友(单项)
        af_agree_add = 2, //同意加好友并加对方为好友(双向)
    };
    public enum e_delfriend
    {
        df_exit_in_list = 0, //只从我的好友列表中删除
        df_remove_from_list = 1, //双向删除
    };
    public enum e_oper_friend_act
    {
        oper_add_blacklist = 3, //加入黑名单
        oper_del_blacklist = 4, //删除黑名单
    };
	public enum yvspeech
	{
		speech_file = 0,              //文件识别
		speech_file_and_url = 1,      //文件识别返回url
		speech_url = 2,               //url识别
		speech_live = 3,              //实时语音识别(未完成)
	}
	public enum YvlogLevel
	{
		LOG_LEVEL_OFF = 0,  //0：关闭日志
		LOG_LEVEL_DEBUG = 1, //1：Debug默认该级别
		LOG_LEVEL_INFO=2,   //2:info
		LOG_LEVEL_ERROR=3   //3:error
	}
}
