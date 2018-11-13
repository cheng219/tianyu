using System.Collections;
using System.Collections.Generic;

public class pt_req_guild_liveness_reward_d50f : st.net.NetBase.Pt {
	public pt_req_guild_liveness_reward_d50f()
	{
		Id = 0xD50F;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_guild_liveness_reward_d50f();
	}
	public uint reward_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		reward_id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(reward_id);
		return writer.data;
	}

}
