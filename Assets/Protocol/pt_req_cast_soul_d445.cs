using System.Collections;
using System.Collections.Generic;

public class pt_req_cast_soul_d445 : st.net.NetBase.Pt {
	public pt_req_cast_soul_d445()
	{
		Id = 0xD445;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_cast_soul_d445();
	}
	public int soul_id;
	public int state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		soul_id = reader.Read_int();
		state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(soul_id);
		writer.write_int(state);
		return writer.data;
	}

}
