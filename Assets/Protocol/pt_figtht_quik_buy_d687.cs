using System.Collections;
using System.Collections.Generic;

public class pt_figtht_quik_buy_d687 : st.net.NetBase.Pt {
	public pt_figtht_quik_buy_d687()
	{
		Id = 0xD687;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_figtht_quik_buy_d687();
	}
	public int type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		return writer.data;
	}

}
