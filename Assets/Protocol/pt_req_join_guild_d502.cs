using System.Collections;
using System.Collections.Generic;

public class pt_req_join_guild_d502 : st.net.NetBase.Pt {
	public pt_req_join_guild_d502()
	{
		Id = 0xD502;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_join_guild_d502();
	}
	public List<int> id_list = new List<int>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenid_list = reader.Read_ushort();
		id_list = new List<int>();
		for(int i_id_list = 0 ; i_id_list < lenid_list ; i_id_list ++)
		{
			int listData = reader.Read_int();
			id_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenid_list = (ushort)id_list.Count;
		writer.write_short(lenid_list);
		for(int i_id_list = 0 ; i_id_list < lenid_list ; i_id_list ++)
		{
			int listData = id_list[i_id_list];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
