using System.Collections;
using System.Collections.Generic;

public class pt_guild_name_d137 : st.net.NetBase.Pt {
	public pt_guild_name_d137()
	{
		Id = 0xD137;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_name_d137();
	}
	public string guild_name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		guild_name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(guild_name);
		return writer.data;
	}

}
