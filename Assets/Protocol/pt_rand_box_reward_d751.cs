using System.Collections;
using System.Collections.Generic;

public class pt_rand_box_reward_d751 : st.net.NetBase.Pt {
	public pt_rand_box_reward_d751()
	{
		Id = 0xD751;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_rand_box_reward_d751();
	}
	public List<st.net.NetBase.rand_box_reward> rand_box_reward = new List<st.net.NetBase.rand_box_reward>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenrand_box_reward = reader.Read_ushort();
		rand_box_reward = new List<st.net.NetBase.rand_box_reward>();
		for(int i_rand_box_reward = 0 ; i_rand_box_reward < lenrand_box_reward ; i_rand_box_reward ++)
		{
			st.net.NetBase.rand_box_reward listData = new st.net.NetBase.rand_box_reward();
			listData.fromBinary(reader);
			rand_box_reward.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenrand_box_reward = (ushort)rand_box_reward.Count;
		writer.write_short(lenrand_box_reward);
		for(int i_rand_box_reward = 0 ; i_rand_box_reward < lenrand_box_reward ; i_rand_box_reward ++)
		{
			st.net.NetBase.rand_box_reward listData = rand_box_reward[i_rand_box_reward];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
