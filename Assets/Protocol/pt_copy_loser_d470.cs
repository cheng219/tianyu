using System.Collections;
using System.Collections.Generic;

public class pt_copy_loser_d470 : st.net.NetBase.Pt {
	public pt_copy_loser_d470()
	{
		Id = 0xD470;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_copy_loser_d470();
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
