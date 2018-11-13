using System.Collections;
using System.Collections.Generic;

public class pt_trade_start_d633 : st.net.NetBase.Pt {
	public pt_trade_start_d633()
	{
		Id = 0xD633;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_trade_start_d633();
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
