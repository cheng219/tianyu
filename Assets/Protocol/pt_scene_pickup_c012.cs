using System.Collections;
using System.Collections.Generic;

public class pt_scene_pickup_c012 : st.net.NetBase.Pt {
	public pt_scene_pickup_c012()
	{
		Id = 0xC012;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_scene_pickup_c012();
	}
	public List<st.net.NetBase.pickup_des> pickup_list = new List<st.net.NetBase.pickup_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenpickup_list = reader.Read_ushort();
		pickup_list = new List<st.net.NetBase.pickup_des>();
		for(int i_pickup_list = 0 ; i_pickup_list < lenpickup_list ; i_pickup_list ++)
		{
			st.net.NetBase.pickup_des listData = new st.net.NetBase.pickup_des();
			listData.fromBinary(reader);
			pickup_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenpickup_list = (ushort)pickup_list.Count;
		writer.write_short(lenpickup_list);
		for(int i_pickup_list = 0 ; i_pickup_list < lenpickup_list ; i_pickup_list ++)
		{
			st.net.NetBase.pickup_des listData = pickup_list[i_pickup_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
