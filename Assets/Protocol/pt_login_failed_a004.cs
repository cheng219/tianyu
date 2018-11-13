using System.Collections;
using System.Collections.Generic;

public class pt_login_failed_a004 : st.net.NetBase.Pt {
	public pt_login_failed_a004()
	{
		Id = 0xA004;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_login_failed_a004();
	}
	public byte reason;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		reason = reader.Read_byte();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_byte(reason);
		return writer.data;
	}

}
