using System.Collections;
using System.Collections.Generic;

public class pt_guild_battle_rest_time_d695 : st.net.NetBase.Pt {
	public pt_guild_battle_rest_time_d695()
	{
		Id = 0xD695;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_battle_rest_time_d695();
	}
	public int time;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		time = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(time);
		return writer.data;
	}

}
