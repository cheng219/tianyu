using System.Collections;
using System.Collections.Generic;

public class pt_req_lev_reward_info_d760 : st.net.NetBase.Pt {
	public pt_req_lev_reward_info_d760()
	{
		Id = 0xD760;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_lev_reward_info_d760();
	}
	public int reward_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		reward_type = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(reward_type);
		return writer.data;
	}

}
