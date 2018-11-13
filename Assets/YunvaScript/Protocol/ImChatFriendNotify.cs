using UnityEngine;
using System.Collections;
namespace YunvaIM
{
    public class ImChatFriendNotify : YunvaMsgBase
    {
        
        public P2PChatMsg chatMsg = null;
        int cloudID;
        string source;
        public ImChatFriendNotify(object Parser)
        {
            uint parser = (uint)Parser;
            chatMsg = new P2PChatMsg();
            chatMsg = chatMessageNotify(parser);
            cloudID =YunVaImInterface. parser_get_integer(parser, 110, 0);
            source = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 111, 0));
            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_CHAT_FRIEND_NOTIFY, this));
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

            p2pChatMsg.cloudMsgID = YunVaImInterface.parser_get_integer(parser, 110, 0);
            p2pChatMsg.cloudResource = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 111, 0));

			YunvaLogPrint.YvDebugLog ("ImChatFriendNotify",string.Format("userID:{0},name:{1},signature:{2},headUrl:{3},sendTime:{4},type:{5},data:{6},imageUrl:{7},audioTime:{8},attach:{9},ext1{10}",  p2pChatMsg.userID, p2pChatMsg.name,p2pChatMsg.signature, p2pChatMsg.headUrl,p2pChatMsg.sendTime,p2pChatMsg.type,p2pChatMsg.data,p2pChatMsg.imageUrl,p2pChatMsg.audioTime,p2pChatMsg.attach,p2pChatMsg.ext1));
            return p2pChatMsg;
        }
    }
}
