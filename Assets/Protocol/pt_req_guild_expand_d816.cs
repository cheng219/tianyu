using System.Collections;
using System.Collections.Generic;

public class pt_req_guild_expand_d816 : st.net.NetBase.Pt {
	public pt_req_guild_expand_d816()
	{
		Id = 0xD816;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_guild_expand_d816();
	}
	public int guild_id;
	public int expand_num;
	public int quick;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		guild_id = reader.Read_int();
		expand_num = reader.Read_int();
		quick = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(guild_id);
		writer.write_int(expand_num);
		writer.write_int(quick);
		return writer.data;
	}

}
