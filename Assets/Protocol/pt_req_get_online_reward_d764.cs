using System.Collections;
using System.Collections.Generic;

public class pt_req_get_online_reward_d764 : st.net.NetBase.Pt {
	public pt_req_get_online_reward_d764()
	{
		Id = 0xD764;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_get_online_reward_d764();
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
