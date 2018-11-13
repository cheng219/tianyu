using System.Collections;
using System.Collections.Generic;

public class pt_guild_storm_city_apply_list_d731 : st.net.NetBase.Pt {
	public pt_guild_storm_city_apply_list_d731()
	{
		Id = 0xD731;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_storm_city_apply_list_d731();
	}
	public List<st.net.NetBase.req_apply_list> req_apply_list = new List<st.net.NetBase.req_apply_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenreq_apply_list = reader.Read_ushort();
		req_apply_list = new List<st.net.NetBase.req_apply_list>();
		for(int i_req_apply_list = 0 ; i_req_apply_list < lenreq_apply_list ; i_req_apply_list ++)
		{
			st.net.NetBase.req_apply_list listData = new st.net.NetBase.req_apply_list();
			listData.fromBinary(reader);
			req_apply_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenreq_apply_list = (ushort)req_apply_list.Count;
		writer.write_short(lenreq_apply_list);
		for(int i_req_apply_list = 0 ; i_req_apply_list < lenreq_apply_list ; i_req_apply_list ++)
		{
			st.net.NetBase.req_apply_list listData = req_apply_list[i_req_apply_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
