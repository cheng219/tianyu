using System.Collections;
using System.Collections.Generic;

public class pt_req_compose_all_d312 : st.net.NetBase.Pt {
	public pt_req_compose_all_d312()
	{
		Id = 0xD312;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_compose_all_d312();
	}
	public int id;
	public uint times;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_int();
		times = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_int(times);
		return writer.data;
	}

}
