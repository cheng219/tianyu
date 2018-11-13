using System.Collections;
using System.Collections.Generic;

public class pt_pk_win_d491 : st.net.NetBase.Pt {
	public pt_pk_win_d491()
	{
		Id = 0xD491;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_pk_win_d491();
	}
	public int state;
	public int rank;
	public int up_rank;
	public List<st.net.NetBase.reward_list> reward = new List<st.net.NetBase.reward_list>();
	public int cd_state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		rank = reader.Read_int();
		up_rank = reader.Read_int();
		ushort lenreward = reader.Read_ushort();
		reward = new List<st.net.NetBase.reward_list>();
		for(int i_reward = 0 ; i_reward < lenreward ; i_reward ++)
		{
			st.net.NetBase.reward_list listData = new st.net.NetBase.reward_list();
			listData.fromBinary(reader);
			reward.Add(listData);
		}
		cd_state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		writer.write_int(rank);
		writer.write_int(up_rank);
		ushort lenreward = (ushort)reward.Count;
		writer.write_short(lenreward);
		for(int i_reward = 0 ; i_reward < lenreward ; i_reward ++)
		{
			st.net.NetBase.reward_list listData = reward[i_reward];
			listData.toBinary(writer);
		}
		writer.write_int(cd_state);
		return writer.data;
	}

}
