using System.Collections;
using System.Collections.Generic;

public class pt_guild_contribute_result_d50a : st.net.NetBase.Pt {
	public pt_guild_contribute_result_d50a()
	{
		Id = 0xD50A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_contribute_result_d50a();
	}
	public byte times;
	public byte type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		times = reader.Read_byte();
		type = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(times);
		writer.write_byte(type);
		return writer.data;
	}

}
