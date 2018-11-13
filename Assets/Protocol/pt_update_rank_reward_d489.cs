using System.Collections;
using System.Collections.Generic;

public class pt_update_rank_reward_d489 : st.net.NetBase.Pt {
	public pt_update_rank_reward_d489()
	{
		Id = 0xD489;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_update_rank_reward_d489();
	}
	public int state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		return writer.data;
	}

}
