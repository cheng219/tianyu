using System.Collections;
using System.Collections.Generic;

public class pt_invite_join_guild_d119 : st.net.NetBase.Pt {
	public pt_invite_join_guild_d119()
	{
		Id = 0xD119;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_invite_join_guild_d119();
	}
	public string usr_name;
	public string guild_name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		usr_name = reader.Read_str();
		guild_name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(usr_name);
		writer.write_str(guild_name);
		return writer.data;
	}

}
