using System.Collections;
using System.Collections.Generic;

public class pt_req_open_lucky_brand_d468 : st.net.NetBase.Pt {
	public pt_req_open_lucky_brand_d468()
	{
		Id = 0xD468;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_open_lucky_brand_d468();
	}
	public int brand_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		brand_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(brand_id);
		return writer.data;
	}

}
