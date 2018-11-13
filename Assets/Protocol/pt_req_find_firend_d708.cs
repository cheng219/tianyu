using System.Collections;
using System.Collections.Generic;

public class pt_req_find_firend_d708 : st.net.NetBase.Pt {
	public pt_req_find_firend_d708()
	{
		Id = 0xD708;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_find_firend_d708();
	}
	public string name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(name);
		return writer.data;
	}

}
