using System.Collections;
using System.Collections.Generic;

public class pt_req_item_decompose_d301 : st.net.NetBase.Pt {
	public pt_req_item_decompose_d301()
	{
		Id = 0xD301;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_item_decompose_d301();
	}
	public List<uint> item_id_list = new List<uint>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenitem_id_list = reader.Read_ushort();
		item_id_list = new List<uint>();
		for(int i_item_id_list = 0 ; i_item_id_list < lenitem_id_list ; i_item_id_list ++)
		{
			uint listData = reader.Read_uint();
			item_id_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenitem_id_list = (ushort)item_id_list.Count;
		writer.write_short(lenitem_id_list);
		for(int i_item_id_list = 0 ; i_item_id_list < lenitem_id_list ; i_item_id_list ++)
		{
			uint listData = item_id_list[i_item_id_list];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
