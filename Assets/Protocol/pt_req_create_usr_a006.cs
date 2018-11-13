using System.Collections;
using System.Collections.Generic;

public class pt_req_create_usr_a006 : st.net.NetBase.Pt {
	public pt_req_create_usr_a006()
	{
		Id = 0xA006;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_create_usr_a006();
	}
	public uint prof;
	public string name;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		prof = reader.Read_uint();
		name = reader.Read_str();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(prof);
		writer.write_str(name);
		return writer.data;
	}

}
