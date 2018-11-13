using System.Collections;
using System.Collections.Generic;

public class pt_req_guild_liveness_info_d50d : st.net.NetBase.Pt {
	public pt_req_guild_liveness_info_d50d()
	{
		Id = 0xD50D;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_guild_liveness_info_d50d();
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
