using System.Collections;
using System.Collections.Generic;

public class pt_sworn_success_d690 : st.net.NetBase.Pt {
	public pt_sworn_success_d690()
	{
		Id = 0xD690;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_sworn_success_d690();
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
