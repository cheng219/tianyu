using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace YunvaIM
{
	public class ImChannelMotifyResp : YunvaMsgBase
	{
		public int result;
		public string msg;
		public List<string> wildcard;
		public ImChannelMotifyResp(object Parser)
		{
			uint parser=(uint)Parser;
			wildcard = new List<string> ();
			result = YunVaImInterface.parser_get_integer(parser, 1, 0);
			msg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
			YunvaLogPrint.YvDebugLog ("ImChannelMotifyResp", string.Format ("result:{0},msg:{1}", result,msg));
			for (int i = 0;; i++) 
			{
				if (YunVaImInterface.parser_is_empty (parser, 3, i))
					break;
				wildcard.Add(YunVaImInterface.IntPtrToString (YunVaImInterface.parser_get_string (parser, 3, i)));
				YunvaLogPrint.YvDebugLog ("ImChannelMotifyResp", string.Format ("wildcard:{0}", wildcard[i]));
			}
			YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_CHANNEL_MODIFY_RESP, this));
		}
		
	}
}