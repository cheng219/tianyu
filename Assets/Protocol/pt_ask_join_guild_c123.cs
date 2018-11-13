using System.Collections;
using System.Collections.Generic;

public class pt_ask_join_guild_c123 : st.net.NetBase.Pt {
	public pt_ask_join_guild_c123()
	{
		Id = 0xC123;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ask_join_guild_c123();
	}
	public uint target_uid;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		target_uid = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(target_uid);
		return writer.data;
	}

}
