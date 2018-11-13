using System.Collections;
using System.Collections.Generic;

public class pt_decompose_result_d302 : st.net.NetBase.Pt {
	public pt_decompose_result_d302()
	{
		Id = 0xD302;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_decompose_result_d302();
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
