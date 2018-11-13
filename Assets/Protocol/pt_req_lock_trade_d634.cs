using System.Collections;
using System.Collections.Generic;

public class pt_req_lock_trade_d634 : st.net.NetBase.Pt {
	public pt_req_lock_trade_d634()
	{
		Id = 0xD634;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_lock_trade_d634();
	}
	public uint session_id;
	public List<st.net.NetBase.trade_item_info> item_list = new List<st.net.NetBase.trade_item_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		session_id = reader.Read_uint();
		ushort lenitem_list = reader.Read_ushort();
		item_list = new List<st.net.NetBase.trade_item_info>();
		for(int i_item_list = 0 ; i_item_list < lenitem_list ; i_item_list ++)
		{
			st.net.NetBase.trade_item_info listData = new st.net.NetBase.trade_item_info();
			listData.fromBinary(reader);
			item_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(session_id);
		ushort lenitem_list = (ushort)item_list.Count;
		writer.write_short(lenitem_list);
		for(int i_item_list = 0 ; i_item_list < lenitem_list ; i_item_list ++)
		{
			st.net.NetBase.trade_item_info listData = item_list[i_item_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
