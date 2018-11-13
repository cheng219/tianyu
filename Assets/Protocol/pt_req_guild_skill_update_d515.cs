using System.Collections;
using System.Collections.Generic;

public class pt_req_guild_skill_update_d515 : st.net.NetBase.Pt {
	public pt_req_guild_skill_update_d515()
	{
		Id = 0xD515;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_guild_skill_update_d515();
	}
	public int skill_id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		skill_id = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(skill_id);
		return writer.data;
	}

}
