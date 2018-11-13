using System.Collections;
using System.Collections.Generic;

public class pt_activity_game_over_d728 : st.net.NetBase.Pt {
	public pt_activity_game_over_d728()
	{
		Id = 0xD728;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_activity_game_over_d728();
	}
	public int state;
	public List<st.net.NetBase.reward_list> activity_reward = new List<st.net.NetBase.reward_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		ushort lenactivity_reward = reader.Read_ushort();
		activity_reward = new List<st.net.NetBase.reward_list>();
		for(int i_activity_reward = 0 ; i_activity_reward < lenactivity_reward ; i_activity_reward ++)
		{
			st.net.NetBase.reward_list listData = new st.net.NetBase.reward_list();
			listData.fromBinary(reader);
			activity_reward.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		ushort lenactivity_reward = (ushort)activity_reward.Count;
		writer.write_short(lenactivity_reward);
		for(int i_activity_reward = 0 ; i_activity_reward < lenactivity_reward ; i_activity_reward ++)
		{
			st.net.NetBase.reward_list listData = activity_reward[i_activity_reward];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
