using System.Collections;
using System.Collections.Generic;

public class pt_req_treasure_d377 : st.net.NetBase.Pt {
	public pt_req_treasure_d377()
	{
		Id = 0xD377;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_treasure_d377();
	}
	public int type;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		type = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(type);
		return writer.data;
	}

}
