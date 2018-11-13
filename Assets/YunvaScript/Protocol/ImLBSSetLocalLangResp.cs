using UnityEngine;
using System.Collections;
namespace YunvaIM
{
	public class ImLBSSetLocalLangResp : YunvaMsgBase
	{
		public int result;
		public string msg;
		public string lang_code;

		public ImLBSSetLocalLangResp(object Parser)
		{
			uint parser = (uint)Parser;
			result = YunVaImInterface.parser_get_integer(parser, 1, 0);
			msg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
			lang_code = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 3, 0));
			YunvaLogPrint.YvDebugLog ("ImLBSSetLocalLangResp", string.Format ("result:{0},msg:{1},lang_code:{2}",result,msg,lang_code));
		    YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_LBS_SET_LOCAL_LANG_RESP, this));
		}
	}
}
