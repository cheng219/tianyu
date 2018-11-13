using System.Collections;
using System.Collections.Generic;

public class pt_req_guild_list_d500 : st.net.NetBase.Pt {
	public pt_req_guild_list_d500()
	{
		Id = 0xD500;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_guild_list_d500();
	}
	public int page;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		page = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(page);
		return writer.data;
	}

}
