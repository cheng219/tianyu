using System.Collections;
using System.Collections.Generic;

public class pt_break_timer_d213 : st.net.NetBase.Pt {
	public pt_break_timer_d213()
	{
		Id = 0xD213;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_break_timer_d213();
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
