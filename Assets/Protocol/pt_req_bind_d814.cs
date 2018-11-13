using System.Collections;
using System.Collections.Generic;

public class pt_req_bind_d814 : st.net.NetBase.Pt {
	public pt_req_bind_d814()
	{
		Id = 0xD814;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_bind_d814();
	}
	public int type;
	public int id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_int();
		id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		writer.write_int(id);
		return writer.data;
	}

}
