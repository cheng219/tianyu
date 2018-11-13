using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace YunvaIM
{
    //搜索好友返回的类
    public class ImFriendSearchResp:YunvaMsgBase
    {
        public int result;
        public string msg;
        public List<xSearchInfo> searchUserInfo;
        public ImFriendSearchResp(object Parser, ProtocolEnum protocol)
        {
           
            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(protocol,  SearchFriendInfo((uint)Parser)));
        }
        public ImFriendSearchResp() { }
        

        private  ImFriendSearchResp SearchFriendInfo(uint parser)
        {
            ImFriendSearchResp friendSearchResp = new ImFriendSearchResp();
            List<xSearchInfo> searchInfoList = new List<xSearchInfo>();

            friendSearchResp.result = YunVaImInterface. parser_get_integer(parser, 1, 0);
            friendSearchResp.msg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
			YunvaLogPrint.YvDebugLog ("ImFriendSearchResp",string.Format("result:{0},msg:{1}",  friendSearchResp.result,  friendSearchResp.msg));
            for (int i = 0; ; i++)
            {
                if (YunVaImInterface.parser_is_empty(parser, 3, i))
                    break;

				uint searchInfo = YunVaImInterface.yvpacket_get_nested_parser(parser);
                YunVaImInterface.parser_get_object(parser, 3, searchInfo, i);
                xSearchInfo userInfo = new xSearchInfo();

                userInfo.yunvaId = YunVaImInterface.parser_get_integer(searchInfo, 1, 0);
                userInfo.userId = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(searchInfo, 2, 0));
                userInfo.nickName = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(searchInfo, 3, 0));
                userInfo.iconUrl = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(searchInfo, 4, 0));
                userInfo.level = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(searchInfo, 5, 0));
                userInfo.vip = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(searchInfo, 6, 0));
                userInfo.ext = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(searchInfo, 7, 0));
				YunvaLogPrint.YvDebugLog ("ImFriendSearchResp",string.Format("yunvaId:{0},userId:{1},nickName:{2},iconUrl:{3},level:{4},vip:{5},ext:{6}",  userInfo.yunvaId, userInfo.userId,userInfo.nickName,userInfo.iconUrl,userInfo.level,userInfo.vip,userInfo.ext));
                searchInfoList.Add(userInfo);
            }
            friendSearchResp.searchUserInfo = searchInfoList;
            return friendSearchResp;
        }
    }
}
