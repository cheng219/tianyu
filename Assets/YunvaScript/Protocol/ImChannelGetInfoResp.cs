using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace YunvaIM
{
    public class ImChannelGetInfoResp : YunvaMsgBase
    {
		public 	List<string>  Game_channel;
        public ImChannelGetInfoResp(object Parser)
        {
            uint parser = (uint)Parser; 
			for (int i = 0;; i++) 
			{
				if (YunVaImInterface.parser_is_empty (parser, 1, i))
						break;
				uint GetInfo = YunVaImInterface.yvpacket_get_nested_parser (parser);
				YunVaImInterface.parser_get_object (parser, 1, GetInfo, i);
				Game_channel.Add (YunVaImInterface.IntPtrToString (YunVaImInterface.parser_get_string (GetInfo, 1, 0)));
				YunvaLogPrint.YvDebugLog ("ImChannelGetInfoResp", string.Format ("Game_channel:{0}", Game_channel[i]));
			}
            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_CHANNEL_GETINFO_RESP, this));
        }
    }
}
