using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace YunvaIM
{
    public class ImCloudmsgLimitNotify : YunvaMsgBase
    {
		public string source;//来源(SYSTEM 系统消息 PUSH 推送消息 userId 好友消息)
		public uint p2pId;//若是好友消息, 则为好友ID
		public uint count;//消息数
		public uint indexId;//当前消息索引
		public uint ptime;//当前消息时间
		public P2PChatMsg packet;//结束索引内容 xP2PChatMsg,  xGroupChatMsg
        public List<P2PChatMsg> pacektList;
        public ImCloudmsgLimitNotify(object Parser)
        {
            uint parser = (uint)Parser;
            pacektList = new List<P2PChatMsg>();
            source = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 1, 0));
            p2pId = YunVaImInterface.parser_get_uint32(parser, 2, 0);
            count = YunVaImInterface.parser_get_uint32(parser, 3, 0);
            indexId = YunVaImInterface.parser_get_uint32(parser, 4, 0);
            ptime = YunVaImInterface.parser_get_uint32(parser, 5, 0);
			YunvaLogPrint.YvDebugLog ("ImCloudmsgLimitNotify",string.Format("source:{0},p2pId:{1},count:{2},indexId:{3},ptime:{4}", source,p2pId,count,indexId,ptime));
            if (source == "P2P")
            {
                uint p2pParser = YunVaImInterface.yvpacket_get_nested_parser(parser);
                YunVaImInterface.parser_get_object(parser, 6, p2pParser, 0);

                for (int i = 0; ; i++)
                {
                    if (YunVaImInterface.parser_is_empty(parser, 6, i))
                        break;
                    uint parserInfo = YunVaImInterface.yvpacket_get_nested_parser(parser);
                    YunVaImInterface.parser_get_object(parser, 6, parserInfo, i);
                    pacektList.Add(chatMessageNotify(parserInfo));
                }

                //		packet = chatMessageNotify(p2pParser);
       //         packet.cloudMsgID = YunVaImInterface.parser_get_integer(parser, 110, 0);
       //         packet.cloudResource = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 111, 0));
                YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_CLOUDMSG_LIMIT_NOTIFY, this));
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
			p2pChatMsg.index = YunVaImInterface.parser_get_integer(parser, 12, 0);

			YunvaLogPrint.YvDebugLog ("ImCloudmsgLimitNotify",string.Format("userID:{0},name:{1},signature:{2},headUrl:{3},sendTime:{4},type:{5},data:{6},imageUrl:{7},audioTime:{8},attach:{9},ext1:{10},index:{11}",  p2pChatMsg.userID, p2pChatMsg.name,p2pChatMsg.signature, p2pChatMsg.headUrl,p2pChatMsg.sendTime,p2pChatMsg.type,p2pChatMsg.data,p2pChatMsg.imageUrl,p2pChatMsg.audioTime,p2pChatMsg.attach,p2pChatMsg.ext1,p2pChatMsg.index));
            p2pChatMsg.cloudMsgID = YunVaImInterface.parser_get_integer(parser, 110, 0);
            p2pChatMsg.cloudResource = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 111, 0));
            return p2pChatMsg;
        }
    }
}
