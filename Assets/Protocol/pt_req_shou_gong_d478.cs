using System.Collections;
using System.Collections.Generic;

public class pt_req_shou_gong_d478 : st.net.NetBase.Pt {
	public pt_req_shou_gong_d478()
	{
		Id = 0xD478;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_shou_gong_d478();
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
