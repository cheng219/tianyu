using System.Collections;
using System.Collections.Generic;

public class pt_shelve_items_info_d550 : st.net.NetBase.Pt {
	public pt_shelve_items_info_d550()
	{
		Id = 0xD550;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_shelve_items_info_d550();
	}
	public byte type;
	public uint page;
	public List<st.net.NetBase.shelve_item_info> shelve_item_list = new List<st.net.NetBase.shelve_item_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_byte();
		page = reader.Read_uint();
		ushort lenshelve_item_list = reader.Read_ushort();
		shelve_item_list = new List<st.net.NetBase.shelve_item_info>();
		for(int i_shelve_item_list = 0 ; i_shelve_item_list < lenshelve_item_list ; i_shelve_item_list ++)
		{
			st.net.NetBase.shelve_item_info listData = new st.net.NetBase.shelve_item_info();
			listData.fromBinary(reader);
			shelve_item_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(type);
		writer.write_int(page);
		ushort lenshelve_item_list = (ushort)shelve_item_list.Count;
		writer.write_short(lenshelve_item_list);
		for(int i_shelve_item_list = 0 ; i_shelve_item_list < lenshelve_item_list ; i_shelve_item_list ++)
		{
			st.net.NetBase.shelve_item_info listData = shelve_item_list[i_shelve_item_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
