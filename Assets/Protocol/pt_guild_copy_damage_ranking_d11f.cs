using System.Collections;
using System.Collections.Generic;

public class pt_guild_copy_damage_ranking_d11f : st.net.NetBase.Pt {
	public pt_guild_copy_damage_ranking_d11f()
	{
		Id = 0xD11F;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_copy_damage_ranking_d11f();
	}
	public List<st.net.NetBase.guild_copy_damage_ranking> guild_copy_damage_ranking = new List<st.net.NetBase.guild_copy_damage_ranking>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenguild_copy_damage_ranking = reader.Read_ushort();
		guild_copy_damage_ranking = new List<st.net.NetBase.guild_copy_damage_ranking>();
		for(int i_guild_copy_damage_ranking = 0 ; i_guild_copy_damage_ranking < lenguild_copy_damage_ranking ; i_guild_copy_damage_ranking ++)
		{
			st.net.NetBase.guild_copy_damage_ranking listData = new st.net.NetBase.guild_copy_damage_ranking();
			listData.fromBinary(reader);
			guild_copy_damage_ranking.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenguild_copy_damage_ranking = (ushort)guild_copy_damage_ranking.Count;
		writer.write_short(lenguild_copy_damage_ranking);
		for(int i_guild_copy_damage_ranking = 0 ; i_guild_copy_damage_ranking < lenguild_copy_damage_ranking ; i_guild_copy_damage_ranking ++)
		{
			st.net.NetBase.guild_copy_damage_ranking listData = guild_copy_damage_ranking[i_guild_copy_damage_ranking];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
