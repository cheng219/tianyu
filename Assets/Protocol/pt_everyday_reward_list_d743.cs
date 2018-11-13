using System.Collections;
using System.Collections.Generic;

public class pt_everyday_reward_list_d743 : st.net.NetBase.Pt {
	public pt_everyday_reward_list_d743()
	{
		Id = 0xD743;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_everyday_reward_list_d743();
	}
	public List<st.net.NetBase.reward_list> everyday_reward_list = new List<st.net.NetBase.reward_list>();
	public int can_get_id;
	public int login_state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort leneveryday_reward_list = reader.Read_ushort();
		everyday_reward_list = new List<st.net.NetBase.reward_list>();
		for(int i_everyday_reward_list = 0 ; i_everyday_reward_list < leneveryday_reward_list ; i_everyday_reward_list ++)
		{
			st.net.NetBase.reward_list listData = new st.net.NetBase.reward_list();
			listData.fromBinary(reader);
			everyday_reward_list.Add(listData);
		}
		can_get_id = reader.Read_int();
		login_state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort leneveryday_reward_list = (ushort)everyday_reward_list.Count;
		writer.write_short(leneveryday_reward_list);
		for(int i_everyday_reward_list = 0 ; i_everyday_reward_list < leneveryday_reward_list ; i_everyday_reward_list ++)
		{
			st.net.NetBase.reward_list listData = everyday_reward_list[i_everyday_reward_list];
			listData.toBinary(writer);
		}
		writer.write_int(can_get_id);
		writer.write_int(login_state);
		return writer.data;
	}

}
