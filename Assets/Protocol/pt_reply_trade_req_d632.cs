using System.Collections;
using System.Collections.Generic;

public class pt_reply_trade_req_d632 : st.net.NetBase.Pt {
	public pt_reply_trade_req_d632()
	{
		Id = 0xD632;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_reply_trade_req_d632();
	}
	public uint req_id;
	public byte reply;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		req_id = reader.Read_uint();
		reply = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(req_id);
		writer.write_byte(reply);
		return writer.data;
	}

}
