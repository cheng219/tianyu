using System.Collections;
using System.Collections.Generic;

public class pt_req_move_guild_item_d385 : st.net.NetBase.Pt {
	public pt_req_move_guild_item_d385()
	{
		Id = 0xD385;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_move_guild_item_d385();
	}
	public int id;
	public int action;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_int();
		action = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_int(action);
		return writer.data;
	}

}
