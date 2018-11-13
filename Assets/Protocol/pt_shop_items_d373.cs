using System.Collections;
using System.Collections.Generic;

public class pt_shop_items_d373 : st.net.NetBase.Pt {
	public pt_shop_items_d373()
	{
		Id = 0xD373;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_shop_items_d373();
	}
	public List<st.net.NetBase.shop_list> shop_items = new List<st.net.NetBase.shop_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenshop_items = reader.Read_ushort();
		shop_items = new List<st.net.NetBase.shop_list>();
		for(int i_shop_items = 0 ; i_shop_items < lenshop_items ; i_shop_items ++)
		{
			st.net.NetBase.shop_list listData = new st.net.NetBase.shop_list();
			listData.fromBinary(reader);
			shop_items.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenshop_items = (ushort)shop_items.Count;
		writer.write_short(lenshop_items);
		for(int i_shop_items = 0 ; i_shop_items < lenshop_items ; i_shop_items ++)
		{
			st.net.NetBase.shop_list listData = shop_items[i_shop_items];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
