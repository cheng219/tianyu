using System.Collections;
using System.Collections.Generic;

public class pt_reply_treasure_info_d971 : st.net.NetBase.Pt {
	public pt_reply_treasure_info_d971()
	{
		Id = 0xD971;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_treasure_info_d971();
	}
	public uint price1;
	public uint price2;
	public uint reward1_item_type;
	public uint reward1_item_amount;
	public uint reward2_item_type;
	public uint reward2_item_amount;
	public List<st.net.NetBase.item_list> reward_info = new List<st.net.NetBase.item_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		price1 = reader.Read_uint();
		price2 = reader.Read_uint();
		reward1_item_type = reader.Read_uint();
		reward1_item_amount = reader.Read_uint();
		reward2_item_type = reader.Read_uint();
		reward2_item_amount = reader.Read_uint();
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
		writer.write_int(price1);
		writer.write_int(price2);
		writer.write_int(reward1_item_type);
		writer.write_int(reward1_item_amount);
		writer.write_int(reward2_item_type);
		writer.write_int(reward2_item_amount);
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
