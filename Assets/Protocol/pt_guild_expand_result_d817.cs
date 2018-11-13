using System.Collections;
using System.Collections.Generic;

public class pt_guild_expand_result_d817 : st.net.NetBase.Pt {
	public pt_guild_expand_result_d817()
	{
		Id = 0xD817;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_expand_result_d817();
	}
	public int result;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		result = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(result);
		return writer.data;
	}

}
