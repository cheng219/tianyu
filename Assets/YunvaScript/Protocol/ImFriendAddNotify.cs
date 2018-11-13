using UnityEngine;
using System.Collections;
using YunvaIM;
namespace YunvaIM
{
    //好友请求通知 
    public class ImFriendAddNotify:YunvaMsgBase
    {
        public int userId;
        public string name;
        public string greet;
        public string sign;
        public string url;
        public Texture2D texture;
        public int cloudMsgId;
        public string cloudMsgSource;
        public  ImFriendAddNotify(object Parser)
        {
            uint parser = (uint)Parser;  
            userId =YunVaImInterface. parser_get_integer(parser, 1, 0);
            greet = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
            name = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 3, 0));
            sign = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 4, 0));
            url = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 5, 0));
            cloudMsgId = YunVaImInterface.parser_get_integer(parser, 110, 0);
            cloudMsgSource = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 111, 0));    

			YunvaLogPrint.YvDebugLog ("ImFriendAddNotify", string.Format ("userId:{0},greet:{1},name:{2},sign:{3},url:{4}",userId,greet,name,sign,url));

            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_FRIEND_ADD_NOTIFY, this));
        }
    }
}
