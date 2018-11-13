using System.Collections;
using System.Collections.Generic;

public class pt_offline_reward_info_c118 : st.net.NetBase.Pt {
	public pt_offline_reward_info_c118()
	{
		Id = 0xC118;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_offline_reward_info_c118();
	}
	public int start_time;
	public int amount_time;
	public List<st.net.NetBase.reward_list> offline_reward = new List<st.net.NetBase.reward_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		start_time = reader.Read_int();
		amount_time = reader.Read_int();
		ushort lenoffline_reward = reader.Read_ushort();
		offline_reward = new List<st.net.NetBase.reward_list>();
		for(int i_offline_reward = 0 ; i_offline_reward < lenoffline_reward ; i_offline_reward ++)
		{
			st.net.NetBase.reward_list listData = new st.net.NetBase.reward_list();
			listData.fromBinary(reader);
			offline_reward.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(start_time);
		writer.write_int(amount_time);
		ushort lenoffline_reward = (ushort)offline_reward.Count;
		writer.write_short(lenoffline_reward);
		for(int i_offline_reward = 0 ; i_offline_reward < lenoffline_reward ; i_offline_reward ++)
		{
			st.net.NetBase.reward_list listData = offline_reward[i_offline_reward];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
