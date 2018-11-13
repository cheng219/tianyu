using System.Collections;
using System.Collections.Generic;

public class pt_gather_jewelry_finish_d796 : st.net.NetBase.Pt {
	public pt_gather_jewelry_finish_d796()
	{
		Id = 0xD796;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_gather_jewelry_finish_d796();
	}
	public int jewelry_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		jewelry_type = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(jewelry_type);
		return writer.data;
	}

}
