using System.Collections;
using System.Collections.Generic;

public class pt_reply_login_dividend_info_d981 : st.net.NetBase.Pt {
	public pt_reply_login_dividend_info_d981()
	{
		Id = 0xD981;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_login_dividend_info_d981();
	}
	public uint active_days;
	public List<uint> reward_ids = new List<uint>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		active_days = reader.Read_uint();
		ushort lenreward_ids = reader.Read_ushort();
		reward_ids = new List<uint>();
		for(int i_reward_ids = 0 ; i_reward_ids < lenreward_ids ; i_reward_ids ++)
		{
			uint listData = reader.Read_uint();
			reward_ids.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(active_days);
		ushort lenreward_ids = (ushort)reward_ids.Count;
		writer.write_short(lenreward_ids);
		for(int i_reward_ids = 0 ; i_reward_ids < lenreward_ids ; i_reward_ids ++)
		{
			uint listData = reward_ids[i_reward_ids];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
