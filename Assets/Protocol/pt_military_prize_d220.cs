using System.Collections;
using System.Collections.Generic;

public class pt_military_prize_d220 : st.net.NetBase.Pt {
	public pt_military_prize_d220()
	{
		Id = 0xD220;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_military_prize_d220();
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
