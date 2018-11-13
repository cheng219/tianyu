using System.Collections;
using System.Collections.Generic;

public class pt_req_cast_soul_reward_c100 : st.net.NetBase.Pt {
	public pt_req_cast_soul_reward_c100()
	{
		Id = 0xC100;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_cast_soul_reward_c100();
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
