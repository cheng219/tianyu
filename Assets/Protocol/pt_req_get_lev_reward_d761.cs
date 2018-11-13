using System.Collections;
using System.Collections.Generic;

public class pt_req_get_lev_reward_d761 : st.net.NetBase.Pt {
	public pt_req_get_lev_reward_d761()
	{
		Id = 0xD761;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_get_lev_reward_d761();
	}
	public int get_lev;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		get_lev = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(get_lev);
		return writer.data;
	}

}
