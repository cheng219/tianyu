using UnityEngine;
using System.Collections;
namespace YunvaIM
{
    public class ImChannelSendMsgResp : YunvaMsgBase
    {
        public int result;
        public string msg;
        public int type;
		public string wildCard;
		public string textMsg;
		public string url;
		public int voiceDurationTime;
		public string expand;
		public int shield;
		public int channel;
		public string flag;

        public ImChannelSendMsgResp(object Parser)
        {
            uint parser = (uint)Parser;

			result = YunVaImInterface.parser_get_integer(parser, 1, 0);
			msg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
		    type = YunVaImInterface.parser_get_uint8(parser, 3, 0);
			wildCard = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 4, 0));
			textMsg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 5, 0));
			url = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 6, 0));
			voiceDurationTime = YunVaImInterface.parser_get_integer(parser, 7, 0);
			expand = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 8, 0));
			shield = YunVaImInterface.parser_get_uint8(parser, 9, 0);
			channel = YunVaImInterface.parser_get_uint8(parser, 10, 0);
			flag = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 11, 0));

			YunvaLogPrint.YvDebugLog ("ImChannelSendMsgResp",string.Format("result:{0},msg:{1},type:{2},wildCard:{3},textMsg:{4},url:{5},voiceDurationTime:{6},expand:{7},shield:{8},channel:{9},flag{10}", result,msg,type,wildCard,textMsg,url,voiceDurationTime,expand,shield,channel,flag));
            //SendChannelMsgResp resp = new SendChannelMsgResp() {
            //    result = (int)result,
            //    msg = (string)msg,
            //    type = (int)type
            //};

            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_CHANNEL_SENDMSG_RESP, this));
        }
    }
}
