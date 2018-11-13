using System.Collections;
using System.Collections.Generic;

public class pt_use_skill_list_d401 : st.net.NetBase.Pt {
	public pt_use_skill_list_d401()
	{
		Id = 0xD401;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_use_skill_list_d401();
	}
	public List<st.net.NetBase.usr_use_skill_list> use_skill_list = new List<st.net.NetBase.usr_use_skill_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenuse_skill_list = reader.Read_ushort();
		use_skill_list = new List<st.net.NetBase.usr_use_skill_list>();
		for(int i_use_skill_list = 0 ; i_use_skill_list < lenuse_skill_list ; i_use_skill_list ++)
		{
			st.net.NetBase.usr_use_skill_list listData = new st.net.NetBase.usr_use_skill_list();
			listData.fromBinary(reader);
			use_skill_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenuse_skill_list = (ushort)use_skill_list.Count;
		writer.write_short(lenuse_skill_list);
		for(int i_use_skill_list = 0 ; i_use_skill_list < lenuse_skill_list ; i_use_skill_list ++)
		{
			st.net.NetBase.usr_use_skill_list listData = use_skill_list[i_use_skill_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
