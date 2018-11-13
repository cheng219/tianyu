using System.Collections;
using System.Collections.Generic;

public class pt_ping_a10a : st.net.NetBase.Pt {
	public pt_ping_a10a()
	{
		Id = 0xA10A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ping_a10a();
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
