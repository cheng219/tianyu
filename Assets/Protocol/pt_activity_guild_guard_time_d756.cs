using System.Collections;
using System.Collections.Generic;

public class pt_activity_guild_guard_time_d756 : st.net.NetBase.Pt {
	public pt_activity_guild_guard_time_d756()
	{
		Id = 0xD756;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_activity_guild_guard_time_d756();
	}
	public int act_id;
	public int surplus_time;
	public int start_state;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		act_id = reader.Read_int();
		surplus_time = reader.Read_int();
		start_state = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(act_id);
		writer.write_int(surplus_time);
		writer.write_int(start_state);
		return writer.data;
	}

}
