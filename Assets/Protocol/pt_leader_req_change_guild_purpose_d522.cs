using System.Collections;
using System.Collections.Generic;

public class pt_leader_req_change_guild_purpose_d522 : st.net.NetBase.Pt {
	public pt_leader_req_change_guild_purpose_d522()
	{
		Id = 0xD522;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_leader_req_change_guild_purpose_d522();
	}
	public string purpose;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		purpose = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(purpose);
		return writer.data;
	}

}
