using System.Collections;
using System.Collections.Generic;

public class pt_marry_success_d688 : st.net.NetBase.Pt {
	public pt_marry_success_d688()
	{
		Id = 0xD688;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_marry_success_d688();
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
