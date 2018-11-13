using System.Collections;
using System.Collections.Generic;

public class pt_reply_buy_open_server_gift_d913 : st.net.NetBase.Pt {
	public pt_reply_buy_open_server_gift_d913()
	{
		Id = 0xD913;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_buy_open_server_gift_d913();
	}
	public byte result;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		result = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(result);
		return writer.data;
	}

}
