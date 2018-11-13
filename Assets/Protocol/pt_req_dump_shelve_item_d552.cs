using System.Collections;
using System.Collections.Generic;

public class pt_req_dump_shelve_item_d552 : st.net.NetBase.Pt {
	public pt_req_dump_shelve_item_d552()
	{
		Id = 0xD552;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_req_dump_shelve_item_d552();
	}
	public uint id;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		id = reader.Read_uint();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(id);
		return writer.data;
	}

}
