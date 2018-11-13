using UnityEngine;
using System.Collections;
using YunvaIM;
namespace YunvaIM
{
    public class ImFriendDelNotify : YunvaMsgBase
    {
        public int del_friend;
        public int del_fromlist;
        public ImFriendDelNotify(object Parser)
        {
            uint parser = (uint)Parser;
            del_friend =YunVaImInterface. parser_get_integer(parser, 1, 0);
            del_fromlist = YunVaImInterface.parser_get_integer(parser, 2, 0);

			YunvaLogPrint.YvDebugLog ("ImFriendDelNotify", string.Format ("del_friend:{0},del_fromlist:{1}",del_friend,del_fromlist));

            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_FRIEND_DEL_NOTIFY, this));
        }

    }
}
