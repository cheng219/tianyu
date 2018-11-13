using System.Collections;
using System.Collections.Generic;

public class pt_tarot_reward_list_c129 : st.net.NetBase.Pt {
	public pt_tarot_reward_list_c129()
	{
		Id = 0xC129;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_tarot_reward_list_c129();
	}
	public List<st.net.NetBase.reward_list> tarot_reward = new List<st.net.NetBase.reward_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lentarot_reward = reader.Read_ushort();
		tarot_reward = new List<st.net.NetBase.reward_list>();
		for(int i_tarot_reward = 0 ; i_tarot_reward < lentarot_reward ; i_tarot_reward ++)
		{
			st.net.NetBase.reward_list listData = new st.net.NetBase.reward_list();
			listData.fromBinary(reader);
			tarot_reward.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lentarot_reward = (ushort)tarot_reward.Count;
		writer.write_short(lentarot_reward);
		for(int i_tarot_reward = 0 ; i_tarot_reward < lentarot_reward ; i_tarot_reward ++)
		{
			st.net.NetBase.reward_list listData = tarot_reward[i_tarot_reward];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
