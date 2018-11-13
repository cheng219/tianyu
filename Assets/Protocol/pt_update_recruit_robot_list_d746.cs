using System.Collections;
using System.Collections.Generic;

public class pt_update_recruit_robot_list_d746 : st.net.NetBase.Pt {
	public pt_update_recruit_robot_list_d746()
	{
		Id = 0xD746;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_recruit_robot_list_d746();
	}
	public List<st.net.NetBase.recruit_robot_list> recruit_robot_list = new List<st.net.NetBase.recruit_robot_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenrecruit_robot_list = reader.Read_ushort();
		recruit_robot_list = new List<st.net.NetBase.recruit_robot_list>();
		for(int i_recruit_robot_list = 0 ; i_recruit_robot_list < lenrecruit_robot_list ; i_recruit_robot_list ++)
		{
			st.net.NetBase.recruit_robot_list listData = new st.net.NetBase.recruit_robot_list();
			listData.fromBinary(reader);
			recruit_robot_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenrecruit_robot_list = (ushort)recruit_robot_list.Count;
		writer.write_short(lenrecruit_robot_list);
		for(int i_recruit_robot_list = 0 ; i_recruit_robot_list < lenrecruit_robot_list ; i_recruit_robot_list ++)
		{
			st.net.NetBase.recruit_robot_list listData = recruit_robot_list[i_recruit_robot_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
