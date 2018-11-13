using System.Collections;
using System.Collections.Generic;

public class pt_guild_storm_city_over_d778 : st.net.NetBase.Pt {
	public pt_guild_storm_city_over_d778()
	{
		Id = 0xD778;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_storm_city_over_d778();
	}
	public string win_guildname;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		win_guildname = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(win_guildname);
		return writer.data;
	}

}
