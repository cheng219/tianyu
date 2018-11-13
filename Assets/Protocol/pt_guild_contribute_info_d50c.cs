using System.Collections;
using System.Collections.Generic;

public class pt_guild_contribute_info_d50c : st.net.NetBase.Pt {
	public pt_guild_contribute_info_d50c()
	{
		Id = 0xD50C;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_contribute_info_d50c();
	}
	public byte times;
	public byte max_times;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		times = reader.Read_byte();
		max_times = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(times);
		writer.write_byte(max_times);
		return writer.data;
	}

}
