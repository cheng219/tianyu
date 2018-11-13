using System.Collections;
using System.Collections.Generic;

public class pt_get_everday_reward_d755 : st.net.NetBase.Pt {
	public pt_get_everday_reward_d755()
	{
		Id = 0xD755;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_get_everday_reward_d755();
	}
	public int reward_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		reward_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(reward_id);
		return writer.data;
	}

}
