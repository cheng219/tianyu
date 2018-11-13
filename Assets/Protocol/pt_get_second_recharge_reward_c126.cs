using System.Collections;
using System.Collections.Generic;

public class pt_get_second_recharge_reward_c126 : st.net.NetBase.Pt {
	public pt_get_second_recharge_reward_c126()
	{
		Id = 0xC126;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_get_second_recharge_reward_c126();
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
