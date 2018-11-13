using System.Collections;
using System.Collections.Generic;

public class pt_store_house_item_chg_d323 : st.net.NetBase.Pt {
	public pt_store_house_item_chg_d323()
	{
		Id = 0xD323;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_store_house_item_chg_d323();
	}
	public List<st.net.NetBase.item_des> item_list = new List<st.net.NetBase.item_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenitem_list = reader.Read_ushort();
		item_list = new List<st.net.NetBase.item_des>();
		for(int i_item_list = 0 ; i_item_list < lenitem_list ; i_item_list ++)
		{
			st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
			listData.fromBinary(reader);
			item_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenitem_list = (ushort)item_list.Count;
		writer.write_short(lenitem_list);
		for(int i_item_list = 0 ; i_item_list < lenitem_list ; i_item_list ++)
		{
			st.net.NetBase.item_des listData = item_list[i_item_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
