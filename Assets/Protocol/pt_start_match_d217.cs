using System.Collections;
using System.Collections.Generic;

public class pt_start_match_d217 : st.net.NetBase.Pt {
	public pt_start_match_d217()
	{
		Id = 0xD217;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_start_match_d217();
	}
	public uint groupid;
	public uint dungeonsid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		groupid = reader.Read_uint();
		dungeonsid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(groupid);
		writer.write_int(dungeonsid);
		return writer.data;
	}

}
