using UnityEngine;
using System.Collections;
namespace YunvaIM
{
    public class ImRemoveContactesResp : YunvaMsgBase
    {
        public int result;
        public string msg;
        public int userId;
        public ImRemoveContactesResp(object Parser)
        {
            uint parser = (uint)Parser;
            result = YunVaImInterface.parser_get_integer(parser, 1, 0);
            msg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
            userId = YunVaImInterface.parser_get_integer(parser, 3, 0);
			YunvaLogPrint.YvDebugLog ("ImRemoveContactesResp", string.Format ("result:{0},msg:{1},userId:{2}",result,msg,userId));
            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_REMOVE_CONTACTES_RESP, this));
        }
    }
}
