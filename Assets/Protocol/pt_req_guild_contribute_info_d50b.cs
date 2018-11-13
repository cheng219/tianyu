using System.Collections;
using System.Collections.Generic;

public class pt_req_guild_contribute_info_d50b : st.net.NetBase.Pt {
	public pt_req_guild_contribute_info_d50b()
	{
		Id = 0xD50B;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_guild_contribute_info_d50b();
	}
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		return writer.data;
	}

}
