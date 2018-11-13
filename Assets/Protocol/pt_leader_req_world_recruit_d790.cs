using System.Collections;
using System.Collections.Generic;

public class pt_leader_req_world_recruit_d790 : st.net.NetBase.Pt {
	public pt_leader_req_world_recruit_d790()
	{
		Id = 0xD790;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_leader_req_world_recruit_d790();
	}
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		return writer.data;
	}

}
