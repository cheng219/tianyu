using System.Collections;
using System.Collections.Generic;

public class pt_cast_soul_crit_d777 : st.net.NetBase.Pt {
	public pt_cast_soul_crit_d777()
	{
		Id = 0xD777;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_cast_soul_crit_d777();
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
