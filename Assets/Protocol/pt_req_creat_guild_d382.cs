using System.Collections;
using System.Collections.Generic;

public class pt_req_creat_guild_d382 : st.net.NetBase.Pt {
	public pt_req_creat_guild_d382()
	{
		Id = 0xD382;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_creat_guild_d382();
	}
	public string name;
	public string purpose;
	public int creat_type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		name = reader.Read_str();
		purpose = reader.Read_str();
		creat_type = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_str(name);
		writer.write_str(purpose);
		writer.write_int(creat_type);
		return writer.data;
	}

}
