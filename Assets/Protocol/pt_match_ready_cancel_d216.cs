using System.Collections;
using System.Collections.Generic;

public class pt_match_ready_cancel_d216 : st.net.NetBase.Pt {
	public pt_match_ready_cancel_d216()
	{
		Id = 0xD216;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_match_ready_cancel_d216();
	}
	public uint groupid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		groupid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(groupid);
		return writer.data;
	}

}
