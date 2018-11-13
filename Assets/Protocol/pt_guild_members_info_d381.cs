using System.Collections;
using System.Collections.Generic;

public class pt_guild_members_info_d381 : st.net.NetBase.Pt {
	public pt_guild_members_info_d381()
	{
		Id = 0xD381;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_members_info_d381();
	}
	public List<st.net.NetBase.guild_member_info> memeber_info_list = new List<st.net.NetBase.guild_member_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenmemeber_info_list = reader.Read_ushort();
		memeber_info_list = new List<st.net.NetBase.guild_member_info>();
		for(int i_memeber_info_list = 0 ; i_memeber_info_list < lenmemeber_info_list ; i_memeber_info_list ++)
		{
			st.net.NetBase.guild_member_info listData = new st.net.NetBase.guild_member_info();
			listData.fromBinary(reader);
			memeber_info_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenmemeber_info_list = (ushort)memeber_info_list.Count;
		writer.write_short(lenmemeber_info_list);
		for(int i_memeber_info_list = 0 ; i_memeber_info_list < lenmemeber_info_list ; i_memeber_info_list ++)
		{
			st.net.NetBase.guild_member_info listData = memeber_info_list[i_memeber_info_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
