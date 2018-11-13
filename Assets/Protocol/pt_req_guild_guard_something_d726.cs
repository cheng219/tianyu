using System.Collections;
using System.Collections.Generic;

public class pt_req_guild_guard_something_d726 : st.net.NetBase.Pt {
	public pt_req_guild_guard_something_d726()
	{
		Id = 0xD726;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_guild_guard_something_d726();
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
