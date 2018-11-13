using System.Collections;
using System.Collections.Generic;

public class pt_req_xiuxing_d476 : st.net.NetBase.Pt {
	public pt_req_xiuxing_d476()
	{
		Id = 0xD476;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_xiuxing_d476();
	}
	public uint state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		return writer.data;
	}

}
