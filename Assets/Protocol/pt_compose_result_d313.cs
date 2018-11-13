using System.Collections;
using System.Collections.Generic;

public class pt_compose_result_d313 : st.net.NetBase.Pt {
	public pt_compose_result_d313()
	{
		Id = 0xD313;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_compose_result_d313();
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
