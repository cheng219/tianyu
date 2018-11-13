using System.Collections;
using System.Collections.Generic;

public class pt_req_guild_battle_info_d560 : st.net.NetBase.Pt {
	public pt_req_guild_battle_info_d560()
	{
		Id = 0xD560;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_guild_battle_info_d560();
	}
	public int index;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		index = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(index);
		return writer.data;
	}

}
