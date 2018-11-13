using System.Collections;
using System.Collections.Generic;

public class pt_update_online_reward_d763 : st.net.NetBase.Pt {
	public pt_update_online_reward_d763()
	{
		Id = 0xD763;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_online_reward_d763();
	}
	public int reward_id;
	public int remain_time;
	public List<int> reward_id_list = new List<int>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		reward_id = reader.Read_int();
		remain_time = reader.Read_int();
		ushort lenreward_id_list = reader.Read_ushort();
		reward_id_list = new List<int>();
		for(int i_reward_id_list = 0 ; i_reward_id_list < lenreward_id_list ; i_reward_id_list ++)
		{
			int listData = reader.Read_int();
			reward_id_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(reward_id);
		writer.write_int(remain_time);
		ushort lenreward_id_list = (ushort)reward_id_list.Count;
		writer.write_short(lenreward_id_list);
		for(int i_reward_id_list = 0 ; i_reward_id_list < lenreward_id_list ; i_reward_id_list ++)
		{
			int listData = reward_id_list[i_reward_id_list];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
