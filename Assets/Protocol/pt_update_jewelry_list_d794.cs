using System.Collections;
using System.Collections.Generic;

public class pt_update_jewelry_list_d794 : st.net.NetBase.Pt {
	public pt_update_jewelry_list_d794()
	{
		Id = 0xD794;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_jewelry_list_d794();
	}
	public List<st.net.NetBase.jewelry_list> jewelry_list = new List<st.net.NetBase.jewelry_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenjewelry_list = reader.Read_ushort();
		jewelry_list = new List<st.net.NetBase.jewelry_list>();
		for(int i_jewelry_list = 0 ; i_jewelry_list < lenjewelry_list ; i_jewelry_list ++)
		{
			st.net.NetBase.jewelry_list listData = new st.net.NetBase.jewelry_list();
			listData.fromBinary(reader);
			jewelry_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenjewelry_list = (ushort)jewelry_list.Count;
		writer.write_short(lenjewelry_list);
		for(int i_jewelry_list = 0 ; i_jewelry_list < lenjewelry_list ; i_jewelry_list ++)
		{
			st.net.NetBase.jewelry_list listData = jewelry_list[i_jewelry_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
