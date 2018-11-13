using System.Collections;
using System.Collections.Generic;

public class pt_req_equips_d352 : st.net.NetBase.Pt {
	public pt_req_equips_d352()
	{
		Id = 0xD352;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_equips_d352();
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
