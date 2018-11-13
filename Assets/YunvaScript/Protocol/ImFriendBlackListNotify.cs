using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace YunvaIM
{
    public class ImFriendBlackListNotify : YunvaMsgBase
    {
        public List<xUserInfo> userInfoList;
        public ImFriendBlackListNotify(object Parser)
        {
            uint parser = (uint)Parser;
            userInfoList = new List<xUserInfo>();
            userInfoList = InsertUserInfo(parser);
            InvokeEventClass myFriendList = new InvokeEventClass(ProtocolEnum.IM_FRIEND_BLACKLIST_NOTIFY, this);
            YunVaImInterface.eventQueue.Enqueue(myFriendList);
        }

        private List<xUserInfo> InsertUserInfo(uint parser, byte cmdId = 1)
        {
            List<xUserInfo> userInfoList = new List<xUserInfo>();
            for (int i = 0; ; i++)
            {

                if (YunVaImInterface.parser_is_empty(parser, cmdId, i))
                    break;

                uint parserUerInfo = YunVaImInterface.yvpacket_get_nested_parser(parser);
                YunVaImInterface.parser_get_object(parser, cmdId, parserUerInfo, i);
                xUserInfo userInfo = new xUserInfo();
                userInfo.nickName = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parserUerInfo, 1, 0));
                userInfo.userId = YunVaImInterface.parser_get_integer(parserUerInfo, 2, 0);
                userInfo.iconUrl = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parserUerInfo, 3, 0));
                userInfo.onLine = YunVaImInterface.parser_get_integer(parserUerInfo, 4, 0);
                userInfo.userLevel = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parserUerInfo, 5, 0));
                userInfo.vipLevel = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parserUerInfo, 6, 0));
                userInfo.ext = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parserUerInfo, 7, 0));
                userInfo.shieldmsg = YunVaImInterface.parser_get_integer(parserUerInfo, 8, 0);
                userInfo.sex = YunVaImInterface.parser_get_integer(parserUerInfo, 9, 0);
                userInfo.group = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parserUerInfo, 10, 0));
                userInfo.remark = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parserUerInfo, 11, 0));
				YunvaLogPrint.YvInfoLog ("ImFriendBlackListNotify",string.Format("nickName:{0},userId:{1},iconUrl:{2},onLine:{3},userLevel:{4},vipLevel:{5},ext:{6},shieldmsg:{7},sex:{8},group:{9},remark:{10}", userInfo.nickName,  userInfo.userId,userInfo.iconUrl, userInfo.onLine,userInfo.userLevel, userInfo.vipLevel, userInfo.ext, userInfo.shieldmsg, userInfo.sex,userInfo.group,userInfo.remark));
                userInfoList.Add(userInfo);
            }
            return userInfoList;
        }
    }
}
