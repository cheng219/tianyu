using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////云娃登录返回 0x11001
public class IMLoginResp
{
    public int result;  //返回结果 不为0即为失败
    public string msg;     //错误描述
    public string nickName;
    public int userId;
    public string iconUrl;
}
public class YunVaUserInfo
{
    public int userId;//云娃ID
    public string nickName;//昵称
    public string iconUrl;//用户图像地址
    public string thirdUserId;//第三方用户ID
    public string thirdUseName;//第三方用户名
}




//好友聊天通知
public class FriendChatNotify
{
    public int userid;//好友ID
    public string name;//好友名称
    public string signature;//好友签名
    public string headurl;//头像地址
    public int sendtime;//发送时间
    public int type;//类型 0:图像文件,1:音频文件,2:文本消息
    public string data;//若为文本类型，则是消息内容，若为音频，则是文件url，若为图像，则是大图像地址
    public string imageurl;//若为图片，则是小图像地址
    public int audiotime;//若为音频文件, 则是文件播放时长(秒)
    public string attach;//若为音频文件，则是附加文本(没有附带文本时为空)
    public string ext1;//扩展字段
}



//频道收到消息通知
public class ChannelChatNotify
{
    public int user_id; //用户ID
    public string message_body;//消息
    public string nickname;//昵称
    public string ext1;//扩展1
    public string ext2;//扩展2
    public int channel;//游戏通道
    public string wildcard;//游戏通道字符串
    public int message_type;//type= 1 语音  type= 2 文本
    public int voiceDuration;//type= 1 语音时 该字段为语音时长
    public string attach;//语音消息的附带文本(可选)
}


public enum CloudMessageType
{
    Invalid,
    SYSTEM,
    PUSH,
    P2P,
    GROUP,
    Max
}

//请求云消息
public class CloudMsgLimitReq
{
    public string source;//来源(SYSTEM 系统消息 PUSH 推送消息 userId 好友消息)
    public int id;      //若是好友消息, 则为好友ID
    public int index;//获取到位置（endid）
    public int limit; //获取条数 
}
public enum e_chat_msgtype
{
    chat_msgtype_image = 0,//图像文件
    chat_msgtype_audio = 1,//音频文件
    chat_msgtype_text = 2,//文本消息
}
public class P2PChatMsg
{
    public int userID;//用户ID
    public string name; //用户名称
    public string signature;//用户签名
    public string headUrl;//图像地址
    public int sendTime;//消息发送时间
    public int type;//消息类型 e_chat_msgtype
    public string data;//若为文本类型，则是消息内容，其他则是文件地址
    public string imageUrl;//若为图片，则是小图像地址
    public int audioTime;//若为音频文件, 则为文件播放时长(秒)
    public string attach;//若为音频文件，则是附加文本(没有附带文本时为空)
    public string ext1; //扩展字段
	public int index;	//消息索引
    public int cloudMsgID;
    public string cloudResource;
    public Texture2D texture;

}

//最近联系人
public class NearChatInfo
{
    public string nickName;
    public int userId;
    public string iconUrl;
    public int online;
    public string userLevel;
    public string vipLevel;
    public string ext;//扩展字段
    public int shieldMsg;//是否屏蔽聊天消息
    public int sex;
    public string group;
    public string remark;
    public int times;
}
public class RecentConact
{
    public int endId;
    public int unread;
    public P2PChatMsg lastMsg;
    public NearChatInfo userInfo;
    public RecentConact()
    {
        if (lastMsg == null)
            lastMsg = new P2PChatMsg();
        if (userInfo == null)
            userInfo = new NearChatInfo();
    }
}

public class xUserInfo
{
    public string nickName; //用户昵称
    public int userId;  //用户ID
    public string iconUrl;  //用户图像地址
    public int onLine; //是否在线
    public string userLevel;//用户等级
    public string vipLevel;//vip等级
    public string ext;//扩展字段
    public int shieldmsg;//是否屏蔽聊天消息
    public int sex; //性别
    public string group;//所在组名称
    public string remark;//备注
    public int times;//最近聊天时间,单位（秒） 
    public Texture2D texture;
}

public class xSearchInfo
{
    public int yunvaId;
    public string userId;
    public string nickName;
    public string iconUrl;
    public string level;
    public string vip;
    public string ext;
}

public class HistoryMsgInfo
{
    public uint index;
    public string ctime;
    public uint userId;
    public string messageBody;
    public string nickName;
    public string ext1;
    public string ext2;
    public int channel;
    public string wildCard;
    public uint messageType;
    public uint voiceDuration;
    public string attach;
}

