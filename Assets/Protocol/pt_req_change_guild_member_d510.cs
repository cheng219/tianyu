using System.Collections;
using System.Collections.Generic;

public class pt_req_change_guild_member_d510 : st.net.NetBase.Pt {
	public pt_req_change_guild_member_d510()
	{
		Id = 0xD510;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_change_guild_member_d510();
	}
	public int target_id;
	public int pos;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		target_id = reader.Read_int();
		pos = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(target_id);
		writer.write_int(pos);
		return writer.data;
	}

}
