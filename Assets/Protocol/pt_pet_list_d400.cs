using System.Collections;
using System.Collections.Generic;

public class pt_pet_list_d400 : st.net.NetBase.Pt {
	public pt_pet_list_d400()
	{
		Id = 0xD400;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_pet_list_d400();
	}
	public List<st.net.NetBase.pet_base_info> pet_list = new List<st.net.NetBase.pet_base_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenpet_list = reader.Read_ushort();
		pet_list = new List<st.net.NetBase.pet_base_info>();
		for(int i_pet_list = 0 ; i_pet_list < lenpet_list ; i_pet_list ++)
		{
			st.net.NetBase.pet_base_info listData = new st.net.NetBase.pet_base_info();
			listData.fromBinary(reader);
			pet_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenpet_list = (ushort)pet_list.Count;
		writer.write_short(lenpet_list);
		for(int i_pet_list = 0 ; i_pet_list < lenpet_list ; i_pet_list ++)
		{
			st.net.NetBase.pet_base_info listData = pet_list[i_pet_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
