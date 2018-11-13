using System.Collections;
using System.Collections.Generic;

public class pt_req_start_cart_escort_d612 : st.net.NetBase.Pt {
	public pt_req_start_cart_escort_d612()
	{
		Id = 0xD612;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_start_cart_escort_d612();
	}
	public byte type;
	public byte cart_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_byte();
		cart_type = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(type);
		writer.write_byte(cart_type);
		return writer.data;
	}

}
