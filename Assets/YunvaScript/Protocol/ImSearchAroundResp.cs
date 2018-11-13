using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace YunvaIM
{
    public class ImSearchAroundResp : YunvaMsgBase
    {
        public int result;
        public string msg;
        public List<xAroundUser> aroundUserList = null;
        public ImSearchAroundResp(object Parser)
        {
            uint parser = (uint)Parser;
            aroundUserList=new List<xAroundUser>();
            result = YunVaImInterface.parser_get_integer(parser, 1, 0);
            msg = YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(parser, 2, 0));
			YunvaLogPrint.YvDebugLog ("ImSearchAroundResp", string.Format ("result:{0},msg:{1}",result,msg));
            for (int i = 0; ; i++)
            {                
                if (YunVaImInterface.parser_is_empty(parser, 3, i))
                    break;               
				uint locationInfo = YunVaImInterface.yvpacket_get_nested_parser(parser);              
                YunVaImInterface.parser_get_object(parser, 3, locationInfo, i); 
                xAroundUser info=new xAroundUser();
                info.yunvaId=YunVaImInterface.parser_get_integer(locationInfo, 1, 0);
                info.nickName= YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(locationInfo, 2, 0));
				info.sex=YunVaImInterface.parser_get_integer(locationInfo, 3, 0);
                info.city=YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(locationInfo, 4, 0));
				info.headicon=YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(locationInfo, 5, 0));
                info.distance=YunVaImInterface.parser_get_integer(locationInfo, 6, 0);
				info.lately=YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(locationInfo, 7, 0));
				info.longitude=YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(locationInfo, 8, 0));
				info.latitude=YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(locationInfo, 9, 0));
				info.ext=YunVaImInterface.IntPtrToString(YunVaImInterface.parser_get_string(locationInfo, 10, 0));
				YunvaLogPrint.YvDebugLog ("ImSearchAroundResp",string.Format("yunvaId:{0},nickName:{1},sex:{2},city:{3},headicon:{4},distance:{5},lately:{6},longitude:{7},latitude:{8},ext:{9}", info.yunvaId, info.nickName,info.sex,info.city,info.headicon,info.distance,info.lately,info.longitude,info.latitude,info.ext));
                aroundUserList.Add(info);
            }
             YunVaImInterface.eventQueue.Enqueue(new InvokeEventClass(ProtocolEnum.IM_SEARCH_AROUND_RESP, this));
        }

    }

    public class xAroundUser
    {
        public int yunvaId;         //用户ID
        public string nickName;     //昵称
        public int sex;             //性别
		public string city;         //所在城市
		public string headicon;     //头像地址
        public int distance;        //距离，单位：米
		public string lately;       //最近活跃时间
		public string longitude;    //经度
		public string latitude;     //纬度
		public string ext;          //扩展字段
    }
}