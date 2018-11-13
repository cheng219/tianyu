using System.Collections;
using System.Collections.Generic;

public class pt_req_login_dividend_reward_d982 : st.net.NetBase.Pt {
	public pt_req_login_dividend_reward_d982()
	{
		Id = 0xD982;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_login_dividend_reward_d982();
	}
	public uint reward_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		reward_id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(reward_id);
		return writer.data;
	}

}
