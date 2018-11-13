using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace YunvaIM
{
    public class ImFriendNearListNotify : YunvaMsgBase
    {
        public List<RecentConact> recentList;
        public ImFriendNearListNotify(object Parse)
        {     
          
            recentList = GetRecentConactList(Parse);
            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_FRIEND_NEARLIST_NOTIFY, this));
        }
        private List<RecentConact> GetRecentConactList(object Parser, byte cmdId = 1)
        {
            uint parser = (uint)Parser;          
            List<RecentConact> recentList = new List<RecentConact>();
            for (int i = 0; ; i++)
            {                
                if (YunVaImInterface.parser_is_empty(parser, cmdId, i))
                    break;               
				uint parserUerInfo = YunVaImInterface.yvpacket_get_nested_parser(parser);              
                YunVaImInterface.parser_get_object(parser, 1, parserUerInfo, i);             
                RecentConact context = new RecentConact();             
                context.endId = YunVaImInterface.parser_get_integer(parserUerInfo, 1, 0);               
                context.unread = YunVaImInterface.parser_get_integer(parserUerInfo, 2, 0);
				YunvaLogPrint.YvInfoLog ("ImFriendNearListNotify", string.Format ("endId:{0},unread:{1}", context.endId,context.unread));
				uint chatParser = YunVaImInterface.yvpacket_get_nested_parser(parser);  
                YunVaImInterface.parser_get_object(parserUerInfo, 3, chatParser, 0);            
               context.lastMsg = chatMessageNotify(chatParser);
				uint nearChatParser = YunVaImInterface.yvpacket_get_nested_parser(parser);
               YunVaImInterface.parser_get_object(parserUerInfo, 4, nearChatParser, 0);
                NearChatInfo nearChatInfo = new NearChatInfo();
                nearChatInfo.nickName = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(nearChatParser, 1, 0));
                nearChatInfo.userId = YunVaImInterface.parser_get_integer(nearChatParser, 2, 0);
                nearChatInfo.iconUrl = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(nearChatParser, 3, 0));
                nearChatInfo.online = YunVaImInterface.parser_get_integer(nearChatParser, 4, 0);
                nearChatInfo.userLevel = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(nearChatParser, 5, 0));
                nearChatInfo.vipLevel = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(nearChatParser, 6, 0));
                nearChatInfo.ext = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(nearChatParser, 7, 0));
                nearChatInfo.shieldMsg = YunVaImInterface.parser_get_integer(nearChatParser, 8, 0);
                nearChatInfo.sex = YunVaImInterface.parser_get_integer(nearChatParser, 9, 0);
                nearChatInfo.group = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(nearChatParser, 10, 0));
                nearChatInfo.remark = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(nearChatParser, 11, 0));
                nearChatInfo.times = YunVaImInterface.parser_get_integer(nearChatParser, 12, 0);
				YunvaLogPrint.YvInfoLog ("ImFriendNearListNotify",string.Format("nickName:{0},userId:{1},iconUrl:{2},onLine:{3},userLevel:{4},vipLevel:{5},ext:{6},shieldmsg:{7},sex:{8},group:{9},remark:{10},times:{11}", nearChatInfo.nickName,  nearChatInfo.userId,nearChatInfo.iconUrl, nearChatInfo.online,nearChatInfo.userLevel, nearChatInfo.vipLevel, nearChatInfo.ext, nearChatInfo.shieldMsg, nearChatInfo.sex,nearChatInfo.group,nearChatInfo.remark,nearChatInfo.times));
                context.userInfo = nearChatInfo;
                recentList.Add(context);

            }
           
            return recentList;
        }
        private  P2PChatMsg chatMessageNotify(uint parser)
        {
            P2PChatMsg p2pChatMsg = new P2PChatMsg();
            p2pChatMsg.userID = YunVaImInterface.parser_get_integer(parser, 1, 0);
            p2pChatMsg.name = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
            p2pChatMsg.signature = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 3, 0));
            p2pChatMsg.headUrl = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 4, 0));
            p2pChatMsg.sendTime = YunVaImInterface.parser_get_integer(parser, 5, 0);
            p2pChatMsg.type = YunVaImInterface.parser_get_integer(parser, 6, 0);
            p2pChatMsg.data = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 7, 0));
            p2pChatMsg.imageUrl = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 8, 0));
            p2pChatMsg.audioTime = YunVaImInterface.parser_get_integer(parser, 9, 0);
            p2pChatMsg.attach = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 10, 0));
            p2pChatMsg.ext1 = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 11, 0));
			YunvaLogPrint.YvInfoLog ("ImFriendNearListNotify",string.Format("userID:{0},name:{1},signature:{2},headUrl:{3},sendTime:{4},type:{5},data:{6},imageUrl:{7},audioTime:{8},attach:{9},ext1{10}",  p2pChatMsg.userID, p2pChatMsg.name,p2pChatMsg.signature, p2pChatMsg.headUrl,p2pChatMsg.sendTime,p2pChatMsg.type,p2pChatMsg.data,p2pChatMsg.imageUrl,p2pChatMsg.audioTime,p2pChatMsg.attach,p2pChatMsg.ext1));
            p2pChatMsg.cloudMsgID = YunVaImInterface.parser_get_integer(parser, 110, 0);
            p2pChatMsg.cloudResource = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 111, 0));
            return p2pChatMsg;
        }
    }
}
