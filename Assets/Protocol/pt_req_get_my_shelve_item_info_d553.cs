using System.Collections;
using System.Collections.Generic;

public class pt_req_get_my_shelve_item_info_d553 : st.net.NetBase.Pt {
	public pt_req_get_my_shelve_item_info_d553()
	{
		Id = 0xD553;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_get_my_shelve_item_info_d553();
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
