using System.Collections;
using System.Collections.Generic;

public class pt_budo_win_d736 : st.net.NetBase.Pt {
	public pt_budo_win_d736()
	{
		Id = 0xD736;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_budo_win_d736();
	}
	public int val;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		val = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(val);
		return writer.data;
	}

}
