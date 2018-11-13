using System.Collections;
using System.Collections.Generic;

public class pt_leader_req_clear_ask_join_guild_list_d519 : st.net.NetBase.Pt {
	public pt_leader_req_clear_ask_join_guild_list_d519()
	{
		Id = 0xD519;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_leader_req_clear_ask_join_guild_list_d519();
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
