using System.Collections;
using System.Collections.Generic;

public class pt_love_reward_list_d808 : st.net.NetBase.Pt {
	public pt_love_reward_list_d808()
	{
		Id = 0xD808;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_love_reward_list_d808();
	}
	public List<st.net.NetBase.love_reward_list> reward = new List<st.net.NetBase.love_reward_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenreward = reader.Read_ushort();
		reward = new List<st.net.NetBase.love_reward_list>();
		for(int i_reward = 0 ; i_reward < lenreward ; i_reward ++)
		{
			st.net.NetBase.love_reward_list listData = new st.net.NetBase.love_reward_list();
			listData.fromBinary(reader);
			reward.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenreward = (ushort)reward.Count;
		writer.write_short(lenreward);
		for(int i_reward = 0 ; i_reward < lenreward ; i_reward ++)
		{
			st.net.NetBase.love_reward_list listData = reward[i_reward];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
