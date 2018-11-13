using System.Collections;
using System.Collections.Generic;

public class pt_love_reward_request_d805 : st.net.NetBase.Pt {
	public pt_love_reward_request_d805()
	{
		Id = 0xD805;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_love_reward_request_d805();
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
