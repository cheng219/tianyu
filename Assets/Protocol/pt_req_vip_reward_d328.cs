using System.Collections;
using System.Collections.Generic;

public class pt_req_vip_reward_d328 : st.net.NetBase.Pt {
	public pt_req_vip_reward_d328()
	{
		Id = 0xD328;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_vip_reward_d328();
	}
	public int vip_lev;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		vip_lev = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(vip_lev);
		return writer.data;
	}

}
