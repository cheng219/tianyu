using System.Collections;
using System.Collections.Generic;

public class pt_req_fly_guild_bonfire_d739 : st.net.NetBase.Pt {
	public pt_req_fly_guild_bonfire_d739()
	{
		Id = 0xD739;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_fly_guild_bonfire_d739();
	}
	public int type;
	public int val;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_int();
		val = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		writer.write_int(val);
		return writer.data;
	}

}
