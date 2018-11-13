using System.Collections;
using System.Collections.Generic;

public class pt_leader_req_clear_guild_items_d520 : st.net.NetBase.Pt {
	public pt_leader_req_clear_guild_items_d520()
	{
		Id = 0xD520;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_leader_req_clear_guild_items_d520();
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
