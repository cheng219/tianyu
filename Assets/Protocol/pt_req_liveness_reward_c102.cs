using System.Collections;
using System.Collections.Generic;

public class pt_req_liveness_reward_c102 : st.net.NetBase.Pt {
	public pt_req_liveness_reward_c102()
	{
		Id = 0xC102;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_liveness_reward_c102();
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
