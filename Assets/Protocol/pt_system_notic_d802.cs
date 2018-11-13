using System.Collections;
using System.Collections.Generic;

public class pt_system_notic_d802 : st.net.NetBase.Pt {
	public pt_system_notic_d802()
	{
		Id = 0xD802;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_system_notic_d802();
	}
	public string body;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		body = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(body);
		return writer.data;
	}

}
