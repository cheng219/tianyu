using System.Collections;
using System.Collections.Generic;

public class pt_update_quell_demon_tier_reward_d772 : st.net.NetBase.Pt {
	public pt_update_quell_demon_tier_reward_d772()
	{
		Id = 0xD772;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_quell_demon_tier_reward_d772();
	}
	public int reward_state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		reward_state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(reward_state);
		return writer.data;
	}

}
