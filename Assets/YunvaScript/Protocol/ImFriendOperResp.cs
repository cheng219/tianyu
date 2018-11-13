using UnityEngine;
using System.Collections;

namespace YunvaIM
{
    public class ImFriendOperResp:YunvaMsgBase
    {
        public int userId;//用户ID
        public int operId;//操作ID
        public int act;//动作
        public int oper_state;//对方状态
        public ImFriendOperResp(object Parser)
        {
            uint parser = (uint)Parser;
            userId =YunVaImInterface.parser_get_integer(parser, 1, 0);
            operId = YunVaImInterface.parser_get_integer(parser, 2, 0);
            act = YunVaImInterface.parser_get_integer(parser, 3, 0);
            oper_state = YunVaImInterface.parser_get_integer(parser, 4, 0);

			YunvaLogPrint.YvDebugLog ("ImFriendOperResp", string.Format ("userId:{0},operId:{1},act:{2},oper_state:{3}",userId,operId,act,oper_state));

            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_FRIEND_OPER_RESP, this));
        }
    }
}
