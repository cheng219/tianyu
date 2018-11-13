using UnityEngine;
using System.Collections;
namespace YunvaIM
{
    public class ImChannelMessageNotify : YunvaMsgBase
    {
        public uint userId;
        public string messageBody;
        public string nickname;
        public string ext1;
        public string ext2;
        public int channel;
        public string wildcard;
        public uint messageType;
        public uint voiceDuration;
        public string attach;
        public uint shield;
        public ImChannelMessageNotify(object Parser)
        {
            uint parser = (uint)Parser;
            userId = YunVaImInterface.parser_get_uint32(parser, 1, 0);
            messageBody = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
            nickname = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 3, 0));
            ext1 = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 4, 0));
            ext2 = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 5, 0));
            channel = YunVaImInterface.parser_get_uint8(parser, 6, 0);
            wildcard = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 7, 0));
            messageType = YunVaImInterface.parser_get_uint32(parser, 8, 0);
            voiceDuration = YunVaImInterface.parser_get_uint32(parser, 9, 0);
            attach = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 10, 0), true);
            shield = YunVaImInterface.parser_get_uint32(parser, 11, 0);

			YunvaLogPrint.YvDebugLog ("ImChannelMessageNotify",string.Format("userId:{0},messageBody:{1},nickname:{2},ext1:{3},ext2:{4},channel:{5},wildcard:{6},messageType:{7},voiceDuration:{8},attach:{9},shield{10}", userId,messageBody,nickname,ext1,ext2,channel,wildcard,messageType,voiceDuration,attach,shield));

            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_CHANNEL_MESSAGE_NOTIFY, this));
        }

    
    }
}
