using System.Collections;
using System.Collections.Generic;

public class pt_req_tarot_c127 : st.net.NetBase.Pt {
	public pt_req_tarot_c127()
	{
		Id = 0xC127;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_tarot_c127();
	}
	public uint req_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		req_type = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(req_type);
		return writer.data;
	}

}
