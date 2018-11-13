using System.Collections;
using System.Collections.Generic;

public class pt_ret_sevenDayRewardsInfo_f001 : st.net.NetBase.Pt {
	public pt_ret_sevenDayRewardsInfo_f001()
	{
		Id = 0xF001;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ret_sevenDayRewardsInfo_f001();
	}
	public byte action;
	public byte day;
	public List<st.net.NetBase.sevenDayReward> rewards_info = new List<st.net.NetBase.sevenDayReward>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		action = reader.Read_byte();
		day = reader.Read_byte();
		ushort lenrewards_info = reader.Read_ushort();
		rewards_info = new List<st.net.NetBase.sevenDayReward>();
		for(int i_rewards_info = 0 ; i_rewards_info < lenrewards_info ; i_rewards_info ++)
		{
			st.net.NetBase.sevenDayReward listData = new st.net.NetBase.sevenDayReward();
			listData.fromBinary(reader);
			rewards_info.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(action);
		writer.write_byte(day);
		ushort lenrewards_info = (ushort)rewards_info.Count;
		writer.write_short(lenrewards_info);
		for(int i_rewards_info = 0 ; i_rewards_info < lenrewards_info ; i_rewards_info ++)
		{
			st.net.NetBase.sevenDayReward listData = rewards_info[i_rewards_info];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
