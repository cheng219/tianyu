using System.Collections;
using System.Collections.Generic;

public class pt_req_up_ride_lev_d431 : st.net.NetBase.Pt {
	public pt_req_up_ride_lev_d431()
	{
		Id = 0xD431;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_up_ride_lev_d431();
	}
	public int quick_buy;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		quick_buy = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(quick_buy);
		return writer.data;
	}

}
