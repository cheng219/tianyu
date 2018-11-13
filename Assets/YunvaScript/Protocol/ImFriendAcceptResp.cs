using UnityEngine;
using System.Collections;
using YunvaIM;
namespace YunvaIM
{
    public class ImFriendAcceptResp:YunvaMsgBase
    {
        public int userId;
        public int affirm;
        public string greet;
        public string name;
        public string iconUrl;
        public ImFriendAcceptResp(object Parser)
        {
            uint parser = (uint)Parser;
            userId = YunVaImInterface.parser_get_integer(parser, 1, 0);
            affirm = YunVaImInterface.parser_get_integer(parser, 2, 0);
            greet = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 3, 0));

			YunvaLogPrint.YvDebugLog ("ImFriendAcceptResp", string.Format ("userId:{0},affirm:{1},greet:{2}",userId,affirm,greet));

            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_FRIEND_ACCEPT_RESP, this));
        }
        public ImFriendAcceptResp() { }
    }
}
