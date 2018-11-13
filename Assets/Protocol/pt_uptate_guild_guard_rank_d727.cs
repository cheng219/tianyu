using System.Collections;
using System.Collections.Generic;

public class pt_uptate_guild_guard_rank_d727 : st.net.NetBase.Pt {
	public pt_uptate_guild_guard_rank_d727()
	{
		Id = 0xD727;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_uptate_guild_guard_rank_d727();
	}
	public List<st.net.NetBase.guild_guard_rank> guild_guard_rank = new List<st.net.NetBase.guild_guard_rank>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenguild_guard_rank = reader.Read_ushort();
		guild_guard_rank = new List<st.net.NetBase.guild_guard_rank>();
		for(int i_guild_guard_rank = 0 ; i_guild_guard_rank < lenguild_guard_rank ; i_guild_guard_rank ++)
		{
			st.net.NetBase.guild_guard_rank listData = new st.net.NetBase.guild_guard_rank();
			listData.fromBinary(reader);
			guild_guard_rank.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenguild_guard_rank = (ushort)guild_guard_rank.Count;
		writer.write_short(lenguild_guard_rank);
		for(int i_guild_guard_rank = 0 ; i_guild_guard_rank < lenguild_guard_rank ; i_guild_guard_rank ++)
		{
			st.net.NetBase.guild_guard_rank listData = guild_guard_rank[i_guild_guard_rank];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
