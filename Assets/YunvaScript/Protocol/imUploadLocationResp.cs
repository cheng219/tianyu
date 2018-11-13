using UnityEngine;
using System.Collections;
namespace YunvaIM
{
    public class ImUploadLocationResp : YunvaMsgBase
    {

        public int result;
        public string msg;
        public ImUploadLocationResp(object Parser)
        {
            uint parser=(uint)Parser;
           result = YunVaImInterface.parser_get_integer(parser, 1, 0);
            msg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
			YunvaLogPrint.YvDebugLog ("ImUploadLocationResp", string.Format ("result:{0},msg:{1}",result,msg));
            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_UPLOAD_LOCATION_RESP, this));
        }
    }
}
