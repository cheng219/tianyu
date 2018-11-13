using System.Collections;
using System.Collections.Generic;

public class pt_trade_finish_d638 : st.net.NetBase.Pt {
	public pt_trade_finish_d638()
	{
		Id = 0xD638;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_trade_finish_d638();
	}
	public byte is_succ;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		is_succ = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(is_succ);
		return writer.data;
	}

}
