using UnityEngine;
using System.Collections;
namespace YunvaIM
{
    public class ImChatFriendResp : YunvaMsgBase
    {
        public int result;
        public string msg;
		public int type;
		public int userId;
        public string flag;
		public string indexId; //index of msg;
		public string text;
		public string audiourl;
		public int audiotime;
		public string imageurl1;
		public string imageurl2;
		public string ext1;


        public ImChatFriendResp(object Parser)
        {
            uint parser = (uint)Parser;

            result = YunVaImInterface.parser_get_integer(parser, 1, 0);
			msg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
			type = YunVaImInterface.parser_get_integer(parser, 3, 0);
			userId = YunVaImInterface.parser_get_integer(parser, 4, 0);
			flag = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 5, 0));
			indexId = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 6, 0));
			text = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 7, 0));
			audiourl = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 8, 0));
			audiotime = YunVaImInterface.parser_get_integer(parser, 9, 0);
			imageurl1 = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 10, 0));
			imageurl2 = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 11, 0));
			ext1 = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 13, 0));

			YunvaLogPrint.YvDebugLog ("ImChatFriendResp",string.Format("result:{0},msg:{1},type:{2},userId:{3},flag:{4},indexId:{5},text:{6},audiourl:{7},audiotime:{8},imageurl1:{9},imageurl2{10},ext1:{11}", result,msg,type,userId,flag,indexId,text,audiourl,audiotime,imageurl1,imageurl2,ext1));

//			ArrayList list = new ArrayList();
//			list.Add(type);
//			list.Add(result);

            //SendFriendMsgResp resp = new SendFriendMsgResp(){
            //    type = (int)type,
            //    result = (int)result
            //};

            YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_CHAT_FRIEND_RESP, this));
        }
    }
}