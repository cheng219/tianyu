using System.Collections;
using System.Collections.Generic;

public class pt_can_worship_d21e : st.net.NetBase.Pt {
	public pt_can_worship_d21e()
	{
		Id = 0xD21E;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_can_worship_d21e();
	}
	public int can_wor;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		can_wor = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(can_wor);
		return writer.data;
	}

}
