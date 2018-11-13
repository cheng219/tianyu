using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace YunvaIM
{
	public class ImLBSGetSpportLangResp : YunvaMsgBase
	{
		public int result;
		public string msg;
		public List<xLanguage> LanguageList = null;
		public ImLBSGetSpportLangResp(object Parser)
		{
			uint parser = (uint)Parser;
			LanguageList=new List<xLanguage>();
			result = YunVaImInterface.parser_get_integer(parser, 1, 0);
			msg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
			YunvaLogPrint.YvDebugLog ("ImLBSGetSpportLangResp", string.Format ("result:{0},msg:{1}",result,msg));
			for (int i = 0; ; i++)
			{                
				if (YunVaImInterface.parser_is_empty(parser, 3, i))
					break;               
				uint locationInfo = YunVaImInterface.yvpacket_get_nested_parser(parser);              
				YunVaImInterface.parser_get_object(parser, 3, locationInfo, i); 
				xLanguage Language=new xLanguage();
				Language.lang_code= YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(locationInfo, 1, 0));
				Language.lang_name=YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(locationInfo, 2, 0));
				LanguageList.Add(Language);
				YunvaLogPrint.YvDebugLog ("ImLBSGetSpportLangResp", string.Format ("lang_code:{0},lang_name:{1}",Language.lang_code,Language.lang_name));
			}
			YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_LBS_GET_SUPPORT_LANG_RESP, this));
		}
		
	}
	
	public class xLanguage
	{
		public string lang_code;    
		public string lang_name;     
	}
}