using System.Collections;
using System.Collections.Generic;

public class pt_req_use_item_d316 : st.net.NetBase.Pt {
	public pt_req_use_item_d316()
	{
		Id = 0xD316;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_use_item_d316();
	}
	public int id;
	public int num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_int();
		num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		writer.write_int(num);
		return writer.data;
	}

}
