using System.Collections;
using System.Collections.Generic;

public class pt_update_liveness_reward_c103 : st.net.NetBase.Pt {
	public pt_update_liveness_reward_c103()
	{
		Id = 0xC103;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_liveness_reward_c103();
	}
	public List<st.net.NetBase.liveness_reward_list> liveness_reward = new List<st.net.NetBase.liveness_reward_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenliveness_reward = reader.Read_ushort();
		liveness_reward = new List<st.net.NetBase.liveness_reward_list>();
		for(int i_liveness_reward = 0 ; i_liveness_reward < lenliveness_reward ; i_liveness_reward ++)
		{
			st.net.NetBase.liveness_reward_list listData = new st.net.NetBase.liveness_reward_list();
			listData.fromBinary(reader);
			liveness_reward.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenliveness_reward = (ushort)liveness_reward.Count;
		writer.write_short(lenliveness_reward);
		for(int i_liveness_reward = 0 ; i_liveness_reward < lenliveness_reward ; i_liveness_reward ++)
		{
			st.net.NetBase.liveness_reward_list listData = liveness_reward[i_liveness_reward];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
