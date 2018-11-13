using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using YunvaIM;
namespace YunvaIM
{
    public class YunVaImSDK : MonoSingleton<YunVaImSDK>
    {
        private Dictionary<string,  System.Action<ImChatFriendResp>> p2pChatResponDic = new Dictionary<string,  System.Action<ImChatFriendResp>>();
        private uint appid = 0;
        public YunVaUserInfo userInfo = new YunVaUserInfo();
        #region 事件监听
        public override void Init() 
        {
            DontDestroyOnLoad(this);
            #region IM_LOGIN
            EventListenerManager.AddListener(ProtocolEnum.IM_THIRD_LOGIN_RESP, CPLoginResponse);
            EventListenerManager.AddListener(ProtocolEnum.IM_GET_THIRDBINDINFO_RESP, GetThirdBindInfo);
           
            #endregion
            #region IM_TOOL
            EventListenerManager.AddListener(ProtocolEnum.IM_RECORD_STOP_RESP, RecordStopRespInfo);
            EventListenerManager.AddListener(ProtocolEnum.IM_RECORD_FINISHPLAY_RESP, RecordFinishPlayRespInfo);
            EventListenerManager.AddListener(ProtocolEnum.IM_SPEECH_STOP_RESP, RecordRecognizeRespInfo);
            EventListenerManager.AddListener(ProtocolEnum.IM_UPLOAD_FILE_RESP, UploadFileRespInfo);
            EventListenerManager.AddListener(ProtocolEnum.IM_DOWNLOAD_FILE_RESP, DownLoadFileRespInfo);
			EventListenerManager.AddListener(ProtocolEnum.IM_UPLOAD_LOCATION_RESP, UploadLocationResp);
			EventListenerManager.AddListener(ProtocolEnum.IM_GET_LOCATION_RESP, GetLocationResp);
			EventListenerManager.AddListener(ProtocolEnum.IM_SEARCH_AROUND_RESP, SearchAroundResp);
			EventListenerManager.AddListener(ProtocolEnum.IM_SHARE_LOCATION_RESP, ShareLocationResp);
			EventListenerManager.AddListener(ProtocolEnum.IM_LBS_SET_LOCATING_TYPE_RESP, LBSSetLocatingTypeResp);
			EventListenerManager.AddListener(ProtocolEnum.IM_LBS_GET_SUPPORT_LANG_RESP, LBSGetSpportLangResp);
			EventListenerManager.AddListener(ProtocolEnum.IM_LBS_SET_LOCAL_LANG_RESP, LBSSetLocalLangResp);
            #endregion
            #region IM_Friend
            EventListenerManager.AddListener(ProtocolEnum.IM_FRIEND_ADD_NOTIFY, (obj) => { });//好友请求通知
            EventListenerManager.AddListener(ProtocolEnum.IM_FRIEND_DEL_NOTIFY, (obj) => { });//删除好友通知
            EventListenerManager.AddListener(ProtocolEnum.IM_FRIEND_LIST_NOTIFY, FriendListNotify);//好友列表推送
            EventListenerManager.AddListener(ProtocolEnum.IM_FRIEND_BLACKLIST_NOTIFY, BlackListNotify);//黑名单列表推送
            EventListenerManager.AddListener(ProtocolEnum.IM_FRIEND_ADD_RESP, (obj) => { });//添加好友回应，对方接受/拒绝
            EventListenerManager.AddListener(ProtocolEnum.IM_FRIEND_ACCEPT_RESP, AcceptOrRefuseResponse);
            EventListenerManager.AddListener(ProtocolEnum.IM_FRIEND_DEL_RESP, DeleteFriendResponse);
            EventListenerManager.AddListener(ProtocolEnum.IM_FRIEND_RECOMMAND_RESP, RecommandFriendRespone);
            EventListenerManager.AddListener(ProtocolEnum.IM_FRIEND_OPER_RESP, BlackListOperationResponse);
            EventListenerManager.AddListener(ProtocolEnum.IM_FRIEND_INFOSET_RESP, SetFriendInfoResponse);
            EventListenerManager.AddListener(ProtocolEnum.IM_USER_GETINFO_RESP, GetUserInfoResponse);
            EventListenerManager.AddListener(ProtocolEnum.IM_FRIEND_SEARCH_RESP, SearchFriendResponse);
            EventListenerManager.AddListener(ProtocolEnum.IM_REMOVE_CONTACTES_RESP, RemoveContactesResp);
            #endregion
			#region IM_FRIEND_CHAT
			EventListenerManager.AddListener(ProtocolEnum.IM_CHAT_FRIEND_RESP, SendFriendMsgResponse);
			EventListenerManager.AddListener(ProtocolEnum.IM_CHAT_FRIEND_NOTIFY, (obj) => { });
			#endregion
			#region IM_CHANNEL_CHAT
			EventListenerManager.AddListener(ProtocolEnum.IM_CHANNEL_SENDMSG_RESP, SendChannelMsgResponse);
			EventListenerManager.AddListener(ProtocolEnum.IM_CHANNEL_MESSAGE_NOTIFY, (obj) => { });
			EventListenerManager.AddListener(ProtocolEnum.IM_CHANNEL_LOGIN_RESP, loginChannelResponse);
//			EventListenerManager.AddListener(ProtocolEnum.IM_CHANNEL_GETINFO_RESP, getChannelInfoResponse);
			EventListenerManager.AddListener(ProtocolEnum.IM_CHANNEL_HISTORY_MSG_RESP, getChannelHistoryMsgResponse);
			EventListenerManager.AddListener(ProtocolEnum.IM_CHANNEL_MODIFY_RESP, ChannelMotifyResponse);
            EventListenerManager.AddListener(ProtocolEnum.IM_CHANNEL_GET_PARAM_RESP, ChannelGetParamResponse);
			#endregion
			#region IM_CLOUND
			EventListenerManager.AddListener(ProtocolEnum.IM_CLOUDMSG_LIMIT_RESP, cloudMsgResponse);
			#endregion
        }
        #endregion
        #region 初始化SDK
        /// <summary>
        /// 初始化
        /// </summary>
		/// <returns>返回 0表示成功，-1表示失败 .</returns>
		/// <param name="context">回调上下文.</param>
        /// <param name="appid">Appid.</param>
		/// <param name="path">保存数据库文件，提供路径.</param>
		/// <param name="isTest">If set to <c>是否为测试环境，true为测试环境</c> is test.</param>
		public int YunVa_Init(uint context, uint appid, string path, bool isTest)
        {
			YunvaLogPrint.YvDebugLog ("YunVa_Init",string.Format("context:{0},appid:{1},path:{2},isTest:{3}", context,appid,path,isTest));
            this.appid = appid;
			return YunVaImInterface.instance.InitSDK (context, appid, path, isTest);
		}
        #endregion

        #region 登录
        //每次断网重连成功，服务器都会下发这个通知。
        public  System.Action  ActionReLoginSuccess;
       
        private  System.Action<ImThirdLoginResp>  ActionLoginResponse;
        
        /// <summary>
        /// 云娃登录（第三方登录方式）CP接入推荐用这种方式
        /// </summary>
        /// <param name="tt"></param>
        /// <param name="gameServerID">服务器ID</param>
        /// <param name="wildCard"></param>
        /// <param name="readStatus"></param>
        /// <param name="Response"></param>   
       
        public void YunVaOnLogin(string tt, string gameServerID, string[] wildCard, int readStatus,  System.Action<ImThirdLoginResp> Response)
        {
			YunvaLogPrint.YvDebugLog ("YunVaOnLogin", string.Format ("tt:{0},gameServerID:{1},readStatus:{2},1", tt, gameServerID, readStatus));
             ActionLoginResponse = Response;

            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_string(parser, 1, tt);
            YunVaImInterface.instance.YVparser_set_string(parser, 2, gameServerID);            
            for (int i = 0; i < wildCard.Length;i++ )
            {
				YunvaLogPrint.YvDebugLog("YunVaOnLogin",string.Format("wildCard-{0}:{1}",i,wildCard[i]));
                YunVaImInterface.instance.YVparser_set_string(parser, 3, wildCard[i]);
            }
            YunVaImInterface.instance.YVparser_set_uint8(parser, 4, readStatus);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_LOGIN, (uint)ProtocolEnum.IM_THIRD_LOGIN_REQ, parser);           
        }
        
        public void YunVaOnLogin(string tt, string gameServerID, string[] wildCard, int readStatus, System.Action<ImThirdLoginResp> Response,System.Action<ImChannelLoginResp> channelLoginResp)
        {
			YunvaLogPrint.YvDebugLog ("YunVaOnLogin", string.Format ("tt:{0},gameServerID:{1},readStatus:{2},2", tt, gameServerID, readStatus));
            ActionLoginResponse = Response;
            ActionLoginChannelResp = channelLoginResp;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_string(parser, 1, tt);
            YunVaImInterface.instance.YVparser_set_string(parser, 2, gameServerID);
            for (int i = 0; i < wildCard.Length; i++)
            {
				YunvaLogPrint.YvDebugLog("YunVaOnLogin",string.Format("wildCard-{0}:{1}",i,wildCard[i]));
                YunVaImInterface.instance.YVparser_set_string(parser, 3, wildCard[i]);
            }
            YunVaImInterface.instance.YVparser_set_uint8(parser, 4, readStatus);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_LOGIN, (uint)ProtocolEnum.IM_THIRD_LOGIN_REQ, parser);
        }

        private  System.Action<ImGetThirdBindInfoResp>  ActionThirdBindInfo;
        /// <summary>
        /// 获取第三方账号信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        public void YunVaGetThirdBindInfo(int appId,string userId, System.Action<ImGetThirdBindInfoResp> Response)
        {
			YunvaLogPrint.YvDebugLog ("YunVaGetThirdBindInfo", string.Format ("appId:{0},userId:{1}", appId, userId));
            ActionThirdBindInfo = Response;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_integer(parser, 1, appId);
			YunVaImInterface.instance.YVparser_set_string(parser, 2, userId);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_LOGIN, (uint)ProtocolEnum.IM_GET_THIRDBINDINFO_REQ, parser); 
        }
        /// <summary>
        /// 登出帐号
        /// </summary>
        public void YunVaLogOut()
        {
			YunvaLogPrint.YvDebugLog ("YunVaLogOut", "YunVaLogOut...");
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_LOGIN, (uint)ProtocolEnum.IM_LOGOUT_REQ, parser);
        }
     
        private void CPLoginResponse(object data)
        {
           
            if (data is ImThirdLoginResp)
            {
               
                ImThirdLoginResp dataResp = new ImThirdLoginResp();
                if ( ActionLoginResponse != null)
                {
                    userInfo.userId = dataResp.userId;
                    userInfo.nickName = dataResp.nickName;
                    userInfo.thirdUseName = dataResp.thirdUseName;
                    userInfo.thirdUserId = dataResp.thirdUserId;
                    userInfo.iconUrl = dataResp.iconUrl;
                   
                     ActionLoginResponse((ImThirdLoginResp)data);
                     ActionLoginResponse = null;
                }
            }
        }
        
        private void ReLoginNotify(object data)
        {
            if( ActionReLoginSuccess!=null)
            {
                 ActionReLoginSuccess();
            }
        }
        private void GetThirdBindInfo(object data)
        {
            if (data is ImGetThirdBindInfoResp)
            {
                if( ActionThirdBindInfo!=null)
                {
                     ActionThirdBindInfo((ImGetThirdBindInfoResp)data);
                     ActionThirdBindInfo = null;
                }
            }
        }
        #endregion

        #region 工具
		public  System.Action<bool> RecordingCallBack;//录音回调
        /// <summary>
        /// 开始录音（最长60秒）
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="ext"></param>
        public void RecordStartRequest(string filePath,string ext="")
        {
			YunvaLogPrint.YvDebugLog ("RecordStartRequest", string.Format ("filePath:{0},ext:{1}", filePath, ext));
			if(RecordingCallBack!=null)
				RecordingCallBack(true);
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_string(parser, 1, filePath);
            YunVaImInterface.instance.YVparser_set_string(parser, 2, ext);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_TOOLS, (uint)ProtocolEnum.IM_RECORD_STRART_REQ, parser);
        }

        private  System.Action<ImRecordStopResp>  ActionRecordStopResponse;
        /// <summary>
        /// 停止录音请求  回调返回录音文件路径名
        /// </summary>
        /// <param name="Response">返回的回调</param>
        public void RecordStopRequest( System.Action<ImRecordStopResp> Response)
        {
			YunvaLogPrint.YvDebugLog ("RecordStopRequest", "RecordStopRequest..."); 
			if(RecordingCallBack!=null)
				RecordingCallBack(false);
            ActionRecordStopResponse = Response;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_TOOLS, (uint)ProtocolEnum.IM_RECORD_STOP_REQ, parser);
        }

        private void RecordStopRespInfo(object data)
        {
            if (data is ImRecordStopResp)
            {
                if( ActionRecordStopResponse!=null)
                {
                     ActionRecordStopResponse((ImRecordStopResp)data);
                     ActionRecordStopResponse = null;
                }
            }
        }
        private Dictionary<string,  System.Action<ImRecordFinishPlayResp>> RecordFinishPlayRespMapping = new Dictionary<string,  System.Action<ImRecordFinishPlayResp>>();
        /// <summary>
        /// 播放录音请求
        /// </summary>
        /// <param name="url">录音的url路径</param>
        /// <param name="Response">回调方法</param>
        /// <param name="filePath">录音文件路径  （可以不必两者都传 但至少要传入一个）</param>
        /// <param name="ext">扩展标记</param>
        public int RecordStartPlayRequest(string filePath, string url, string ext,  System.Action<ImRecordFinishPlayResp> Response)
        {
			YunvaLogPrint.YvDebugLog ("RecordStartPlayRequest", string.Format ("filePath:{0},url:{1},ext:{2}", filePath, url,ext));
			if(!RecordFinishPlayRespMapping.ContainsKey(ext)){
				RecordFinishPlayRespMapping.Add(ext, Response);
			}

            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			if(!string.IsNullOrEmpty(url)){
				YunVaImInterface.instance.YVparser_set_string(parser, 1, url);
			}

			if(!string.IsNullOrEmpty(filePath)){
				YunVaImInterface.instance.YVparser_set_string(parser, 2, filePath);
			}
			else{
				Debug.Log(string.Format("{0}: not local voice", url));
				YunVaImInterface.instance.YVparser_set_string(parser, 2, "");
			}

			YunVaImInterface.instance.YVparser_set_string(parser, 3, ext);
           	return YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_TOOLS, (uint)ProtocolEnum.IM_RECORD_STARTPLAY_REQ, parser);
        }

        private void RecordFinishPlayRespInfo(object data)
        {
            if (data is ImRecordFinishPlayResp)
            {
                ImRecordFinishPlayResp reData = (ImRecordFinishPlayResp)data;
				string key = reData.ext;
                 System.Action<ImRecordFinishPlayResp> callback;
				if(RecordFinishPlayRespMapping.TryGetValue(key, out callback)){
					if(callback != null){
						callback(reData);
					}

					RecordFinishPlayRespMapping.Remove(key);
				}
				else{
					Debug.LogError (key + ": callback not found");
				}
            }
        }

        /// <summary>
        /// 停止播放语音
        /// </summary>
        public void RecordStopPlayRequest()
        {
			YunvaLogPrint.YvDebugLog ("RecordStopPlayRequest","RecordStopPlayRequest...");
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_TOOLS, (uint)ProtocolEnum.IM_RECORD_STOPPLAY_REQ, parser);
        }

        private Dictionary<string,  System.Action<ImSpeechStopResp>> RecognizeRespMapping = new Dictionary<string,  System.Action<ImSpeechStopResp>>();
        /// <summary>
        /// 开始语音识别
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Response"></param>
        /// <param name="ext"></param>

		public void SpeechStartRequest(string filePath, string ext, System.Action<ImSpeechStopResp> Response,int type=(int)yvspeech.speech_file,string url="")
        {
			YunvaLogPrint.YvDebugLog ("SpeechStartRequest", string.Format ("filePath:{0},ext:{1},type:{2},url:{3}", filePath, ext,type,url));
			//string ext = DateTime.Now.ToFileTime().ToString();
            if (!RecognizeRespMapping.ContainsKey(ext))
                RecognizeRespMapping.Add(ext, Response);
            else
            {
                Debug.LogError("ext标识已经存在，得是一个唯一标识");
            }
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_string(parser, 1, filePath);
            YunVaImInterface.instance.YVparser_set_string(parser, 2, ext);
			YunVaImInterface.instance.YVparser_set_integer(parser, 3, type);
			YunVaImInterface.instance.YVparser_set_string(parser, 4, url);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_TOOLS, (uint)ProtocolEnum.IM_SPEECH_START_REQ, parser);
        }
        private void RecordRecognizeRespInfo(object data)
        {
            if (data is ImSpeechStopResp)
            {
                
				Debug.Log("record recognize...");

                ImSpeechStopResp reData = (ImSpeechStopResp)data;

				string key = reData.ext;
                 System.Action<ImSpeechStopResp> callback;
				if(RecognizeRespMapping.TryGetValue(key, out callback)){
					if(callback != null){
						callback(reData);
					}

					RecognizeRespMapping.Remove(key);
				}
            }
        }

        /// <summary>
        /// 设置语音识别语言
        /// </summary>
        /// <param name="langueage"></param>
		public void SpeechSetLanguage(Yvimspeech_language langueage=Yvimspeech_language.im_speech_zn,yvimspeech_outlanguage outlanguage=yvimspeech_outlanguage.im_speechout_simplified)
		{
			YunvaLogPrint.YvDebugLog ("SpeechSetLanguage", string.Format ("langueage:{0},outlanguage:{1}", langueage, outlanguage));
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_integer(parser, 1, (int)langueage);
			YunVaImInterface.instance.YVparser_set_integer(parser, 2, (int)outlanguage);
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_TOOLS, (uint)ProtocolEnum.IM_SPEECH_SETLANGUAGE_REQ, parser);
		}
        private  System.Action<ImUploadFileResp>  ActionUploadFileResp;
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="Response"></param>
        /// <param name="fileId"></param>
		public void UploadFileRequest(string filePath,string fileId,  System.Action<ImUploadFileResp> Response)
        {
			YunvaLogPrint.YvDebugLog ("UploadFileRequest", string.Format ("filePath:{0},fileId:{1}", filePath, fileId));
            ActionUploadFileResp = Response;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_string(parser, 1, filePath);
            YunVaImInterface.instance.YVparser_set_string(parser, 2, fileId);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_TOOLS, (uint)ProtocolEnum.IM_UPLOAD_FILE_REQ, parser);
        }
        private void UploadFileRespInfo(object data)
        {
            if (data is ImUploadFileResp)
            {
                if (ActionUploadFileResp != null)
                {
                     ActionUploadFileResp((ImUploadFileResp)data);
                     ActionUploadFileResp = null;
                    
                }
            }
        }

        private  System.Action<ImDownLoadFileResp>  ActionDownLoadFileResp;
        /// <summary>
        /// 下载文件请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Response"></param>
        /// <param name="filePath"></param>
        /// <param name="fileid"></param>
        public void DownLoadFileRequest(string url, string filePath, string fileid,  System.Action<ImDownLoadFileResp> Response)
        {
			YunvaLogPrint.YvDebugLog ("DownLoadFileRequest", string.Format ("url:{0},filePath:{1},fileid:{2}",url,filePath, fileid));
            ActionDownLoadFileResp = Response;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_string(parser, 1, url);
            YunVaImInterface.instance.YVparser_set_string(parser, 2, filePath);
            YunVaImInterface.instance.YVparser_set_string(parser, 3, fileid);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_TOOLS, (uint)ProtocolEnum.IM_DOWNLOAD_FILE_REQ, parser);
        }
        private void DownLoadFileRespInfo(object data)
        {
            if (data is ImDownLoadFileResp)
            {
                if( ActionDownLoadFileResp!=null)
                {
                     ActionDownLoadFileResp((ImDownLoadFileResp)data);
                     ActionDownLoadFileResp = null;
                }
            }
        }
        //
        /// <summary>
        /// 设置音量信息。
        /// </summary>
        /// <param name="length">声音长度</param>
        /// <param name="isVolume">true为返回音量，false不返回音量</param>
         public void RecordSetInfoReq(bool isVolume=false)
        {
			YunvaLogPrint.YvDebugLog ("RecordSetInfoReq", string.Format ("isVolume:{0}",isVolume));
            RecordSetInfoReq(isVolume,60);
        }
        
        public void RecordSetInfoReq(bool isVolume,int length)
        {
			YunvaLogPrint.YvDebugLog ("RecordSetInfoReq", string.Format ("isVolume:{0},length:{1}",isVolume,length));
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            if(isVolume)
            { 
                YunVaImInterface.instance.YVparser_set_integer(parser, 1, length);
                YunVaImInterface.instance.YVparser_set_integer(parser, 2, 1);
            }
            else
            {
                YunVaImInterface.instance.YVparser_set_integer(parser, 1, length);
                YunVaImInterface.instance.YVparser_set_integer(parser, 2, 0);
            }
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_TOOLS, (uint)ProtocolEnum.IM_RECORD_SETINFO_REQ, parser);
        }

		public bool CheckCacheFile(string url){
			YunvaLogPrint.YvDebugLog ("CheckCacheFile", string.Format ("url:{0}",url));
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_string(parser, 1, url);
			int ret = YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_TOOLS, (uint)ProtocolEnum.IM_TOOL_HAS_CACHE_FILE, parser);
			return ret == 0;
		}

		private System.Action<ImUploadLocationResp> ActionUploadLocationResp;
		/// <summary>
		/// 上传地理位置请求
		/// </summary>
		/// <param name="Response"></param>
        public void UploadLocationReq(System.Action<ImUploadLocationResp> Response)
		{
			YunvaLogPrint.YvDebugLog ("UploadLocationReq","UploadLocationReq...");
			ActionUploadLocationResp = Response;
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
      
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_LBS, (uint)ProtocolEnum.IM_UPLOAD_LOCATION_REQ, parser);
		}
		private void  UploadLocationResp(object data)
		{
			if(data is ImUploadLocationResp)
			{
				if(ActionUploadLocationResp!=null)
				{
					ActionUploadLocationResp((ImUploadLocationResp)data);
					ActionUploadLocationResp = null;
				}
			}
		}
		private System.Action<ImGetLocationResp> ActionGetLocationResp;
		/// <summary>
		/// 获取位置信息请求
		/// </summary>
		/// <param name="Response"></param>
		public void GetLocationReq(System.Action<ImGetLocationResp> Response)
		{
			YunvaLogPrint.YvDebugLog ("GetLocationReq","GetLocationReq...");
			ActionGetLocationResp = Response;
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
           
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_LBS, (uint)ProtocolEnum.IM_GET_LOCATION_REQ, parser);
		}
		private void GetLocationResp(object data)
		{
			if(data is ImGetLocationResp)
			{
				if(ActionGetLocationResp!=null)
				{
					ActionGetLocationResp((ImGetLocationResp)data);
					ActionGetLocationResp = null;
				}
			}
		}
		private System.Action<ImSearchAroundResp> ActionSearchAroundResp;
		/// <summary>
		/// 搜索（附近）用户
		/// </summary>
		/// <param name="range"></param>
		/// <param name="city"></param>
		/// <param name="sex"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageNum"></param>
		/// <param name="Response"></param>
		public void SearchAroundReq(int range, string city, int sex, int time, int pageIndex, int pageSize, string ext, System.Action<ImSearchAroundResp> Response,string province="",string district="",string detail="")
		{
			YunvaLogPrint.YvDebugLog ("SearchAroundReq",string.Format("range:{0},city:{1},sex:{2},time:{3},pageIndex:{4},pageSize:{5},ext:{6},province:{7},district:{8},detail:{9}", range,city,sex,time,pageIndex,pageSize,ext,province,district,detail));
			ActionSearchAroundResp = Response;
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_integer(parser, 1, range);
			YunVaImInterface.instance.YVparser_set_string(parser, 2, city);
			YunVaImInterface.instance.YVparser_set_integer(parser, 3, sex);
			YunVaImInterface.instance.YVparser_set_integer(parser, 4, time);
			YunVaImInterface.instance.YVparser_set_integer(parser, 5, pageIndex);
			YunVaImInterface.instance.YVparser_set_integer(parser, 6, pageSize);
			YunVaImInterface.instance.YVparser_set_string(parser, 7, ext);
			YunVaImInterface.instance.YVparser_set_string(parser, 8, province);
			YunVaImInterface.instance.YVparser_set_string(parser, 9, district);
			YunVaImInterface.instance.YVparser_set_string(parser, 10, detail);


			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_LBS, (uint)ProtocolEnum.IM_SEARCH_AROUND_REQ, parser);
		}
		private void SearchAroundResp(object data)
		{
			if(data is ImSearchAroundResp)
			{
				if(ActionSearchAroundResp!=null)
				{
					ActionSearchAroundResp((ImSearchAroundResp)data);
					ActionSearchAroundResp = null;
				}
			}
		}
		private System.Action<ImShareLocationResp> ActionShareLocationResp;
		/// <summary>
		/// 隐藏地理位置请求
		/// </summary>
		/// <param name="range">Range.</param>
		/// <param name="Response">Response.</param>
		public void ShareLocationReq(int hide, System.Action<ImShareLocationResp> Response)
		{
			YunvaLogPrint.YvDebugLog ("ShareLocationReq",string.Format("hide:{0}", hide));
			ActionShareLocationResp = Response;
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_integer(parser, 1, hide);
			
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_LBS, (uint)ProtocolEnum.IM_SHARE_LOCATION_REQ, parser);
		}
		private void ShareLocationResp(object data)
		{
			if(data is ImShareLocationResp)
			{
				if(ActionShareLocationResp!=null)
				{
					ActionShareLocationResp((ImShareLocationResp)data);
					ActionShareLocationResp = null;
				}
			}
		}

		private System.Action<ImLBSSetLocatingTypeResp> ActionImLBSSetLocatingTypeResp;
		/// <summary>
		/// 设置定位方式
		/// </summary>
		/// <param name="isGpsLocate">定位方式 GPS<c>true</c> is gps locate.</param>
		/// <param name="isWifiLocate">定位方式 WIFI <c>true</c> is wifi locate.</param>
		/// <param name="isCellLocate">I定位方式 基站 <c>true</c> is cell locate.</param>
		/// <param name="isNetWork">定位方式 网络 <c>true</c> is net work.</param>
		/// <param name="isBluetooth">暂不可用，定位方式 蓝牙<c>true</c> is bluetooth.</param>
		/// <param name="Response">Response.</param>
		public void ImLBSSetLocatingTypeReq(bool isGpsLocate, bool isWifiLocate, bool isCellLocate, bool isNetWork, bool isBluetooth, System.Action<ImLBSSetLocatingTypeResp> Response)
		{
			YunvaLogPrint.YvDebugLog ("ImLBSSetLocatingTypeReq",string.Format("isGpsLocate:{0},isWifiLocate:{1},isCellLocate:{2},isNetWork:{3},isBluetooth:{4}", isGpsLocate,isWifiLocate,isCellLocate,isNetWork,isBluetooth));
			ActionImLBSSetLocatingTypeResp = Response;
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			if (isGpsLocate)
				YunVaImInterface.instance.YVparser_set_uint8(parser, 1, 0);
			else
				YunVaImInterface.instance.YVparser_set_uint8(parser, 1, 1);
			if (isWifiLocate)
				YunVaImInterface.instance.YVparser_set_uint8(parser, 2, 0);
			else
				YunVaImInterface.instance.YVparser_set_uint8(parser, 2, 1);
			if (isCellLocate)
				YunVaImInterface.instance.YVparser_set_uint8(parser,3, 0);
			else
				YunVaImInterface.instance.YVparser_set_uint8(parser, 3, 1);
			if (isNetWork)
				YunVaImInterface.instance.YVparser_set_uint8(parser,4, 0);
			else
				YunVaImInterface.instance.YVparser_set_uint8(parser, 4, 1);
			if (isBluetooth)
				YunVaImInterface.instance.YVparser_set_uint8(parser, 5, 0);
			else
				YunVaImInterface.instance.YVparser_set_uint8(parser,5, 1);
			
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_LBS, (uint)ProtocolEnum.IM_LBS_SET_LOCATING_TYPE_REQ, parser);
		}
		private void LBSSetLocatingTypeResp(object data)
		{
			if(data is ImLBSSetLocatingTypeResp)
			{
				if(ActionImLBSSetLocatingTypeResp!=null)
				{
					ActionImLBSSetLocatingTypeResp((ImLBSSetLocatingTypeResp)data);
					ActionImLBSSetLocatingTypeResp = null;
				}
			}
		}

		private  System.Action<ImLBSGetSpportLangResp>  ActionImLBSGetSpportLangResp;
		/// <summary>
		/// 获取支持的（包括搜索、返回信息等）本地化语言列表请求
		/// </summary>
		/// <param name="Response">Response.</param>
		public void ImLBSGetSpportLangReq(System.Action<ImLBSGetSpportLangResp> Response)
		{
			YunvaLogPrint.YvDebugLog ("ImLBSGetSpportLangReq","ImLBSGetSpportLangReq...");
			ActionImLBSGetSpportLangResp = Response;
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_LBS, (uint)ProtocolEnum.IM_LBS_GET_SUPPORT_LANG_REQ, parser);
		}
		private void LBSGetSpportLangResp(object data)
		{
			if (data is ImLBSGetSpportLangResp)
			{
				if (ActionImLBSGetSpportLangResp != null)
				{
					ActionImLBSGetSpportLangResp((ImLBSGetSpportLangResp)data);
					ActionImLBSGetSpportLangResp = null;
					
				}
			}
		}

		private  System.Action<ImLBSSetLocalLangResp>  ActionImLBSSetLocalLangResp;
		/// <summary>
		/// 设置LBS本地化语言
		/// </summary>
		/// <param name="Response">Response.</param>
		public void ImLBSSetLocalLangReq(string lang_code,string country_code, System.Action<ImLBSSetLocalLangResp> Response)
		{
			YunvaLogPrint.YvDebugLog ("ImLBSSetLocalLangReq",string.Format("lang_code:{0},country_code:{1}", lang_code,country_code));
			ActionImLBSSetLocalLangResp = Response;
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_string(parser, 1, lang_code);
			YunVaImInterface.instance.YVparser_set_string(parser, 2, country_code);
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_LBS, (uint)ProtocolEnum.IM_LBS_SET_LOCAL_LANG_REQ, parser);
		}
		private void LBSSetLocalLangResp(object data)
		{
			if (data is ImLBSSetLocalLangResp)
			{
				if (ActionImLBSSetLocalLangResp != null)
				{
					ActionImLBSSetLocalLangResp((ImLBSSetLocalLangResp)data);
					ActionImLBSSetLocalLangResp = null;
					
				}
			}
		}

        #endregion

        #region 好友
        #region 通知（不需要请求，由服务器自动下发）
        public System.Action<ImFriendListNotify> ActionFriendListNotify;//好友列表
        private void FriendListNotify(object data)
        {
            if (!(data is ImFriendListNotify))
                return;
            if( ActionFriendListNotify!=null)
            {
                ActionFriendListNotify((ImFriendListNotify)data);
            }
        }
        public System.Action<ImFriendBlackListNotify> ActionBlackListNotify;//黑名单列表
        private void BlackListNotify(object data)
        {
            if (!(data is ImFriendBlackListNotify))
                return;
            if ( ActionBlackListNotify != null)
            {
                ActionBlackListNotify((ImFriendBlackListNotify)data);
            }
        }

        #endregion
        /// <summary>
        /// 发送添加好友请求
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="greet"></param>
        public void FriendAddRequest(int userId,string greet="")
        {
			YunvaLogPrint.YvDebugLog ("FriendAddRequest",string.Format("userId:{0},greet:{1}", userId,greet));
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_integer(parser, 1, userId);
            YunVaImInterface.instance.YVparser_set_string(parser, 2, greet);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_FRIEND, (uint)ProtocolEnum.IM_FRIEND_ADD_REQ, parser);
        }
        private  System.Action<ImFriendAcceptResp>  ActionAcceptOrRefuse;
        /// <summary>
        /// 发送同意或者拒绝对方加好友的请求
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="affirm"></param>
        /// <param name="Response"></param>
        /// <param name="greet"></param>
        public void AcceptOrRefuseRequest(int userId, e_addfriend_affirm affirm,  System.Action<ImFriendAcceptResp> Response, string greet = "")
        {
			YunvaLogPrint.YvDebugLog ("AcceptOrRefuseRequest",string.Format("userId:{0},affirm:{1},greet:{2}", userId,affirm,greet));
             ActionAcceptOrRefuse=Response;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_integer(parser, 1, userId);
            YunVaImInterface.instance.YVparser_set_integer(parser, 2, (int)affirm);
            YunVaImInterface.instance.YVparser_set_string(parser, 3, greet);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_FRIEND, (uint)ProtocolEnum.IM_FRIEND_ADD_ACCEPT, parser);

        }
        private void AcceptOrRefuseResponse(object data)
        {
           if(!(data is ImFriendAcceptResp))
               return;
            if( ActionAcceptOrRefuse!=null)
            {
                 ActionAcceptOrRefuse((ImFriendAcceptResp)data);
                 ActionAcceptOrRefuse=null;
            }
        }
        private  System.Action<ImFriendDelResp>  ActionDeleteFriendRequest;
        /// <summary>
        /// 删除好友请求
        /// </summary>
        /// <param name="userId">被删除好友的ID</param>
        /// <param name="act">0是只从我的好友列表删除，1是双向删除</param>
        /// <param name="Response">删除完成的回调</param>
        public void DeleteFriendRequest(int userId, e_delfriend act,  System.Action<ImFriendDelResp> Response)
        {
			YunvaLogPrint.YvDebugLog ("DeleteFriendRequest",string.Format("userId:{0},act:{1}", userId,act));
             ActionDeleteFriendRequest = Response;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_integer(parser, 1, userId);
            YunVaImInterface.instance.YVparser_set_integer(parser, 2, (int)act);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_FRIEND, (uint)ProtocolEnum.IM_FRIEND_DEL_REQ, parser);
        }
        private void DeleteFriendResponse(object data)
        {
            if (!(data is ImFriendDelResp))
                return;
            if( ActionDeleteFriendRequest!=null)
            {
                 ActionDeleteFriendRequest((ImFriendDelResp)data);
                 ActionDeleteFriendRequest = null;
            }
        }
        private  System.Action<ImFriendSearchResp>  ActionSearchFriendRequest;
        /// <summary>
        /// 搜索好友请求
        /// </summary>
        /// <param name="keyWrold">搜索的关键字</param>
        /// <param name="Respone">搜索的回调</param>
        ///<param name="searchCount">返回搜索个数</param>
        public void SearchFriendRequest(string keyWrold,  System.Action<ImFriendSearchResp> Respone, int searchCount)
        {
			YunvaLogPrint.YvDebugLog ("SearchFriendRequest",string.Format("keyWrold:{0},searchCount:{1}", keyWrold,searchCount));
             ActionSearchFriendRequest = Respone;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_string(parser, 1, keyWrold);
            YunVaImInterface.instance.YVparser_set_integer(parser, 2, 0);
            YunVaImInterface.instance.YVparser_set_integer(parser, 3, searchCount);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_FRIEND, (uint)ProtocolEnum.IM_FRIEND_SEARCH_REQ, parser);
        }
        private void SearchFriendResponse(object data)
        {
           
            if (!(data is ImFriendSearchResp))
                return;
            if( ActionSearchFriendRequest!=null)
            {
                 ActionSearchFriendRequest((ImFriendSearchResp)data);
                 ActionSearchFriendRequest = null;
            }
        }



        private  System.Action<ImFriendSearchResp>  ActionRecommandFriendRequest;
        /// <summary>
        /// 获取推荐好友的请求
        /// </summary>
        /// <param name="recommandCount">推荐的数量</param>
        /// <param name="Response">回调</param>
        public void RecommandFriendRequest(int recommandCount,  System.Action<ImFriendSearchResp> Response = null)
        {
			YunvaLogPrint.YvDebugLog ("RecommandFriendRequest",string.Format("recommandCount:{0}", recommandCount));
             ActionRecommandFriendRequest = Response;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_integer(parser, 1, 0);
            YunVaImInterface.instance.YVparser_set_integer(parser, 2, recommandCount);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_FRIEND, (uint)ProtocolEnum.IM_FRIEND_RECOMMAND_REQ, parser);

        }
        private void RecommandFriendRespone(object data)
        {
            if (!(data is ImFriendSearchResp))
                return; 
            if( ActionRecommandFriendRequest!=null)
            {
                 ActionRecommandFriendRequest((ImFriendSearchResp)data);
                 ActionRecommandFriendRequest = null;
            }
        }
        private  System.Action<ImFriendOperResp>  ActionBlackListOperation;
        /// <summary>
        /// 黑名单的操作请求
        /// </summary>
        /// <param name="myUserId">自己的Uid</param>
        /// <param name="operUserId">被操作的Uid</param>
        /// <param name="Response">回调</param>
        /// <param name="act">3为加入黑名单，4为删除黑名单</param>
        public void BlackListOperation(int myUserId, int operUserId,  System.Action<ImFriendOperResp> Response, e_oper_friend_act act = e_oper_friend_act.oper_add_blacklist)
        {
			YunvaLogPrint.YvDebugLog ("BlackListOperation",string.Format("myUserId:{0},operUserId:{1},act:{2}", myUserId,operUserId,act));
             ActionBlackListOperation = Response;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_integer(parser, 1, myUserId);
            YunVaImInterface.instance.YVparser_set_integer(parser, 2, operUserId);
            YunVaImInterface.instance.YVparser_set_integer(parser, 3, (int)act);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_FRIEND, (uint)ProtocolEnum.IM_FRIEND_OPER_REQ, parser);

        }
        private void BlackListOperationResponse(object data)
        {
            if (!(data is ImFriendOperResp))
                return;
            if( ActionBlackListOperation!=null)
            {
                 ActionBlackListOperation((ImFriendOperResp)data);
                 ActionBlackListOperation = null;
            }
        }
        private  System.Action<object>  ActionSetFriendInfoRequest;
        /// <summary>
        /// 设置好友信息
        /// </summary>
        /// <param name="friendId"></param>
        /// <param name="group"></param>
        /// <param name="note"></param>
        /// <param name="Response"></param>
        public void SetFriendInfoRequest(int friendId,string group,string note, System.Action<object> Response)
        {
			YunvaLogPrint.YvDebugLog ("SetFriendInfoRequest",string.Format("friendId:{0},group:{1},note:{2}", friendId,group,note));
             ActionSetFriendInfoRequest = Response;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_integer(parser, 1, friendId);
            YunVaImInterface.instance.YVparser_set_string(parser, 2, group);
            YunVaImInterface.instance.YVparser_set_string(parser, 3, note);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_FRIEND, (uint)ProtocolEnum.IM_FRIEND_INFOSET_REQ, parser);

        }
        private void SetFriendInfoResponse(object data)
        {
            if( ActionSetFriendInfoRequest!=null)
            {
                 ActionSetFriendInfoRequest(data);
                 ActionSetFriendInfoRequest = null;
            }
        }
        private  System.Action<ImUserGetInfoResp>  ActionGetUserInfoRequest;
        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Response"></param>
        public void GetUserInfoRequest(int userId,  System.Action<ImUserGetInfoResp> Response)
        {
			YunvaLogPrint.YvDebugLog ("GetUserInfoRequest",string.Format("userId:{0}", userId));
             ActionGetUserInfoRequest = Response;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_integer(parser, 1, userId);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_FRIEND, (uint)ProtocolEnum.IM_USER_GETINFO_REQ, parser);
        }

        private void GetUserInfoResponse(object data)
        {
            if (!(data is ImUserGetInfoResp))
                return;
            if( ActionGetUserInfoRequest!=null)
            {
                 ActionGetUserInfoRequest((ImUserGetInfoResp)data);
                 ActionGetUserInfoRequest = null;
            }
        }

        //删除最近联系人
        private  System.Action<ImRemoveContactesResp>  ActionRemoveContactesResp;
        public void RemoveContactesReq(int userId,  System.Action<ImRemoveContactesResp> Response)
        {
			YunvaLogPrint.YvDebugLog ("RemoveContactesReq",string.Format("userId:{0}", userId));
             ActionRemoveContactesResp = Response;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_integer(parser, 1, userId);
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_FRIEND, (uint)ProtocolEnum.IM_REMOVE_CONTACTES_REQ, parser);
        }
        private void RemoveContactesResp(object data)
        {
            if (!(data is ImRemoveContactesResp))
                return;
            if( ActionRemoveContactesResp!=null)
            {
                 ActionRemoveContactesResp((ImRemoveContactesResp)data);
                 ActionRemoveContactesResp = null;
            }
        }
        #endregion

		#region 好友聊天

        
        private Queue< System.Action<ImChatFriendResp>> SendFriendMsgRespQ = new Queue< System.Action<ImChatFriendResp>>();

		private Dictionary<string,  System.Action<ImChatFriendResp>> SendFriendMsgRespMapping = new Dictionary<string,  System.Action<ImChatFriendResp>>();

	//	private  System.Action<SendFriendMsgResp>  System.ActionSendFriendMsgResp;
		/// <summary>
		/// 发送好友聊天-文本
		/// </summary>
		/// <param name="userId">好友ID.</param>
		/// <param name="textMsg">消息内容</param>
		/// <param name="Response">Response.</param>
		/// <param name="ext">扩展字段.</param>
        
        public int SendP2PTextMessage(int userId, string textMsg,  System.Action<ImChatFriendResp> Response, string flag, string ext = "")
		{
			YunvaLogPrint.YvDebugLog ("SendP2PTextMessage", string.Format ("userId:{0},textMsg:{1},flag:{2},ext:{3}",userId,textMsg,flag,ext));
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_integer(parser, 1, userId);
			YunVaImInterface.instance.YVparser_set_string(parser, 2, textMsg);
			YunVaImInterface.instance.YVparser_set_string(parser, 3, ext);
            
            YunVaImInterface.instance.YVparser_set_string(parser, 4, flag);
			int result = YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CHAT, (uint)ProtocolEnum.IM_CHAT_FRIEND_TEXT_REQ, parser);

			if(result == 0){

				//Debug.Log(string.Format("<debug> p2p send flag: {0}", flag));
				
				//SendFriendMsgRespQ.Enqueue(Response);
				 System.Action<ImChatFriendResp> callback;
				if(SendFriendMsgRespMapping.TryGetValue(flag, out callback)){
					callback = Response;
				}
				else{
					SendFriendMsgRespMapping.Add(flag, Response);
				}
			}

			return result;
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameUserID">CP游戏的uid</param>
        /// <param name="textMsg"></param>
        /// <param name="Response"></param>
        /// <param name="flag"></param>
        /// <param name="ext"></param>
         public void SendP2PTextMessage(string gameUserID, string textMsg,  System.Action<ImChatFriendResp> Response, string flag, string ext = "")
        {
			YunvaLogPrint.YvDebugLog ("SendP2PTextMessage", string.Format ("gameUserID:{0}",gameUserID));
             YunVaGetThirdBindInfo((int)appid,gameUserID,(data)=>
                 {
                     SendP2PTextMessage(data.yunvaId, textMsg, Response, flag, ext);
                 });
        }
		/// <summary>
		/// 发送好友聊天 - 语音
		/// </summary>
		/// <param name="userId">好友ID.</param>
		/// <param name="filePath">语音文件路径.</param>
		/// <param name="audioTime">文件播放时长(秒).</param>
		/// <param name="txt">附带文本(可选).</param>
		/// <param name="Response">Response.</param>
		/// <param name="ext">扩展字段.</param>
        
        public int SendP2PVoiceMessage(int userId, string filePath, int audioTime, string txt,  System.Action<ImChatFriendResp> Response, string flag, string ext = "")
		{
			YunvaLogPrint.YvDebugLog ("SendP2PVoiceMessage", string.Format ("userId:{0},filePath:{1},audioTime:{2},txt:{3},flag:{4},ext:{5}",userId,filePath,audioTime,txt,flag,ext));
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_integer(parser, 1, userId);
			YunVaImInterface.instance.YVparser_set_string(parser, 2, filePath);
			YunVaImInterface.instance.YVparser_set_integer(parser, 3, audioTime);
			if(!string.IsNullOrEmpty(txt)){
				YunVaImInterface.instance.YVparser_set_string(parser, 4, txt);
			}
			YunVaImInterface.instance.YVparser_set_string(parser, 5, ext);
            
            YunVaImInterface.instance.YVparser_set_string(parser, 6, flag);
			int result = YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CHAT, (uint)ProtocolEnum.IM_CHAT_FRIEND_AUDIO_REQ, parser);

			if (result == 0) {
				
								//SendFriendMsgRespQ.Enqueue(Response);

								System.Action<ImChatFriendResp> callback;
								if (SendFriendMsgRespMapping.TryGetValue (flag, out callback)) {
										callback = Response;
								} else {
										SendFriendMsgRespMapping.Add (flag, Response);
								}
						} 
			else {
				  Debug.Log("no network before send..");
			     }

			return result;
		}
		private void SendFriendMsgResponse(object data)
		{

            if (data is ImChatFriendResp)
			{
                ImChatFriendResp dataResp = (ImChatFriendResp)data;
                
//                Debug.Log(string.Format("发送文本的flag:{0},userid:{1},result:{2},data.msg:{3}", dataResp.flag, dataResp.userId, dataResp.result,dataResp.msg));
				 System.Action<ImChatFriendResp> callback;
				string key = dataResp.flag;
				//Debug.Log(string.Format("<debug> p2p resp flag: {0}", key));

				if(SendFriendMsgRespMapping.TryGetValue(key, out callback)){
					SendFriendMsgRespMapping.Remove(key);
					if(callback != null){
						callback(dataResp);
					}
				}
			}	
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameUserID">CP游戏的userID</param>
        /// <param name="filePath"></param>
        /// <param name="audioTime"></param>
        /// <param name="txt"></param>
        /// <param name="Response"></param>
        /// <param name="flag"></param>
        /// <param name="ext"></param>
        public void SendP2PVoiceMessage(string gameUserID, string filePath, int audioTime, string txt, System.Action<ImChatFriendResp> Response, string flag, string ext = "")
        {
			YunvaLogPrint.YvDebugLog ("SendP2PVoiceMessage", string.Format ("gameUserID:{0}",gameUserID));
            YunVaGetThirdBindInfo((int)appid, gameUserID, (data) =>
            {
                SendP2PVoiceMessage(data.yunvaId, filePath, audioTime, txt, Response,flag,ext);
            });
        }

		#endregion
		#region 频道聊天


        
        private Queue< System.Action<ImChannelSendMsgResp>> SendChannelMsgRespQ = new Queue< System.Action<ImChannelSendMsgResp>>();

		private Dictionary<string,  System.Action<ImChannelSendMsgResp>> SendChannelMsgRespMapping = new Dictionary<string,  System.Action<ImChannelSendMsgResp>>();

		/// <summary>
		/// 发送频道文字消息请求
		/// </summary>
		/// <param name="textMsg">发送内容</param>
		/// <param name="wildCard">通配符</param>
		/// <param name="Response">Response.</param>
		/// <param name="ext">扩展字段</param>
        
		public int SendChannelTextMessage(string textMsg, string wildCard,  System.Action<ImChannelSendMsgResp> Response, string expand,string flag="")
		{
			YunvaLogPrint.YvDebugLog ("SendChannelTextMessage", string.Format ("textMsg:{0},wildCard:{1},expand:{2},flag:{3}",textMsg,wildCard,expand,flag));
			System.Action<ImChannelSendMsgResp> callback;
			if(SendChannelMsgRespMapping.TryGetValue(flag, out callback)){
				callback = Response;
			}
			else{
				SendChannelMsgRespMapping.Add(flag, Response);
			}
			string ext = flag;
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_string(parser, 1, textMsg);
			YunVaImInterface.instance.YVparser_set_string(parser, 2, wildCard);
			YunVaImInterface.instance.YVparser_set_string(parser, 3, expand);
			YunVaImInterface.instance.YVparser_set_string(parser, 4, ext);
			int result = YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CHANNEL, (uint)ProtocolEnum.IM_CHANNEL_TEXTMSG_REQ, parser);
			Debug.Log ("SendChannelTextMessage return:"+result);
			if(result == 0){
				//to be fixed;
				//even result == 0, the resp may not be back;
				//this may mess up the callback chain;
				
				//SendChannelMsgRespQ.Enqueue(Response);


			}
			else if(result==1004)
			{
				Debug.Log("yunva come in 1004...");
//				System.Action<ImChannelSendMsgResp> callback;
//				if(SendChannelMsgRespMapping.TryGetValue(flag, out callback)){
//					callback = Response;
//				}
//				else{
//					SendChannelMsgRespMapping.Add(flag, Response);
//				}
			}
			else{
				Debug.Log("no network before send..");
			}

			return result;
		}

        
		public int SendChannelVoiceMessage(string voicePath, int voiceDurationTime, string wildCard, string text,  System.Action<ImChannelSendMsgResp> Response,string expand, string flag="")
        {
			YunvaLogPrint.YvDebugLog ("SendChannelVoiceMessage", string.Format ("voicePath:{0},voiceDurationTime:{1},wildCard:{2},text:{3},expand:{4},flag:{5}",voicePath,voiceDurationTime,wildCard,text,expand,flag));
			string ext = flag;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YVparser_set_string(parser, 1, voicePath);
            YunVaImInterface.instance.YVparser_set_integer(parser, 2, voiceDurationTime);
            YunVaImInterface.instance.YVparser_set_string(parser, 3, wildCard);
			if(!string.IsNullOrEmpty(text)){
				YunVaImInterface.instance.YVparser_set_string(parser, 4, text);
			}
			YunVaImInterface.instance.YVparser_set_string(parser, 5, expand);
			YunVaImInterface.instance.YVparser_set_string(parser, 6, ext);
            int result = YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CHANNEL, (uint)ProtocolEnum.IM_CHANNEL_VOICEMSG_REQ, parser);
			Debug.Log ("SendChannelVoiceMessage return:"+result);
			if(result == 0){
				//event result == 0; the resp may not be back;
				
				//SendChannelMsgRespQ.Enqueue(Response);

				 System.Action<ImChannelSendMsgResp> callback;
				if(SendChannelMsgRespMapping.TryGetValue(flag, out callback)){
					callback = Response;
				}
				else{
					SendChannelMsgRespMapping.Add(flag, Response);
				}
			}
			else{
				Debug.Log("no network before send..");
			}

			return result;
        }

		private void SendChannelMsgResponse(object data)
		{
            if (data is ImChannelSendMsgResp)
			{
                ImChannelSendMsgResp resp = data as ImChannelSendMsgResp;

				
//				if(SendChannelMsgRespQ.Count > 0){
//                     System.Action<ImChannelSendMsgResp> callback = SendChannelMsgRespQ.Dequeue();
//					if(callback != null){
//						callback(resp);
//					}
//				}

				 System.Action<ImChannelSendMsgResp> callback;
				string key = resp.flag;
				Debug.Log ("recive falg:"+key);
				if(SendChannelMsgRespMapping.TryGetValue(key, out callback)){
					SendChannelMsgRespMapping.Remove(key);
					if(callback != null){
						callback(resp);
					}
				}
			}	
		} 

		private  System.Action<ImChannelMotifyResp>  ActionImChannelMotifyResp;
		/// <summary>
		/// 登陆频道
		/// </summary>
		/// <param name="channel">通道（0-9）</param>
		/// <param name="wildCard">通配符</param>
		/// <param name="Response">Response.</param>
		public void LoginChannel(int channel,string wildCard,System.Action<ImChannelMotifyResp> Response)
		{
			YunvaLogPrint.YvDebugLog ("LoginChannel", string.Format ("channel:{0},wildCard:{1}",channel,wildCard));
			ActionImChannelMotifyResp = Response;
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_integer(parser, 1, 1);
			YunVaImInterface.instance.YVparser_set_integer(parser, 2, channel);
			YunVaImInterface.instance.YVparser_set_string(parser, 3, wildCard);
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CHANNEL, (uint)ProtocolEnum.IM_CHANNEL_MODIFY_REQ, parser);
		}

		/// <summary>
		/// 登出频道
		/// </summary>
		/// <param name="channel">通道（0-9）</param>
		/// <param name="wildCard">通配符</param>
		/// <param name="Response">Response.</param>
		public void LogoutChannel(int channel,string wildCard,System.Action<ImChannelMotifyResp> Response)
		{
			YunvaLogPrint.YvDebugLog ("LogoutChannel", string.Format ("channel:{0},wildCard:{1}",channel,wildCard));
			ActionImChannelMotifyResp = Response;
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_integer(parser, 1, 0);
			YunVaImInterface.instance.YVparser_set_integer(parser, 2, channel);
			YunVaImInterface.instance.YVparser_set_string(parser, 3, wildCard);
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CHANNEL, (uint)ProtocolEnum.IM_CHANNEL_MODIFY_REQ, parser);
		}

		private void ChannelMotifyResponse(object data)
		{
			if(!(data is ImChannelMotifyResp))
				return;
			if( ActionImChannelMotifyResp!=null)
			{
				ActionImChannelMotifyResp((ImChannelMotifyResp)data);
				ActionImChannelMotifyResp=null;
			}
		}
		
		private  System.Action<ImChannelLoginResp>  ActionLoginChannelResp;
//		/// <summary>
//		/// 频道登陆
//		/// </summary>
//		/// <param name="pgameServiceID">游戏服务ID</param>
//		/// <param name="wildCard">通配符</param>
//		/// <param name="Response">Response.</param>
//		public void loginChannel(string pgameServiceID,string[] wildCard, System.Action<ImChannelLoginResp> Response)
//		{
//			 ActionLoginChannelResp = Response;
//			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
//			YunVaImInterface.instance.YVparser_set_string(parser, 1, pgameServiceID);
//			for(int i=0;i<wildCard.Length;i++)
//			{
//				YunVaImInterface.instance.YVparser_set_string(parser, 2, wildCard[i]);
//			}
//			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CHANNEL, (uint)ProtocolEnum.IM_CHANNEL_LOGIN_REQ, parser);
//		}
        private void loginChannelResponse(object data)
        {
            if (!(data is ImChannelLoginResp))
                return;
            if (ActionLoginChannelResp != null)
            {
                ActionLoginChannelResp((ImChannelLoginResp)data);
                ActionLoginChannelResp = null;
            }

        }
//		
//		/// <summary>
//		/// 频道退出
//		/// </summary>
//		public void logoutChannel()
//		{
//			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
//			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CHANNEL, (uint)ProtocolEnum.IM_CHANNEL_LOGOUT_REQ, parser);
//		}
//		
//		private  System.Action<ImChannelGetInfoResp>  ActionGetChannelInfoResp;
//		/// <summary>
//		/// 获取频道信息请求
//		/// </summary>
//		public void getChannelInfo( System.Action<ImChannelGetInfoResp> Response)
//		{
//			 ActionGetChannelInfoResp = Response;
//			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
//			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CHANNEL, (uint)ProtocolEnum.IM_CHANNEL_GETINFO_REQ, parser);
//		}
//		private void getChannelInfoResponse(object data)
//		{
//			if(!(data is ImChannelGetInfoResp))
//				return;
//			if( ActionGetChannelInfoResp!=null)
//			{
//				 ActionGetChannelInfoResp((ImChannelGetInfoResp)data);
//				 ActionGetChannelInfoResp=null;
//			}
//		}


        private System.Action<ImChannelHistoryMsgResp> ActionGetChannelHistoryMsgResp;
		/// <summary>
		/// 频道获取历史消息请求
		/// </summary>
		/// <param name="index">消息索引	(当前最大索引号,索引为0请求最后count条记录)</param>
		/// <param name="count">请求条数	正数为index向后请求 负数为index向前请求 (时间排序)</param>
		/// <param name="wildcard">游戏通道字符串</param>
		/// <param name="Response">Response.</param>

		public void getChannelHistoryMsg(int index,int count,string wildcard, System.Action<ImChannelHistoryMsgResp> Response)
		{
			YunvaLogPrint.YvDebugLog ("getChannelHistoryMsg", string.Format ("index:{0},count:{1},wildcard:{2}",index,count,wildcard));
			 ActionGetChannelHistoryMsgResp = Response;
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_integer(parser, 1, index);
			YunVaImInterface.instance.YVparser_set_integer(parser, 2, count);
			YunVaImInterface.instance.YVparser_set_string(parser, 3, wildcard);
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CHANNEL, (uint)ProtocolEnum.IM_CHANNEL_HISTORY_MSG_REQ, parser);
		}
		private void getChannelHistoryMsgResponse(object data)
		{
            if (!(data is ImChannelHistoryMsgResp))
                return;
			if( ActionGetChannelHistoryMsgResp!=null)
			{
				 ActionGetChannelHistoryMsgResp((ImChannelHistoryMsgResp)data);
				 ActionGetChannelHistoryMsgResp = null;
			}
		}
        private Action<ImChannelGetParamResp> ActionChannelGetParamResp;
        /// <summary>
        /// 获取公告信息
        /// </summary>
        /// <param name="Response"></param>
        public void getChannelGetParamReq(Action<ImChannelGetParamResp> Response)
        {
			YunvaLogPrint.YvDebugLog ("getChannelGetParamReq","getChannelGetParamReq...");
            ActionChannelGetParamResp = Response;
            uint parser = YunVaImInterface.instance.YVpacket_get_parser();
            YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CHANNEL, (uint)ProtocolEnum.IM_CHANNEL_GET_PARAM_REQ, parser);

        }
        private void ChannelGetParamResponse(object data)
        {
            if (!(data is ImChannelGetParamResp)) return;
            if(ActionChannelGetParamResp!=null)
            {
                ActionChannelGetParamResp((ImChannelGetParamResp)data);
                ActionChannelGetParamResp = null;
            }
        }
		#endregion

		#region 离线云消息模块
		private  System.Action<ImCloudMsgLimitResp>  ActioncloudMsgResp;
		/// <summary>
		/// 请求云消息
		/// </summary>
		/// <param name="source">来源(userId 好友消息)</param>
		/// <param name="id">若是好友消息, 则为好友ID</param>
		/// <param name="end">获取到位置（endid）</param>
		/// <param name="limit">获取条数</param>
		/// <param name="Response">Response.</param>
		public void cloudMsgRequest(string source,int id,int end,int limit, System.Action<ImCloudMsgLimitResp> Response)
		{
			YunvaLogPrint.YvDebugLog ("cloudMsgRequest", string.Format ("source:{0},id:{1},end:{2},limit:{3}",source,id,end,limit));
            if (limit < -20) limit = -20;
			 ActioncloudMsgResp = Response;
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_string(parser, 1, source);
			YunVaImInterface.instance.YVparser_set_integer(parser, 2, id);
			YunVaImInterface.instance.YVparser_set_integer(parser, 3, end);
			YunVaImInterface.instance.YVparser_set_integer(parser, 4, limit);
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CLOUND, (uint)ProtocolEnum.IM_CLOUDMSG_LIMIT_REQ, parser);
		}

		private void cloudMsgResponse(object data)
		{
			if(!(data is ImCloudMsgLimitResp))
				return;
			if( ActioncloudMsgResp!=null)
			{
				 ActioncloudMsgResp((ImCloudMsgLimitResp)data);
				 ActioncloudMsgResp=null;
			}
		}

        /// <summary>
		/// 云消息确认
        /// </summary>
		/// <param name="id">对应 CLOUDMSG_ID::110</param>
		/// <param name="source">对应 CLOUDMSG_SOURCE::111</param>
		public void cloudMsgConfirmRequest(int id,string source)
		{
			YunvaLogPrint.YvDebugLog ("cloudMsgConfirmRequest", string.Format ("id:{0},source:{1}",id,source));
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_integer(parser, 1, id);
			YunVaImInterface.instance.YVparser_set_string(parser, 2, source);
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CLOUND, (uint)ProtocolEnum.IM_CLOUDMSG_READ_STATUS, parser);
		}

		/// <summary>
		/// 离线消息忽略
		/// </summary>
		/// <param name="source">对应 云消息SOURCE</param>
		/// <param name="id">好友ID，或者群ID</param>
		/// <param name="index">忽略到的位置， 0：表示全部忽略</param>
		public void outLineMsgIgnoreRequest(string source,int id,int index)
		{
			YunvaLogPrint.YvDebugLog ("outLineMsgIgnoreRequest", string.Format ("source:{0},id:{1},index:{2}",source,id,index));
			uint parser = YunVaImInterface.instance.YVpacket_get_parser();
			YunVaImInterface.instance.YVparser_set_string(parser, 1, source);
			YunVaImInterface.instance.YVparser_set_integer(parser, 2, id);
			YunVaImInterface.instance.YVparser_set_integer(parser, 3, index);
			YunVaImInterface.instance.YV_SendCmd(CmdChannel.IM_CLOUND, (uint)ProtocolEnum.IM_CLOUDMSG_IGNORE_REQ, parser);
		}

		/// <summary>
		/// 是否打印日志
		/// </summary>
		/// <param name="logLevel">LOG_LEVEL_OFF = 0,  //0：关闭日志,LOG_LEVEL_DEBUG = 1 //1：Debug默认该级别</param>
		public void YvLog_setLogLevel(int logLevel = (int)YvlogLevel.LOG_LEVEL_DEBUG)
		{
			YunvaLogPrint.YvLog_setLogLevel (logLevel);
		}


		#endregion
    }
}
