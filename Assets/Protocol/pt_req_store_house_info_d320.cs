using System.Collections;
using System.Collections.Generic;

public class pt_req_store_house_info_d320 : st.net.NetBase.Pt {
	public pt_req_store_house_info_d320()
	{
		Id = 0xD320;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_store_house_info_d320();
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
