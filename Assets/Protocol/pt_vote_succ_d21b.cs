using System.Collections;
using System.Collections.Generic;

public class pt_vote_succ_d21b : st.net.NetBase.Pt {
	public pt_vote_succ_d21b()
	{
		Id = 0xD21B;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_vote_succ_d21b();
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
