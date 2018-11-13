using System.Collections;
using System.Collections.Generic;

public class pt_lucky_brand_list_d469 : st.net.NetBase.Pt {
	public pt_lucky_brand_list_d469()
	{
		Id = 0xD469;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_lucky_brand_list_d469();
	}
	public int bradn_id;
	public List<st.net.NetBase.brand_reward> brand_reward = new List<st.net.NetBase.brand_reward>();
	public List<st.net.NetBase.lucky_brand_list> lucky_brand = new List<st.net.NetBase.lucky_brand_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		bradn_id = reader.Read_int();
		ushort lenbrand_reward = reader.Read_ushort();
		brand_reward = new List<st.net.NetBase.brand_reward>();
		for(int i_brand_reward = 0 ; i_brand_reward < lenbrand_reward ; i_brand_reward ++)
		{
			st.net.NetBase.brand_reward listData = new st.net.NetBase.brand_reward();
			listData.fromBinary(reader);
			brand_reward.Add(listData);
		}
		ushort lenlucky_brand = reader.Read_ushort();
		lucky_brand = new List<st.net.NetBase.lucky_brand_list>();
		for(int i_lucky_brand = 0 ; i_lucky_brand < lenlucky_brand ; i_lucky_brand ++)
		{
			st.net.NetBase.lucky_brand_list listData = new st.net.NetBase.lucky_brand_list();
			listData.fromBinary(reader);
			lucky_brand.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(bradn_id);
		ushort lenbrand_reward = (ushort)brand_reward.Count;
		writer.write_short(lenbrand_reward);
		for(int i_brand_reward = 0 ; i_brand_reward < lenbrand_reward ; i_brand_reward ++)
		{
			st.net.NetBase.brand_reward listData = brand_reward[i_brand_reward];
			listData.toBinary(writer);
		}
		ushort lenlucky_brand = (ushort)lucky_brand.Count;
		writer.write_short(lenlucky_brand);
		for(int i_lucky_brand = 0 ; i_lucky_brand < lenlucky_brand ; i_lucky_brand ++)
		{
			st.net.NetBase.lucky_brand_list listData = lucky_brand[i_lucky_brand];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
