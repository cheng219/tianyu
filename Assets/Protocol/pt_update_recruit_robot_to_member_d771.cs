using System.Collections;
using System.Collections.Generic;

public class pt_update_recruit_robot_to_member_d771 : st.net.NetBase.Pt {
	public pt_update_recruit_robot_to_member_d771()
	{
		Id = 0xD771;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_recruit_robot_to_member_d771();
	}
	public List<int> recruit_uid_list = new List<int>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenrecruit_uid_list = reader.Read_ushort();
		recruit_uid_list = new List<int>();
		for(int i_recruit_uid_list = 0 ; i_recruit_uid_list < lenrecruit_uid_list ; i_recruit_uid_list ++)
		{
			int listData = reader.Read_int();
			recruit_uid_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenrecruit_uid_list = (ushort)recruit_uid_list.Count;
		writer.write_short(lenrecruit_uid_list);
		for(int i_recruit_uid_list = 0 ; i_recruit_uid_list < lenrecruit_uid_list ; i_recruit_uid_list ++)
		{
			int listData = recruit_uid_list[i_recruit_uid_list];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
