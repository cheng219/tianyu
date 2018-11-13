using System.Collections;
using System.Collections.Generic;

public class pt_req_confirm_trade_d636 : st.net.NetBase.Pt {
	public pt_req_confirm_trade_d636()
	{
		Id = 0xD636;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_confirm_trade_d636();
	}
	public uint session_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		session_id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(session_id);
		return writer.data;
	}

}
