using System.Collections;
using System.Collections.Generic;

public class pt_guild_battle_state_d694 : st.net.NetBase.Pt {
	public pt_guild_battle_state_d694()
	{
		Id = 0xD694;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_battle_state_d694();
	}
	public int state;
	public string win_guild_name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		state = reader.Read_int();
		win_guild_name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(state);
		writer.write_str(win_guild_name);
		return writer.data;
	}

}
