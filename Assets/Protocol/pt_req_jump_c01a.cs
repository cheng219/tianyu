using System.Collections;
using System.Collections.Generic;

public class pt_req_jump_c01a : st.net.NetBase.Pt {
	public pt_req_jump_c01a()
	{
		Id = 0xC01A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_jump_c01a();
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
