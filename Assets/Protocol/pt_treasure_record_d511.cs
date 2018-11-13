using System.Collections;
using System.Collections.Generic;

public class pt_treasure_record_d511 : st.net.NetBase.Pt {
	public pt_treasure_record_d511()
	{
		Id = 0xD511;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_treasure_record_d511();
	}
	public List<st.net.NetBase.treasure_record_list> treasure_info = new List<st.net.NetBase.treasure_record_list>();
	public byte free_flag;
	public byte half_price_flag;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lentreasure_info = reader.Read_ushort();
		treasure_info = new List<st.net.NetBase.treasure_record_list>();
		for(int i_treasure_info = 0 ; i_treasure_info < lentreasure_info ; i_treasure_info ++)
		{
			st.net.NetBase.treasure_record_list listData = new st.net.NetBase.treasure_record_list();
			listData.fromBinary(reader);
			treasure_info.Add(listData);
		}
		free_flag = reader.Read_byte();
		half_price_flag = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lentreasure_info = (ushort)treasure_info.Count;
		writer.write_short(lentreasure_info);
		for(int i_treasure_info = 0 ; i_treasure_info < lentreasure_info ; i_treasure_info ++)
		{
			st.net.NetBase.treasure_record_list listData = treasure_info[i_treasure_info];
			listData.toBinary(writer);
		}
		writer.write_byte(free_flag);
		writer.write_byte(half_price_flag);
		return writer.data;
	}

}
