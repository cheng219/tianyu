using System.Collections;
using System.Collections.Generic;

public class pt_reply_order_da01 : st.net.NetBase.Pt {
	public pt_reply_order_da01()
	{
		Id = 0xDA01;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_order_da01();
	}
	public string order_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		order_id = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(order_id);
		return writer.data;
	}

}
