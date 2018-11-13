using UnityEngine;
using System.Collections;
namespace YunvaIM
{
	public class ImLBSSetLocatingTypeResp : YunvaMsgBase
	{
		public int result;
		public string msg;

		public ImLBSSetLocatingTypeResp(object Parser)
		{
			uint parser = (uint)Parser;
			result = YunVaImInterface.parser_get_integer(parser, 1, 0);
			msg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
			YunvaLogPrint.YvDebugLog ("ImLBSSetLocatingTypeResp", string.Format ("result:{0},msg:{1}",result,msg));
			YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_LBS_SET_LOCATING_TYPE_RESP, this));
		}
	}
}
