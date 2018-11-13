using System.Collections;
using System.Collections.Generic;

public class pt_req_back_to_city_c112 : st.net.NetBase.Pt {
	public pt_req_back_to_city_c112()
	{
		Id = 0xC112;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_back_to_city_c112();
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
