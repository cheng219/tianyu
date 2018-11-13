using System.Collections;
using System.Collections.Generic;

public class pt_req_change_oath_d548 : st.net.NetBase.Pt {
	public pt_req_change_oath_d548()
	{
		Id = 0xD548;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_change_oath_d548();
	}
	public string oath;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		oath = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(oath);
		return writer.data;
	}

}
