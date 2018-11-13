using System.Collections;
using System.Collections.Generic;

public class pt_req_net_a003 : st.net.NetBase.Pt {
	public pt_req_net_a003()
	{
		Id = 0xA003;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_net_a003();
	}
	public uint uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		uid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(uid);
		return writer.data;
	}

}
