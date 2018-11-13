using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace YunvaIM
{
    //云消息通知
    public class ImCloudMsgNotify : YunvaMsgBase
    {
        public string source;//来源(SYSTEM 系统消息 PUSH 推送消息 userId 好友消息)
        public int id;//若是好友消息, 则为好友ID
        public int beginid;//开始索引
        public int endid;//结束索引
        public int time; //结束索引时间
        public List<P2PChatMsg> packet = new List<P2PChatMsg>();//结束索引内容  xP2PChatMsg,  xGroupChatMsg
        public int unread;	//未读消息数
        public int cloudId;
        public string cloudResource;
        public ImCloudMsgNotify() { }
        public ImCloudMsgNotify(object Parser)
        {
            uint parser = (uint)Parser;
            if (YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 1, 0)) == "P2P")
            {
                source = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 1, 0));
                id = YunVaImInterface.parser_get_integer(parser, 2, 0);
                beginid = YunVaImInterface.parser_get_integer(parser, 3, 0);
                endid = YunVaImInterface.parser_get_integer(parser, 4, 0);
                time = YunVaImInterface.parser_get_integer(parser, 5, 0);
				uint p2pParser = YunVaImInterface.yvpacket_get_nested_parser(parser);
                YunVaImInterface.parser_get_object(parser, 6, p2pParser, 0);
                packet.Add(chatMessageNotify(p2pParser));
                unread = YunVaImInterface.parser_get_integer(parser, 7, 0);
                cloudId = YunVaImInterface.parser_get_integer(parser, 110, 0);
                cloudResource = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 111, 0));

				YunvaLogPrint.YvInfoLog ("ImCloudMsgNotify",string.Format("source:{0},id:{1},beginid:{2},endid:{3},time:{4},unread:{5},cloudId:{6},cloudResource:{7}",  source, id,beginid,endid,time,unread,cloudId,cloudResource));

                YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_CLOUDMSG_NOTIFY, this));
            }
        }
        private P2PChatMsg chatMessageNotify(uint parser)
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

			YunvaLogPrint.YvInfoLog ("ImCloudMsgNotify",string.Format("userID:{0},name:{1},signature:{2},headUrl:{3},sendTime:{4},type:{5},data:{6},imageUrl:{7},audioTime:{8},attach:{9},ext1:{10}",  p2pChatMsg.userID, p2pChatMsg.name,p2pChatMsg.signature, p2pChatMsg.headUrl,p2pChatMsg.sendTime,p2pChatMsg.type,p2pChatMsg.data,p2pChatMsg.imageUrl,p2pChatMsg.audioTime,p2pChatMsg.attach,p2pChatMsg.ext1));

            p2pChatMsg.cloudMsgID = YunVaImInterface.parser_get_integer(parser, 110, 0);
            p2pChatMsg.cloudResource = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 111, 0));
            return p2pChatMsg;
        }



    }
}
