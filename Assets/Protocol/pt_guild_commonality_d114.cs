using System.Collections;
using System.Collections.Generic;

public class pt_guild_commonality_d114 : st.net.NetBase.Pt {
	public pt_guild_commonality_d114()
	{
		Id = 0xD114;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_commonality_d114();
	}
	public int guild_id;
	public string guild_name;
	public string president_name;
	public int guild_ranking;
	public int lev;
	public int member_amount;
	public int guild_resource;
	public int guild_exp;
	public int donation_times;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		guild_id = reader.Read_int();
		guild_name = reader.Read_str();
		president_name = reader.Read_str();
		guild_ranking = reader.Read_int();
		lev = reader.Read_int();
		member_amount = reader.Read_int();
		guild_resource = reader.Read_int();
		guild_exp = reader.Read_int();
		donation_times = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(guild_id);
		writer.write_str(guild_name);
		writer.write_str(president_name);
		writer.write_int(guild_ranking);
		writer.write_int(lev);
		writer.write_int(member_amount);
		writer.write_int(guild_resource);
		writer.write_int(guild_exp);
		writer.write_int(donation_times);
		return writer.data;
	}

}
