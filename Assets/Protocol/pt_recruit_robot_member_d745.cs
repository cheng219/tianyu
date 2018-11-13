using System.Collections;
using System.Collections.Generic;

public class pt_recruit_robot_member_d745 : st.net.NetBase.Pt {
	public pt_recruit_robot_member_d745()
	{
		Id = 0xD745;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_recruit_robot_member_d745();
	}
	public int add_or_remove;
	public List<int> recruit_robot_member = new List<int>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		add_or_remove = reader.Read_int();
		ushort lenrecruit_robot_member = reader.Read_ushort();
		recruit_robot_member = new List<int>();
		for(int i_recruit_robot_member = 0 ; i_recruit_robot_member < lenrecruit_robot_member ; i_recruit_robot_member ++)
		{
			int listData = reader.Read_int();
			recruit_robot_member.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(add_or_remove);
		ushort lenrecruit_robot_member = (ushort)recruit_robot_member.Count;
		writer.write_short(lenrecruit_robot_member);
		for(int i_recruit_robot_member = 0 ; i_recruit_robot_member < lenrecruit_robot_member ; i_recruit_robot_member ++)
		{
			int listData = recruit_robot_member[i_recruit_robot_member];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
