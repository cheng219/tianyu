using System.Collections;
using System.Collections.Generic;

public class pt_update_lev_reward_d762 : st.net.NetBase.Pt {
	public pt_update_lev_reward_d762()
	{
		Id = 0xD762;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_lev_reward_d762();
	}
	public List<int> lev_reward_list = new List<int>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenlev_reward_list = reader.Read_ushort();
		lev_reward_list = new List<int>();
		for(int i_lev_reward_list = 0 ; i_lev_reward_list < lenlev_reward_list ; i_lev_reward_list ++)
		{
			int listData = reader.Read_int();
			lev_reward_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenlev_reward_list = (ushort)lev_reward_list.Count;
		writer.write_short(lenlev_reward_list);
		for(int i_lev_reward_list = 0 ; i_lev_reward_list < lenlev_reward_list ; i_lev_reward_list ++)
		{
			int listData = lev_reward_list[i_lev_reward_list];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
