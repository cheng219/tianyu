using UnityEngine;
using System.Collections;
using YunvaIM;
namespace YunvaIM
{
    //删除好友回应
    public class ImFriendDelResp:YunvaMsgBase
    {
        public int result;
        public string msg;
        public int del_friend;
        public int act;
        public ImFriendDelResp(object Parser)
        {
            uint parser=(uint)Parser;
            result = YunVaImInterface.parser_get_integer(parser, 1, 0);
            msg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
            del_friend = YunVaImInterface.parser_get_integer(parser, 3, 0);
            act = YunVaImInterface.parser_get_integer(parser, 4, 0);

			YunvaLogPrint.YvDebugLog ("ImFriendDelResp", string.Format ("result:{0},msg:{1},del_friend:{2},act:{3}",result,msg,del_friend,act));

            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_FRIEND_DEL_RESP, this));
        }
    }
}
