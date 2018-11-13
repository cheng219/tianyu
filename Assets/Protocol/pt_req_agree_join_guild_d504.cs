using System.Collections;
using System.Collections.Generic;

public class pt_req_agree_join_guild_d504 : st.net.NetBase.Pt {
	public pt_req_agree_join_guild_d504()
	{
		Id = 0xD504;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_agree_join_guild_d504();
	}
	public List<int> uid_list = new List<int>();
	public int action;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenuid_list = reader.Read_ushort();
		uid_list = new List<int>();
		for(int i_uid_list = 0 ; i_uid_list < lenuid_list ; i_uid_list ++)
		{
			int listData = reader.Read_int();
			uid_list.Add(listData);
		}
		action = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenuid_list = (ushort)uid_list.Count;
		writer.write_short(lenuid_list);
		for(int i_uid_list = 0 ; i_uid_list < lenuid_list ; i_uid_list ++)
		{
			int listData = uid_list[i_uid_list];
			writer.write_int(listData);
		}
		writer.write_int(action);
		return writer.data;
	}

}
