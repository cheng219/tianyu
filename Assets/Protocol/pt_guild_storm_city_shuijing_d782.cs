using System.Collections;
using System.Collections.Generic;

public class pt_guild_storm_city_shuijing_d782 : st.net.NetBase.Pt {
	public pt_guild_storm_city_shuijing_d782()
	{
		Id = 0xD782;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_storm_city_shuijing_d782();
	}
	public int shuijing_type;
	public int cur_hp;
	public int max_hp;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		shuijing_type = reader.Read_int();
		cur_hp = reader.Read_int();
		max_hp = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(shuijing_type);
		writer.write_int(cur_hp);
		writer.write_int(max_hp);
		return writer.data;
	}

}
