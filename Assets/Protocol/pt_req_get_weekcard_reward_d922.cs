using System.Collections;
using System.Collections.Generic;

public class pt_req_get_weekcard_reward_d922 : st.net.NetBase.Pt {
	public pt_req_get_weekcard_reward_d922()
	{
		Id = 0xD922;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_get_weekcard_reward_d922();
	}
	public uint type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		return writer.data;
	}

}
