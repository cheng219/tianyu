using System.Collections;
using System.Collections.Generic;

public class pt_vip_info_d329 : st.net.NetBase.Pt {
	public pt_vip_info_d329()
	{
		Id = 0xD329;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_vip_info_d329();
	}
	public int vip_lev;
	public int vip_exp;
	public List<int> already_get_reward_lev_list = new List<int>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		vip_lev = reader.Read_int();
		vip_exp = reader.Read_int();
		ushort lenalready_get_reward_lev_list = reader.Read_ushort();
		already_get_reward_lev_list = new List<int>();
		for(int i_already_get_reward_lev_list = 0 ; i_already_get_reward_lev_list < lenalready_get_reward_lev_list ; i_already_get_reward_lev_list ++)
		{
			int listData = reader.Read_int();
			already_get_reward_lev_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(vip_lev);
		writer.write_int(vip_exp);
		ushort lenalready_get_reward_lev_list = (ushort)already_get_reward_lev_list.Count;
		writer.write_short(lenalready_get_reward_lev_list);
		for(int i_already_get_reward_lev_list = 0 ; i_already_get_reward_lev_list < lenalready_get_reward_lev_list ; i_already_get_reward_lev_list ++)
		{
			int listData = already_get_reward_lev_list[i_already_get_reward_lev_list];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
