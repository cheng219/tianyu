using System.Collections;
using System.Collections.Generic;

public class pt_guild_storm_city_portal_d781 : st.net.NetBase.Pt {
	public pt_guild_storm_city_portal_d781()
	{
		Id = 0xD781;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_storm_city_portal_d781();
	}
	public int start_state;
	public int surplus_time;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		start_state = reader.Read_int();
		surplus_time = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(start_state);
		writer.write_int(surplus_time);
		return writer.data;
	}

}
