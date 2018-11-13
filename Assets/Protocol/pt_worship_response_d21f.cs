using System.Collections;
using System.Collections.Generic;

public class pt_worship_response_d21f : st.net.NetBase.Pt {
	public pt_worship_response_d21f()
	{
		Id = 0xD21F;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_worship_response_d21f();
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
