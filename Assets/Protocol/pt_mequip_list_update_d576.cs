using System.Collections;
using System.Collections.Generic;

public class pt_mequip_list_update_d576 : st.net.NetBase.Pt {
	public pt_mequip_list_update_d576()
	{
		Id = 0xD576;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_mequip_list_update_d576();
	}
	public List<st.net.NetBase.item_des> items = new List<st.net.NetBase.item_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenitems = reader.Read_ushort();
		items = new List<st.net.NetBase.item_des>();
		for(int i_items = 0 ; i_items < lenitems ; i_items ++)
		{
			st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
			listData.fromBinary(reader);
			items.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenitems = (ushort)items.Count;
		writer.write_short(lenitems);
		for(int i_items = 0 ; i_items < lenitems ; i_items ++)
		{
			st.net.NetBase.item_des listData = items[i_items];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
