using System.Collections;
using System.Collections.Generic;

public class pt_rewards_return_d122 : st.net.NetBase.Pt {
	public pt_rewards_return_d122()
	{
		Id = 0xD122;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_rewards_return_d122();
	}
	public int rewards_type;
	public int rewards_state;
	public List<st.net.NetBase.rewards_receive_list> rewards_receive_list = new List<st.net.NetBase.rewards_receive_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		rewards_type = reader.Read_int();
		rewards_state = reader.Read_int();
		ushort lenrewards_receive_list = reader.Read_ushort();
		rewards_receive_list = new List<st.net.NetBase.rewards_receive_list>();
		for(int i_rewards_receive_list = 0 ; i_rewards_receive_list < lenrewards_receive_list ; i_rewards_receive_list ++)
		{
			st.net.NetBase.rewards_receive_list listData = new st.net.NetBase.rewards_receive_list();
			listData.fromBinary(reader);
			rewards_receive_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(rewards_type);
		writer.write_int(rewards_state);
		ushort lenrewards_receive_list = (ushort)rewards_receive_list.Count;
		writer.write_short(lenrewards_receive_list);
		for(int i_rewards_receive_list = 0 ; i_rewards_receive_list < lenrewards_receive_list ; i_rewards_receive_list ++)
		{
			st.net.NetBase.rewards_receive_list listData = rewards_receive_list[i_rewards_receive_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
