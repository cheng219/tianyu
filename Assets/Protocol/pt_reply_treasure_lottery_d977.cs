using System.Collections;
using System.Collections.Generic;

public class pt_reply_treasure_lottery_d977 : st.net.NetBase.Pt {
	public pt_reply_treasure_lottery_d977()
	{
		Id = 0xD977;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_treasure_lottery_d977();
	}
	public List<st.net.NetBase.item_list> reward_info = new List<st.net.NetBase.item_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenreward_info = reader.Read_ushort();
		reward_info = new List<st.net.NetBase.item_list>();
		for(int i_reward_info = 0 ; i_reward_info < lenreward_info ; i_reward_info ++)
		{
			st.net.NetBase.item_list listData = new st.net.NetBase.item_list();
			listData.fromBinary(reader);
			reward_info.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenreward_info = (ushort)reward_info.Count;
		writer.write_short(lenreward_info);
		for(int i_reward_info = 0 ; i_reward_info < lenreward_info ; i_reward_info ++)
		{
			st.net.NetBase.item_list listData = reward_info[i_reward_info];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
