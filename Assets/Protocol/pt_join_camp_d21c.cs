using System.Collections;
using System.Collections.Generic;

public class pt_join_camp_d21c : st.net.NetBase.Pt {
	public pt_join_camp_d21c()
	{
		Id = 0xD21C;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_join_camp_d21c();
	}
	public uint campid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		campid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(campid);
		return writer.data;
	}

}
