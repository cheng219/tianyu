using System.Collections;
using System.Collections.Generic;

public class pt_get_snatch_soldier_reward_d795 : st.net.NetBase.Pt {
	public pt_get_snatch_soldier_reward_d795()
	{
		Id = 0xD795;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_get_snatch_soldier_reward_d795();
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
