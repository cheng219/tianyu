using System.Collections;
using System.Collections.Generic;

public class pt_req_guild_items_log_d526 : st.net.NetBase.Pt {
	public pt_req_guild_items_log_d526()
	{
		Id = 0xD526;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_guild_items_log_d526();
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
