using System.Collections;
using System.Collections.Generic;

public class pt_draw_d127 : st.net.NetBase.Pt {
	public pt_draw_d127()
	{
		Id = 0xD127;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_draw_d127();
	}
	public List<st.net.NetBase.draw_item_list> draw_item_list = new List<st.net.NetBase.draw_item_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lendraw_item_list = reader.Read_ushort();
		draw_item_list = new List<st.net.NetBase.draw_item_list>();
		for(int i_draw_item_list = 0 ; i_draw_item_list < lendraw_item_list ; i_draw_item_list ++)
		{
			st.net.NetBase.draw_item_list listData = new st.net.NetBase.draw_item_list();
			listData.fromBinary(reader);
			draw_item_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lendraw_item_list = (ushort)draw_item_list.Count;
		writer.write_short(lendraw_item_list);
		for(int i_draw_item_list = 0 ; i_draw_item_list < lendraw_item_list ; i_draw_item_list ++)
		{
			st.net.NetBase.draw_item_list listData = draw_item_list[i_draw_item_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
