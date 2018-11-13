using System.Collections;
using System.Collections.Generic;

public class pt_guild_notice_d138 : st.net.NetBase.Pt {
	public pt_guild_notice_d138()
	{
		Id = 0xD138;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_notice_d138();
	}
	public string data;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		data = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(data);
		return writer.data;
	}

}
