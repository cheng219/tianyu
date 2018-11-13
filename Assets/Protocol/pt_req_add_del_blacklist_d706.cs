using System.Collections;
using System.Collections.Generic;

public class pt_req_add_del_blacklist_d706 : st.net.NetBase.Pt {
	public pt_req_add_del_blacklist_d706()
	{
		Id = 0xD706;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_add_del_blacklist_d706();
	}
	public int state;
	public List<int> uid_list = new List<int>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		ushort lenuid_list = reader.Read_ushort();
		uid_list = new List<int>();
		for(int i_uid_list = 0 ; i_uid_list < lenuid_list ; i_uid_list ++)
		{
			int listData = reader.Read_int();
			uid_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		ushort lenuid_list = (ushort)uid_list.Count;
		writer.write_short(lenuid_list);
		for(int i_uid_list = 0 ; i_uid_list < lenuid_list ; i_uid_list ++)
		{
			int listData = uid_list[i_uid_list];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
