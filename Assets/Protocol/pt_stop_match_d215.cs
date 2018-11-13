using System.Collections;
using System.Collections.Generic;

public class pt_stop_match_d215 : st.net.NetBase.Pt {
	public pt_stop_match_d215()
	{
		Id = 0xD215;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_stop_match_d215();
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
