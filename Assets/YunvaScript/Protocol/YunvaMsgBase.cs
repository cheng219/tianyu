using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using YunvaIM;
public class YunvaMsgBase
{

    public static YunvaMsgBase GetMsg(uint CmdId, object Parser)
    {
        switch (CmdId)
        {
            case (uint)ProtocolEnum.IM_THIRD_LOGIN_RESP:
                return new ImThirdLoginResp(Parser);
            case (uint)ProtocolEnum.IM_FRIEND_ADD_RESP:
                return new ImFriendAddResp(Parser);
            case (uint)ProtocolEnum.IM_FRIEND_ADD_NOTIFY:
                return new ImFriendAddNotify(Parser);
            case (uint)ProtocolEnum.IM_FRIEND_ACCEPT_RESP:
                return new ImFriendAcceptResp(Parser);
            case (uint)ProtocolEnum.IM_FRIEND_DEL_RESP:
                return new ImFriendDelResp(Parser);
            case (uint)ProtocolEnum.IM_FRIEND_RECOMMAND_RESP:
                return new ImFriendSearchResp(Parser, ProtocolEnum.IM_FRIEND_RECOMMAND_RESP);
            case (uint)ProtocolEnum.IM_FRIEND_SEARCH_RESP:
                return new ImFriendSearchResp(Parser, ProtocolEnum.IM_FRIEND_SEARCH_RESP);
            case (uint)ProtocolEnum.IM_FRIEND_OPER_RESP:
                return new ImFriendOperResp(Parser);
            case (uint)ProtocolEnum.IM_USER_GETINFO_RESP:
                return new ImUserGetInfoResp(Parser);
            case (uint)ProtocolEnum.IM_CLOUDMSG_NOTIFY:
                return new ImCloudMsgNotify(Parser);
            case (uint)ProtocolEnum.IM_FRIEND_DEL_NOTIFY://删除好友通知
                return new ImFriendDelNotify(Parser);
            case (uint)ProtocolEnum.IM_FRIEND_LIST_NOTIFY://好友列表
                return new ImFriendListNotify(Parser);
            case (uint)ProtocolEnum.IM_FRIEND_BLACKLIST_NOTIFY://黑名单列表
                return new ImFriendBlackListNotify(Parser);
            case (uint)ProtocolEnum.IM_CHAT_FRIEND_NOTIFY:
                return new ImChatFriendNotify(Parser);
            case (uint)ProtocolEnum.IM_CLOUDMSG_LIMIT_RESP:
                return new ImCloudMsgLimitResp(Parser);
            case (uint)ProtocolEnum.IM_CLOUDMSG_LIMIT_NOTIFY:
                return new ImCloudmsgLimitNotify(Parser);
            case (uint)ProtocolEnum.IM_CHAT_FRIEND_RESP:
                return new ImChatFriendResp(Parser);
            case (uint)ProtocolEnum.IM_CHANNEL_MESSAGE_NOTIFY:
                return new ImChannelMessageNotify(Parser);
            case (uint)ProtocolEnum.IM_CHANNEL_GETINFO_RESP:
                return new ImChannelGetInfoResp(Parser);
            case (uint)ProtocolEnum.IM_CHANNEL_SENDMSG_RESP:
                return new ImChannelSendMsgResp(Parser);
            case (uint)ProtocolEnum.IM_CHANNEL_HISTORY_MSG_RESP:
                return new ImChannelHistoryMsgResp(Parser);
            case (uint)ProtocolEnum.IM_RECORD_STOP_RESP:
                return new ImRecordStopResp(Parser);
            case (uint)ProtocolEnum.IM_SPEECH_STOP_RESP:
                return new ImSpeechStopResp(Parser);
            case (uint)ProtocolEnum.IM_RECORD_FINISHPLAY_RESP:
                return new ImRecordFinishPlayResp(Parser);
            case (uint)ProtocolEnum.IM_NET_STATE_NOTIFY:
                return new ImNetStateNotify(Parser);
            case (uint)ProtocolEnum.IM_RECORD_VOLUME_NOTIFY:
                return new ImRecordVolumeNotify(Parser);
            case (uint) ProtocolEnum.IM_RECONNECTION_NOTIFY:
                return new ImReconnectionNotify(Parser);
            case  (uint)ProtocolEnum.IM_UPLOAD_FILE_RESP:
                return new ImUploadFileResp(Parser);
            case (uint)ProtocolEnum.IM_DOWNLOAD_FILE_RESP:
                return new ImDownLoadFileResp(Parser);
            case (uint)ProtocolEnum.IM_FRIEND_NEARLIST_NOTIFY:
                return new ImFriendNearListNotify(Parser);
			case (uint)ProtocolEnum.IM_RECORD_PLAY_PERCENT_NOTIFY:
				return new ImPlayPercentNotify(Parser);
            case (uint)ProtocolEnum.IM_GET_THIRDBINDINFO_RESP:
                return new ImGetThirdBindInfoResp(Parser);
            case (uint)ProtocolEnum.IM_REMOVE_CONTACTES_RESP:
                return new ImRemoveContactesResp(Parser);
			case (uint)ProtocolEnum.IM_CHANNEL_MODIFY_RESP:
                return new ImChannelMotifyResp(Parser);
			case (uint)ProtocolEnum.IM_CHANNEL_PUSH_MSG_NOTIFY:
				    return new ImChannelPushMsgNotify(Parser);
			case (uint) ProtocolEnum.IM_UPLOAD_LOCATION_RESP:
				return new ImUploadLocationResp(Parser);
			case (uint)ProtocolEnum.IM_GET_LOCATION_RESP:
				return new ImGetLocationResp(Parser);
			case (uint)ProtocolEnum.IM_SEARCH_AROUND_RESP:
				return new ImSearchAroundResp(Parser);
		    case (uint)ProtocolEnum.IM_SHARE_LOCATION_RESP:
			    return new ImShareLocationResp(Parser);
			case (uint)ProtocolEnum.IM_LBS_SET_LOCATING_TYPE_RESP:
				return new ImLBSSetLocatingTypeResp(Parser);
			case (uint)ProtocolEnum.IM_LBS_GET_SUPPORT_LANG_RESP:
				return new ImLBSGetSpportLangResp(Parser);
			case (uint)ProtocolEnum.IM_LBS_SET_LOCAL_LANG_RESP:
			    return new ImLBSSetLocalLangResp(Parser);
            case (uint)ProtocolEnum.IM_CHANNEL_GET_PARAM_RESP:
                return new ImChannelGetParamResp(Parser);
            case (uint)ProtocolEnum.IM_CHANNEL_LOGIN_RESP:
                return new ImChannelLoginResp(Parser);
            default:
                return null;
        }
    }
}


