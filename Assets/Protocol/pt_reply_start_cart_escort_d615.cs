using System.Collections;
using System.Collections.Generic;

public class pt_reply_start_cart_escort_d615 : st.net.NetBase.Pt {
	public pt_reply_start_cart_escort_d615()
	{
		Id = 0xD615;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_start_cart_escort_d615();
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
