using System.Collections;
using System.Collections.Generic;

public class pt_req_cart_pos_d613 : st.net.NetBase.Pt {
	public pt_req_cart_pos_d613()
	{
		Id = 0xD613;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_cart_pos_d613();
	}
	public byte type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(type);
		return writer.data;
	}

}
