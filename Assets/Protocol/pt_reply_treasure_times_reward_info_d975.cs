using System.Collections;
using System.Collections.Generic;

public class pt_reply_treasure_times_reward_info_d975 : st.net.NetBase.Pt {
	public pt_reply_treasure_times_reward_info_d975()
	{
		Id = 0xD975;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_treasure_times_reward_info_d975();
	}
	public uint rest_time;
	public uint times;
	public List<st.net.NetBase.treasure_times_reward> reward_info = new List<st.net.NetBase.treasure_times_reward>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		rest_time = reader.Read_uint();
		times = reader.Read_uint();
		ushort lenreward_info = reader.Read_ushort();
		reward_info = new List<st.net.NetBase.treasure_times_reward>();
		for(int i_reward_info = 0 ; i_reward_info < lenreward_info ; i_reward_info ++)
		{
			st.net.NetBase.treasure_times_reward listData = new st.net.NetBase.treasure_times_reward();
			listData.fromBinary(reader);
			reward_info.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(rest_time);
		writer.write_int(times);
		ushort lenreward_info = (ushort)reward_info.Count;
		writer.write_short(lenreward_info);
		for(int i_reward_info = 0 ; i_reward_info < lenreward_info ; i_reward_info ++)
		{
			st.net.NetBase.treasure_times_reward listData = reward_info[i_reward_info];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
