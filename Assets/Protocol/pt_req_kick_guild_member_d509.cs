using System.Collections;
using System.Collections.Generic;

public class pt_req_kick_guild_member_d509 : st.net.NetBase.Pt {
	public pt_req_kick_guild_member_d509()
	{
		Id = 0xD509;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_kick_guild_member_d509();
	}
	public int tagert_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		tagert_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(tagert_id);
		return writer.data;
	}

}
