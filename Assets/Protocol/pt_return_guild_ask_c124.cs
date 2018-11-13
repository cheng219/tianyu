using System.Collections;
using System.Collections.Generic;

public class pt_return_guild_ask_c124 : st.net.NetBase.Pt {
	public pt_return_guild_ask_c124()
	{
		Id = 0xC124;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_return_guild_ask_c124();
	}
	public int ask_uid;
	public string ask_name;
	public string ask_guild_name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ask_uid = reader.Read_int();
		ask_name = reader.Read_str();
		ask_guild_name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(ask_uid);
		writer.write_str(ask_name);
		writer.write_str(ask_guild_name);
		return writer.data;
	}

}
