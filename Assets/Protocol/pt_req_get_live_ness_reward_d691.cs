using System.Collections;
using System.Collections.Generic;

public class pt_req_get_live_ness_reward_d691 : st.net.NetBase.Pt {
	public pt_req_get_live_ness_reward_d691()
	{
		Id = 0xD691;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_get_live_ness_reward_d691();
	}
	public int live_ness_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		live_ness_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(live_ness_id);
		return writer.data;
	}

}
