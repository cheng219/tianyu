using System.Collections;
using System.Collections.Generic;

public class pt_req_order_da00 : st.net.NetBase.Pt {
	public pt_req_order_da00()
	{
		Id = 0xDA00;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_order_da00();
	}
	public uint type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		return writer.data;
	}

}
