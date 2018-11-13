using System.Collections;
using System.Collections.Generic;

public class pt_guild_liveness_reward_succ_d51a : st.net.NetBase.Pt {
	public pt_guild_liveness_reward_succ_d51a()
	{
		Id = 0xD51A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_liveness_reward_succ_d51a();
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
