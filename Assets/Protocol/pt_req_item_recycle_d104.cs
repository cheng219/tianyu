using System.Collections;
using System.Collections.Generic;

public class pt_req_item_recycle_d104 : st.net.NetBase.Pt {
	public pt_req_item_recycle_d104()
	{
		Id = 0xD104;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_item_recycle_d104();
	}
	public List<st.net.NetBase.recycle_list> recycle_list = new List<st.net.NetBase.recycle_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenrecycle_list = reader.Read_ushort();
		recycle_list = new List<st.net.NetBase.recycle_list>();
		for(int i_recycle_list = 0 ; i_recycle_list < lenrecycle_list ; i_recycle_list ++)
		{
			st.net.NetBase.recycle_list listData = new st.net.NetBase.recycle_list();
			listData.fromBinary(reader);
			recycle_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenrecycle_list = (ushort)recycle_list.Count;
		writer.write_short(lenrecycle_list);
		for(int i_recycle_list = 0 ; i_recycle_list < lenrecycle_list ; i_recycle_list ++)
		{
			st.net.NetBase.recycle_list listData = recycle_list[i_recycle_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
