using System.Collections;
using System.Collections.Generic;

public class pt_mequip_list_req_d574 : st.net.NetBase.Pt {
	public pt_mequip_list_req_d574()
	{
		Id = 0xD574;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_mequip_list_req_d574();
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
