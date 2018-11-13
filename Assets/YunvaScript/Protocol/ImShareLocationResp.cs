using UnityEngine;
using System.Collections;
namespace YunvaIM
{
	public class ImShareLocationResp : YunvaMsgBase
	{
		public int result;
		public string msg;

		public ImShareLocationResp(object Parser)
		{
			uint parser = (uint)Parser;
			result = YunVaImInterface.parser_get_integer(parser, 1, 0);
			msg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
			YunvaLogPrint.YvDebugLog ("ImShareLocationResp", string.Format ("result:{0},msg:{1}",result,msg));
			YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_SHARE_LOCATION_RESP, this));
		}
	}
}
