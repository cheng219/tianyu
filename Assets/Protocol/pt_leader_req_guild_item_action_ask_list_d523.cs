using System.Collections;
using System.Collections.Generic;

public class pt_leader_req_guild_item_action_ask_list_d523 : st.net.NetBase.Pt {
	public pt_leader_req_guild_item_action_ask_list_d523()
	{
		Id = 0xD523;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_leader_req_guild_item_action_ask_list_d523();
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
