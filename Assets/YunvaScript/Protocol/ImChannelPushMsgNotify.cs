using UnityEngine;
using System.Collections;
namespace YunvaIM
{
	public class ImChannelPushMsgNotify : YunvaMsgBase
	{
		public string type;
		public string data;
		public ImChannelPushMsgNotify(object Parser)
		{
			uint parser = (uint)Parser;

			type = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 1, 0));
			data = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));

			YunvaLogPrint.YvDebugLog ("ImChannelPushMsgNotify", string.Format ("type:{0},data:{1}", type,data));

			YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_CHANNEL_PUSH_MSG_NOTIFY, this));
		}	
	}
}