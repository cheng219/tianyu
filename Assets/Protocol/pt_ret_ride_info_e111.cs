using System.Collections;
using System.Collections.Generic;

public class pt_ret_ride_info_e111 : st.net.NetBase.Pt {
	public pt_ret_ride_info_e111()
	{
		Id = 0xE111;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ret_ride_info_e111();
	}
	public int ride_lev;
	public List<st.net.NetBase.ride_list> ride_list = new List<st.net.NetBase.ride_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ride_lev = reader.Read_int();
		ushort lenride_list = reader.Read_ushort();
		ride_list = new List<st.net.NetBase.ride_list>();
		for(int i_ride_list = 0 ; i_ride_list < lenride_list ; i_ride_list ++)
		{
			st.net.NetBase.ride_list listData = new st.net.NetBase.ride_list();
			listData.fromBinary(reader);
			ride_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(ride_lev);
		ushort lenride_list = (ushort)ride_list.Count;
		writer.write_short(lenride_list);
		for(int i_ride_list = 0 ; i_ride_list < lenride_list ; i_ride_list ++)
		{
			st.net.NetBase.ride_list listData = ride_list[i_ride_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
