using System.Collections;
using System.Collections.Generic;

public class pt_cart_escort_succ_d616 : st.net.NetBase.Pt {
	public pt_cart_escort_succ_d616()
	{
		Id = 0xD616;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_cart_escort_succ_d616();
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
