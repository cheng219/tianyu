using System.Collections;
using System.Collections.Generic;

public class pt_wrath_of_heaven_d789 : st.net.NetBase.Pt {
	public pt_wrath_of_heaven_d789()
	{
		Id = 0xD789;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_wrath_of_heaven_d789();
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
