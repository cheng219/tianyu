using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace YunvaIM
{
    public class ImChannelHistoryMsgResp : YunvaMsgBase
    {
        public List<HistoryMsgInfo> channelHisList;
        public ImChannelHistoryMsgResp(object Parser)
        {
            uint parser = (uint)Parser;
            channelHisList = new List<HistoryMsgInfo>();
            int index = 0;
            while (!YunVaImInterface.parser_is_empty(parser, 1, index))
            {
                uint hisParser = YunVaImInterface.yvpacket_get_nested_parser(parser);
                YunVaImInterface.parser_get_object(parser, 1, hisParser, index);
                HistoryMsgInfo hisChannel = GetChannelMessageNotify(hisParser);
                channelHisList.Add(hisChannel);
                index++;
            }
            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_CHANNEL_HISTORY_MSG_RESP, this));
        }
        public static HistoryMsgInfo GetChannelMessageNotify(uint parser)
        {
            HistoryMsgInfo channelMsg = new HistoryMsgInfo();
            channelMsg.index = YunVaImInterface.parser_get_uint32(parser, 1, 0);
            channelMsg.ctime = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
            channelMsg.userId = YunVaImInterface.parser_get_uint32(parser, 3, 0);
            channelMsg.messageBody = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 4, 0));
            channelMsg.nickName = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 5, 0));
            channelMsg.ext1 = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 6, 0));
            channelMsg.ext2 = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 7, 0));
            channelMsg.channel = YunVaImInterface.parser_get_uint8(parser, 8, 0);
            channelMsg.wildCard = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 9, 0));
            channelMsg.messageType = YunVaImInterface.parser_get_uint32(parser, 10, 0);
            channelMsg.voiceDuration = YunVaImInterface.parser_get_uint32(parser, 11, 0);
            channelMsg.attach = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 12, 0), true);

			YunvaLogPrint.YvDebugLog ("ImChannelHistoryMsgResp",string.Format("index:{0},ctime:{1},userId:{2},messageBody:{3},nickName:{4},ext1:{5},ext2:{6},channel:{7},wildCard:{8},messageType:{9},voiceDuration:{10},attach:{11}", channelMsg.index, channelMsg.ctime,channelMsg.userId,channelMsg.messageBody,channelMsg.nickName,channelMsg.ext1, channelMsg.ext2,channelMsg.channel, channelMsg.wildCard,channelMsg.messageType,channelMsg.voiceDuration,channelMsg.attach));

            return channelMsg;
        }
    }

}
